using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPLOAD.SHARE.Interfaces;

namespace UPLOAD.SHARE.Entities
{
    public class City :IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// de uno a varios
        /// con Countryid va a saberlo mapear  puedo agregar provincia directamente qeu pertenes a un pais directamente 
        /// </summary>
        public int ProvinciaId { get; set; }
        public Provincia? Provincia { get; set; }


        public ICollection<User>? Users { get; set; }

    }
}
