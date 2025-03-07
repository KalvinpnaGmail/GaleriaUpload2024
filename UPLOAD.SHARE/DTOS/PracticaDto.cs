using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPLOAD.SHARE.DTOS
{
    public class PracticaDto
    {
        public string ObraSocial { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Importe1 { get; set; }
        public decimal Importe2 { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string CodigoInterno { get; set; }
        public string OtroCodigo { get; set; }

        public string cod_obrasocial { get; set; }
        public string nro_conv { get; set; }
    }
}