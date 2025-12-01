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
        private List<ItemCarrito> _listaProductos;
        private int _idUsuarioCajero;
        private VentaDAO ventaDAO = new VentaDAO();
        private decimal _totalVenta = 0;

        public Ticket(List<ItemCarrito> listaRecibida, int idUsuario)
        {
            InitializeComponent();
            this._listaProductos = listaRecibida;
            this._idUsuarioCajero = idUsuario;

        }

        private void Ticket_Load(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            lblIdEmpleado.Text = _idUsuarioCajero.ToString();
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
            _totalVenta = 0;

            foreach (var item in _listaProductos)
            {
                dgvTicket.Rows.Add(item.Cantidad, item.Nombre, item.Importe.ToString("C2"));
                _totalVenta += item.Importe;
            }

            lblTotal.Text = _totalVenta.ToString("C2");    
            lblGranTotal.Text = _totalVenta.ToString("C2"); 
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
                decimal cambio = efectivoEntregado - _totalVenta;
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
            foreach (var item in this._listaProductos)
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
                decimal cambio = efectivo - _totalVenta;
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
            // 1. Validar pago
            decimal efectivo = 0;
            decimal.TryParse(txtEfectivo.Text, out efectivo);

            if (efectivo < _totalVenta)
            {
                MessageBox.Show("El efectivo es insuficiente para cubrir el total.", "Pago Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEfectivo.Focus();
                return;
            }

            // 2. Preguntar confirmación (Opcional, para evitar clics dobles)
            if (MessageBox.Show("¿Confirmar venta?", "Procesar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            // 3. LLAMAR AL DAO PARA GUARDAR EN BD
            VentaDAO dao = new VentaDAO();
            int idOrdenGenerada;
            string mensaje;

            bool exito = dao.RealizarVenta(_idUsuarioCajero, _listaProductos, out idOrdenGenerada, out mensaje);

            if (exito)
            {
                MessageBox.Show($"¡Venta registrada con éxito!\nTicket #{idOrdenGenerada}", "Venta Finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra el ticket y vuelve al menú para la siguiente venta
            }
            else
            {
                MessageBox.Show("Error crítico al guardar la venta:\n" + mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
