using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPLOAD.SHARE.Entities
{
    public class CabeceraImage
    {
        public int Id { get; set; }
        public ICollection<Image>? Images { get; set; }

        [Display(Name = "Imagen")]
        ///para saber las cantidad de provincia que tienen el pais
        public int ImagenNumber => Images == null || Images.Count == 0 ? 0 : Images.Count;
    }
}
