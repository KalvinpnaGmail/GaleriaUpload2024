using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.Service;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/obraSociales")]
    public class ObraSocialesController : ControllerBase
    {
        private readonly IObraSocialService _obraSocialService;

        public ObraSocialesController(IObraSocialService obraSocialService)
        {
            _obraSocialService = obraSocialService;
        }

        [HttpGet("DevuelveObraSociales")]
        public async Task<ActionResult<IEnumerable<ObraSocial>>> GetAsync()
        {
            var response = await _obraSocialService.GetObrasSocialesAsync();

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
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _obraSocialService.GetComboAsync());
        }
    }
}