using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Service
{
    public interface IApiServiceAcler
    {
        Task<Dictionary<string, decimal>> ObtenerValorPracticaAsync(string codigoPractica, string codOS, string nroConv);

        //Clinicas
        Task<ActionResponse<IEnumerable<Clinica>>> GetClinicasAsync();

        //me devuelve una lista NO PAGINADA UNA LISTA O OBJETOS
        Task<IEnumerable<Clinica>> GetClinicaComboAsync();

        ///Obra Sociales
        Task<ActionResponse<IEnumerable<ObraSocial>>> GetObrasSocialesAsync();

        //me devuelve una lista NO PAGINADA UNA LISTA O OBJETOS
        Task<IEnumerable<ObraSocial>> GetObraSocialesComboAsync();

        ////Practicas nomenclador nacional
        Task<List<PracticaDto>> GetPracticasAsync();
    }
}