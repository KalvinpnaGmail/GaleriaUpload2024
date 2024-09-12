using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Servicios;

namespace UPLOAD.WEB.Pages.Documentos
{
    [Authorize(Roles = "Admin")]
    public partial class DocumentFormUpload
    {
        
      
        private List<Clinica> clinicas;
        private EditContext editContext = null!;
        [Parameter, EditorRequired] public Image Image { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        public bool FormPostedSuccessfully { get; set; }
        protected override void OnInitialized()
        {
            editContext = new EditContext(Image);
            // Aquí debes inicializar `clinicas` y cargar los datos.
        }
        protected override async Task OnInitializedAsync()
        {
            await LoadClinicasAsync();
        }


        private async Task LoadClinicasAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Clinica>>("/api/clinicas/DevuelveClinicas");
            // var responseHttp = await repository.GetAsync<List<Clinica>>("/api/clinicas/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            clinicas = responseHttp.Response;
        }

       
        private async Task CreteUserAsync()
        {


            NavigationManager.NavigateTo("/");
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {

            //sino fue editado el formulario o grabado
            var formWasModified = editContext.IsModified();
            if (!formWasModified || FormPostedSuccessfully)
            {
                return;
            }
            ///si fue editado saco un alerta
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmaciòn",
                Text = "¿Deseas abandonar la pàgina?, se perderan los cambios.",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si"
            });
            /// si la confirmacion del usuario presiono que si
            /// si esto no es vacio para eso uso IsNullOrEmpty
            var confirm = !string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            ///sino quiero que pierda los cambios obliga que se quede en el formulario
            context.PreventNavigation();
        }

        private void OnValidSubmit()
        {
            // Lógica para manejar el envío del formulario
        }
    }


}


