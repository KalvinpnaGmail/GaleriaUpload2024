using System.Net.Http.Json;
using UPLOAD.SHARE.DTOS;

namespace UPLOAD.WEB.Servicios
{
    public class PracticaApiService
    {
        private readonly HttpClient _httpClient;

        public PracticaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PracticaDto>> GetPracticasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PracticaDto>>("api/practicas/GetAll");
        }
    }
}