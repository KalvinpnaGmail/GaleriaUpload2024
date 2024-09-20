using System.ComponentModel.DataAnnotations;

namespace UPLOAD.SHARE.Entities
{
    public class Clinica
    {

        public int ID { get; set; }

        [Display(Name = "Nro.Prestador")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string? MATRICULA { get; set; }

        [Display(Name = "Clinica")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string? DENOMINACION { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string? CATEGORIA { get; set; }


    }
}
