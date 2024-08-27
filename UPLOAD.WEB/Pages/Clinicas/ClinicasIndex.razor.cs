using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Clinicas
{
    [Authorize(Roles = "Admin")]
    public partial class ClinicasIndex
    {
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;




        //        @inject IDialogService _dialogServicio
        //@inject ISnackbar _snackBar
        //@inject SweetAlertService Swal;
        //@inject NavigationManager _navigationServicio;
        //@inject IRepository repository;

        public List<Clinica> listaClinicas = new List<Clinica>();
        private string searchString1 = "";
        private Clinica selectedItem1 = null;
        private bool _loading = true;

        private async Task LoadAsync()
        {
            var result = await repository.GetAsync<List<Clinica>>("/api/clinicas/DevuelveClinicas");
            // var result = await _libroServicio.Lista();

            if (!result.Error)
            {
                listaClinicas = result.Response!;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            // _menuServicio.SetMenu(new BreadcrumbItem("Clinicas", href: null));
            await LoadAsync();
            _loading = false;
        }
        private bool FilterFunc1(Clinica element) => FilterFunc(element, searchString1);

        private bool FilterFunc(Clinica element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            if (element.DENOMINACION.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.MATRICULA.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }


        private async Task Delete(Clinica clinica)
        {
            SweetAlertResult result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Esta seguro?",
                Text = $"Eliminar Clinica: {clinica.DENOMINACION}",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Si, eliminar",
                CancelButtonText = "No, volver"
            });


        }
    }
}
