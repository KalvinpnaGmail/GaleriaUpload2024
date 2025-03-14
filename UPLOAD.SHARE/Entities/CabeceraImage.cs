using System.ComponentModel.DataAnnotations;

namespace UPLOAD.SHARE.Entities
{
    public class CabeceraImage
    {
        public int Id { get; set; }

        [Display(Name = "Obra Social")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ObraSocial { get; set; } = string.Empty;

        [Display(Name = "Periodo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Periodo { get; set; } = DateTime.MinValue;

        // Lista de imágenes asociadas
        public List<Image> Images { get; set; } = new();

        // Método para contar los documentos
        public int CantidadDocumentos => Images.Count;
    }
}