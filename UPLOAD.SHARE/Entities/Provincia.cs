using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPLOAD.SHARE.Interfaces;

namespace UPLOAD.SHARE.Entities
{
    public class Provincia : IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "Provincia")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }
        public Country? Country { get; set; }

        public ICollection<Ciudad>? Ciudades { get; set; }

        [Display(Name = "Ciudades")]
        public int CiuadadesNumber => Ciudades == null || Ciudades.Count == 0 ? 0 : Ciudades.Count;
   
    }
}
