using ProyectoFinalGerman.BACKEND.DAOs;
using ProyectoFinalGerman.MODELOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalGerman
{
    public partial class AgregarProducto : Form
    {
        private int contador = 0;
        private byte[] imagenBytes = null;
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.Title = "Seleccionar foto del producto";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbFoto.Image = Image.FromFile(ofd.FileName);
                pbFoto.SizeMode = PictureBoxSizeMode.Zoom;

                using (MemoryStream ms = new MemoryStream())
                {
                    pbFoto.Image.Save(ms, ImageFormat.Png);
                    imagenBytes = ms.ToArray();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSumar_Click(object sender, EventArgs e)
        {
            contador++;
            txtCantidad.Text = contador.ToString();
        }

        private void btnRestar_Click(object sender, EventArgs e)
        {
            if (contador > 0)
            {
                contador--;
                txtCantidad.Text = contador.ToString();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // VALIDACIONES
            if (string.IsNullOrWhiteSpace(txtNombreProducto.Text) ||
                string.IsNullOrWhiteSpace(txtPrecioProducto.Text))
            {
                MessageBox.Show("El Nombre y el Precio son obligatorios.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboCategoria.SelectedIndex == -1 || cboCategoria.SelectedValue == null)
            {
                DialogResult result = MessageBox.Show(
                    "No has seleccionado una categoría.\n\n¿Deseas crear una nueva?",
                    "Categoría Faltante", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    AbrirFormularioCategoria();
                    return;
                }
                else
                {
                    return;
                }
            }

            try
            {
                Producto prod = new Producto();
                prod.NombreProducto = txtNombreProducto.Text.Trim();
                prod.Descripcion = rtbDescripcionProducto.Text.Trim();

                decimal precio = 0;
                decimal.TryParse(txtPrecioProducto.Text, out precio);
                prod.Precio = precio;

                int stock = 0;
                int.TryParse(txtCantidad.Text, out stock);
                prod.Existencias = stock;

                // Obtener ID del Combo
                prod.IdCategoria = Convert.ToInt32(cboCategoria.SelectedValue);

                prod.FotoProducto = imagenBytes;

                // Guardar en DB
                ProductoDAO dao = new ProductoDAO();
                string mensaje;
                string usuarioActual = Sesion.UsuarioAcceso;

                if (dao.InsertarProducto(prod, usuarioActual, out mensaje))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AbrirFormularioCategoria();
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

        private void AgregarProducto_Load(object sender, EventArgs e)
        {
            txtCantidad.Text = "0";

            CargarCategorias();
        }

        private void CargarCategorias()
        {
            ProductoDAO dao = new ProductoDAO();
            DataTable dtCategorias = dao.ObtenerCategorias();

            if (dtCategorias.Rows.Count > 0)
            {
                cboCategoria.DataSource = dtCategorias;
                cboCategoria.DisplayMember = "nombreCategoria";
                cboCategoria.ValueMember = "IdCategoria";
                cboCategoria.SelectedIndex = -1;
            }
            else
            {
                // No hay categoirías
                DialogResult result = MessageBox.Show(
                    "No existen categorías registradas.\nNo se puede registrar un producto sin categoría.\n\n¿Deseas crear una ahora?",
                    "Faltan Datos",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    AbrirFormularioCategoria();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void AbrirFormularioCategoria()
        {
            Agregarcategoria RE = new Agregarcategoria();
            RE.ShowDialog();
            CargarCategorias();
        }


        private void LimpiarFormulario()
        {
            txtNombreProducto.Clear();
            txtPrecioProducto.Clear();
            rtbDescripcionProducto.Clear();
            contador = 0;
            txtCantidad.Text = contador.ToString();
            pbFoto.Image = null;
            imagenBytes = null;
            cboCategoria.SelectedIndex = -1;
            txtNombreProducto.Focus();
        }

    }
}
