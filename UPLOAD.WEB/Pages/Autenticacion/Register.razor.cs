using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Enums;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Servicios;

namespace UPLOAD.WEB.Pages.Autenticacion
{
    public partial class Register
    {
        private UserDTO userDTO = new();
        private List<Country>? countries;
        private List<Provincia>? provincias;
        private List<City>? cities;
        private bool loading;
        private string? imageUrl;

        private int selectedStateId = 0;  // Define e inicializa el valor del país seleccionado
        private int selectedCountryId = 0;  // Define e inicializa el valor del país seleccionado

        // private string? selectedCountryId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;
        [Parameter, SupplyParameterFromQuery] public bool IsAdmin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync();
        }

        private void ImageSelected(string imagenBase64)
        {
            userDTO.Photo = imagenBase64;
            imageUrl = null;
        }

        private async Task CountryChangedAsync(int newValue)
        {
            //var selectedCountry = Convert.ToInt32(e.Value);
            // int.TryParse(e, out int selectedCountry);
            selectedCountryId = newValue;  // Actualizamos el valor seleccionado.
            //Console.WriteLine($"CountryChangedAsync called with value: {selectedCountryId}");
            provincias = null;
            cities = null;
            userDTO.CityId = 0;
            await LoadStatesAsyn(selectedCountryId);
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

        private async Task ProvinciaChangedAsync(int newValue)
        {
            //var selectedState = Convert.ToInt32(e.Value!);
            selectedStateId = newValue;  // A
            cities = null;
            userDTO.CityId = 0;
            await LoadCitiesAsyn(selectedStateId);
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
            if (IsAdmin)
            {
                userDTO.UserType = UserType.Admin;
            }

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