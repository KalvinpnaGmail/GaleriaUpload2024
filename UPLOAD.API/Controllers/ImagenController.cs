using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using UPLOAD.API.Data;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    public class ImagenController : Controller
    {
        [ApiController]
        [Route("[controller]")]
        public class ImageController : Controller
        {

            private readonly string _usuario;
            private readonly string _pass;
            private readonly string _llave;
            private DataContext _contexto;


            public ImageController(DataContext contexto, IConfiguration configuration)
            {
                _usuario = configuration["Cloud:usuario"]!;
                _pass = configuration["Cloud:pass"]!;
                _llave = configuration["Cloud:llave"]!;
                _contexto = contexto;
            }

            [HttpGet]
            public async Task<IReadOnlyCollection<Image>> GetAll()
            {
                return await _contexto.Images.ToArrayAsync(); ;
            }


            [HttpPost]
            public async Task<Image> Create(ImageRequet request)
            {
                var image = new Image() { Name = request.Name };
                //Cloudinary
                image.URL = await Upload(request.Base64);

                await _contexto.Images.AddAsync(image);
                await _contexto.SaveChangesAsync();

                return image;
            }

            private async Task<string> Upload(string base64)
            {


                // Es esta parte deben poner sus credenciales de Cloudinary: username, api_key, api_secret
                var cloudinary = new Cloudinary(new Account(_usuario, _pass, _llave));


                cloudinary.Api.Secure = true;
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(Guid.NewGuid().ToString(), new MemoryStream(Convert.FromBase64String(base64))),

                    Type = "upload",
                    Tags = "NrodeOss"
                };
                var respuesta = await cloudinary.UploadAsync(uploadParams);
                return respuesta.SecureUrl.AbsoluteUri;
            }






        }
    }
}
