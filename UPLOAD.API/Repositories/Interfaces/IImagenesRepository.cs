using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Interfaces
{
    public interface IImagenesRepository
    {
        Task<ActionResponse<IEnumerable<Image>>> GetAsync();
    }
}
