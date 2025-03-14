using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Drawing;
using System.Text.Json;
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
        private int currentPage = 1; // La página inicial es 1

        private void OnRowsPerPageChanged(int newRowsPerPage)
        {
            rowsPerPage = newRowsPerPage;
        }

        private void OnPageChanged(int page)
        {
            currentPage = page + 1; // MudBlazor usa una base 0, por eso sumamos 1
        }

        private async Task LoadAsync()
        {
            var responseHppt = await repository.GetAsync<List<PracticaDto>>("/api/ApiAcler/GetAllPracticas");
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

        private async Task ObtenerValorPractica(PracticaDto practica)
        {
            var valor = await repository.GetAsync<Dictionary<string, decimal>>(
                $"/api/ApiAcler/obtenervalorpractica?codigoPractica={practica.Codigo}&codOS={practica.cod_obrasocial}&nroConv={practica.nro_conv}"
            );

            if (!valor.Error)
            {
                if (valor.Response != null && valor.Response.ContainsKey("GASTOS + HONORARIOS"))
                {
                    practica.ValorPractica = valor.Response["GASTOS + HONORARIOS"].ToString();
                }
                else
                {
                    practica.ValorPractica = "0";
                }

                StateHasChanged(); // 🔄 Actualiza la interfaz
            }
            else
            {
                var message = await valor.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
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

        private async Task CalcularValoresPaginaActual()
        {
            _loading = true;

            // Obtener las filas de la página actual
            var practicasPaginaActual = listapracticas.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

            // Llamar a ObtenerValorPractica para cada práctica
            foreach (var practica in practicasPaginaActual)
            {
                await ObtenerValorPractica(practica);
            }

            _loading = false;
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