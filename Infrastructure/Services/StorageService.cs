using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class StorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "zipfiles";

    public StorageService(IConfiguration config)
    {
        _blobServiceClient = new BlobServiceClient(config["MyConfig:X"]);
    }
    
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        
        var blobClient = containerClient.GetBlobClient(file.FileName);
        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

        return blobClient.Uri.ToString();
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task DeleteEverything()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.DeleteIfExistsAsync();
    }
}