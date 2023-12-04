using System.Text.RegularExpressions;

namespace UPLOAD.API.Helpers
{
    public class AclerHelper
    {

        public class Acler
        {
           
            public double formatearDecimal(string valor)
            {
                string result2 = valor.Substring(0, valor.Length - 2);

                double valorAConvertir = double.Parse(result2);
                return valorAConvertir;
            }





            public string ProcesarJsonInvalido(string json)
            {
                string resultado1 = json.Replace("[", "");
                string resultado2 = resultado1.Replace("]", "");

                string datos = Regex.Replace(resultado2, @"[^\w\ ,.@-]", "",
                                RegexOptions.None, TimeSpan.FromSeconds(1.5));
                return datos;
            }

            public string ProcesarJsonInvalido2(string json)
            {


                string resultado1 = json.Replace("\n", "");
                string resultado2 = resultado1.Replace("\t", "");
                string resultado3 = resultado1.Replace("\r", "");
                string datos = resultado3.Trim();
                return datos;
            }

        }
    }
}
