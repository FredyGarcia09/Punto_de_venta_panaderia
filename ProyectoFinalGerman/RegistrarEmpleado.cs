using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ProyectoFinalGerman
{
    public partial class RegistrarEmpleado : Form
    {
        public RegistrarEmpleado()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#DAA897");
            groupBox2.BackColor = ColorTranslator.FromHtml("#BFB6AD");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            btnRegistrarUsuario.BackColor = ColorTranslator.FromHtml("#F7E1D5");
            btnRegistrarUsuario.FlatAppearance.BorderSize = 0;
            btnRegresar.FlatAppearance.BorderSize = 0;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void RegistrarEmpleado_Load(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Menu RE = new Menu();
            RE.Show();
            this.Hide();
        }
    }
}
