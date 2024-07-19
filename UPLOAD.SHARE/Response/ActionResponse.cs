namespace UPLOAD.SHARE.Response
{

    public class ActionResponse<T>
    {
        //si la accion que ejecuete es exito o no
        public bool WasSuccess { get; set; }
        //mensaje de error y ponerner ? a String?  no siempre hay mensaje por eso es OPCIONAL nulo
        public string? Message { get; set; }
        public T? Result { get; set; }

    }
}
