namespace API.Interfaces;

public interface IFileService
{
    Task<string> UploadFromContentAsync(string name, byte[] content, string contentType = "application/octet-stream");

    Task<byte[]> DownloadToContent(string name);

    Task DeleteAsync(string name);
}
