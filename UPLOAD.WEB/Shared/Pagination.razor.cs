using Microsoft.AspNetCore.Components;
using System.Net.WebSockets;

namespace UPLOAD.WEB.Shared
{
    public partial class Pagination
    {
        /// <summary>
        /// lista de todas las pagina Pagemodel 194 paise y pagino de 10 tengo 20 objteos
        /// 
        /// </summary>
        private List<PageModel> links = new();
        [Parameter] public int CurrentPage { get; set; } = 1;
        [Parameter] public int TotalPages { get; set; } = 1;
        //Radio - controla la cant de pagina que debe mostrar el componente  botonera maximo de botones a mostrar
        [Parameter] public int Radio { get; set; } = 10;

        /// <summary>
        ///SelectedPage- envio que pagina cambie por evento
        /// </summary>
        [Parameter] public EventCallback<int> SelectedPage { get; set; }

        protected override void OnParametersSet()
        {
            //var previusLinkEnable = CurrentPage != 1;
            //var previusLinkPage = CurrentPage - 1;



            links.Add(new PageModel
            {
                Text="Anterior",
                Page= CurrentPage - 1,
                Enable= CurrentPage != 1

            });


            links.Add(new PageModel
            {
                Text = "Siguiente",
                Page = CurrentPage + 1,
                Enable = CurrentPage != TotalPages

            });

            for (int i=1; i<=TotalPages; i++)
            {
                links.Add(new PageModel
                {
                    Text = $"{i}",
                    Page =i,
                    Active = i == CurrentPage
                });
            }
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
            public bool Active { get; set; }

        }
    }
}
