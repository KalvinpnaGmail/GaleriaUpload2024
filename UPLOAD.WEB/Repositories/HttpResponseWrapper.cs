using System.Net;

namespace UPLOAD.WEB.Repositories
{
    //me devuelve las respuestas  200 500 600
    /// <summary>
    /// es un envoltorio de repuestas
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //T ES CLASE GENERICA
    public class HttpResponseWrapper<T>
    {
        //
        //
        //T? PORQUE PUEDE SER NULO NO TODAS LAS RESPUESTAS HTTP DAN RESPUESTAS
        //ERROR = SI HUBO ERROR TRUE O FALSE
        //HttpResponseMessage= OBJEETO LE MANDO EL ERROR DEL MENSAJE
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;  //
            Response = response;//
            HttpResponseMessage = httpResponseMessage;//
        }

        public bool Error { get; set; }

        public T? Response { get; set; }

        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode= HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            }
            else if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Tienes que logearte para hacer esta operación";
            }
            else if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esta operación";
            }

            return "Ha ocurrido un error inesperado";
        }
    }

}
