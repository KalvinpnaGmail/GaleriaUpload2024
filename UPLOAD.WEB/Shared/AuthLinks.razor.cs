using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using UPLOAD.WEB.Pages.Autenticacion;

namespace UPLOAD.WEB.Shared
{
    public partial class AuthLinks
    {
        private string? photoUser;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            var claims = authenticationState.User.Claims.ToList();
            var photoClaim = claims.FirstOrDefault(x => x.Type == "Photo");
            if (photoClaim is not null)
            {
                photoUser = photoClaim.Value;
            }
        }

        private void ShowModalLogOut()
        {
            var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
            DialogService.Show<Logout>("Cerrar su Sesiòn", closeOnEscapeKey);
        }
    }
}