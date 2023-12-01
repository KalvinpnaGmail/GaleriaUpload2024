using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("/api/clinicas")]
    public class ClinicasController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ClinicasController()
        {
            _httpClient = new HttpClient();
        }


        [HttpGet("DevuelveClinicas")]
        public async Task<Clinica[]> GetAsync()
        {
            //var URL = "http://192.168.1.19/json/Clinicas.php";
            var URL = "http://181.98.176.128/json/Clinicas.php";


            var response = await _httpClient.GetAsync(URL);   //solicitud

            var content = await response.Content.ReadAsStringAsync();  //leer contenido

            Acler acler = new Acler();

            var json = acler.ProcesarJsonInvalido2(content);


            var respuesta = JsonSerializer.Deserialize<List<Clinica>>(json).ToArray();

            return respuesta;
        }



    }
}
