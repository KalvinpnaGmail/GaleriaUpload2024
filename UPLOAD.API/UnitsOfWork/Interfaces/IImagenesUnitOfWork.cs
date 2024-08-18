using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Interfaces
{
    public interface IImagenesUnitOfWork
    {
        ///Lista de pais que venga con los provincias
        Task<ActionResponse<IEnumerable<Image>>> GetAsync();
    }
}
