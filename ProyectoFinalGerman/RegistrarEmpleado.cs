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
    public partial class RegistrarEmpleado : Form
    {
        public RegistrarEmpleado()
        {
            InitializeComponent();

            // Fondo del formulario y groupBox1 (Superior)
            this.BackColor = ColorTranslator.FromHtml("#C5836F");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7A895");
            btnInicio.BackColor = ColorTranslator.FromHtml("#D7A895");
            btnInicio.FlatAppearance.BorderSize = 0;

            // Eliminar bordes de las cajas de texto
            groupBox3.BackColor = ColorTranslator.FromHtml("#C5836F");
            btnRegistrar.BackColor = ColorTranslator.FromHtml("#FFE8D6");
            btnRegistrar.FlatAppearance.BorderSize = 0;
            txtNombre.BorderStyle = BorderStyle.None;
            txtApellido.BorderStyle = BorderStyle.None;
            txtUsuario.BorderStyle = BorderStyle.None;
            txtContraseña.BorderStyle = BorderStyle.None;
            txtRfc.BorderStyle = BorderStyle.None;
            txtCurp.BorderStyle = BorderStyle.None;
            txtEmail.BorderStyle = BorderStyle.None;
            txtEmail2.BorderStyle = BorderStyle.None;
            txtDireccion.BorderStyle = BorderStyle.None;
            txtTelefono.BorderStyle = BorderStyle.None;
            txtTelefono2.BorderStyle = BorderStyle.None;
            txtTipoDeSangre.BorderStyle = BorderStyle.None;

            // Fondo de las cajas de texto del mismo color que el groupBox3
            txtNombre.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtApellido.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtUsuario.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtContraseña.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtRfc.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtCurp.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtEmail.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtEmail2.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtDireccion.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtTelefono.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtTelefono2.BackColor = ColorTranslator.FromHtml("#C5836F");
            txtTipoDeSangre.BackColor = ColorTranslator.FromHtml("#C5836F");
            comboBoxTipoUsuario.BackColor = ColorTranslator.FromHtml("#FFE8D6");
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            Menu mn = new Menu();
            mn.Show();
            this.Hide();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter){
                e.Handled = true;
            }
        }

        private void txtApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtRfc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtCurp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtEmail2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtDireccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtTelefono2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void txtTipoDeSangre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }
    }
}
