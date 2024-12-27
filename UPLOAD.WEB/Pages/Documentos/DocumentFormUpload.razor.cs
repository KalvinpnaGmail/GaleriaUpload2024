using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Servicios;
using MudBlazor;

namespace UPLOAD.WEB.Pages.Documentos
{
    [Authorize(Roles = "Admin")]
    public partial class DocumentFormUpload
    {
        public List<ObraSocial>? ObraSociales { get; set; }
        private EditContext editContext = null!;
        [Parameter, EditorRequired] public Image Image { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        private bool uploading = false;
        private string uploadMessage = "";

        private readonly List<ImagenDTO> loadedImages = new List<ImagenDTO>();

        private DateTime? _date3 = null;

        private string _modalImageBase64 = string.Empty;
        private bool _isModalOpen = false;
        public bool FormPostedSuccessfully { get; set; }

        protected override void OnInitialized()
        {
            if (Image == null)
            {
                // Esto asegura que no sea nulo y evita errores en el formulario.
                Image = new Image
                {
                    // Puedes inicializar valores predeterminados aquí si es necesario.
                    // Predeterminar el período como el mes anterior al actual
                    // Inicializar el período con el mes anterior al actual
                    Periodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)
                };
            }

            editContext = new EditContext(Image);
            // Aquí debes inicializar `clinicas` y cargar los datos.
        }

        protected override async Task OnInitializedAsync()
        {
            // Cargar las clínicas de forma asíncrona
            await LoadObraSocialesAsync();
            var now = DateTime.Now;
            _date3 = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
        }

        private async Task LoadObraSocialesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<ObraSocial>>("/api/obraSociales/DevuelveObraSociales");

            // var responseHttp = await repository.GetAsync<List<Clinica>>("/api/clinicas/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            ObraSociales = responseHttp.Response;
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            //sino fue editado el formulario o grabado
            if (Image == null || editContext == null)
            {
                return; // Si no hay un contexto válido, permitimos la navegación.
            }

            var formWasModified = editContext.IsModified();
            if (!formWasModified || FormPostedSuccessfully)
            {
                return;
            }
            ///si fue editado saco un alerta
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmaciòn",
                Text = "¿Deseas abandonar la pàgina?, se perderan los cambios.",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si"
            });
            /// si la confirmacion del usuario presiono que si
            /// si esto no es vacio para eso uso IsNullOrEmpty
            var confirm = !string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            ///sino quiero que pierda los cambios obliga que se quede en el formulario
            context.PreventNavigation();
        }

        public void Dispose()
        {
            //Elimina la suscripción al evento LocationChanging:

            //Cuando se suscribe al evento LocationChanging en el NavigationManager en el método OnInitialized,
            //el objeto actual(this) se convierte en un "oyente" del evento.
            //NavigationManager.LocationChanging -= OnBeforeInternalNavigation;
        }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            uploading = true;

            try
            {
                var files = e.GetMultipleFiles();
                foreach (var file in files)
                {
                    if (file.Size > 3 * 1024 * 1024)
                    {
                        await SweetAlertService.FireAsync("Error", $"El archivo {file.Name} supera el límite de 3 MB.", SweetAlertIcon.Error);
                        continue;
                    }

                    using var memoryStream = new MemoryStream();
                    await file.OpenReadStream(maxAllowedSize: 3 * 1024 * 1024).CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    var imagenDTO = new ImagenDTO(file.Name, Convert.ToBase64String(imageData));

                    // Agrega el archivo cargado a la lista de archivos
                    //loadedFiles.Add(imagenDTO);

                    loadedImages.Add(imagenDTO);
                    // Actualizar las propiedades de Image directamente
                    Image.Name = imagenDTO.Name;
                    Image.Url = imagenDTO.Base64;
                    await UploadImage(imagenDTO);
                }

                await SweetAlertService.FireAsync("Éxito", "Todas las imágenes se han cargado correctamente.", SweetAlertIcon.Success);
            }
            catch (Exception ex)
            {
                uploadMessage = $"Error al cargar las imágenes: {ex.Message}";
            }
            finally
            {
                uploading = false;
            }
        }

        private async Task UploadImage(ImagenDTO imagenDTO)
        {
            try
            {
                var response = await Repository.PostAsync("api/imagenes", imagenDTO);

                if (response.Error)
                {
                    var message = await response.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                uploadMessage = $"Error al cargar la imagen: {ex.Message}";
            }
        }

        private void DeleteImage(ImagenDTO imagen)
        {
            // Eliminar la imagen de la lista loadedImages
            loadedImages.Remove(imagen);

            // Mostrar un mensaje de éxito
            SweetAlertService.FireAsync("Éxito", $"La imagen {imagen.Name} fue eliminada correctamente.", SweetAlertIcon.Success);
        }

        // Método para abrir el modal con la imagen completa
        // Función para abrir el modal con la imagen seleccionada
        private void OpenModal(string base64Image)
        {
            _modalImageBase64 = base64Image;
            _isModalOpen = true;
            // SweetAlertService.FireAsync("Modal", "Modal abierto con éxito!", SweetAlertIcon.Info);
        }

        // Método para cerrar el modal

        private void Return()
        {
            NavigationManager.NavigateTo("/document");
        }

        private async Task OnValidSubmit()
        {
            if (loadedImages.Count == 0)
            {
                await SweetAlertService.FireAsync("Error", "Debes cargar al menos una imagen antes de guardar.", SweetAlertIcon.Error);
                return;
            }

            // Aquí puedes implementar la lógica para guardar la información en la base de datos local.
            await SweetAlertService.FireAsync("Éxito", "Documentación subida correctamente.", SweetAlertIcon.Success);
            FormPostedSuccessfully = true;

            // Redirige al usuario después de completar el proceso.
            Return();
            //NavigationManager.NavigateTo("/document");
        }
    }
}