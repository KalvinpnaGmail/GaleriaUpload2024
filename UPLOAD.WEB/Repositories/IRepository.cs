namespace UPLOAD.WEB.Repositories
{
    public interface IRepository
    {
        //
        /// es un servicio de lo proviendo de lo que se esta haciendo en backedn
        ///
        ///
        ///
        ///
        ///

        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);

        Task<HttpResponseWrapper<T>> GetAsync<T>(string url, CancellationToken cancellationToken);  // Nueva sobrecarga

        //sin que me devuelva nadie
        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);

        //trespondes lo que me devuelve en el body
        Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);

        Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url);

        ///delet no devuelve respueta

        ///put no devuelve respuesta
        Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model);

        //put que devuelve respuesta por sobrecarga
        Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model);

        Task<HttpResponseWrapper<object>> GetAsync(string url);
    }
}