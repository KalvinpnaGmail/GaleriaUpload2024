using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.WEB.Pages.Documentos
{
    [Authorize(Roles = "Admin")]
    public partial class DocumentForm
    {
        private EditContext editContext =null!;
        [Parameter, EditorRequired] public Image Image { get; set; } = null!;
        [EditorRequired, Parameter] public EventCallback OnvalidSubmit { get; set; }
        [EditorRequired, Parameter]public EventCallback ReturnAction {  get; set; }
        public bool FormPostedSuccessfully { get; set; }
        
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;

        protected override void OnInitialized()
        {
            editContext = new (Image);
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formWasModified = editContext.IsModified();
            if (!formWasModified || FormPostedSuccessfully)
            {
                return;
            }

            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmaciòn",
                Text = "¿Deseas abandonar la pàgina?, se perderan los cambios.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si"
            });
            /// si la confirmacion del usuario presiono que si
            var confirm = !string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            context.PreventNavigation();
        }


    }
}
