using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.ObraSociaels
{
    public partial class ObraSocialesIndex
    {
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        public List<ObraSocial>? ObraSociales { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            //var result = await repository.GetAsync<List<Clinica>>("/api/clinicas/DevuelveClinicas");

            var responseHppt = await repository.GetAsync<List<ObraSocial>>("/api/obraSociales/DevuelveObraSociales");
            if (responseHppt.Error)
            {
                var message = await responseHppt.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ObraSociales = responseHppt.Response!;
        }
    }
}