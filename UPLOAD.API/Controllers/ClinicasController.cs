using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

using UPLOAD.SHARE.Entities;
using static UPLOAD.API.Helpers.AclerHelper;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("/api/clinicas")]
    public class ClinicasController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string usuario;
        private readonly string pass;
        private readonly string url;

        public ClinicasController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            usuario= configuration["AclerApi:usuario"]!;
            pass= configuration["AclerApi:pass"]!;
            url = configuration["AclerApi:url"]!;
        }


        [HttpGet("DevuelveClinicas")]
        public async Task<Clinica[]> GetAsync()
        {
          


            string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{usuario}:{pass}"));

            // Agregar encabezado de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);
            try
            {
                // Realizar la solicitud GET (o la solicitud que necesites)
                HttpResponseMessage response = await _httpClient.GetAsync(url+"json/Clinicas.php");

                // Verificar la respuesta
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Acler acler = new Acler();
                    var json = acler.ProcesarJsonInvalido2(responseData);
                    var respuesta = JsonSerializer.Deserialize<List<Clinica>>(json).ToArray();
                    return respuesta;

                }
                else
                {
                    return Array.Empty<Clinica>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }



    }
}
