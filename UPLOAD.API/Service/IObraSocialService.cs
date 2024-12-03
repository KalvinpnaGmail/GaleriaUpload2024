using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Service
{
    public interface IObraSocialService
    {
        Task<ActionResponse<IEnumerable<ObraSocial>>> GetObrasSocialesAsync();

        //me devuelve una lista NO PAGINADA UNA LISTA O OBJETOS
        Task<IEnumerable<ObraSocial>> GetComboAsync();
    }
}