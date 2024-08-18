using UPLOAD.SHARE.DTOS;

namespace UPLOAD.API.Helpers
{
    public static class QueryableExtensions
    {
        //vamos a extender queryable metodos de extension UNA CONSULTA NO MATERIALIZA ES UN SELECT SIN EJECUTAR LA INSTRUCCION
        //deber ser estatica y crearle un metodo static

        //el metodo se llama Paginate la clave es this (hay una clase Iqueryable pero yo le voy a extenser el metodo PAginate basado
        ///en la pagination de PAGINATIONDTO
        public static IQueryable<T>Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            // skip:cuantos te vas a saltar(esa formula es generica siempre es asi)   take:cuantos vas a tomar    
            //
            return queryable
                .Skip((pagination.Page-1)*pagination.RecordsNumber)
                .Take(pagination.RecordsNumber);


        }
            
    }
}
