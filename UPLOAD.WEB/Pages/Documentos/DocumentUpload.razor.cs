﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using UPLOAD.WEB.Repositories;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;

namespace UPLOAD.WEB.Pages.Documentos
{
    [Authorize(Roles = "Admin")]
    public partial class DocumentUpload
    {
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private bool uploading = false;
        private string uploadMessage = "";
        private string? imageName;
        private string? base64Image;
        private readonly List<ImagenDTO> loadedImages = new();

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            uploading = true;

            try
            {
                var files = e.GetMultipleFiles();
                foreach (var file in files)
                {
                    using var memoryStream = new MemoryStream();
                    await file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024).CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    imageName = file.Name;
                    base64Image = Convert.ToBase64String(imageData);

                    // Llamar al método de la API para cargar la imagen
                    await UploadImage(imageName, base64Image);
                }
                // Mensaje único al final
                var toast = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000
                });
                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Todas las imagenes se han cargado.");
                //await SweetAlertService.FireAsync("Éxito", "Todas las imágenes se han cargado correctamente.", SweetAlertIcon.Success);
                // Redirigir solo después de procesar todas las imágenes
                Return();
            }
            catch (Exception ex)
            {
                uploadMessage = $"Error al cargar la imagen en la nube: {ex.Message}";
            }

            uploading = false;
        }

        private async Task UploadImage(string imageName, string base64Image)
        {
            try

            {
                var imagenDTO = new ImagenDTO(imageName, base64Image);

                // Llamar a la API para cargar la imagen utilizando
                //
                var response = await Repository.PostAsync("api/imagenes", imagenDTO);
                //var response = await Repository.PostAsync<ImagenDTO>("api/imagenes", imagenDTO);
                if (response.Error)
                {
                    var message = await response.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message);
                    return;
                }

                loadedImages.Add(imagenDTO);
                //Return();
            }
            catch (Exception ex)
            {
                uploadMessage = $"Error al cargar la imagen: {ex.Message}";
            }
        }

        private void Return()
        {
            NavigationManager.NavigateTo("/document");
        }
    }
}