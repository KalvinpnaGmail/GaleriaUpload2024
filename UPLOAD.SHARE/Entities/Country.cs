using System.ComponentModel.DataAnnotations;
using UPLOAD.SHARE.Interfaces;

namespace UPLOAD.SHARE.Entities
{
    public class Country : IEntityWithName
    {

        public int Id { get; set; }

        [Display(Name = "País")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 1 a varios
        /// </summary>
        public ICollection<Provincia>? Provincias { get; set; }

        [Display(Name = "Provincia")]
        ///para saber las cantidad de provincia que tienen el pais
        public int ProvinciaNumer => Provincias == null || Provincias.Count == 0 ? 0 : Provincias.Count;


    }


}
