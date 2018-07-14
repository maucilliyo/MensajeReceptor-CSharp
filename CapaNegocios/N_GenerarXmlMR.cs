using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CapaNegocios
{
    public class N_GenerarXmlMR
    {
        public static XmlDocument xmlEnvia = new XmlDocument();
        static string rptEstado = "";
        //
        public static async Task<string> xmlMR_Sf(string ClaveDoc, string NumeroCedulaEmisor, string tipoCedEmisor, string FechaEmisionDoc, string Mensaje,
             string DetalleMensaje, string TotalImpuesto, string TotalFactura, string NumeroCedulaReceptor, string tipoCedReceptor, string NumeroConsecutivoReceptor)
        {
            await Task.Run(() =>
               { //
                   XmlDocument doc = new XmlDocument();
                   XmlDeclaration xmlDecalracion = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                   XmlNode root = doc.DocumentElement;
                   doc.InsertBefore(xmlDecalracion, root);
                   //Nodo Raiz Factura electronica
                   XmlElement raiz = doc.CreateElement("MensajeReceptor");
                   doc.AppendChild(raiz);
                   //
                   //******__INICIO DEL DOCUEMENTO____******\\\\\\
                   //Clave
                   //Fecha
                   string dd = DateTime.Now.ToString("dd");
                   string MM = DateTime.Now.ToString("MM");
                   string yyyy = DateTime.Now.ToString("yyyy");
                   //
                   string fechaEmision = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                   //Clave documento
                   XmlElement Clave = doc.CreateElement("Clave");
                   Clave.AppendChild(doc.CreateTextNode(ClaveDoc));
                   raiz.AppendChild(Clave);
                   //Numero de cedula emisor
                   XmlElement numCedEmisor = doc.CreateElement("NumeroCedulaEmisor");
                   numCedEmisor.AppendChild(doc.CreateTextNode(NumeroCedulaEmisor));
                   raiz.AppendChild(numCedEmisor);
                   //Fecha de emision del documento
                   XmlElement fechaDelDocumento = doc.CreateElement("FechaEmisionDoc");
                   fechaDelDocumento.AppendChild(doc.CreateTextNode(fechaEmision));
                   raiz.AppendChild(fechaDelDocumento);
                   //Tipo de mensaje Documento 1 Aceptado, 2 aceptado parcialmente, 3 rechazado
                   XmlElement tipoMensaje = doc.CreateElement("Mensaje");
                   tipoMensaje.AppendChild(doc.CreateTextNode(Mensaje));
                   raiz.AppendChild(tipoMensaje);
                   //Detalle del mensaje Texto
                   XmlElement detMensaje = doc.CreateElement("DetalleMensaje");
                   detMensaje.AppendChild(doc.CreateTextNode(DetalleMensaje));
                   raiz.AppendChild(detMensaje);
                   //Total del impuesto del Documento
                   XmlElement montoImpuesto = doc.CreateElement("MontoTotalImpuesto");
                   montoImpuesto.AppendChild(doc.CreateTextNode(TotalImpuesto));
                   raiz.AppendChild(montoImpuesto);
                   //Total del Factura del documento
                   XmlElement montoTotalFactura = doc.CreateElement("TotalFactura");
                   montoTotalFactura.AppendChild(doc.CreateTextNode(TotalFactura));
                   raiz.AppendChild(montoTotalFactura);
                   //Num cedula Receptor
                   XmlElement cedReceptor = doc.CreateElement("NumeroCedulaReceptor");
                   cedReceptor.AppendChild(doc.CreateTextNode(NumeroCedulaReceptor));
                   raiz.AppendChild(cedReceptor);
                   //Consecutivo receptor
                   XmlElement ConsecutivoReceptor = doc.CreateElement("NumeroConsecutivoReceptor");
                   ConsecutivoReceptor.AppendChild(doc.CreateTextNode(NumeroConsecutivoReceptor));
                   raiz.AppendChild(ConsecutivoReceptor);

                   //Atributos
                   const string iu = "https://tribunet.hacienda.go.cr/docs/esquemas/2017/v4.2/mensajeReceptor",
                       xsi = "http://www.w3.org/2001/XMLSchema-instance";
                   //
                   XmlAttribute thlocopace = doc.CreateAttribute("xmlns");
                   thlocopace.Value = iu;
                   raiz.SetAttributeNode(thlocopace);
                   //
                   XmlAttribute xsiNamespace = doc.CreateAttribute("xmlns:xsi");
                   xsiNamespace.Value = xsi;
                   raiz.SetAttributeNode(xsiNamespace);
                   //
                   raiz.SetAttribute("schemaLocation", xsi,
                   @"https://tribunet.hacienda.go.cr/docs/esquemas/2017/v4.2/mensajeReceptor https://tribunet.hacienda.go.cr/docs/esquemas/2016/v4.2/MensajeReceptor_4.2.xsd");
                   //
                   doc.AppendChild(raiz);
                   ////****___FINAL____*****\\\\
                   string carpetaFactura = (@"MR\" + yyyy + "\\" + MM + "\\" + dd + "\\" + NumeroConsecutivoReceptor + @"\");
                   Directory.CreateDirectory(carpetaFactura);

                   xmlEnvia = doc;
                   XmlTextWriter xmlTextWriter = new XmlTextWriter((carpetaFactura + (NumeroConsecutivoReceptor + "_01_SF.xml")), new System.Text.UTF8Encoding(false));
                   xmlEnvia.WriteTo(xmlTextWriter);
                   xmlTextWriter.Close();
                   //Firmar Xml
                   N_Firma firmarXml = new N_Firma();
                   firmarXml.FirmaXML_Xades(carpetaFactura + (NumeroConsecutivoReceptor), N_Datos_Receptor.certificado);
                   //
                   string directorio = carpetaFactura + NumeroConsecutivoReceptor;
                   string archivoXml = directorio + "_02_Firmado.xml";//Carga el archivo firmado

                   rptEstado = enviarHecienda(ClaveDoc, archivoXml, tipoCedEmisor, NumeroCedulaEmisor, tipoCedReceptor, NumeroCedulaReceptor, NumeroConsecutivoReceptor, directorio);
               });
            return rptEstado;
        }
        public static string enviarHecienda(string clave, string archivoXml, string tipoCedEmisor, string cedEmisor, string tipoCedRecep, string cedRecep,
                                 string consecutivoRecep, string directorio)
        {
            xmlEnvia.Load(archivoXml);
            N_Comunicacion enviaFactura = new N_Comunicacion();
            Emisor myEmisor = new Emisor();
            myEmisor.numeroIdentificacion = cedEmisor;
            myEmisor.TipoIdentificacion = tipoCedEmisor;

            Receptor myReceptor = new Receptor();
            myReceptor.numeroIdentificacion = cedRecep;
            myReceptor.TipoIdentificacion = tipoCedRecep;

            Recepcion myRecepcion = new Recepcion();
            myRecepcion.emisor = myEmisor;
            myRecepcion.receptor = myReceptor;

            myRecepcion.clave = clave;
            myRecepcion.fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            myRecepcion.comprobanteXml = N_Funciones.EncodeStrToBase64(xmlEnvia.OuterXml);
            myRecepcion.consecutivoReceptor = consecutivoRecep;

            string Token = "";
            Token = getToken();


            enviaFactura.EnvioDatos(Token, myRecepcion);
            string jsonEnvio = "";
            jsonEnvio = enviaFactura.jsonEnvio;
            string jsonRespuesta = "";
            jsonRespuesta = enviaFactura.jsonRespuesta;
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter((directorio + "_03_jsonEnvio.txt"));
            outputFile.Write(jsonEnvio);
            outputFile.Close();
            outputFile = new System.IO.StreamWriter((directorio + "_04_jsonRespuesta.txt"));
            outputFile.Write(jsonRespuesta);
            outputFile.Close();

            if (!(enviaFactura.xmlRespuesta == null))
            {
                enviaFactura.xmlRespuesta.Save((directorio + "_05_RESP.xml"));
            }
            else
            {
                outputFile = new System.IO.StreamWriter((directorio + "_05_RESP_SinRespuesta.txt"));
                outputFile.Write("");
                outputFile.Close();
            }
            return enviaFactura.mensajeRespuesta;
        }
        public static string getToken()
        {
            try
            {
                N_TokenHacienda iTokenHacienda = new N_TokenHacienda();
                iTokenHacienda.GetTokenHacienda(N_Datos_Receptor.APIUser, N_Datos_Receptor.APIClave);
                return iTokenHacienda.accessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
