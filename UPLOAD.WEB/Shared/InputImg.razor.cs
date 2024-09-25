using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;


namespace UPLOAD.WEB.Shared
{
    public partial class InputImg
    {
        private string? imageBase64;
        [Parameter] public string Label { get; set; } = "Imagen";
        [Parameter] public string? ImageURL { get; set; }
        [Parameter] public EventCallback<string> ImageSelected { get; set; }
        IList<IBrowserFile> _files = new List<IBrowserFile>();

        private async Task UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
            foreach (var file in files)
            {
                var arrBytes = new byte[file.Size];
                await file.OpenReadStream().ReadAsync(arrBytes);
                imageBase64 = Convert.ToBase64String(arrBytes);
                ImageURL = null;
                await ImageSelected.InvokeAsync(imageBase64);
                StateHasChanged();
            }
        }


    }
}