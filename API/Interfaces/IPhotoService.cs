using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<DeletionResult> DeletionPhotoAsync(string PublicId);
    }
}
