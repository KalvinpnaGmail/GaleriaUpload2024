using Microsoft.AspNetCore.Components;

namespace UPLOAD.WEB.Shared
{
    public partial class Pagination
    {
        /// <summary>
        /// lista de todas las pagina Pagemodel 194 paise y pagino de 10 tengo 20 objteos
        ///
        /// </summary>
        private List<PageModel> links = null!;

        private int selectedOptionValue = 10;
        [Parameter] public int CurrentPage { get; set; } = 1;
        [Parameter] public int TotalPages { get; set; } = 1;

        //Radio - controla la cant de pagina que debe mostrar el componente  botonera maximo de botones a mostrar
        [Parameter] public int Radio { get; set; } = 10;

        /// <summary>
        ///SelectedPage- envio que pagina cambie por evento
        /// </summary>
        [Parameter] public EventCallback<int> SelectedPage { get; set; }

        [Parameter] public EventCallback<int> RecordsNumber { get; set; }

        protected override void OnParametersSet()
        {
            //var previusLinkEnable = CurrentPage != 1;
            //var previusLinkPage = CurrentPage - 1;

            links = new List<PageModel>();

            links.Add(new PageModel
            {
                Text = "«",
                Page = CurrentPage - 1,
                Enable = CurrentPage != 1
            });

            links.Add(new PageModel
            {
                Text = "»",
                Page = CurrentPage != TotalPages ? CurrentPage + 1 : CurrentPage,
                Enable = CurrentPage != TotalPages
            });

            for (int i = 1; i <= TotalPages; i++)
            {
                links.Add(new PageModel
                {
                    Text = $"{i}",
                    Page = i,
                    Enable = i == CurrentPage
                });
            }
        }

        private async Task InternalSelectedPage(PageModel pageModel)
        {
            if (pageModel.Page == CurrentPage || pageModel.Page == 0)
            {
                return;
            }
            ///invoque la interfaz y la refresque y cambio la pagina
            await SelectedPage.InvokeAsync(pageModel.Page);
        }

        private async Task InternalRecordsNumberSelected(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                selectedOptionValue = Convert.ToInt32(e.Value.ToString());
            }
            await RecordsNumber.InvokeAsync(selectedOptionValue);
        }

        private class PageModel
        {
            //representa los boones que tento que
            /// <summary>
            /// texto 1
            /// </summary>
            public string Text { get; set; } = null!;

            //nro 1
            public int Page { get; set; }

            /// <summary>
            /// pagina 1 esta activa?
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// esta pagina esta activa?
            /// </summary>
        }
    }
}