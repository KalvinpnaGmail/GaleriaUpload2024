using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Shared;

namespace UPLOAD.WEB.Pages.Provincias
{
    [Authorize(Roles = "Admin")]
    public partial class ProvinciasEdit
    {
        private Provincia? provincia;

        /// para referencias la no navegacion  <summary>
        /// para poder cambiar la propiedad FormPostedSuccessfully
        private FormWihtName<Provincia>? provinciaForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;


        ////pasame id como parametro para ver que provincia es
        [EditorRequired, Parameter] public int CountryId { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

    }
}
