using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Countries
{
    public partial class CountryEdit
    {
        private Country? country;

        /// para referencias la no navegacion  <summary>
        /// para poder cambiar la propiedad FormPostedSuccessfully
        private CountryForm? countryForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;


        ////pasame id como parametro para ver que pais es
        [EditorRequired, Parameter] public int Id { get; set; }


        /// <summary>
        /// onparameterSe  como estoy pasando unparametro y depende del valor
        /// yo quiero estar seguro que el paramtro este seteado id
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            //$ es interpolacion c# para pasar como parametro
            var responseHttp = await Repository.GetAsync<Country>($"/api/countries/{Id}");
            if (responseHttp.Error)
            {
                //si el usuario me cambio el pais por la qstring
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/countries");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);

                }
            }

            //sino hubo errores y el pais es traido
            else
            {
                country = responseHttp.Response;
            }
        }

        ///cuadndo el usario dice que si va a cambiar
        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/countries", country);
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
            countryForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/countries");
        }
    }




    }

