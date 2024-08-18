using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Service
{
    public interface IClinicaService
    {
        //Task<Clinica[]> GetClinicasAsync();
        //Task<ActionResponse<IEnumerable<Clinica[]>>> GetClinicasAsync();
        Task<ActionResponse<IEnumerable<Clinica>>> GetClinicasAsync();
    }
}
