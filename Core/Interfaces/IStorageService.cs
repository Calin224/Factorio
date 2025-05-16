using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task DeleteFileAsync(string fileName);
    Task DeleteEverything();
}