using UPLOAD.SHARE.DTOS;

namespace UPLOAD.SHARE.Service
{
    public interface IPracticaService
    {
        Task<List<PracticaDto>> GetPracticasAsync();
    }
}