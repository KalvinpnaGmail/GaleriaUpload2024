using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Interfaces
{
    public interface IGenericRepository<T>  where T : class
    {
        Task<ActionResponse<T>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<T>>> GetAsync();

        Task<ActionResponse<T>> AddAsync(T entity);

        Task<ActionResponse<T>> DeleteAsync(int id);

        Task<ActionResponse<T>> UpdateAsync(T entity);


        ///tengo qque hace un get para paginar <summary>
       
        Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination);
       // yo te paso el objeto pagination y me devuelve cuantas paginas me genera ese objeto
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

       
    }
}
