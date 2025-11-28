using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalGerman
{
    public partial class AgregarProducto : Form
    {
        private int contador = 0;
        public AgregarProducto()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#CD9B81");
            groupBox1.BackColor = ColorTranslator.FromHtml("#E9CEBB");
            groupBox2.BackColor = ColorTranslator.FromHtml("#A8B29B");
            btnAgregarFoto.BackColor = ColorTranslator.FromHtml("#CD9B81");
            btnRegresar.FlatAppearance.BorderSize = 0;
            btnSumar.FlatAppearance.BorderSize = 0;
            btnRestar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnAgregarFoto.FlatAppearance.BorderSize = 0;

        }

        private void btnAgregarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrirDialogo = new OpenFileDialog();
            abrirDialogo.Filter = "Archivos PNG (*.png)|*.png";
            abrirDialogo.Title = "Seleccionar Archivo PNG";
            if (abrirDialogo.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = Image.FromFile(abrirDialogo.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo cargar la imagen: " + ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu RE = new Menu();
            RE.Show();
            this.Hide();
        }

        private void btnSumar_Click(object sender, EventArgs e)
        {
            contador++;
            txtCantidad.Text = contador.ToString();
        }

        private void btnRestar_Click(object sender, EventArgs e)
        {
            contador--;
            txtCantidad.Text = contador.ToString();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Agregarcategoria RE = new Agregarcategoria();
            RE.Show();
            this.Hide();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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
