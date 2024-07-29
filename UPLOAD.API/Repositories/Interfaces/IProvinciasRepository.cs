using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Interfaces
{
    public interface IProvinciasRepository
    {
        /// tenga un pais traeme las provincias
        Task<ActionResponse<Provincia>> GetAsync(int id);


        ///Lista de pais que venga con los provincias
        Task<ActionResponse<IEnumerable<Provincia>>> GetAsync();
    }
}
