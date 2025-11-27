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
        public Menu()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#F8E7D8");
            groupBox4.BackColor = ColorTranslator.FromHtml("#C9C7B6");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            btnAgregarProducto.FlatAppearance.BorderSize = 0;
            btnBorrarProducto.FlatAppearance.BorderSize = 0;
            btnEditarProducto.FlatAppearance.BorderSize = 0;
            btnRegistrarAdmin.FlatAppearance.BorderSize = 0;
            btnRegistrarEmpleado.FlatAppearance.BorderSize = 0;
            btnRegistrarVenta.FlatAppearance.BorderSize = 0;
            btnReporteCambios.FlatAppearance.BorderSize = 0;
            btnReporteVentas.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.BorderSize = 0;
        }


        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            MenuPan RE = new MenuPan();
            RE.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnRegistrarEmpleado_Click(object sender, EventArgs e)
        {
            RegistrarEmpleado ReEm = new RegistrarEmpleado();
            ReEm.Show();
            this.Hide();
        }
    }
}
