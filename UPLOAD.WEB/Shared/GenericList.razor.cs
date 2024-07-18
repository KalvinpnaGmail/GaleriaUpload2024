using Microsoft.AspNetCore.Components;


namespace UPLOAD.WEB.Shared
{
    public partial class GenericList<Titem>
    {
        //RenderFragment: que le vamos a pasar un pedazo de codigo de blazor yy se va a llamar Loading
        [Parameter]
        public RenderFragment? Loading { get; set; }


        // contemplamos si no hay registros
        [Parameter]
        public RenderFragment? NoRecords { get; set; }


        // contemplamos cuerpo que quiero pintar y si va a hacer obligatorio para evitar el warning
        // con el nulo  = null!
        [Parameter, EditorRequired] public RenderFragment Body { get; set; } = null!;


        // contemplamos Item del body
        [Parameter, EditorRequired] public List<Titem> MyList { get; set; } = null!;



    }
}
