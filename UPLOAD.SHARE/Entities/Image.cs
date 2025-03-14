using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace UPLOAD.SHARE.Entities
{
    public class Image
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Link")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Url { get; set; } = string.Empty;

        [Display(Name = "Obra Social")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ObraSocial { get; set; } = string.Empty;

        [Display(Name = "Periodo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Periodo { get; set; } = DateTime.MinValue; // Asigna una fecha válida según tu lógica

        // Método para formatear la propiedad Periodo
        public string ObtenerPeriodoFormateado()
        {
            // Formatea la fecha en el formato "MM/yyyy"
            return Periodo.ToString("MM/yyyy");
        }

        // Relación con CabeceraImage
        public int CabeceraImageId { get; set; }

        public CabeceraImage? CabeceraImage { get; set; } // Permitir valores nulos
    }
}