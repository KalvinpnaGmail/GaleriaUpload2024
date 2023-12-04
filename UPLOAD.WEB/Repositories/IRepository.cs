namespace UPLOAD.WEB.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url);
     

        //sin que me devuelva nadie
        Task<HttpResponseWrapper<object>> Post<T>(string url, T model);

        //trespondes me devuelve el body
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model);

    }
}
