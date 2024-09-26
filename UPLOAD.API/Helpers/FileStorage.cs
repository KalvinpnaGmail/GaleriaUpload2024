
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace UPLOAD.API.Helpers
{
    public class FileStorage : IFileStorage
    {

        private readonly string _connectionString;

        public FileStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage")!;
            ;
        }
        public async Task RemoveFileAsync(string path, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(path);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();  ///usar esto por si la foto no exsite asi no contralo
        }

        public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);///crea la carpeta
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);///acceso publico sin token
            var fileName = $"{Guid.NewGuid()}{extention}";//se arma nombre de archivo unico para cada foto con el Guid n con letras y guiiones
            var blob = client.GetBlobClient(fileName);//nueva instancia con el nombre del archivo

            using (var ms = new MemoryStream(content))   
            {
                await blob.UploadAsync(ms);
            }

            return blob.Uri.ToString();  ///nos devuelve la ruta donde almacenamos la foto
        }
    }


}
