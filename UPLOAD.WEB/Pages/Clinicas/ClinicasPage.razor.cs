using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Clinicas
{
    public partial class ClinicasPage
    {

        [Inject] private IRepository Repository { get; set; } = null!;
        public List<Clinica>? Clinicas { get; set; }



        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHppt = await Repository.GetAsync<List<Clinica>>("/api/clinicas/DevuelveClinicas");
            Clinicas = responseHppt.Response!;
         }
    }
}
