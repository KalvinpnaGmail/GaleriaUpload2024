using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace UPLOAD.API.Helpers
{
    public class FileStorage : IFileStorage
    {
        private readonly Cloudinary _cloudinary;

        public FileStorage(IConfiguration configuration)
        {
            var cloudName = configuration["Cloud:usuario"];
            var apiKey = configuration["Cloud:pass"];
            var apiSecret = configuration["Cloud:llave"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
        {
            using (var ms = new MemoryStream(content))
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(Guid.NewGuid().ToString(), ms),
                    Folder = containerName, // Nombre de la carpeta en Cloudinary
                    UseFilename = true,
                    UniqueFilename = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString(); // Devuelve la URL segura
            }
        }

        public async Task RemoveFileAsync(string path, string containerName)
        {
            var publicId = Path.GetFileNameWithoutExtension(path); // Obtiene el ID del archivo
            var deleteParams = new DeletionParams(publicId);

            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}