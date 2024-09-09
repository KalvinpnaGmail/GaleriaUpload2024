using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Enums;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Servicios;

namespace UPLOAD.WEB.Pages.Autenticacion
{
    public  partial class Register
    {
        private UserDTO userDTO = new();

        private List<Country>? countries;
        private List<Provincia>? states;
        private List<City>? cities;
        private bool loading;
        int selectedCountry;


        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync();
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


        private async Task CountryChangedAsync(ChangeEventArgs e)
        {
            var selectedCountry = Convert.ToInt32(e.Value!);
            states = null;
            cities = null;
            userDTO.CityId = 0;
            await LoadStatesAsyn(selectedCountry);
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

            states = responseHttp.Response;
        }


        private async Task CreateUserAsync()
        {
            userDTO.UserName = userDTO.Email;
            userDTO.UserType = UserType.User;
            var responseHttp = await Repository.PostAsync<UserDTO, TokenDTO>("/api/accounts/CreateUser", userDTO);
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

