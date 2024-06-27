using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Countries
{
    public partial class CountryIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;
        public List<Country>? Countries { set; get; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries");
            Countries = responseHttp.Response;
        }
    }
}
