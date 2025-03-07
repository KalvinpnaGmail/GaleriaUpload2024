using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Service;
using UPLOAD.WEB.Pages.Practicas;

namespace UPLOAD.API.Service
{
    public class PracticaService : IPracticaService
    {
        private readonly HttpClient _httpClient;

        public PracticaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PracticaDto>> GetPracticasAsync()
        {
            string soapRequest = @"
        <soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                          xmlns:xsd='http://www.w3.org/2001/XMLSchema'
                          xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'
                          xmlns:wsac='https://181.228.28.10/wsacler'>
            <soapenv:Header/>
            <soapenv:Body>
                <wsac:GETALL_PRACTICAS soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'/>
            </soapenv:Body>
        </soapenv:Envelope>";

            var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "https://181.228.28.10/wsacler/wsacler4.php/GETALL_PRACTICAS");

            // Agregamos la autenticación básica
            var byteArray = Encoding.ASCII.GetBytes("wacler@245:Fritolin.542");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Hacemos la solicitud POST al servicio SOAP
            HttpResponseMessage response = await _httpClient.PostAsync("https://181.228.28.10:443/wsacler/wsacler4.php", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error en la solicitud SOAP: {response.StatusCode}");
            }

            string xmlResponse = await response.Content.ReadAsStringAsync();
            return ParsePracticasFromXml(xmlResponse);
        }

        //con net vieja
        //    private List<PracticaDto> ParsePracticasFromXml(string xml)
        //    {
        //        var practicas = new List<PracticaDto>();
        //        var doc = new XmlDocument();
        //        doc.LoadXml(xml);

        //        XmlNodeList items = doc.GetElementsByTagName("item");
        //        foreach (XmlNode item in items)
        //        {
        //            string datos = item["datos"]?.InnerText;
        //            if (string.IsNullOrEmpty(datos)) continue;

        //            var partes = datos.Split(';');
        //            if (partes.Length < 9) continue;

        //            practicas.Add(new PracticaDto
        //            {
        //                ObraSocial = partes[0].Trim(),
        //                Codigo = partes[1].Trim(),
        //                Descripcion = partes[2].Trim(),
        //                Importe1 = decimal.TryParse(partes[3].Trim(), out var i1) ? i1 : 0,
        //                Importe2 = decimal.TryParse(partes[4].Trim(), out var i2) ? i2 : 0,
        //                FechaInicio = DateTime.TryParse(partes[5].Trim(), out var f1) ? f1 : DateTime.MinValue,
        //                FechaFin = DateTime.TryParse(partes[6].Trim(), out var f2) ? f2 : DateTime.MinValue,
        //                CodigoInterno = partes[7].Trim(),
        //                OtroCodigo = partes[8].Trim()
        //            });
        //        }

        //        return practicas;
        //    }
        //}

        //si tengo net 6 para adelante anda esto
        private List<PracticaDto> ParsePracticasFromXml(string xml)
        {
            var practicas = new List<PracticaDto>();
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList items = doc.GetElementsByTagName("item");
            foreach (XmlNode item in items)
            {
                string datos = item["datos"]?.InnerText;
                string codOs = item["cod_os"]?.InnerText;  // Extraer el valor de <cod_os>
                string nroconv = item["nroconv"]?.InnerText;  // Extraer el valor de <nroconv>

                if (string.IsNullOrEmpty(datos)) continue;

                // Divide los datos eliminando espacios en blanco automáticamente
                var partes = datos.Split(new char[] { ';' }, StringSplitOptions.TrimEntries);
                if (partes.Length < 9) continue;

                practicas.Add(new PracticaDto
                {
                    ObraSocial = partes[0],
                    Codigo = partes[1],
                    Descripcion = partes[2],
                    Importe1 = decimal.TryParse(partes[3], out var i1) ? i1 : 0,
                    Importe2 = decimal.TryParse(partes[4], out var i2) ? i2 : 0,
                    FechaInicio = DateTime.TryParse(partes[5], out var f1) ? f1 : DateTime.MinValue,
                    FechaFin = DateTime.TryParse(partes[6], out var f2) ? f2 : DateTime.MinValue,
                    CodigoInterno = partes[7],
                    OtroCodigo = partes[8],
                    cod_obrasocial = codOs,  // Asignar el valor de cod_os al nuevo campo
                    nro_conv = nroconv
                });
            }

            return practicas;
        }

        //        ¿Qué pasa si en el futuro queremos guardar Practica en la base de datos?
        //Podemos hacer una conversión entre PracticaDto y Practica en el Backend.

        //private List<Practica> ConvertToEntities(List<PracticaDto> dtos)
        //{
        //    return dtos.Select(dto => new Practica
        //    {
        //        ObraSocial = dto.ObraSocial,
        //        Codigo = dto.Codigo,
        //        Descripcion = dto.Descripcion,
        //        Importe1 = dto.Importe1,
        //        Importe2 = dto.Importe2,
        //        FechaInicio = dto.FechaInicio,
        //        FechaFin = dto.FechaFin,
        //        CodigoInterno = dto.CodigoInterno,
        //        OtroCodigo = dto.OtroCodigo
        //    }).ToList();
        //}
    }
}