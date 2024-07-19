using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _contex;
        private readonly DbSet<T> _entity;


        public GenericRepository(DataContext contex)
        {
            _contex = contex;
            _entity = _contex.Set<T>();
        }
        public async Task<ActionResponse<T>> AddAsync(T entity)
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

        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = exception.Message,
            };
        }

        public async Task<ActionResponse<T>> DeleteAsync(int id)
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

        public async Task<ActionResponse<T>> GetAsync(int id)
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

        public async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true,
                Result= await _entity.ToListAsync(),
            };
        }

        public async Task<ActionResponse<T>> UpdateAsync(T entity)
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

    }
}
