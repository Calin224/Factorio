using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ItemsController(IGenericRepository<Folder> repo, IStorageService blobStorage) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Folder>> CreateItem(IFormFile file, int folderId)
    {
        var folder = await repo.GetByIdAsync(folderId);

        if (folder == null)
            repo.Add(folder);

        string fileUrl = await blobStorage.UploadFileAsync(file);

        var item = new Item()
        {
            Name = file.Name,
            Url = fileUrl,
            ContentType = file.ContentType
        };
        
        folder.Items.Add(item);

        if (await repo.SaveAllAsync()) return Ok(folder);
        
        return BadRequest("File could not be uploaded");
    }
}