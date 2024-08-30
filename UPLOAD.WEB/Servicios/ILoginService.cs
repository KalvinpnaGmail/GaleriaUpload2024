namespace UPLOAD.WEB.Servicios
{
    public interface ILoginService
    {
        Task LoginAsync(string token);

        Task LogoutAsync();

    }
}
