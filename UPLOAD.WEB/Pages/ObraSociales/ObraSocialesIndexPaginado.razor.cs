using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.ObraSociales
{
    [Authorize(Roles = "Admin")]
    public partial class ObraSocialesIndexPaginado
    {
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        //        @inject IDialogService _dialogServicio
        //@inject ISnackbar _snackBar
        //@inject SweetAlertService Swal;
        //@inject NavigationManager _navigationServicio;
        //@inject IRepository repository;

        public List<ObraSocial>? ObraSociales { get; set; }

        private string searchString1 = "";
        private ObraSocial selectedItem1 = null;
        private bool _loading = true;
        private string rowsPerPageString = "Filas por Pagina:";

        private async Task LoadAsync()
        {
            var responseHppt = await repository.GetAsync<List<ObraSocial>>("/api/obraSociales/DevuelveObraSociales");
            //var result = await repository.GetAsync<List<ObraSocial>>("/api/clinicas/DevuelveObrasSociales");
            // var result = await _libroServicio.Lista();
            if (responseHppt.Error)
            {
                var message = await responseHppt.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ObraSociales = responseHppt.Response!;
        }

        protected override async Task OnInitializedAsync()
        {
            // _menuServicio.SetMenu(new BreadcrumbItem("Clinicas", href: null));
            await LoadAsync();
            _loading = false;
        }

        private bool FilterFunc1(ObraSocial element) => FilterFunc(element, searchString1);

        private bool FilterFunc(ObraSocial element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            if (element.C02.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CUENTA.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}