using Microsoft.AspNetCore.Mvc;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Service;

namespace UPLOAD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticasController : ControllerBase
    {
        private readonly IPracticaService _practicaService;

        public PracticasController(IPracticaService practicaService)
        {
            _practicaService = practicaService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<PracticaDto>>> GetAll()
        {
            var practicas = await _practicaService.GetPracticasAsync();
            return Ok(practicas);
        }
    }
}