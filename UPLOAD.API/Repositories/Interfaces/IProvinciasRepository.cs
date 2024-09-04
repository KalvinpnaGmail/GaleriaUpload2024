using System.Threading.Tasks;
using UPLOAD.SHARE.DTOS;
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

        //no puedo paginar todo los provincias le debo mandtar que pais es porq eso no uso el generico
        Task<ActionResponse<IEnumerable<Provincia>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<IEnumerable<Provincia>> GetComboAsync(int countryId);

    }
}
