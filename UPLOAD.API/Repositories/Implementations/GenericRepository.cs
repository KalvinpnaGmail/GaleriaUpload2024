using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Helpers;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Implementations
{
    /// <summary>
    /// virtual : son metodos que se pueden sobreescribir el get generegico no me sirve porque solo me trae nos nombrers de los paises
    /// necetiso un get mas especialiaza un pais con los provinicas
    /// </summary>
    
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _contex;
        private readonly DbSet<T> _entity;


        public GenericRepository(DataContext contex)
        {
            _contex = contex;
            _entity = _contex.Set<T>();
        }
        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
        {
           _contex.Add(entity);
            try
            {
                ///que grabe
                await _contex.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            catch (Exception exception)
            {

                return ExceptionActionResponse(exception);
            }
          
        }

        private  ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = exception.Message,
            };
        }

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row== null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "Registro no encontrado",

                };
            }
            try
            {
                _entity.Remove(row);
                await _contex.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                };
            }
            catch 
            {

                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message="No se pudo borrar, posee relaciones con otros Registros"
                };
            }
        }

        public virtual async Task<ActionResponse<T>> GetAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row == null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "Registro no encontrado",

                };
            }
            return new ActionResponse<T>
             {
                   WasSuccess = true,
                   Result=row,
             };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true,
                Result= await _entity.ToListAsync(),
            };
        }

        public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
        {
            _contex.Update(entity);
            try
            {
                ///que grabe
                await _contex.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            catch (Exception exception)
            {

                return ExceptionActionResponse(exception);
            }
        }

        private ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = "Ya Existe el Registro no se pudo grabar"
            };
        }



        ///para la paginacion
        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();

            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true,
                Result = await queryable
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();
            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }


    }
}
