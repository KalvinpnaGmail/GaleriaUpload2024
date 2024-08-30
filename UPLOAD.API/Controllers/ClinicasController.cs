using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.Service;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/clinicas")]
    public class ClinicasController : ControllerBase
    {
        private readonly IClinicaService _clinicaService;

        public ClinicasController(IClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        [HttpGet("DevuelveClinicas")]
        public async Task<ActionResult<IEnumerable<Clinica>>> GetAsync()
        {
            var response = await _clinicaService.GetClinicasAsync();

            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
    }

}