using System;
using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
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
            FolderName = folderDto.FolderName,
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

    [HttpGet("all-folders")]
    public async Task<ActionResult<IReadOnlyList<Folder>>> GetAllFolders()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var spec = new FolderSpecification(user.Id);
        var folders = await repo.ListAsync(spec);

        return Ok(folders);
    }

    [HttpGet("folder/{folder_id}")]
    public async Task<ActionResult<Folder>> GetFolderByName(int folder_id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var spec = new FolderSpecification(user.Id, folder_id);
        var folder = await repo.GetEntityWithSpec(spec);

        return Ok(folder);
    }
}
