
using System.Text.Json;
using System.Text;

namespace UPLOAD.WEB.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;


        //json es stadandar de java y c# Mayusculas para ahorrame los json ignore porque 
        //espero mayusculas
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //httpcliente en el programs en la web lo inyectamos las api
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)

            {
                //vamos a leer la respuesta ya que no hay error
                var response = await UnserializeAnswer<T>(responseHttp, _jsonDefaultOptions);
                ///devuelva la respueta de T, le digo que no hay error, y  responseHttp
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }


        ///Este post no devuelve nada
        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            //al modelo string lo convertimos a un Objeto
            var mesageJSON = JsonSerializer.Serialize(model);
            //Lo codifico---utf8
            var messageContet = new StringContent(mesageJSON, Encoding.UTF8, "application/json");
            //HAcemos el post
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            ///y luego mando la repusta ya que no devuelvo el objeto

            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            ///todo me lo devuelve como string asi que lo tengo que convertir a un objeto si es una imagen no la puedo leer como String sino strings
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions)!;
        }

        public async Task<HttpResponseWrapper<object>> DeleteAsync(string url)
        {
            var responseHTTP = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);


        }

        public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);

        }

        public async  Task<HttpResponseWrapper<TResponse>> PutAsync<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);

        }
    }

}
