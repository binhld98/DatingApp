namespace API.Interfaces;

public interface IPhotoService
{
    Task<string> AddPhotoAsync(IFormFile file);
    Task DeletePhotoAsync(string name);
}
