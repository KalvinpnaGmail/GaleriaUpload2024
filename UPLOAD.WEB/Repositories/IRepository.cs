namespace UPLOAD.WEB.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);
     

        //sin que me devuelva nadie
        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);

        //trespondes lo que me devuelve en el body
        Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model);



        ///delet no devuelve respueta
        Task<HttpResponseWrapper<object>> DeleteAsync(string url);
        ///put no devuelve respuesta
        Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model);
        //put que devuelve respuesta por sobrecarga
        Task<HttpResponseWrapper<TResponse>> PutAsync<T, TResponse>(string url, T model);






    }
}
