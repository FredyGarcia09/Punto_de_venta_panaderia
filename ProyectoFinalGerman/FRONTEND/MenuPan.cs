using ProyectoFinalGerman.BACKEND.DAOs;
using ProyectoFinalGerman.FRONTEND.Helpers;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ProyectoFinalGerman
{
    public partial class MenuPan : Form
    {
        private ProductoDAO productoDAO = new ProductoDAO();
        public MenuPan()
        {
            InitializeComponent();
            groupBox1.BackColor = ColorTranslator.FromHtml("#C9C7B6");
            groupBox3.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            groupBox2.BackColor = ColorTranslator.FromHtml("#C89B84");
            flpMenuPrincipal.BackColor = EstilosUI.ColorFondoGrid;
            btnMenu.FlatAppearance.BorderSize = 0;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.BorderSize = 0;

            // PRUEBA EN EL PANEL NUEVO
            ControlProducto prueba = new ControlProducto();
            prueba.BackColor = Color.Red;
            prueba.Size = new Size(150, 200);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string nombreProducto = dataGridView1.SelectedRows[0].Cells["Producto"].Value.ToString();

                DialogResult respuesta = MessageBox.Show(
                    $"¿Seguro que deseas eliminar '{nombreProducto}' de la cuenta?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    CalcularTotalGeneral();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto de la tabla para eliminarlo.");
            }
        }
        
        private void panelMenuOpciones_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuPan_Load(object sender, EventArgs e)
        {
            CargarMenuDinamico();
            ConfigurarTablaCuenta();
        }
        private void ConfigurarTablaCuenta()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Producto", "Producto");
            dataGridView1.Columns.Add("Precio", "Precio");
            dataGridView1.Columns.Add("Cantidad", "Cant.");
            dataGridView1.Columns.Add("Importe", "Importe");

            dataGridView1.Columns["Id"].Visible = false;

            // Formato de moneda
            dataGridView1.Columns["Precio"].DefaultCellStyle.Format = "C2";
            dataGridView1.Columns["Importe"].DefaultCellStyle.Format = "C2";

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            EstilosUI.AplicarEstiloGrid(dataGridView1);
        }

        private void CargarMenuDinamico()
        {
            // Panel principal
            flpMenuPrincipal.Controls.Clear();
            flpMenuPrincipal.FlowDirection = FlowDirection.TopDown; // Lista vertical
            flpMenuPrincipal.WrapContents = false; // No permitir que se pongan al lado
            flpMenuPrincipal.AutoScroll = true;

            // Traer datos
            List<Producto> listaProductos;
            try
            {
                listaProductos = productoDAO.ObtenerProductosParaMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error SQL: " + ex.Message);
                return;
            }

            if (listaProductos == null || listaProductos.Count == 0) return;

            // Variables de control
            string categoriaActual = "";
            FlowLayoutPanel flpGridProductos = null;

            int anchoDisponible = flpMenuPrincipal.ClientSize.Width - 30;

            foreach (Producto prod in listaProductos)
            {
                // Por categoria
                if (prod.NombreCategoria != categoriaActual)
                {
                    categoriaActual = prod.NombreCategoria;

                    FlowLayoutPanel flpContenedorCategoria = new FlowLayoutPanel();
                    flpContenedorCategoria.FlowDirection = FlowDirection.TopDown;
                    flpContenedorCategoria.AutoSize = true;
                    flpContenedorCategoria.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    flpContenedorCategoria.Width = anchoDisponible;
                    flpContenedorCategoria.Margin = new Padding(0, 0, 0, 20); // Espacio entre categorías

                    // Titulo
                    Label lblTitulo = new Label();
                    lblTitulo.Text = "  " + categoriaActual.ToUpper();
                    lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    lblTitulo.ForeColor = Color.DarkSlateGray;
                    lblTitulo.BackColor = EstilosUI.ColorFondoGrid;
                    lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
                    lblTitulo.Height = 40;
                    lblTitulo.Width = anchoDisponible;
                    lblTitulo.Margin = new Padding(0);

                    // Productos
                    flpGridProductos = new FlowLayoutPanel();
                    flpGridProductos.FlowDirection = FlowDirection.LeftToRight; // Izquierda a Derecha
                    flpGridProductos.WrapContents = true; // Que bajen si no caben
                    flpGridProductos.AutoSize = true;
                    flpGridProductos.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    flpGridProductos.Width = anchoDisponible;
                    flpGridProductos.BackColor = EstilosUI.ColorSeleccion;
                    flpGridProductos.Padding = new Padding(10);

                    
                    flpContenedorCategoria.Controls.Add(lblTitulo);
                    flpContenedorCategoria.Controls.Add(flpGridProductos);

                    flpMenuPrincipal.Controls.Add(flpContenedorCategoria);
                }

                // Tarjeta de producto
                ControlProducto tarjeta = new ControlProducto();
                tarjeta.CargarDatos(prod.IdProducto, prod.NombreProducto, prod.Precio, prod.FotoProducto);

                tarjeta.Size = new Size(160, 200);
                tarjeta.Visible = true;
                tarjeta.Margin = new Padding(10); // Espacio entre tarjetas

                tarjeta.Click += AgregarAlCarrito_Click;

                if (flpGridProductos != null)
                {
                    flpGridProductos.Controls.Add(tarjeta);
                }
            }
        }

        private void AgregarAlCarrito_Click(object sender, EventArgs e)
        {
            ControlProducto tarjeta = sender as ControlProducto;

            if (tarjeta != null)
            {
                int id = tarjeta.IdProducto;
                string nombre = tarjeta.Nombre;
                decimal precio = tarjeta.Precio;
                bool existe = false;

                // Buscamos si ya existe en el Grid para sumar cantidad
                foreach (DataGridViewRow fila in dataGridView1.Rows)
                {
                    if (fila.Cells["Id"].Value != null && Convert.ToInt32(fila.Cells["Id"].Value) == id)
                    {
                        int cantidad = Convert.ToInt32(fila.Cells["Cantidad"].Value) + 1;
                        fila.Cells["Cantidad"].Value = cantidad;
                        fila.Cells["Importe"].Value = (cantidad * precio);
                        existe = true;
                        break;
                    }
                }

                // Si no existe, lo agregamos nuevo
                if (!existe)
                {
                    dataGridView1.Rows.Add(id, nombre, precio, 1, precio);
                }

                CalcularTotalGeneral();
            }
        }

        private void CalcularTotalGeneral()
        {
            decimal total = 0;
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["Importe"].Value != null)
                    total += Convert.ToDecimal(fila.Cells["Importe"].Value);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || (dataGridView1.Rows.Count == 1 && dataGridView1.Rows[0].IsNewRow))
            {
                MessageBox.Show("El carrito está vacío, favor de revisar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            List<ItemCarrito> listaParaTicket = new List<ItemCarrito>();

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["Id"].Value != null)
                {
                    ItemCarrito item = new ItemCarrito();
                    item.IdProducto = Convert.ToInt32(fila.Cells["Id"].Value);
                    item.Nombre = fila.Cells["Producto"].Value.ToString();
                    item.Precio = Convert.ToDecimal(fila.Cells["Precio"].Value);
                    item.Cantidad = Convert.ToInt32(fila.Cells["Cantidad"].Value);

                    listaParaTicket.Add(item);
                }
            }

            Ticket frmTicket = new Ticket(listaParaTicket, 1);
            frmTicket.Show();
            dataGridView1.Rows.Clear();
            CalcularTotalGeneral(); 

        }

    }
}
