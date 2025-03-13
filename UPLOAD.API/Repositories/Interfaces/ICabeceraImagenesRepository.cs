using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Interfaces
{
    public interface ICabeceraImagenesRepository
    {
        Task<ActionResponse<IEnumerable<CabeceraImage>>> GetAsync();

        Task<ActionResponse<CabeceraImage>> GetAsync(int id);
    }
}