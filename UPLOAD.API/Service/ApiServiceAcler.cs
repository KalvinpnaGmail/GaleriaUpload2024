using CloudinaryDotNet;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;
using static UPLOAD.API.Helpers.AclerHelper;
using System.Text.Json;
using System.Xml;
using UPLOAD.SHARE.DTOS;

namespace UPLOAD.API.Service
{
    public class ApiServiceAcler : IApiServiceAcler
    {#if true

	#endif
        private readonly HttpClient _httpClient;
        private readonly string _usuario;
        private readonly string _pass;
        private readonly string _url;
        private readonly string _usuarioService;
        private readonly string _passService;

        public ApiServiceAcler(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true });
            _usuario = configuration["AclerApi:usuario"]!;
            _pass = configuration["AclerApi:pass"]!;
            _url = configuration["AclerApi:url"]!;
            _usuarioService = configuration["WebService:usuario"]!;
            _passService = configuration["Webservice:password"]!;
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
            // string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));

            // Agregamos la autenticación básica
            var byteArray = Encoding.ASCII.GetBytes($"{_usuarioService}:{_passService}");

            // var byteArray = Encoding.ASCII.GetBytes("wacler@245:Fritolin.542");
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

        public async Task<ActionResponse<IEnumerable<ObraSocial>>> GetObrasSocialesAsync()
        {
            try
            {
                // Configura la autenticación básica
                string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

                // Realiza la solicitud HTTP GET

                HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=os_vigentes_gral");
                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                    string responseData = await response.Content.ReadAsStringAsync();
                    Acler acler = new Acler();
                    var json = acler.ProcesarJsonInvalido2(responseData);
                    var obraSociales = JsonSerializer.Deserialize<List<ObraSocial>>(json);

                    // Retorna la lista de clínicas envuelta en un ActionResponse
                    return new ActionResponse<IEnumerable<ObraSocial>>
                    {
                        WasSuccess = true,
                        Result = obraSociales
                    };
                }
                else
                {
                    // Si la solicitud no es exitosa, retorna un ActionResponse con un mensaje de error
                    return new ActionResponse<IEnumerable<ObraSocial>>
                    {
                        WasSuccess = false,
                        Result = null,
                        Message = "Error al obtener las obras Sociales"
                    };
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retorna un ActionResponse con los detalles del error
                return new ActionResponse<IEnumerable<ObraSocial>>
                {
                    WasSuccess = false,
                    Result = null,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<IEnumerable<ObraSocial>> GetObraSocialesComboAsync()
        {
            try
            {
                // Configura la autenticación básica
                string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

                // Realiza la solicitud HTTP GET

                HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=os_vigentes_gral");

                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                    string responseData = await response.Content.ReadAsStringAsync();
                    Acler acler = new Acler();
                    var json = acler.ProcesarJsonInvalido2(responseData);
                    var obraSociales = JsonSerializer.Deserialize<List<ObraSocial>>(json);

                    // Retorna la lista de clínicas
                    return obraSociales;
                }

                // Manejo de errores: puedes retornar una lista vacía si prefieres no lanzar una excepción
                return new List<ObraSocial>();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Dependiendo del uso, puedes registrar el error y/o lanzar una excepción
                throw new Exception($"Error al obtener datos: {ex.Message}", ex);
            }
        }

        public async Task<Dictionary<string, decimal>> ObtenerValorPracticaAsync(string codigoPractica, string codOS, string nroConv)
        {
            // Construcción de la URL completa con el archivo PHP
            var url = $"{_url}api_cta.php?action=obtenerValorPractica&codigoPractica={codigoPractica}&codOS={codOS}&nroConv={nroConv}";

            // Configuración de la autorización básica
            var byteArray = Encoding.ASCII.GetBytes($"{_usuario}:{_pass}");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Realizamos la solicitud GET
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserializar a un diccionario con claves limpias
                var datos = JsonSerializer.Deserialize<Dictionary<string, decimal>>(jsonString);
                var datosLimpios = datos.ToDictionary(k => k.Key.Trim(), v => v.Value);

                return datosLimpios;
            }
            else
            {
                // Si la respuesta no es exitosa, lanzamos una excepción
                throw new Exception($"Error al obtener datos de la API externa: {response.ReasonPhrase}");
            }
        }

        public async Task<ActionResponse<IEnumerable<Clinica>>> GetClinicasAsync()
        {
            try
            {
                // Configura la autenticación básica
                string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

                // Realiza la solicitud HTTP GET
                HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=clinicas");

                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                    string responseData = await response.Content.ReadAsStringAsync();
                    Acler acler = new Acler();
                    var json = acler.ProcesarJsonInvalido2(responseData);
                    var clinicas = JsonSerializer.Deserialize<List<Clinica>>(json);

                    // Retorna la lista de clínicas envuelta en un ActionResponse
                    return new ActionResponse<IEnumerable<Clinica>>
                    {
                        WasSuccess = true,
                        Result = clinicas
                    };
                }
                else
                {
                    // Si la solicitud no es exitosa, retorna un ActionResponse con un mensaje de error
                    return new ActionResponse<IEnumerable<Clinica>>
                    {
                        WasSuccess = false,
                        Result = null,
                        Message = "Error al obtener las clínicas"
                    };
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retorna un ActionResponse con los detalles del error
                return new ActionResponse<IEnumerable<Clinica>>
                {
                    WasSuccess = false,
                    Result = null,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<IEnumerable<Clinica>> GetClinicaComboAsync()
        {
            try
            {
                // Configura la autenticación básica
                string credencialesBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_usuario}:{_pass}"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credencialesBase64);

                // Realiza la solicitud HTTP GET
                HttpResponseMessage response = await _httpClient.GetAsync(_url + "api_acler.php?action=clinicas");

                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud es exitosa, deserializa la respuesta en una lista de clínicas
                    string responseData = await response.Content.ReadAsStringAsync();
                    Acler acler = new Acler();
                    var json = acler.ProcesarJsonInvalido2(responseData);
                    var clinicas = JsonSerializer.Deserialize<List<Clinica>>(json);

                    // Retorna la lista de clínicas
                    return clinicas;
                }

                // Manejo de errores: puedes retornar una lista vacía si prefieres no lanzar una excepción
                return new List<Clinica>();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Dependiendo del uso, puedes registrar el error y/o lanzar una excepción
                throw new Exception($"Error al obtener datos: {ex.Message}", ex);
            }
        }
    }
}