using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ItemsController(IGenericRepository<Folder> repo, IStorageService blobStorage) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Folder>> CreateItem(IFormFile file, int folderId, string? folderName = null)
    {
        var folder = await repo.GetByIdAsync(folderId);
        if (folder == null)
        {
            if (string.IsNullOrEmpty(folderName))
                return BadRequest("Folder does not exist and no name was provided to create a new one.");

            folder = new Folder
            {
                Name = folderName,
                Items = new List<Item>()
            };

            repo.Add(folder);
        }

        string fileUrl = await blobStorage.UploadFileAsync(file);

        var item = new Item()
        {
            Name = file.Name,
            Url = fileUrl,
            ContentType = file.ContentType,
        };
        
        folder.Items.Add(item);

        if (await repo.SaveAllAsync()) return Ok(folder);
        
        return BadRequest("File could not be uploaded");
    }
}