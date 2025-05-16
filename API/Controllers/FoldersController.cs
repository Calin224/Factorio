using System;
using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class FoldersController(IGenericRepository<Folder> repo, UserManager<AppUser> userManager) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Folder>> CreateFolder([FromBody] FolderDto folderDto)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var folder = new Folder
        {
            Name = folderDto.Name,
            AppUserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        repo.Add(folder);
        if (await repo.SaveAllAsync())
        {
            return Ok(folder);
        }
        return BadRequest("Problem creating folder");
    }
}
