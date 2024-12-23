using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UPLOAD.WEB.Shared
{
    public partial class InputImg
    {
        private string? imageBase64;
        [Parameter] public string Label { get; set; } = "Imagen";
        [Parameter] public string? ImageURL { get; set; }
        [Parameter] public EventCallback<string> ImageSelected { get; set; }
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        private IList<IBrowserFile> _files = new List<IBrowserFile>();

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

                //
                //Esa l�nea notifica a cualquier componente o m�todo que est� suscrito al evento ImageSelected, pas�ndole la representaci�n en Base64 de la imagen procesada. Esto permite que otras partes de la aplicaci�n respondan a la carga de la imagen, como actualizar una vista previa, guardar los datos, o ejecutar l�gica adicional.
                ////
                await ImageSelected.InvokeAsync(imageBase64);
                StateHasChanged();
            }
        }
    }
}