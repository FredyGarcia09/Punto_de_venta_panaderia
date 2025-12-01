using ProyectoFinalGerman.BACKEND.DAOs;
using ProyectoFinalGerman.MODELOS;     
using ProyectoFinalGerman.MODELOS.ITEMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalGerman
{
    public partial class Ticket : Form
    {
        private List<ItemCarrito> listaProductos;
        private int idUsuarioCajero;
        private VentaDAO ventaDAO = new VentaDAO();
        private decimal totalVenta = 0;
        public Ticket(List<ItemCarrito> listaRecibida, int idUsuario)
        {
            InitializeComponent();
            this.listaProductos = listaRecibida;
            this.idUsuarioCajero = idUsuario;
        }

        private void Ticket_Load(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            lblIdEmpleado.Text = idUsuarioCajero.ToString();
            LlenarTablaYCalcularTotal();
            //ProcesarVentaEnBD();
            txtEfectivo.Select();
        }

        private void LlenarTablaYCalcularTotal()
        {
            if (dgvTicket.Columns.Count == 0)
            {
                dgvTicket.Columns.Add("Cant", "Cant");
                dgvTicket.Columns.Add("Producto", "Producto");
                dgvTicket.Columns.Add("Importe", "Importe");
                dgvTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            dgvTicket.Rows.Clear();
            totalVenta = 0;

            foreach (var item in listaProductos)
            {
                dgvTicket.Rows.Add(item.Cantidad, item.Nombre, item.Importe.ToString("C2"));
                totalVenta += item.Importe;
            }

            lblTotal.Text = totalVenta.ToString("C2");    
            lblGranTotal.Text = totalVenta.ToString("C2"); 
        }

        // EVENTO PARA CALCULAR CAMBIO 
        private void txtEfectivo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEfectivo.Text))
                {
                    lblCambio.Text = "$0.00";
                    return;
                }

                decimal efectivoEntregado = Convert.ToDecimal(txtEfectivo.Text);
                decimal cambio = efectivoEntregado - totalVenta;
                lblCambio.Text = cambio.ToString("C2");

                if (cambio < 0)
                    lblCambio.ForeColor = Color.Red;
                else
                    lblCambio.ForeColor = Color.Black; 
            }
            catch
            {
                lblCambio.Text = "Error";
            }
        }

        private void CalcularTotales()
        {
            decimal subtotal = 0;
            foreach (var item in this.listaProductos)
            {
                subtotal += item.Importe;
            }
            lblTotal.Text = subtotal.ToString("C2");
        }

        private void txtEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }

            if (e.KeyChar == '.')
            {
                if ((sender as TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true; 
                }
                return;
            }
            e.Handled = true;
        }

        private void txtEfectivo_TextChanged_1(object sender, EventArgs e)
        {
            decimal efectivo = 0;

            if (decimal.TryParse(txtEfectivo.Text, out efectivo))
            {
                decimal cambio = efectivo - totalVenta;
                lblCambio.Text = cambio.ToString("C2");
                if (cambio < 0)
                {
                    lblCambio.ForeColor = Color.Red;
                }
                else
                {
                    lblCambio.ForeColor = Color.Black; 
                }
            }
            else
            {
                lblCambio.Text = "$0.00";
            }
        }

        private void btnRegresa_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            string mensajeResultado = "";

            bool ventaExitosa = ventaDAO.GuardarVentaCompleta(idUsuarioCajero, listaProductos, out mensajeResultado);

            if (ventaExitosa)
            {
                MessageBox.Show(mensajeResultado, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show(mensajeResultado, "Error en la venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
