using System.Text;
using System.Text.Json;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;
using static UPLOAD.API.Helpers.AclerHelper;

namespace UPLOAD.API.Service
{
    public class ObraSocialService : IObraSocialService
    {
        private readonly HttpClient _httpClient;
        private readonly string _usuario;
        private readonly string _pass;
        private readonly string _url;

        public ObraSocialService(IConfiguration configuration)
        {
            _httpClient = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true });
            _usuario = configuration["AclerApi:usuario"]!;
            _pass = configuration["AclerApi:pass"]!;
            _url = configuration["AclerApi:url"]!;
        }

        public async Task<IEnumerable<ObraSocial>> GetComboAsync()
        {
            try
            {
                // Configura la autenticación básica
                string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

                // Realiza la solicitud HTTP GET

                HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=os_vigentes_gral");

                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                    string responseData = await response.Content.ReadAsStringAsync();
                    Acler acler = new Acler();
                    var json = acler.ProcesarJsonInvalido2(responseData);
                    var obraSociales = JsonSerializer.Deserialize<List<ObraSocial>>(json);

                    // Retorna la lista de clínicas
                    return obraSociales;
                }

                // Manejo de errores: puedes retornar una lista vacía si prefieres no lanzar una excepción
                return new List<ObraSocial>();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Dependiendo del uso, puedes registrar el error y/o lanzar una excepción
                throw new Exception($"Error al obtener datos: {ex.Message}", ex);
            }
        }

        public async Task<ActionResponse<IEnumerable<ObraSocial>>> GetObrasSocialesAsync()
        {
            try
            {
                // Configura la autenticación básica
                string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

                // Realiza la solicitud HTTP GET

                HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=os_vigentes_gral");
                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                    string responseData = await response.Content.ReadAsStringAsync();
                    Acler acler = new Acler();
                    var json = acler.ProcesarJsonInvalido2(responseData);
                    var obraSociales = JsonSerializer.Deserialize<List<ObraSocial>>(json);

                    // Retorna la lista de clínicas envuelta en un ActionResponse
                    return new ActionResponse<IEnumerable<ObraSocial>>
                    {
                        WasSuccess = true,
                        Result = obraSociales
                    };
                }
                else
                {
                    // Si la solicitud no es exitosa, retorna un ActionResponse con un mensaje de error
                    return new ActionResponse<IEnumerable<ObraSocial>>
                    {
                        WasSuccess = false,
                        Result = null,
                        Message = "Error al obtener las obras Sociales"
                    };
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retorna un ActionResponse con los detalles del error
                return new ActionResponse<IEnumerable<ObraSocial>>
                {
                    WasSuccess = false,
                    Result = null,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}