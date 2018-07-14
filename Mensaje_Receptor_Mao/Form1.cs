using CapaNegocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mensaje_Receptor_Mao
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbTipoMensaje.DataSource = TipoMensaje();
            cbTipoMensaje.DisplayMember = "Nombre";
            cbTipoMensaje.ValueMember = "id";
        }
        DataTable TipoMensaje()
        {
            DataTable tabla = new DataTable();
            //Agregar Columnas
            tabla.Columns.Add("Nombre");
            tabla.Columns.Add("id");
            //agregar Filas
            tabla.Rows.Add("--seleccione--", 0);
            tabla.Rows.Add("Aceptado", 1);
            tabla.Rows.Add("Aceptacion parcial", 2);
            tabla.Rows.Add("Rechazo", 3);
            return tabla;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Documentos XML|*.xml";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblDireccion.Text = openFileDialog1.FileName;
                cargarXml();
            }  
        }
        void cargarXml()
        {
            if (lblDireccion.Text == "No se ha selecionado ningun archivo")
            {
                MessageBox.Show("Debe seleccionar un documento", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            N_Aceptar Aceptar = new N_Aceptar();
            Aceptar.getPrueba(lblDireccion.Text);
            txtClave.Text = Aceptar.getPrueba(lblDireccion.Text);
            txtCedEmisor.Text = Aceptar._CedulaEmisor;
            txtTipoCedEmisor.Text = Aceptar._TipoCedulaEmisor;
            txtFechaEmision.Text = Aceptar._FechaEmision;
            txtTotalImpuesto.Text = Aceptar._MontoImpuesto.ToString("N2");
            txtTotalFactura.Text = Aceptar._TotalFactura.ToString("N2");
            txtCedReceptor.Text = Aceptar._CedulaRecep;
            txtTipoCedReceptor.Text = Aceptar._TipoCedulaRecep;
            txtEmailEmi.Text = Aceptar._Email;
            if (txtClave.Text == string.Empty)
            {
                MessageBox.Show("Parese que el documento no es fatura ni ticket", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            //Validar
            if (cbTipoMensaje.SelectedIndex == 0) { MessageBox.Show("Debe selecionar un tipo de mensaje"); return; }
            //
            decimal totalFactura = Convert.ToDecimal(txtTotalFactura.Text);
            decimal toatalImpuesto = Convert.ToDecimal(txtTotalImpuesto.Text);
            //Crear xmlSin firmar Mensaje Receptor
            string consecutivoRecep = txtCasaTerminal.Text + txtTipo.Text + txtConsecutivoRecep.Text;
            //
            txtClaveConsultar.Text = txtClave.Text + "-" + consecutivoRecep;
            //
            frmEspere Wait = new frmEspere();
            Wait.Show();
            //
           string respuesta= await N_GenerarXmlMR.xmlMR_Sf(txtClave.Text, txtCedEmisor.Text, txtTipoCedEmisor.Text, txtFechaEmision.Text, cbTipoMensaje.SelectedValue.ToString(),
                          txtDetalleMensaje.Text, toatalImpuesto.ToString(), totalFactura.ToString(), txtCedReceptor.Text, txtTipoCedReceptor.Text, consecutivoRecep);
            Wait.Close();
            MessageBox.Show(respuesta);

        }

        private void cbTipoMensaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoMensaje.SelectedIndex == 1) { txtTipo.Text = "05"; }
            if (cbTipoMensaje.SelectedIndex == 2) { txtTipo.Text = "06"; }
            if (cbTipoMensaje.SelectedIndex == 3) { txtTipo.Text = "07"; }
        }

        private void txtClaveConsultar_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
                   string dd = DateTime.Now.ToString("dd");
                   string MM = DateTime.Now.ToString("MM");
                   string yyyy = DateTime.Now.ToString("yyyy");
                   try
                   {
                      string consecutivoRecep = Environment.CurrentDirectory + (@"\MR\" + yyyy + "\\" + MM + "\\" + dd + "\\" 
                                                     + txtCasaTerminal.Text + txtTipo.Text + txtConsecutivoRecep.Text + @"\");
                       System.Diagnostics.Process.Start(consecutivoRecep);
                   }
                   catch (Exception)
                   {

                       MessageBox.Show("Carpeta no encontrada");
                   }

        }
    }
}
