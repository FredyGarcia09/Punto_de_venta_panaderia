using ProyectoFinalGerman.FRONTEND;
using ProyectoFinalGerman.MODELOS;
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
    public partial class Menu : Form
    {
        private Form _loginForm;
        private bool _esLogout = false;
        public Menu(Form login)
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#F8E7D8");
            groupBox4.BackColor = ColorTranslator.FromHtml("#C9C7B6");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            btnAgregarProducto.FlatAppearance.BorderSize = 0;
            btnEditarProducto.FlatAppearance.BorderSize = 0;
            btnRegistrarEmpleado.FlatAppearance.BorderSize = 0;
            btnRegistrarVenta.FlatAppearance.BorderSize = 0;
            btnReporteCambios.FlatAppearance.BorderSize = 0;
            btnReporteVentas.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnAdministrarUsuarios.FlatAppearance.BorderSize = 0;
            btnAuditoriaProd.FlatAppearance.BorderSize = 0;
            _loginForm = login;

            if (Sesion.Rol != "Administrador")
            {
                btnRegistrarEmpleado.Enabled = false;
                btnAdministrarUsuarios.Enabled = false;
                btnReporteCambios.Enabled = false;
                btnReporteVentas.Enabled = false;

                btnRegistrarEmpleado.Visible = false;
                btnAdministrarUsuarios.Visible = false;
                btnReporteCambios.Visible = false;
                btnReporteVentas.Visible = false;
            }
        }


        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            MenuPan RE = new MenuPan();
            RE.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnReporteVentas_Click(object sender, EventArgs e)
        {
            ReporteVentas RE = new ReporteVentas();
            RE.ShowDialog();
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            AgregarProducto RE = new AgregarProducto();
            RE.ShowDialog();
        }

        private void btnRegistrarEmpleado_Click(object sender, EventArgs e)
        {
            RegistrarEmpleado RE = new RegistrarEmpleado();
            RE.ShowDialog();
        }

        private void btnEditarProducto_Click(object sender, EventArgs e)
        {
            EditarProducto RE = new EditarProducto();
            RE.ShowDialog();
        }

        private void btnReporteCambios_Click(object sender, EventArgs e)
        {
            ReporteCambios RE = new ReporteCambios();
            RE.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditarEmpleado RE = new EditarEmpleado();
            RE.ShowDialog();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            lblNombre.Text = Sesion.NombreCompleto;
            lblRol.Text = Sesion.Rol;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Deseas cerrar sesión?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Sesion.CerrarSesion();
                _esLogout = true;
                _loginForm.Show();
                this.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AuditoriaProductos RE = new AuditoriaProductos();
            RE.ShowDialog();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_esLogout)
            {
                Application.Exit();
            }
        }
    }
}
