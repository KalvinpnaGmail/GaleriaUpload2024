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

        private readonly string _usuario;
        private readonly string _pass;
        private readonly string _llave;
        private readonly DataContext _contexto;


        public ImageController(DataContext contexto, IConfiguration configuration)
        {
            _usuario = configuration["Cloud:usuario"]!;
            _pass = configuration["Cloud:pass"]!;
            _llave = configuration["Cloud:llave"]!;
            _contexto = contexto;
        }

       

        [HttpGet("DevuelveImagenes")]
        public async Task<IActionResult> Get()
        {
            //no me logues todas las operaciones SI SOLO ES LECTURA

            return Ok(await _contexto.Images.AsNoTracking().ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var imagen = await _contexto.Images.FirstOrDefaultAsync(x => x.ImageId == id);
            if (imagen is null)
            {
                return NotFound();
            }

            return Ok(imagen);
        }

        //[HttpPost]
        //public async Task<IActionResult> PostAsync(Image image, ImagenDTO request)
        //{
        //    _contexto.Add(image);
        //    await _contexto.SaveChangesAsync();
        //    return Ok(image);
        //}



        [HttpPost]
        public async Task<IActionResult> Create(ImagenDTO request)
        {
            var obraSocial = "Sancor";
            //var periodo = DateTime.Now;
            var image = new Image
            {
                Name = request.Name,             //Cloudinary
                ObraSocial = obraSocial,
                //Periodo = periodo,
                Url = await Upload(request.Base64, obraSocial)
            };
           

            await _contexto.Images.AddAsync(image);
            await _contexto.SaveChangesAsync();
          


           


            return Ok(image);
        }


       
        //[HttpPost]
        //public async Task<Image> Create(ImagenDTO request)
        //{
        //    string type = "upload";
        //    string tags = "NroOs";
        //    string folder = "Os";

        //    // Llamar al método Upload y obtener la URL
        //    string imageUrl = await Upload(request, type, tags, folder);

        //    if (imageUrl == null)
        //    {
        //        // Manejo de error si la carga falla
        //        throw new Exception("Error al subir la imagen.");
        //    }

        //    var image = new Image
        //    {
        //        Name = request.Name,
        //        Url = imageUrl
        //    };

        //    await _contexto.Images.AddAsync(image);
        //    await _contexto.SaveChangesAsync();

        //    return image;
        //}


        [HttpPut]
        public async Task<IActionResult> Put(Image image)
        {
            _contexto.Update(image);
            await _contexto.SaveChangesAsync();
            return Ok(image);
            //NoContent es igual al ok pero no me interesa que devuelve
            //return NoContent();
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



        //private async Task<string> Upload(ImagenDTO request, string type, string tags, string folder)
        //{
        //    try
        //    {
        //        if (request == null)
        //        {
        //            throw new ArgumentException("El DTO de la imagen no puede ser nulo.");
        //        }

        //        if(string.IsNullOrEmpty(request.Base64))
        //{
        //            throw new ArgumentException("El string base64 en el DTO de la imagen no puede estar vacío.");
        //        }

        //        // Convertir el string base64 a bytes
        //        byte[] fileBytes = await GetBytesFromBase64String(request.Base64);
        //        {
        //            throw new FormatException("Formato base64 inválido en el DTO de la imagen.");
        //        }

        //        using (var stream = new MemoryStream(fileBytes))
        //        {
        //            var cloudinary = new Cloudinary(new Account(_usuario, _pass, _llave));
        //            cloudinary.Api.Secure = true;

        //            var uploadParams = new ImageUploadParams()
        //            {
        //                File = new FileDescription(Guid.NewGuid().ToString(), stream),
        //                Type = type,         // Usando el parámetro type pasado al método
        //                Tags = tags,         // Usando el parámetro tags pasado al método
        //                Folder = folder      // Usando el parámetro folder pasado al método
        //            };

        //            var respuesta = await cloudinary.UploadAsync(uploadParams);

        //            if (respuesta.Error != null)
        //            {
        //                throw new Exception($"Error al subir la imagen: {respuesta.Error.Message}");
        //            }

        //            return respuesta.SecureUrl.AbsoluteUri;
        //        }
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        Console.WriteLine($"Argumento inválido: {ex.Message}");
        //        return null!; // o manejo específico según tu aplicación
        //    }
        //    catch (FormatException ex)
        //    {
        //        Console.WriteLine($"Formato base64 inválido: {ex.Message}");
        //        return null!; // o manejo específico según tu aplicación
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error general: {ex.Message}");
        //        return null!; // o manejo específico según tu aplicación
        //    }
        //}

        //private async Task<byte[]> GetBytesFromBase64String(string base64String)
        //{
        //    // Separar el encabezado del string base64 si lo tiene
        //    var base64Parts = base64String.Split(',');

        //    if (base64Parts.Length > 1)
        //    {
        //        base64String = base64Parts[1];
        //    }

        //    // Convertir el string base64 a bytes
        //    return Convert.FromBase64String(base64String);
        //}



        private async Task<string> Upload(string base64, string obraSocial)
        {


            // Es esta parte deben poner sus credenciales de Cloudinary: username, api_key, api_secret
            var cloudinary = new Cloudinary(new Account(_usuario, _pass, _llave));



            cloudinary.Api.Secure = true;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), new MemoryStream(Convert.FromBase64String(base64))),

                Type = "upload",
                //Tags = $"{obraSocial},{periodo:yyyyMMdd}",
                Tags =obraSocial,
                Folder = "Os"
            
            };
            var respuesta = await cloudinary.UploadAsync(uploadParams);
            return respuesta.SecureUrl.AbsoluteUri;
        }

        private async Task<string> UpdateCloudinary(Image image, string base64)
        {
            try
            {
                // Validar entrada
                if (image == null)
                {
                    throw new ArgumentException("La entidad Image no puede ser nula.");
                }

                if (string.IsNullOrEmpty(image.Url))
                {
                    throw new ArgumentException("El URL del recurso no puede estar vacío.");
                }

                if (string.IsNullOrEmpty(base64))
                {
                    throw new ArgumentException("El string base64 no puede estar vacío.");
                }

                // Es esta parte deben poner sus credenciales de Cloudinary: username, api_key, api_secret
                var cloudinary = new Cloudinary(new Account(_usuario, _pass, _llave));
                cloudinary.Api.Secure = true;

                var uploadParams = new ImageUploadParams()
                {
                    PublicId = image.Url,
                    File = new FileDescription(Guid.NewGuid().ToString(), new MemoryStream(Convert.FromBase64String(base64))),
                    Type = "upload",
                    Tags = image.ObraSocial,
                    Folder = image.ObtenerPeriodoFormateado(),
                    Overwrite = true // Permite sobrescribir el archivo existente
                };

                var respuesta = await cloudinary.UploadAsync(uploadParams);

                if (respuesta.Error != null)
                {
                    throw new Exception($"Error al actualizar la imagen: {respuesta.Error.Message}");
                }

                return respuesta.SecureUrl.AbsoluteUri;
            }
            catch (ArgumentException ex)
            {
                // Manejo de errores específicos de argumentos
                Console.WriteLine($"Argumento inválido: {ex.Message}");
                return null!; // o una URL de error, o un valor por defecto
            }
            catch (FormatException ex)
            {
                // Manejo de errores de formato (ej. base64 inválido)
                Console.WriteLine($"Formato base64 inválido: {ex.Message}");
                return null!; // o una URL de error, o un valor por defecto
            }
            catch (Exception ex)
            {
                // Manejo de errores genéricos
                Console.WriteLine($"Error general: {ex.Message}");
                return null!; // o una URL de error, o un valor por defecto
            }
        }
    }



  


}

    








      






