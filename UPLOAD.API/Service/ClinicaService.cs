using System.Text;
using System.Text.Json;
using UPLOAD.API.Service;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;
using static UPLOAD.API.Helpers.AclerHelper;

public class ClinicaService : IClinicaService
{
     
        private readonly HttpClient _httpClient;
        private readonly string _usuario;
        private readonly string _pass;
        private readonly string _url;

        public ClinicaService(IConfiguration configuration)
        {
            _httpClient = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true });
            _usuario = configuration["AclerApi:usuario"]!;
            _pass = configuration["AclerApi:pass"]!;
            _url = configuration["AclerApi:url"]!;
        }

    public async Task<ActionResponse<IEnumerable<Clinica>>> GetClinicasAsync()
    {
        try
        {
            // Configura la autenticación básica
            string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

            // Realiza la solicitud HTTP GET
            HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=clinicas");

            if (response.IsSuccessStatusCode)
            {
                // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                string responseData = await response.Content.ReadAsStringAsync();
                Acler acler = new Acler();
                var json = acler.ProcesarJsonInvalido2(responseData);
                var clinicas = JsonSerializer.Deserialize<List<Clinica>>(json);

                // Retorna la lista de clínicas envuelta en un ActionResponse
                return new ActionResponse<IEnumerable<Clinica>>
                {
                    WasSuccess = true,
                    Result = clinicas
                };
            }
            else
            {
                // Si la solicitud no es exitosa, retorna un ActionResponse con un mensaje de error
                return new ActionResponse<IEnumerable<Clinica>>
                {
                    WasSuccess = false,
                    Result = null,
                    Message = "Error al obtener las clínicas"
                };
            }
        }
        catch (Exception ex)
        {
            // Si ocurre una excepción, retorna un ActionResponse con los detalles del error
            return new ActionResponse<IEnumerable<Clinica>>
            {
                WasSuccess = false,
                Result = null,
                Message = $"Error: {ex.Message}"
            };
        }
    }


    public async Task<IEnumerable<Clinica>> GetComboAsync()
    {
        try
        {
            // Configura la autenticación básica
            string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

            // Realiza la solicitud HTTP GET
            HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=clinicas");

            if (response.IsSuccessStatusCode)
            {
                // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                string responseData = await response.Content.ReadAsStringAsync();
                Acler acler = new Acler();
                var json = acler.ProcesarJsonInvalido2(responseData);
                var clinicas = JsonSerializer.Deserialize<List<Clinica>>(json);

                // Retorna la lista de clínicas
                return clinicas;
            }

            // Manejo de errores: puedes retornar una lista vacía si prefieres no lanzar una excepción
            return new List<Clinica>();
        }
        catch (Exception ex)
        {
            // Manejo de excepciones
            // Dependiendo del uso, puedes registrar el error y/o lanzar una excepción
            throw new Exception($"Error al obtener datos: {ex.Message}", ex);
        }
    }




}



