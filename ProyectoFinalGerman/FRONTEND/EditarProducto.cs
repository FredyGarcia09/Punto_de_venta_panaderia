using ProyectoFinalGerman.BACKEND.DAOs;
using ProyectoFinalGerman.MODELOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalGerman
{
    public partial class EditarProducto : Form
    {
        private byte[] imagenBytesActual = null;
        private bool cargandoDatos = false;
        private bool productoActualDescontinuado = false;

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

            if(Sesion.Rol != "Administrador")
            {
                btnBorrar.Enabled = false;
                btnBorrar.Visible = false;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (cboProductos.SelectedIndex == -1 || cboProductos.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un producto primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProducto = Convert.ToInt32(cboProductos.SelectedValue);
            string nombreProducto = txtNombre.Text;

            string advertencia = $"⚠ ADVERTENCIA DE SEGURIDAD ⚠\n\n" +
                                 $"Estás a punto de ELIMINAR PERMANENTEMENTE el producto: '{nombreProducto}'.\n\n" +
                                 $"ALERTA: Esto eliminará también TODOS los registros de ventas históricos donde aparezca este producto.\n" +
                                 $"Tus reportes financieros del pasado podrían cuadrar mal si haces esto.\n\n" +
                                 $"Lo recomendable es usar el botón 'Descontinuar'.\n\n" +
                                 $"¿Estás absolutamente seguro de querer eliminarlo?";

            DialogResult respuesta = MessageBox.Show(advertencia,
                                                     "Confirmar Eliminación Física",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Error,
                                                     MessageBoxDefaultButton.Button2);

            if (respuesta == DialogResult.Yes)
            {
                ProductoDAO dao = new ProductoDAO();
                string mensaje;

                if (dao.EliminarProducto(idProducto, Sesion.UsuarioAcceso, out mensaje))
                {
                    MessageBox.Show(mensaje, "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cargandoDatos = true;
                    CargarListaProductos();
                    cargandoDatos = false;
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboProductos.SelectedIndex == -1) return;

            try
            {
                Producto prod = new Producto();
                prod.IdProducto = Convert.ToInt32(cboProductos.SelectedValue);
                prod.NombreProducto = txtNombre.Text.Trim();
                prod.Descripcion = rtbDescripcionProducto.Text.Trim();
                prod.Precio = Convert.ToDecimal(txtPrecio.Text);
                prod.Existencias = Convert.ToInt32(txtCantidad.Text);
                prod.IdCategoria = Convert.ToInt32(cboCategoria.SelectedValue);
                prod.FotoProducto = imagenBytesActual;

                ProductoDAO dao = new ProductoDAO();
                string msg;
                if (dao.ActualizarProducto(prod, Sesion.UsuarioAcceso, out msg))
                {
                    MessageBox.Show(msg, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Recargar lista
                    cargandoDatos = true;
                    CargarListaProductos();
                    cargandoDatos = false;
                }
                else
                {
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }


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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cargandoDatos = true;
            CargarListaProductos();
            cargandoDatos = false;
        }

        private void cboProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoDatos || cboProductos.SelectedIndex == -1) return;

            try
            {
                int idProd = Convert.ToInt32(cboProductos.SelectedValue);
                ProductoDAO dao = new ProductoDAO();
                Producto prod = dao.ObtenerPorId(idProd);

                if (prod != null)
                {
                    txtNombre.Text = prod.NombreProducto;
                    txtPrecio.Text = prod.Precio.ToString();
                    txtCantidad.Text = prod.Existencias.ToString();
                    rtbDescripcionProducto.Text = prod.Descripcion;
                    cboCategoria.SelectedValue = prod.IdCategoria;

                    productoActualDescontinuado = prod.Descontinuado;

                    ActualizarBotonEstado();

                    if (prod.FotoProducto != null && prod.FotoProducto.Length > 0)
                    {
                        imagenBytesActual = prod.FotoProducto;
                        using (MemoryStream ms = new MemoryStream(prod.FotoProducto))
                        {
                            pbFoto.Image = Image.FromStream(ms);
                            pbFoto.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                    else
                    {
                        pbFoto.Image = null;
                        imagenBytesActual = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }

        private void EditarProducto_Load(object sender, EventArgs e)
        {
            cargandoDatos = true;
            CargarCategorias();
            chkMostrarDescontinuados.Checked = false;
            CargarListaProductos();
            cargandoDatos = false;
        }

        private void CargarCategorias()
        {
            ProductoDAO dao = new ProductoDAO();
            cboCategoria.DataSource = dao.ObtenerCategorias();
            cboCategoria.DisplayMember = "nombreCategoria";
            cboCategoria.ValueMember = "IdCategoria";
        }

        private void CargarListaProductos()
        {
            ProductoDAO dao = new ProductoDAO();

            DataTable dt = dao.ObtenerTodos(chkMostrarDescontinuados.Checked);

            if (dt.Rows.Count == 0 && !chkMostrarDescontinuados.Checked)
            {
                if (cboProductos.Items.Count == 0)
                {
                    MessageBox.Show("No hay productos activos para mostrar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Nombre formateado que viene de MYSQL (NombreCompletoDisplay)
            cboProductos.DataSource = dt;
            cboProductos.DisplayMember = "NombreCompletoDisplay";
            cboProductos.ValueMember = "IdProducto";

            cboProductos.SelectedIndex = -1;
            LimpiarCampos();
        }

        private void ActualizarBotonEstado()
        {
            if (productoActualDescontinuado)
            {
                btnCambiarEstado.Text = "Dar de Alta (Reactivar)";
                btnCambiarEstado.BackColor = Color.LightGreen;
            }
            else
            {
                btnCambiarEstado.Text = "Descontinuar Producto";
                btnCambiarEstado.BackColor = Color.LightSalmon;
            }
        }

        private void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            if (cboProductos.SelectedIndex == -1) return;

            int id = Convert.ToInt32(cboProductos.SelectedValue);
            ProductoDAO dao = new ProductoDAO();
            string msg;

            bool nuevoEstado = !productoActualDescontinuado;

            if (dao.CambiarEstado(id, nuevoEstado, "Admin", out msg))
            {
                MessageBox.Show(msg, "Estado Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cargandoDatos = true;
                CargarListaProductos();
                cargandoDatos = false;
            }
            else
            {
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imágenes|*.jpg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbFoto.Image = Image.FromFile(ofd.FileName);
                pbFoto.SizeMode = PictureBoxSizeMode.Zoom;
                using (MemoryStream ms = new MemoryStream())
                {
                    pbFoto.Image.Save(ms, ImageFormat.Png);
                    imagenBytesActual = ms.ToArray();
                }
            }
        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtCantidad.Clear();
            rtbDescripcionProducto.Clear();
            pbFoto.Image = null;
            btnCambiarEstado.Text = "Cambiar Estado";
            btnCambiarEstado.BackColor = SystemColors.Control;
        }

        private void btnSumar_Click(object sender, EventArgs e)
        {
            int c = 0; 
            int.TryParse(txtCantidad.Text, out c); 
            txtCantidad.Text = (++c).ToString();
        }

        private void btnRestar_Click(object sender, EventArgs e)
        {
            int c = 0; 
            int.TryParse(txtCantidad.Text, out c); 
            if (c > 0) txtCantidad.Text = (--c).ToString();
        }
    }
}
