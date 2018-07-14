using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CapaNegocios
{
     public class N_Aceptar
    {
        public string _CedulaRecep { get; set; }
        public string _TipoCedulaRecep { get; set; }
        public string _FechaEmision { get; set; }
        public decimal _MontoImpuesto { get; set; }
        public decimal _TotalFactura { get; set; }
        public string _CedulaEmisor { get; set; }
        public string _TipoCedulaEmisor { get; set; }
        public string  _Email { get; set; }
        //
        public string getPrueba(string fileXml)
        {
            string xNombre= string.Empty;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileXml);
            //
            XmlNodeList Factura = xDoc.GetElementsByTagName("FacturaElectronica");
            XmlNodeList ticket = xDoc.GetElementsByTagName("TiqueteElectronico");
            if (Factura.Count==1)
            {
                XmlNodeList xLista = ((XmlElement)Factura[0]).GetElementsByTagName("Clave");
                XmlNodeList FechaEmision = ((XmlElement)Factura[0]).GetElementsByTagName("FechaEmision");
                XmlNodeList documentoCedulaRecep = xDoc.GetElementsByTagName("Receptor");
                XmlNodeList cedulaRecep = ((XmlElement)documentoCedulaRecep[0]).GetElementsByTagName("Numero");
                XmlNodeList TipoCedulaRecep = ((XmlElement)documentoCedulaRecep[0]).GetElementsByTagName("Tipo");
                XmlNodeList documentoCedula = xDoc.GetElementsByTagName("Emisor");
                XmlNodeList cedula = ((XmlElement)documentoCedula[0]).GetElementsByTagName("Numero");
                XmlNodeList TipoCedula = ((XmlElement)documentoCedula[0]).GetElementsByTagName("Tipo");
                XmlNodeList EmailEmisor = ((XmlElement)documentoCedula[0]).GetElementsByTagName("CorreoElectronico");
                //
                XmlNodeList ResumenFactura = xDoc.GetElementsByTagName("ResumenFactura");
                XmlNodeList montoImpuesto = ((XmlElement)ResumenFactura[0]).GetElementsByTagName("TotalImpuesto");
                XmlNodeList montoFactura = ((XmlElement)ResumenFactura[0]).GetElementsByTagName("TotalComprobante");
                //
                foreach (XmlElement nodo in xLista) { xNombre = nodo.InnerText; }
                foreach (XmlElement nodo in cedula) { _CedulaEmisor = nodo.InnerText; }
                foreach (XmlElement nodo in TipoCedula) { _TipoCedulaEmisor = nodo.InnerText; }
                foreach (XmlElement nodo in FechaEmision) { _FechaEmision = nodo.InnerText; }
                foreach (XmlElement nodo in montoImpuesto) { _MontoImpuesto =Convert.ToDecimal(nodo.InnerText); }
                foreach (XmlElement nodo in montoFactura) { _TotalFactura = Convert.ToDecimal(nodo.InnerText); }
                foreach (XmlElement nodo in cedulaRecep) { _CedulaRecep = nodo.InnerText; }
                foreach (XmlElement nodo in TipoCedulaRecep) { _TipoCedulaRecep = nodo.InnerText; }
                foreach (XmlElement nodo in EmailEmisor) { _Email = nodo.InnerText; }
            }
            else if (ticket.Count == 1)
            {
                XmlNodeList xLista = ((XmlElement)ticket[0]).GetElementsByTagName("Clave");
                XmlNodeList FechaEmision = ((XmlElement)ticket[0]).GetElementsByTagName("FechaEmision");
                XmlNodeList cedulaRecep = ((XmlElement)ticket[0]).GetElementsByTagName("Numero");
                XmlNodeList TipoCedulaRecep = ((XmlElement)ticket[0]).GetElementsByTagName("Tipo");
                XmlNodeList documentoCedula = xDoc.GetElementsByTagName("Emisor");
                XmlNodeList cedula = ((XmlElement)documentoCedula[0]).GetElementsByTagName("Numero");
                XmlNodeList TipoCedula = ((XmlElement)documentoCedula[0]).GetElementsByTagName("Tipo");
                XmlNodeList EmailEmisor = ((XmlElement)documentoCedula[0]).GetElementsByTagName("CorreoElectronico");
                //
                XmlNodeList ResumenFactura = xDoc.GetElementsByTagName("ResumenFactura");
                XmlNodeList montoImpuesto = ((XmlElement)ResumenFactura[0]).GetElementsByTagName("TotalImpuesto");
                XmlNodeList montoFactura = ((XmlElement)ResumenFactura[0]).GetElementsByTagName("TotalComprobante");
                //
                foreach (XmlElement nodo in xLista) { xNombre = nodo.InnerText; }
                foreach (XmlElement nodo in cedula) { _CedulaEmisor = nodo.InnerText; }
                foreach (XmlElement nodo in TipoCedula) { _TipoCedulaEmisor = nodo.InnerText; }
                foreach (XmlElement nodo in FechaEmision) { _FechaEmision = nodo.InnerText; }
                foreach (XmlElement nodo in montoImpuesto) { _MontoImpuesto = Convert.ToDecimal(nodo.InnerText); }
                foreach (XmlElement nodo in montoFactura) { _TotalFactura = Convert.ToDecimal(nodo.InnerText); }
                foreach (XmlElement nodo in cedulaRecep) { _CedulaRecep = nodo.InnerText; }
                foreach (XmlElement nodo in TipoCedulaRecep) { _TipoCedulaRecep = nodo.InnerText; }
                foreach (XmlElement nodo in EmailEmisor) { _Email = nodo.InnerText; }
            }
            //
            return xNombre;
        }
    }
}
