using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{

    [ApiController]
    [Route("api/imagenes")]

    public class ImageController : ControllerBase
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

        [HttpGet("DevuelveImagenes")]
        public async Task<IReadOnlyCollection<Image>> GetAll()
        {
            return await _contexto.Images.ToArrayAsync(); ;
        }


        [HttpPost]
        public async Task<Image> Create(ImagenDTO request)
        {
            var image = new Image() { Name = request.Name };
            //Cloudinary
            image.Url = await Upload(request.Base64);

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
                Tags = "NrodeOss",
                Folder="Os"
            };
            var respuesta = await cloudinary.UploadAsync(uploadParams);
            return respuesta.SecureUrl.AbsoluteUri;
        }






    }
}
