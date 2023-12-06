namespace UPLOAD.WEB.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url);
     

        //sin que me devuelva nadie
        Task<HttpResponseWrapper<object>> Post<T>(string url, T model);

        //trespondes me devuelve el body
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model);

        ///delet no devuelve respueta
        Task<HttpResponseWrapper<object>> Delete(string url);
        ///delet no devuelve respuesta
        Task<HttpResponseWrapper<object>> Put<T>(string url, T model);
        //put que devuelve respuesta por sobrecarga
        Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T model);




    }
}
