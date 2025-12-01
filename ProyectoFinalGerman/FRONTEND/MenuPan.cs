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
            btnMenu.FlatAppearance.BorderSize = 0;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.BorderSize = 0;

            // PRUEBA EN EL PANEL NUEVO
            ControlProducto prueba = new ControlProducto();
            prueba.BackColor = Color.Red;
            prueba.Size = new Size(150, 200);

            // Si esto funciona, verás un cuadro rojo
            flpMenuPrincipal.Controls.Add(prueba);
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
            Menu RE = new Menu();
            RE.Show();
            this.Hide();
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
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarMenuDinamico()
        {
            // 1. Limpiar pantalla
            flpMenuPrincipal.Controls.Clear();

            // Configuración visual obligatoria
            flpMenuPrincipal.FlowDirection = FlowDirection.TopDown;
            flpMenuPrincipal.WrapContents = false;
            flpMenuPrincipal.AutoScroll = true;

            // 2. Obtener datos de la BD
            List<Producto> listaProductos = null;
            try
            {
                listaProductos = productoDAO.ObtenerProductosParaMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
                return;
            }

            if (listaProductos == null || listaProductos.Count == 0)
            {
                return; // No hay productos
            }

            // --- VARIABLES DE CONTROL (TIENEN QUE ESTAR AFUERA DEL FOREACH) ---
            string categoriaActual = "";
            FlowLayoutPanel flpCuadricula = null;

            // ... dentro de CargarMenuDinamico ...

            int contadorVisual = 0; // Para contar qué está pasando
            int anchoDisponible = flpMenuPrincipal.ClientSize.Width - 25;

            foreach (Producto prod in listaProductos)
            {
                // A. CATEGORÍA
                if (prod.NombreCategoria != categoriaActual)
                {
                    categoriaActual = prod.NombreCategoria;

                    // Creamos el panel y lo pintamos de VERDE para verlo
                    Panel pnlCategoria = new Panel();
                    pnlCategoria.MinimumSize = new Size(500, 0);
                    pnlCategoria.Width = flpMenuPrincipal.Width - 50; // Intentar ocupar todo el ancho
                    pnlCategoria.AutoSize = true; // Que crezca hacia abajo (altura)
                    pnlCategoria.Padding = new Padding(0, 0, 0, 20);

                    // Mantenemos el color verde para ver si ya se "infla"
                    pnlCategoria.BackColor = Color.LightGreen;

                    Label lblTitulo = new Label();
                    lblTitulo.Text = categoriaActual.ToUpper();
                    lblTitulo.Dock = DockStyle.Top;
                    lblTitulo.Height = 30;
                    lblTitulo.BackColor = Color.Yellow; // <--- COLOR DE PRUEBA

                    flpCuadricula = new FlowLayoutPanel();
                    flpCuadricula.FlowDirection = FlowDirection.LeftToRight;
                    flpCuadricula.WrapContents = true;
                    flpCuadricula.AutoSize = true;
                    flpCuadricula.Dock = DockStyle.Top;
                    flpCuadricula.BackColor = Color.Orange; // <--- COLOR DE PRUEBA

                    pnlCategoria.Controls.Add(flpCuadricula);
                    pnlCategoria.Controls.Add(lblTitulo);

                    // AGREGAMOS AL PANEL AZUL
                    flpMenuPrincipal.Controls.Add(pnlCategoria);
                }

                // B. PRODUCTO
                ControlProducto tarjeta = new ControlProducto();
                tarjeta.CargarDatos(prod.IdProducto, prod.NombreProducto, prod.Precio, prod.FotoProducto);

                // FORZAR TAMAÑO Y COLOR (Para que no sea invisible)
                tarjeta.Size = new Size(150, 180);
                tarjeta.BackColor = Color.White;
                tarjeta.Visible = true;

                if (flpCuadricula != null)
                {
                    flpCuadricula.Controls.Add(tarjeta);
                    contadorVisual++;
                }
            }

            // MENSAJE FINAL (Para confirmar que el código sí pasó por aquí)
            MessageBox.Show("Se intentaron dibujar " + contadorVisual + " tarjetas.");
        }

        private void AgregarAlCarrito_Click(object sender, EventArgs e)
        {
            ControlProducto tarjeta = (ControlProducto)sender;
            int id = tarjeta.IdProducto;
            string nombre = tarjeta.Nombre;
            decimal precio = tarjeta.Precio;

            bool existe = false;

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["Id"].Value != null && Convert.ToInt32(fila.Cells["Id"].Value) == id)
                {
                    int cantidadActual = Convert.ToInt32(fila.Cells["Cantidad"].Value);
                    cantidadActual++; 

                    fila.Cells["Cantidad"].Value = cantidadActual;

                    fila.Cells["Importe"].Value = (cantidadActual * precio); 

                    existe = true;
                    break; 
                }
            }

            if (!existe)
            {
                dataGridView1.Rows.Add(id, nombre, precio, 1, precio);
            }
            CalcularTotalGeneral();
        }

        private void CalcularTotalGeneral()
        {
            decimal total = 0;
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["Importe"].Value != null)
                {
                    total += Convert.ToDecimal(fila.Cells["Importe"].Value);
                }
            }
            // Si tienes un label llamado lblTotal:
            // lblTotal.Text = total.ToString("C2");
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
