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
    public partial class EditarProducto : Form
    {
        public EditarProducto()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#CD9B81");
            groupBox1.BackColor = ColorTranslator.FromHtml("#E9CEBB");
            groupBox2.BackColor = ColorTranslator.FromHtml("#A8B29B");
            btnRegresar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnBorrar.FlatAppearance.BorderSize = 0;
            btnAgregarFoto.FlatAppearance.BorderSize = 0;
            btnRestar.FlatAppearance.BorderSize = 0;
            btnSumar.FlatAppearance.BorderSize = 0;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Menu RE = new Menu();
            RE.Show();
            this.Hide();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "¿Estás seguro que quieres borrar este producto?", 
                "Confirmar Eliminación",                           
                MessageBoxButtons.YesNo,                           
                MessageBoxIcon.Warning                             
            );

            if (resultado == DialogResult.Yes)
            {
                MessageBox.Show("Producto eliminado exitosamente."); 

            }
            else
            {
                MessageBox.Show("Operación cancelada.");
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
           
            
            
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Space)
            {
                return;
            }

            if (!char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            if (e.KeyChar == '.')
            {
                if (((TextBox)sender).Text.Contains('.'))
                {
                    e.Handled = true;
                    return;
                }
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
