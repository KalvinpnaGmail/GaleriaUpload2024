using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPLOAD.SHARE.Interfaces;

namespace UPLOAD.SHARE.Entities
{
    public class Ciudad :IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "Ciudades")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public int CiudadId { get; set; }
        public Provincia? Provincia { get; set; }
    }
}
