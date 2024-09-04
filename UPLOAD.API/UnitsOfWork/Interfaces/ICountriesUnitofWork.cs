using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Interfaces
{
    public interface ICountriesUnitofWork
    {

        Task<ActionResponse<Country>> GetAsync(int id);


        ///Lista de pais que venga con los provincias
        Task<ActionResponse<IEnumerable<Country>>> GetAsync();

        Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);
        Task<IEnumerable<Country>> GetComboAsync();

    }
}
