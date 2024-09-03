using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Service
{
    public interface IClinicaService
    {
        //Task<Clinica[]> GetClinicasAsync();
        //Task<ActionResponse<IEnumerable<Clinica[]>>> GetClinicasAsync();

        //ACA DEVUELDE LOS DAOTS E INFORMACIN ADICIONALL  ES UN CONTENERO DE UNA CLASE
        Task<ActionResponse<IEnumerable<Clinica>>> GetClinicasAsync();

        //me devuelve una lista NO PAGINADA UNA LISTA O OBJETOS
        Task<IEnumerable<Clinica>> GetComboAsync();
    }
}
