using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.Service;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiAclerController : ControllerBase
    {
        private readonly IApiServiceAcler _apiServiceAcler;

        public ApiAclerController(IApiServiceAcler apiServiceAcler)
        {
            _apiServiceAcler = apiServiceAcler;
        }

        [HttpGet("GetAllPracticas")]
        public async Task<ActionResult<List<PracticaDto>>> GetAll()
        {
            var practicas = await _apiServiceAcler.GetPracticasAsync();
            return Ok(practicas);
        }

        [HttpGet("DevuelveObraSociales")]
        public async Task<ActionResult<IEnumerable<ObraSocial>>> GetObrasSocialesAsync()
        {
            var response = await _apiServiceAcler.GetObrasSocialesAsync();

            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboObrasSocialesAsync()
        {
            return Ok(await _apiServiceAcler.GetObraSocialesComboAsync());
        }

        [HttpGet("DevuelveClinicas")]
        public async Task<ActionResult<IEnumerable<Clinica>>> GetAsync()
        {
            var response = await _apiServiceAcler.GetClinicasAsync();

            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpGet("comboClinicas")]
        public async Task<IActionResult> GetClinicaComboAsync()
        {
            return Ok(await _apiServiceAcler.GetClinicaComboAsync());
        }

        [HttpGet("obtener-valor-practica")]
        public async Task<IActionResult> ObtenerValorPractica([FromQuery] string codigoPractica, [FromQuery] string codOS, [FromQuery] string nroConv)
        {
            try
            {
                var result = await _apiServiceAcler.ObtenerValorPracticaAsync(codigoPractica, codOS, nroConv);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}