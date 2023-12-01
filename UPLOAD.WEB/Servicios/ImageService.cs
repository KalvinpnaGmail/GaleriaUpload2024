using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;


namespace UPLOAD.WEB.Services
{
    public class ImageService : IImageService
    {

        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7207/");
        }


        public async Task<IReadOnlyCollection<Image>> GetAllImages()
        {
            return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<Image>>("/Image");
        }
       

        //public async Task<Image> UploadImage(ImagenDTO request)
        //{
        //    return await _httpClient.PostAsJsonAsync<Image>("/Image", request);
        //}


        public async Task<Image> UploadImage(ImagenDTO request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/Image", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Image>(responseContent);
            }
            else
            {
                // Manejar error en la respuesta (por ejemplo, lanzar una excepción)
                throw new Exception($"Error al cargar la imagen. Código de estado: {response.StatusCode}");
            }
        }

       

      
    }
}
