
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CapaNegocios
{
    class N_Comunicacion
    {
        public XmlDocument xmlRespuesta { get; set; }
        public string jsonEnvio { get; set; }
        public string jsonRespuesta { get; set; }
        public string mensajeRespuesta { get; set; }
        public string estadoFactura { get; set; }
        public string statusCode { get; set; }
        string URL_RECEPCION = "https://api.comprobanteselectronicos.go.cr/recepcion-sandbox/v1/";
        //
        public void EnvioDatos(string TK, Recepcion objRecepcion)
        {
            try
            {

                HttpClient http = new HttpClient();
                Newtonsoft.Json.Linq.JObject JsonObject = new Newtonsoft.Json.Linq.JObject();
                JsonObject.Add(new JProperty("clave", objRecepcion.clave));
                JsonObject.Add(new JProperty("fecha", objRecepcion.fecha));
                JsonObject.Add(new JProperty("emisor",
                                             new JObject(new JProperty("tipoIdentificacion", objRecepcion.emisor.TipoIdentificacion),
                                                         new JProperty("numeroIdentificacion", objRecepcion.emisor.numeroIdentificacion))));
                JsonObject.Add(new JProperty("receptor",
                                             new JObject(new JProperty("tipoIdentificacion", objRecepcion.receptor.TipoIdentificacion),
                                                         new JProperty("numeroIdentificacion", objRecepcion.receptor.numeroIdentificacion))));

                JsonObject.Add(new JProperty("consecutivoReceptor", objRecepcion.consecutivoReceptor));

                JsonObject.Add(new JProperty("comprobanteXml", objRecepcion.comprobanteXml));

                jsonEnvio = JsonObject.ToString();

                StringContent oString = new StringContent(JsonObject.ToString());

                http.DefaultRequestHeaders.Add("authorization", ("Bearer " + TK));

                HttpResponseMessage response = http.PostAsync((URL_RECEPCION + "recepcion"), oString).Result;
                if (response.StatusCode.ToString() == "ServiceUnavailable" || response.StatusCode.ToString() == "BadGateway")
                {
                    mensajeRespuesta = "Servidor de Hacienda caído";
                }
                else
                {
                    Thread.Sleep(5000);//Tiempo de 3 segundos para esperar aprobacion o rechazo de hacienda
                    string consultaClave = objRecepcion.clave + "-" + objRecepcion.consecutivoReceptor;
                    ConsultaEstatus(TK, consultaClave);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async void ConsultaEstatus(string TK, string claveConsultar)
        {
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("authorization", ("Bearer " + TK));

            HttpResponseMessage response = http.GetAsync((URL_RECEPCION + ("recepcion/" + claveConsultar))).Result;

            HttpResponseHeaders responseHeadersCollection = response.Headers;

            IEnumerable<string> values;
            if (responseHeadersCollection.TryGetValues("X-Error-Cause", out values))
            {
                // var forcast = await response.Content.ReadAsStringAsync();
            }
            string res = await response.Content.ReadAsStringAsync();

            jsonRespuesta = res.ToString();

            RespuestaHacienda RH = Newtonsoft.Json.JsonConvert.DeserializeObject<RespuestaHacienda>(res);
            estadoFactura = RH.ind_estado;
            if ((RH.respuesta_xml != null))
            {
                xmlRespuesta = N_Funciones.DecodeBase64ToXML(RH.respuesta_xml);
            }
            estadoFactura = RH.ind_estado;
            statusCode = response.StatusCode.ToString();
            mensajeRespuesta = ("Confirmación: " + (statusCode + "\n") + ("Estado: " + estadoFactura));
        }
    }
}
