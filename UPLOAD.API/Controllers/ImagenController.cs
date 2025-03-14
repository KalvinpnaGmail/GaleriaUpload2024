using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/imagenes")]
    public class ImageController : ControllerBase
    {
        private readonly DataContext _contexto;
        private readonly string _usuario;
        private readonly string _pass;
        private readonly string _llave;

        public ImageController(DataContext contexto, IConfiguration configuration)
        {
            _usuario = configuration["Cloud:usuario"]!;
            _pass = configuration["Cloud:pass"]!;
            _llave = configuration["Cloud:llave"]!;
            _contexto = contexto;
        }

        // ✅ Incluir CabeceraImage al devolver imágenes
        [HttpGet("DevuelveImagenes")]
        public async Task<IActionResult> Get()
        {
            var resultado = await _contexto.Images
                .AsNoTracking()
                .GroupBy(i => new { i.ObraSocial, Periodo = i.ObtenerPeriodoFormateado() })
                .Select(g => new
                {
                    g.Key.ObraSocial,
                    g.Key.Periodo,
                    CantidadDocumentos = g.Count()
                })
                .ToListAsync();

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var imagen = await _contexto.Images
                .Include(i => i.CabeceraImage) // Incluir relación
                .FirstOrDefaultAsync(x => x.Id == id);

            if (imagen is null)
            {
                return NotFound();
            }

            return Ok(imagen);
        }

        // ✅ Crear CabeceraImage si no existe y asociar la imagen
        [HttpPost]
        public async Task<IActionResult> PostAsync(ImagenDTO request)
        {
            var obraSocial = "Sancor";
            var periodo = DateTime.Now; // Se usa la fecha actual como periodo

            // Buscar si ya existe una CabeceraImage con la misma ObraSocial y Periodo
            var cabecera = await _contexto.CabeceraImages
                .FirstOrDefaultAsync(c => c.ObraSocial == obraSocial && c.Periodo.Year == periodo.Year && c.Periodo.Month == periodo.Month);

            // Si no existe, crearla
            if (cabecera == null)
            {
                cabecera = new CabeceraImage
                {
                    ObraSocial = obraSocial,
                    Periodo = periodo,
                };
                await _contexto.CabeceraImages.AddAsync(cabecera);
                await _contexto.SaveChangesAsync();
            }

            var image = new Image
            {
                Name = request.Name,
                ObraSocial = obraSocial,
                Periodo = periodo,
                Url = await Upload(request.Base64, obraSocial),
                CabeceraImage = cabecera // Asignar la imagen a la cabecera
            };

            await _contexto.Images.AddAsync(image);
            await _contexto.SaveChangesAsync();

            return Ok(image);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Image image)
        {
            _contexto.Update(image);
            await _contexto.SaveChangesAsync();
            return Ok(image);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var image = await _contexto.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            _contexto.Remove(image);
            await _contexto.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Nuevo endpoint para obtener todas las CabeceraImage con sus imágenes
        [HttpGet("cabeceras")]
        public async Task<IActionResult> GetCabeceras()
        {
            var cabeceras = await _contexto.CabeceraImages
                .Include(c => c.Images) // Incluir la lista de imágenes asociadas
                .ToListAsync();

            return Ok(cabeceras);
        }

        private async Task<string> Upload(string base64, string obraSocial)
        {
            var cloudinary = new Cloudinary(new Account(_usuario, _pass, _llave));
            cloudinary.Api.Secure = true;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), new MemoryStream(Convert.FromBase64String(base64))),
                Type = "upload",
                Tags = obraSocial,
                Folder = "Os"
            };
            var respuesta = await cloudinary.UploadAsync(uploadParams);
            return respuesta.SecureUrl.AbsoluteUri;
        }
    }
}