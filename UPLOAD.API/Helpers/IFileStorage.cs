namespace UPLOAD.API.Helpers
{
    public interface IFileStorage
    {
        /// metodos para guardar en el storage  containerName= nombre de carpetas <summary>
        /// 
        
       
        Task<string> SaveFileAsync(byte[] content, string extention, string containerName);

        Task RemoveFileAsync(string path, string containerName);

        async Task<string> EditFileAsync(byte[] content, string extention, string containerName, string path)
        {
            if (path is not null)
            {
                await RemoveFileAsync(path, containerName);
            }

            return await SaveFileAsync(content, extention, containerName);
        }
    }

}

