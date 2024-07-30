using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Shared;

namespace UPLOAD.WEB.Pages.Provincias
{
    public partial class ProvinciasCreate
    {
        private Provincia provincia = new();


        /// para referencias la no navegacion 
        private FormWihtName<Provincia>? provinciaForm;

        [Parameter] public int CountryId { get; set; }


        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        public async Task CreateAsync()
        {  ///solo se ejecute cuando el formualario sea valido que paso las validaciones
            provincia.CountryId = CountryId;
            var responseHttp = await Repository.PostAsync("/api/provincias", provincia);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();

            ///si paso las validaciones
            ///tostadita abajo y final informativo se usa tostadita
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Provincia creada con éxito.");

        }




        private void Return()
        {
            //si lo grabo
            provinciaForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo($"/countries/details/{CountryId}");
        }
    }
}
