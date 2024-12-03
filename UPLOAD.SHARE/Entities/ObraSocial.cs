using System.ComponentModel.DataAnnotations;

namespace UPLOAD.SHARE.Entities
{
    public class ObraSocial
    {
        public int ID { get; set; }

        [Display(Name = "Nro.Obra Social")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string? C01 { get; set; }

        [Display(Name = "Nombre O.S.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string? C02 { get; set; }

        [Display(Name = "Nombre Fantasia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string? C03 { get; set; }
    }
}