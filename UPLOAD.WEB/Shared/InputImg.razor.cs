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
            // Definir tamaño máximo permitido (3MB)
            long maxFileSize = 1024 * 1024 * 3;



            foreach (var file in files)
            {
                
                // Validar si el tamaño del archivo excede el tamaño máximo permitido
                if (file.Size > maxFileSize)
                {
                    // Si excede, puedes mostrar un mensaje o manejarlo según sea necesario
                    await SweetAlertService.FireAsync("Error", "El archivo excede el tamaño máximo permitido de 3 MB", "error");
                    continue; // Pasar al siguiente archivo
                }
                this._files.Add(file);
                // Procesar el archivo si está dentro del límite de tamaño
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