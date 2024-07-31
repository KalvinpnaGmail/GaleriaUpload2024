using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Shared;

namespace UPLOAD.WEB.Pages.Cities
{
    public partial class CityCreate
    {
        private City city = new();

        [Parameter] public int ProvinciaId { get; set; }



        /// para referencias la no navegacion 
        private FormWihtName<City>? cityForm;


        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        private async Task CreateAsync()
        {
            city.ProvinciaId = ProvinciaId;
            var responseHttp = await Repository.PostAsync($"/api/cities", city);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }


            Return();

            ///tostadita abajo y final informativo se usa tostadita
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");

        }

        private void Return()
        {
            //si lo grabo
            cityForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo($"/countries/details/{ProvinciaId}");

        }
    }
}
