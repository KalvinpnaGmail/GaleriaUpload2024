using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Enums;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Servicios;
using static MudBlazor.Colors;

namespace UPLOAD.WEB.Pages.Autenticacion
{
    public  partial class Register
    {
        private UserDTO userDTO = new();
        private List<Country>? countries;
        private List<Provincia>? provincias;
        private List<City>? cities;
        private bool loading;

        private int selectedStateId = 0;  // Define e inicializa el valor del país seleccionado
        //private int selectedCountryId = 0;  // Define e inicializa el valor del país seleccionado

        private string? selectedCountryId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync();
        }

        private async Task CountryChangedAsync(string id)
        {
            // var selectedCountry = Convert.ToInt32(id);
            int.TryParse(id, out int selectedCountry);
           // selectedCountryId = value;
            Console.WriteLine($"CountryChangedAsync called with value: {selectedCountryId}");
            provincias = null;
            cities = null;
            userDTO.CityId = 0;
            await LoadStatesAsyn(selectedCountry);
            //await InvokeAsync(StateHasChanged);
        }




        private async Task LoadCountriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            countries = responseHttp.Response;
        }

        private async Task ProvinciaChangedAsync(int selectedState)
        {
            //var selectedState = Convert.ToInt32(e.Value!);
            cities = null;
            userDTO.CityId = 0;
            await LoadCitiesAsyn(selectedState);
        }

      



        private async Task LoadStatesAsyn(int countryId)
        {
            var responseHttp = await Repository.GetAsync<List<Provincia>>($"/api/provincias/combo/{countryId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            provincias = responseHttp.Response;
        }

        private async Task LoadCitiesAsyn(int selectedState)
        {
            var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{selectedState}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            cities = responseHttp.Response;
        }



        private async Task CreteUserAsync()
        {
            userDTO.UserName = userDTO.Email;
            userDTO.UserType = UserType.User;
            loading = true;

            var responseHttp = await Repository.PostAsync<UserDTO, TokenDTO>("/api/accounts/CreateUser", userDTO);
            loading = false;

            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await LoginService.LoginAsync(responseHttp.Response!.Token);
            NavigationManager.NavigateTo("/");
        }
    }


}

