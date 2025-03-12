using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class CabeceraImageController : ControllerBase
    {
        private readonly DataContext _contexto;

        public CabeceraImageController(DataContext contexto)
        {
            _contexto = contexto;
        }

        // Crear una nueva cabecera de imagen
        [HttpPost]
        public async Task<IActionResult> PostAsync(CabeceraImage cabecera)
        {
            if (cabecera == null)
            {
                return BadRequest("Los datos de la cabecera son incorrectos.");
            }

            await _contexto.CabeceraImages.AddAsync(cabecera);
            await _contexto.SaveChangesAsync();

            return Ok(cabecera);
        }

        // Listar todas las cabeceras de imágenes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cabeceras = await _contexto.CabeceraImages
                .Include(c => c.Images) // Incluimos las imágenes relacionadas
                .AsNoTracking()
                .ToListAsync();

            return Ok(cabeceras);
        }

        // Listar las imágenes asociadas a una cabecera por ID
        [HttpGet("{id}/imagenes")]
        public async Task<IActionResult> GetImagesByCabeceraId(int id)
        {
            var cabecera = await _contexto.CabeceraImages
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cabecera == null)
            {
                return NotFound("Cabecera no encontrada.");
            }

            return Ok(cabecera.Images);
        }
    }
}