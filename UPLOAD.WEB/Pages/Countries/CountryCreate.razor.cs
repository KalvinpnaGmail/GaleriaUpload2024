using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Countries
{
    public partial class CountryCreate
    {

        private Country country = new();


        /// para referencias la no navegacion 
        private CountryForm? countryForm;


        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;




        /// <summary>
        /// aca pasa si ya lo valido
        /// </summary>
        /// <returns></returns>
        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync("/api/countries", country);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
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
            countryForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/countries");
        }
    }
}
