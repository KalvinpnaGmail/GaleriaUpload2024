using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.Service;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ApiService _apiService;

        public ApiController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("obtener-valor-practica")]
        public async Task<IActionResult> ObtenerValorPractica([FromQuery] string codigoPractica, [FromQuery] string codOS, [FromQuery] string nroConv)
        {
            try
            {
                var result = await _apiService.ObtenerValorPracticaAsync(codigoPractica, codOS, nroConv);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}