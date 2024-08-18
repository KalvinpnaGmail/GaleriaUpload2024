using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Implementations
{
    public class ImagenesRepository : IImagenesRepository
    {
        public Task<ActionResponse<IEnumerable<Image>>> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}
