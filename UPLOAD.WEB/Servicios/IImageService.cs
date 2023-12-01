using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.WEB.Services
{
    public interface IImageService
    {
        Task<IReadOnlyCollection<Image>> GetAllImages();
        Task<Image> UploadImage(ImagenDTO request);
    }
}
