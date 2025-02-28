using System.Text;
using System.Text.Json;
using UPLOAD.SHARE.Entities;

///1/primero van atributos privados
///2-constructores
// 3-prop publicas
// 4-Metodos publicas
///5-Metods privados

namespace UPLOAD.WEB.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;

        //json es stadandar de java y c# Mayusculas para ahorrame los json ignore porque
        //espero mayusculas
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            //estandar de java empieza minuscula c# en mayusculas..para errarme json ignore

            PropertyNameCaseInsensitive = true,
        };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //httpcliente en el programs en la web lo inyectamos las api
        }

        public async Task<HttpResponseWrapper<object>> GetAsync(string url)
        {
            var responseHTTP = await _httpClient.GetAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            //si funciono httpClient
            if (responseHttp.IsSuccessStatusCode)

            {
                //vamos a leer la respuesta ya que no hay error
                var response = await UnserializeAnswer<T>(responseHttp, _jsonDefaultOptions);
                ///devuelva la respueta de T, le digo que no hay error, y  responseHttp
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }
            //
            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        ///Este post no devuelve nada
        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var mesageJSON = JsonSerializer.Serialize(model);
            //Lo codifico---utf8
            var messageContet = new StringContent(mesageJSON, Encoding.UTF8, "application/json");
            //HAcemos el post
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            ///y luego mando la repusta ya que no devuelvo el objeto

            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TActionResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TActionResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            ///todo me lo devuelve como string asi que lo tengo que convertir a un objeto si es una imagen no la puedo leer como String sino strings
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions)!;
        }

        public async Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url)
        {
            var responseHttp = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp); ;
        }

        public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TActionResponse>(responseHttp, _jsonDefaultOptions);
                //devuelve (response:como quedo objeeto modificado, y responshttp:la respuesta pra hacer algo con ella
                return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<TActionResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<Clinica[]> GetClinicasAsync()
        {
            // Llamar al método GetAsync<T> con la URL de la API de clínicas
            var response = await GetAsync<Clinica[]>("api/clinicas/DevuelveClinicas");

            if (!response.Error)
            {
                return response.Response!;
            }

            // Manejar el caso de error (puedes lanzar una excepción o devolver un arreglo vacío)
            return Array.Empty<Clinica>();
        }
    }
}