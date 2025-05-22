using System.Security.Claims;
using System.Web;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resend;

namespace API.Controllers;

public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> _userManager)
    : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new AppUser()
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var res = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
        if (!res.Succeeded)
        {
            return StatusCode(201);
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }

        var user = await signInManager.UserManager.GetUserByEmail(User);

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.Id
        });
    }

    [HttpGet("auth-status")]
    public ActionResult AuthStatus()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false
        });
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return Ok(new { message = "Dacă contul există, am trimis un email pentru resetarea parolei." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetUrl =
                $"https://site-ul-tau.com/reset-password?email={forgotPasswordDto.Email}&token={encodedToken}";

            // Creare directă a clientului folosind cheia API
            IResend resend = ResendClient.Create("re_9pFBSnus_3aHmE9NsjJjamJ6W7rqCJQQY");

            var emailMessage = new EmailMessage()
            {
                From = "onboarding@resend.dev",
                To = forgotPasswordDto.Email,
                Subject = "Resetare parolă",
                HtmlBody = $@"<p>Dă click pe <a href='{resetUrl}'>acest link</a> pentru a-ți reseta parola.</p>"
            };

            var resp = await resend.EmailSendAsync(emailMessage);

            return Ok(new { message = "Email de resetare trimis cu succes." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la trimiterea email-ului: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }

            return BadRequest(new
            {
                error = "Eroare la trimiterea email-ului",
                details = ex.Message
            });
        }
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("Utilizatorul nu există");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                resetPasswordDto.Token,
                resetPasswordDto.NewPassword
            );

            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            return Ok(new { message = "Parola a fost resetată cu succes" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("external-login")]
    public IActionResult ExternalLogin(string? returnUrl = null)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl ?? "/" });
        var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return Challenge(properties, "Google");
    }

    [HttpGet("external-login-callback")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError != null)
            return BadRequest($"Eroare de la Google: {remoteError}");

        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return BadRequest("Nu s-au putut obține informațiile externe.");

        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded)
            return Redirect("http://localhost:4200/");   

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
        var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var createResult = await signInManager.UserManager.CreateAsync(user);
        if (!createResult.Succeeded)
            return BadRequest($"error: {createResult.Errors}");

        await signInManager.UserManager.AddLoginAsync(user, info);
        await signInManager.SignInAsync(user, isPersistent: false);

        return Redirect("http://localhost:4200/");
    }

    [HttpGet("all-users")]
    public async Task<ActionResult> GetAllUsers()
    {
        var users = _userManager.Users.Select(u => new
        {
            u.Id,
            u.FirstName,
            u.LastName,
            u.Email
        });
        return Ok(await users.ToListAsync());
    }
}