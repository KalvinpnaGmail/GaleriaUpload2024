using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.DTOS;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Practicas
{
    public partial class PracticasIndex
    {
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        public List<PracticaDto> listapracticas = new List<PracticaDto>();
        private string searchString1 = "";
        private PracticaDto selectedItem1 = null;
        private bool _loading = true;
        private int rowsPerPage = 25;

        private void OnRowsPerPageChanged(int newRowsPerPage)
        {
            rowsPerPage = newRowsPerPage;
        }

        private async Task LoadAsync()
        {
            //var result = await repository.GetAsync<List<Clinica>>("/api/clinicas/DevuelveClinicas");

            var responseHppt = await repository.GetAsync<List<PracticaDto>>("/api/practicas/GetAll");
            if (responseHppt.Error)
            {
                var message = await responseHppt.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            listapracticas = responseHppt.Response!;
        }

        protected override async Task OnInitializedAsync()
        {
            // _menuServicio.SetMenu(new BreadcrumbItem("Clinicas", href: null));
            await LoadAsync();
            _loading = false;
        }

        private bool FilterFunc1(PracticaDto element) => FilterFunc(element, searchString1);

        private bool FilterFunc(PracticaDto element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            if (element.Codigo.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Descripcion.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task Delete(PracticaDto practicadto)
        {
            SweetAlertResult result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Esta seguro?",
                Text = $"Eliminar Codigo: {practicadto.Codigo}",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Si, eliminar",
                CancelButtonText = "No, volver"
            });
        }
    }
}