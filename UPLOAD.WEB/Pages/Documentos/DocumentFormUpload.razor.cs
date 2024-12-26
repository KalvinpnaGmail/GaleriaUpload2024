using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Servicios;

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
        private string? imageName;
        private string? base64Image;
        private readonly List<ImagenDTO> loadedImages = new();

        private DateTime? _date3 = null;
        //private DateTime _date3;

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

        private async Task CreateUserAsync()
        {
            NavigationManager.NavigateTo("/");
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
                //var response = await Repository.PostAsync("api/imagenes", imagenDTO);
                var response = await Repository.PostAsync("api/imagenes", imagenDTO);

                //var response = await Repository.PostAsync<ImagenDTO>("api/imagenes", imagenDTO);
                if (response.Error)
                {
                    var message = await response.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message);
                    return;
                }

                loadedImages.Add(imagenDTO);
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

        private void OnValidSubmit()
        {
            // Lógica para manejar el envío del formulario
        }
    }
}