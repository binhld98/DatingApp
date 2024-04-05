using API.Interfaces;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly IFileService _fileService;

    public PhotoService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<string> AddPhotoAsync(IFormFile file)
    {
        var guid = Guid.NewGuid();
        var fileName = guid.ToString() + Path.GetExtension(file.FileName);

        using var content = new MemoryStream();
        file.CopyTo(content);

        return await _fileService.UploadFromContentAsync(fileName, content.ToArray(), file.ContentType);
    }

    public async Task DeletePhotoAsync(string name)
    {
        await _fileService.DeleteAsync(name);
    }
}
