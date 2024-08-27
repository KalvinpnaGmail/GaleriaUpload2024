using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;


namespace UPLOAD.WEB.Pages.Countries
{
    [Authorize(Roles = "Admin")]
    public partial class CountryIndex
    {
        //para la paginacion
        private int currentPage = 1;
        private int totalPages;

        //para la paginacion

        /// para referencias la no navegacion 

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        public List<Country>? Countries { set; get; }


        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();

        }

        private async Task LoadAsync(int page = 1)
        {
            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }


        private async Task<bool> LoadListAsync(int page)
        {
            //se pueden pasar varios parametors por qstring $"api/countries?page={page}&totalRecords=20
            //var responseHttp = await repository.GetAsync<List<Country>>($"api/countries?page={page}&totalRecords=20");
            var responseHttp = await repository.GetAsync<List<Country>>($"api/countries?page={page}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Countries = responseHttp.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            var responseHttp = await repository.GetAsync<int>("api/countries/totalPages");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;
        }


        //lo tengo que cambiar para la paginacion 
        //private async Task LoadAsync()
        //{

        //    var responseHttp = await repository.GetAsync<List<Country>>("/api/countries");
        //    if (responseHttp.Error)
        //    {
        //        var message = await responseHttp.GetErrorMessageAsync();
        //        await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
        //        return;
        //    }
        //    Countries = responseHttp.Response;
        //}
        private async Task DeleteAsync(Country country)
        {
            ///si fue editado saco un alerta
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmaciòn",
                Text = $"¿Desea Borrar el pais : {country.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si"
            });
            /// si la confirmacion del usuario presiono que si
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await repository.DeleteAsync<Country>($"api/countries/{country.Id}");
            if (responseHttp.Error)
            {
                //si el usuario me cambio el pais por la qstring
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/countries");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);

                }
                return;
            }
            await LoadAsync();
            ///tostadita abajo y final informativo se usa tostadita
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Warning, message: "Registro Borrado  con éxito.");

        }



        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }


    }
}

       
