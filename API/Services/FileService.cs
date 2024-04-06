using System.Diagnostics;
using API.Configs;
using API.Interfaces;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace API.Services;

public class FileService : IFileService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger _logger;
    private readonly Stopwatch _sw;

    public FileService(IOptions<BlobStorageConfigs> options, ILogger<FileService> logger)
    {
        _containerClient = new BlobContainerClient(
            new Uri(options.Value.ContainerUri),
            new AzureSasCredential(options.Value.RWD_SAS)
        );

        _logger = logger;
        _sw = new Stopwatch();
    }

    public async Task<string> UploadFromContentAsync(string name, byte[] content, string contentType = "application/octet-stream")
    {
        _sw.Restart();

        var blobClient = _containerClient.GetBlobClient(name);

        if (await blobClient.ExistsAsync())
        {
            throw new Exception("Blob exists");
        }

        var blobHttpHeader = new BlobHttpHeaders() { ContentType = contentType };

        await blobClient.UploadAsync(new BinaryData(content), new BlobUploadOptions() { HttpHeaders = blobHttpHeader });

        _sw.Stop();
        _logger.LogInformation("[UploadFromContentAsync] Enlapsed in: " + _sw.ElapsedMilliseconds);

        return _containerClient.Uri.AbsoluteUri + "/" + name;
    }

    public async Task<byte[]> DownloadToContent(string name)
    {
        _sw.Restart();

        var blobClient = _containerClient.GetBlobClient(name);

        if (!await blobClient.ExistsAsync())
        {
            throw new Exception("Blob does not exists");
        }

        var result = await blobClient.DownloadContentAsync();
        _sw.Stop();
        _logger.LogInformation("[DownloadToContent] Enlapsed in: " + _sw.ElapsedMilliseconds);

        return result.Value.Content.ToArray();
    }

    public async Task DeleteAsync(string name)
    {
        _sw.Restart();

        var blobClient = _containerClient.GetBlobClient(name);

        if (!await blobClient.ExistsAsync())
        {
            throw new Exception("Blob does not exists");
        }

        await blobClient.DeleteAsync();

        _sw.Stop();
        _logger.LogInformation("[DownloadToContent] Enlapsed in: " + _sw.ElapsedMilliseconds);
    }
}
