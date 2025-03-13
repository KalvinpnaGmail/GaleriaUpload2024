using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Interfaces
{
    public interface ICabeceraImagenesUnitOfWork
    {
        Task<ActionResponse<IEnumerable<CabeceraImage>>> GetAsync();

        Task<ActionResponse<CabeceraImage>> GetAsync(int id);
    }
}