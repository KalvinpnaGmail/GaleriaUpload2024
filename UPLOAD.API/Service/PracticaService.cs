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

        private List<PracticaDto> ParsePracticasFromXml(string xml)
        {
            var practicas = new List<PracticaDto>();
            var totalImporte = new Dictionary<string, decimal>();
            var count = new Dictionary<string, int>();

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList items = doc.GetElementsByTagName("item");
            foreach (XmlNode item in items)
            {
                string datos = item["datos"]?.InnerText;
                if (string.IsNullOrEmpty(datos)) continue;

                var partes = datos.Split(';');
                if (partes.Length < 9) continue;

                // Crear la práctica
                var practica = new PracticaDto
                {
                    ObraSocial = partes[0].Trim(),
                    Codigo = partes[1].Trim(),
                    Descripcion = partes[2].Trim(),
                    Importe1 = decimal.TryParse(partes[3].Trim(), out var i1) ? i1 : 0,
                    Importe2 = decimal.TryParse(partes[4].Trim(), out var i2) ? i2 : 0,
                    FechaInicio = DateTime.TryParse(partes[5].Trim(), out var f1) ? f1 : DateTime.MinValue,
                    FechaFin = DateTime.TryParse(partes[6].Trim(), out var f2) ? f2 : DateTime.MinValue,
                    CodigoInterno = partes[7].Trim(),
                    OtroCodigo = partes[8].Trim()
                };

                // Verificar si la combinación de CodigoInterno y OtroCodigo ya existe
                if (!practicas.Any(p => p.CodigoInterno == practica.CodigoInterno && p.OtroCodigo == practica.OtroCodigo))
                {
                    // Agregar práctica a la lista
                    practicas.Add(practica);

                    // Calcular el promedio de Importe2
                    if (!totalImporte.ContainsKey(practica.Codigo))
                    {
                        totalImporte[practica.Codigo] = 0;
                        count[practica.Codigo] = 0;
                    }

                    totalImporte[practica.Codigo] += practica.Importe2;
                    count[practica.Codigo]++;
                }
            }

            // Calcular promedios y asignar a las prácticas correspondientes
            foreach (var codigo in totalImporte.Keys)
            {
                var promedio = totalImporte[codigo] / count[codigo];

                // Asignar el promedio a las prácticas correspondientes
                foreach (var practica in practicas.Where(p => p.Codigo == codigo))
                {
                    practica.PromedioPractica = promedio;
                }
            }

            return practicas; // Solo retorna la lista de prácticas
        }
    }
}

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