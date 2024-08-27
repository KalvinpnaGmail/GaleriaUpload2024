using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;
using System.Net;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Shared;

namespace UPLOAD.WEB.Pages.Provincias
{
    [Authorize(Roles = "Admin")]
    public partial class ProvinciaEdit
    {
        private Provincia? provincia;

        /// para referencias la no navegacion  <summary>
        /// para poder cambiar la propiedad FormPostedSuccessfully
        private FormWihtName<Provincia>? provinciaForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;


        ////pasame id como parametro para ver que provicia esta
        [Parameter] public int ProvinciaId { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            //$ es interpolacion c# para pasar como parametro
            var responseHttp = await Repository.GetAsync<Provincia>($"/api/provincias/{ProvinciaId}");
            if (responseHttp.Error)
            {
                //si el usuario me cambio el pais por la qstring
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                   
                    Return();
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;

                }
            }

            //sino hubo errores y el pais es traido
            else
            {
                provincia = responseHttp.Response;
            }
        }



        private async Task SaveAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/provincias", provincia);
            //si hay error al actualiza lo pintamos
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }
            //
            //si grabo con exito lo mando a la pantalla de country 
            Return();
            ///luego de mandarlo a la pantalla  pinto mensaje de cambio
            ///tostadita abajo y final informativo se usa tostadita
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios Guardados.");

        }


        private void Return()
        {
            //si lo grabo
            provinciaForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo($"/countries/details/{provincia!.CountryId}");
        }

    }
}
