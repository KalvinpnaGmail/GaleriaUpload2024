using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UPLOAD.SHARE.Interfaces;

namespace UPLOAD.SHARE.Entities
{
    public class Provincia : IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "Provincia/Estado")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// de uno a varios
        /// con Countryid va a saberlo mapear  puedo agregar provincia directamente qeu pertenes a un pais directamente 
        /// </summary>
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        //importan pra que no hayas un ciclo en program agregar
             //    builder.Services.
            //AddControllers().
            //AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        public ICollection<City>? Cities { get; set; }


        ///para saber las cantidad de provincia que tienen el pais <summary>
        /// para saber las cantidad de provincia que tienen el pais
        [Display(Name = "Ciudades")]
        public int CitiesNumer => Cities == null || Cities.Count == 0 ? 0 : Cities.Count;


    }
}
