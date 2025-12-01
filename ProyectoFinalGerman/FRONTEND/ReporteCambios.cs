using ProyectoFinalGerman.BACKEND.DAOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProyectoFinalGerman
{
    public partial class ReporteCambios : Form
    {
        public ReporteCambios()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#CD9B81");
            groupBox2.BackColor = ColorTranslator.FromHtml("#E9CEBB");
            groupBox1.BackColor = ColorTranslator.FromHtml("#A8B29B");
            btnRegresar.FlatAppearance.BorderSize = 0;
            
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnComparar_Click(object sender, EventArgs e)
        {
            // 1. VALIDACIÓN: ¿Hay productos seleccionados?
            if (clbProductos.CheckedItems.Count == 0)
            {
                MessageBox.Show("Selecciona al menos un producto de la lista izquierda.", "Atención");
                return;
            }

            // 2. CONSTRUIR STRING DE IDs "1,5,9"
            StringBuilder idsBuilder = new StringBuilder();
            foreach (DataRowView item in clbProductos.CheckedItems)
            {
                idsBuilder.Append(item["IdProducto"] + ",");
            }
            string listaIds = idsBuilder.ToString().TrimEnd(','); // Quitar última coma

            // 3. OBTENER DATOS
            ReporteDAO dao = new ReporteDAO();
            DataTable dt = dao.ObtenerComparativo(dtpMes1.Value, dtpMes2.Value, listaIds);

            // 4. LLENAR GRID
            LlenarGrid(dt);

            // 5. LLENAR GRÁFICA
            LlenarGrafica(dt);
        }

        private void ReporteCambios_Load(object sender, EventArgs e)
        {
            // 1. Configurar Fechas (Formato solo Mes y Año visualmente)
            dtpMes1.Format = DateTimePickerFormat.Custom;
            dtpMes1.CustomFormat = "MMMM yyyy"; // "Noviembre 2025"

            dtpMes2.Format = DateTimePickerFormat.Custom;
            dtpMes2.CustomFormat = "MMMM yyyy";

            // Poner meses diferentes por defecto
            dtpMes1.Value = DateTime.Now;
            dtpMes2.Value = DateTime.Now.AddMonths(-1);

            // 2. Estilos visuales
            // (Si usas tu clase EstilosUI, aplícala aquí al Grid)
            // EstilosUI.AplicarEstiloGrid(dgvDatos);

            // 3. Cargar la lista de CheckBox
            CargarListaProductos();
        }

        private void CargarListaProductos()
        {
            ReporteDAO dao = new ReporteDAO();
            DataTable dt = dao.ObtenerListaProductos();

            clbProductos.DataSource = dt;
            clbProductos.DisplayMember = "nombreProducto";
            clbProductos.ValueMember = "IdProducto";

            // Opcional: Marcar todos por defecto (Si son pocos)
            // for (int i = 0; i < clbProductos.Items.Count; i++) clbProductos.SetItemChecked(i, true);
        }

        private void LlenarGrid(DataTable dt)
        {
            dgvDatos.DataSource = dt;

            // Ajustar nombres de columnas según las fechas seleccionadas
            string nombreMes1 = dtpMes1.Value.ToString("MMMM yyyy");
            string nombreMes2 = dtpMes2.Value.ToString("MMMM yyyy");

            if (dgvDatos.Columns.Contains("VentasMes1"))
            {
                dgvDatos.Columns["VentasMes1"].HeaderText = "Ventas: " + nombreMes1;
                dgvDatos.Columns["VentasMes1"].DefaultCellStyle.Format = "C2";
            }
            if (dgvDatos.Columns.Contains("VentasMes2"))
            {
                dgvDatos.Columns["VentasMes2"].HeaderText = "Ventas: " + nombreMes2;
                dgvDatos.Columns["VentasMes2"].DefaultCellStyle.Format = "C2";
            }

            if (dgvDatos.Columns.Contains("Precio"))
                dgvDatos.Columns["Precio"].DefaultCellStyle.Format = "C2";
        }

        private void LlenarGrafica(DataTable dt)
        {
            // 1. LIMPIEZA TOTAL (Orden Correcto)
            chartVentas.Series.Clear();
            chartVentas.ChartAreas.Clear();
            chartVentas.Legends.Clear(); // ¡Importante limpiar leyendas viejas!
            chartVentas.Titles.Clear();

            // 2. CREAR ÁREA Y LEYENDA (Primero)
            ChartArea area = new ChartArea();
            chartVentas.ChartAreas.Add(area);

            // Creamos la leyenda con un nombre específico
            Legend miLeyenda = new Legend("LeyendaPrincipal");
            chartVentas.Legends.Add(miLeyenda);

            // Título
            chartVentas.Titles.Add("Comparativo de Ventas");

            // Nombres de las series basados en los meses
            string serieMes1 = dtpMes1.Value.ToString("MMM yy");
            string serieMes2 = dtpMes2.Value.ToString("MMM yy");

            // 3. CREAR SERIES Y VINCULARLAS A LA LEYENDA
            Series s1 = new Series(serieMes1);
            s1.ChartType = SeriesChartType.Column;
            s1.Color = Color.DarkOrange;
            s1.Legend = "LeyendaPrincipal"; // <--- AQUÍ EVITAMOS EL ERROR

            Series s2 = new Series(serieMes2);
            s2.ChartType = SeriesChartType.Column;
            s2.Color = Color.DarkGreen;
            s2.Legend = "LeyendaPrincipal"; // <--- VINCULACIÓN EXPLÍCITA

            // Llenar datos punto por punto
            foreach (DataRow row in dt.Rows)
            {
                string producto = row["Producto"].ToString();
                decimal v1 = Convert.ToDecimal(row["VentasMes1"]);
                decimal v2 = Convert.ToDecimal(row["VentasMes2"]);

                // Agregar puntos
                s1.Points.AddXY(producto, v1);
                s2.Points.AddXY(producto, v2);
            }

            // 4. AGREGAR SERIES AL CHART (Al final)
            chartVentas.Series.Add(s1);
            chartVentas.Series.Add(s2);
        }


    }
}
