using CloudinaryDotNet;
using System.Net.Http.Headers;
using System.Text;

namespace UPLOAD.API.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _usuario;
        private readonly string _pass;
        private readonly string _url;

        public ApiService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _usuario = configuration["AclerApi:usuario"];
            _pass = configuration["AclerApi:pass"];
            _url = configuration["AclerApi:url"];
        }

        public async Task<string> ObtenerValorPracticaAsync(string codigoPractica, string codOS, string nroConv)
        {
            // Construcción de la URL completa con el archivo PHP
            var url = $"{_url}api_cta.php?action=obtenerValorPractica&codigoPractica={codigoPractica}&codOS={codOS}&nroConv={nroConv}";

            // Configuración de la autorización básica
            var byteArray = Encoding.ASCII.GetBytes($"{_usuario}:{_pass}");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Realizamos la solicitud GET
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Si la respuesta es exitosa, leemos el contenido
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Si la respuesta no es exitosa, lanzamos una excepción
                throw new Exception($"Error al obtener datos de la API externa: {response.ReasonPhrase}");
            }
        }
    }
}