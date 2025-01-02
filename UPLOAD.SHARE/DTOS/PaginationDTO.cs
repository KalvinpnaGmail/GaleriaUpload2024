namespace UPLOAD.SHARE.DTOS
{
    public class PaginationDTO
    {
        //este en realidad se usa para pasar un parametro
        public int Id { get; set; }

        //asumimos que si no nos dicen nada vamos a la pagina 1
        public int Page { get; set; } = 1;

        //nro de registro por pagina asumimos por defecto 10
        public int RecordsNumber { get; set; } = 10;

        public string? Filter { get; set; }

        public string? CategoryFilter { get; set; }
    }
}