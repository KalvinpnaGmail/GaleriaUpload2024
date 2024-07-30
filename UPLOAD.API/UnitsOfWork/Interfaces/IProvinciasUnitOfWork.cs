using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Interfaces
{
    public interface IProvinciasUnitOfWork
    {
        Task<ActionResponse<Provincia>> GetAsync(int id);


        ///Lista de pais que venga con los provincias
        Task<ActionResponse<IEnumerable<Provincia>>> GetAsync();
    }
}
