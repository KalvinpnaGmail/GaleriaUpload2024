using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Documentos
{
    public partial class DocumentIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;
        public List<Image>? Imagenes { get; set; }



        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }



        private async Task LoadAsync()
        {

            var responseHppt = await Repository.GetAsync<List<Image>>("/api/imagenes/DevuelveImagenes");

            Imagenes = responseHppt.Response!;


        }
    }
}
