using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

using UPLOAD.SHARE.Entities;

namespace UPLOAD.WEB.Pages.Countries
{
    public partial class CountryForm
    {
        private EditContext editContext = null!;



        [EditorRequired, Parameter] public Country Country { get; set; } = null!;


        ///apr <summary>
        ///si digo eventcallback significa que le avamos a pasar codigo
        ///onvalidsumit cuando se valida todo onvalid
        /// Para saber que hacer si es editar o agregar
        /// </summary>
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }


        /// <summary>
        /// es si cancela accion de regresar 
        /// </summary>

        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }


        [Inject] public SweetAlertService SweetAlertService { get; set; }=null!;

        /// <summary>
        /// si el forumlario lo pudimos postear o no por si se va
        /// ormPostedSuccessfully
        /// </summary>
        public bool FormPostedSuccessfully { get; set; }

     
        protected override void OnInitialized()
        {
            editContext = new(Country);
        }

        /// <summary>
        /// para preguntar si me sali del formulario sin guardar los cambio 
        ///  private async Task OnBeforeInternalNavigation(LocationChangingContext context)

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
    }
}
