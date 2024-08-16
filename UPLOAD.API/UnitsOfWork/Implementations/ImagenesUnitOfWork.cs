using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Services;

namespace UPLOAD.API.UnitsOfWork.Implementations
{
    public class ImagenesUnitOfWork : GenericUnitOfWork<Image>, IImagenesUnitOfWork
    {
        private readonly IImagenesRepository _imagenesRepository;

        public ImagenesUnitOfWork(IGenericRepository<Image> repository,  IImagenesRepository imagenesRepository) : base(repository)
        {
            _imagenesRepository = imagenesRepository;
        }



       

        public Task<IReadOnlyCollection<Image>> GetAllImages()
        {
            throw new NotImplementedException();
        }

        public Task<Image> UploadImage(ImagenDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
