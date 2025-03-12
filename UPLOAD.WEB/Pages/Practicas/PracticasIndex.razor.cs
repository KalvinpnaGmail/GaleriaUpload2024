using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
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

        private void OnRowsPerPageChanged(int newRowsPerPage)
        {
            rowsPerPage = newRowsPerPage;
        }

        private async Task LoadAsync()
        {
            //var result = await repository.GetAsync<List<Clinica>>("/api/clinicas/DevuelveClinicas");

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

        private async Task<TableData<PracticaDto>> LoadPracticasPaginadasAsync(TableState state, CancellationToken cancellationToken)
        {
            var skip = state.Page * state.PageSize;
            var take = state.PageSize;

            var response = await repository.GetAsync<List<PracticaDto>>($"/api/ApiAcler/GetPracticasPaginadas?skip={skip}&take={take}");
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return new TableData<PracticaDto> { Items = new List<PracticaDto>(), TotalItems = 0 };
            }

            listapracticas = response.Response!;

            // Obtener valores para los 25 registros cargados
            foreach (var practica in listapracticas)
            {
                if (cancellationToken.IsCancellationRequested)
                    return new TableData<PracticaDto> { Items = listapracticas, TotalItems = 500 };

                var valor = await repository.GetAsync<string>($"/api/ApiAcler/ObtenerValorPractica?codigo={practica.Codigo}&codOS={practica.cod_obrasocial}&nroConv={practica.nro_conv}");

                if (!valor.Error)
                {
                    practica.ValorPractica = valor.Response;
                }
            }

            return new TableData<PracticaDto> { Items = listapracticas, TotalItems = 500 };
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
                    practica.ValorPractica = "No existe valor";
                }

                StateHasChanged(); // 🔄 Actualiza la interfaz
            }
            else
            {
                var message = await valor.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
        }

        private async Task ObtenerValorPractica2(PracticaDto practica)
        {
            var valor = await repository.GetAsync<string>($"/api/ApiAcler/obtenervalorpractica?codigoPractica={practica.Codigo}&codOS={practica.cod_obrasocial}&nroConv={practica.nro_conv}");

            if (!valor.Error)
            {
                try
                {
                    // 🔽 Deserializamos la respuesta JSON en un diccionario
                    var valoresPractica = JsonSerializer.Deserialize<Dictionary<string, decimal>>(valor.Response);

                    // 🔽 Verificamos si el diccionario contiene la clave "GASTOS + HONORARIOS"
                    if (valoresPractica != null && valoresPractica.ContainsKey("GASTOS + HONORARIOS"))
                    {
                        practica.ValorPractica = valoresPractica["GASTOS + HONORARIOS"].ToString();
                    }
                    else
                    {
                        practica.ValorPractica = "No existe valor";
                    }

                    StateHasChanged(); // 🔄 Actualiza la tabla para reflejar los cambios
                }
                catch (JsonException ex)
                {
                    await sweetAlertService.FireAsync("Error", $"Error al procesar los datos: {ex.Message}", SweetAlertIcon.Error);
                }
            }
            else
            {
                var message = await valor.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
        }

        //private async Task ObtenerValorPractica(PracticaDto practica)
        //{
        //    var valor = await repository.GetAsync<string>($"/api/ApiAcler/obtenervalorpractica?codigoPractica={practica.Codigo}&codOS={practica.cod_obrasocial}&nroConv={practica.nro_conv}");

        //    if (!valor.Error)
        //    {
        //        //if (valoresPractica.ContainsKey("GASTOS RX"))
        //        //{
        //        //    var gastosRx = valoresPractica["GASTOS RX"];
        //        //    Console.WriteLine($"Gastos RX: {gastosRx}");
        //        //}

        //        practica.ValorPractica = valor.Response;
        //        StateHasChanged(); // 🔄 Actualiza la tabla para reflejar los cambios
        //    }
        //    else
        //    {
        //        var message = await valor.GetErrorMessageAsync();
        //        await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
        //    }
        //}
    }
}