using Microsoft.AspNetCore.Components;
using UPLOAD.WEB.Servicios;

namespace UPLOAD.WEB.Pages.Autenticacion
{
    public partial class Logout
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoginService.LogoutAsync();
            NavigationManager.NavigateTo("/");
        }

    }
}