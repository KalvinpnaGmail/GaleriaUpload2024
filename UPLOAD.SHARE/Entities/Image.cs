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
    }
}
