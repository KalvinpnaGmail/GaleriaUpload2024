using CurrieTechnologies.Razor.SweetAlert2;
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
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        IList<IBrowserFile> _files = new List<IBrowserFile>();

        private async Task UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
            // Definir tama�o m�ximo permitido (3MB)
            long maxFileSize = 1024 * 1024 * 3;



            foreach (var file in files)
            {
                
                // Validar si el tama�o del archivo excede el tama�o m�ximo permitido
                if (file.Size > maxFileSize)
                {
                    // Si excede, puedes mostrar un mensaje o manejarlo seg�n sea necesario
                    await SweetAlertService.FireAsync("Error", "El archivo excede el tama�o m�ximo permitido de 3 MB", "error");
                    continue; // Pasar al siguiente archivo
                }
                this._files.Add(file);
                // Procesar el archivo si est� dentro del l�mite de tama�o
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