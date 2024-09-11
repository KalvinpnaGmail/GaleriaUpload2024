using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Runtime.CompilerServices;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Documentos
{
    [Authorize(Roles = "Admin")]
    public partial class DocumentIndex
    {
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        public List<Image>? Imagenes { get; set; }



        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }



        private async Task LoadAsync()
        {

            var responseHppt = await repository.GetAsync<List<Image>>("/api/imagenes/DevuelveImagenes");
            if (responseHppt.Error)
            {
                var message = await responseHppt.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Imagenes = responseHppt.Response!;
        }
       
        private async Task DeleteAsync(Image image)
        {
            ///si fue editado saco un alerta
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmaciòn",
                Text = $" Desea Eliminar la Imagen: {image.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si"
            });
            /// si la confirmacion del usuario presiono que si
            /// si esto no es vacio para eso uso IsNullOrEmpty
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHppt = await repository.DeleteAsync<Image>($"api/imagenes/{image.ImageId}");
            if (responseHppt.Error)
            {
                if(responseHppt.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound) 
                {
                    navigationManager.NavigateTo("/document");
                }
                else
                {
                    var mensajeError = await responseHppt.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);

                }
                return;
            }
            await LoadAsync();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro Eliminado.");
        }
    }
}
