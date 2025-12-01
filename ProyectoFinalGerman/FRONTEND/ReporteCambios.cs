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
            if (clbProductos.CheckedItems.Count == 0)
            {
                MessageBox.Show("Selecciona al menos un producto de la lista izquierda.", "Atención");
                return;
            }

            // CONSTRUIR STRING DE IDs "1,5,9"
            StringBuilder idsBuilder = new StringBuilder();
            foreach (DataRowView item in clbProductos.CheckedItems)
            {
                idsBuilder.Append(item["IdProducto"] + ",");
            }
            string listaIds = idsBuilder.ToString().TrimEnd(','); // Quitar última coma

            ReporteDAO dao = new ReporteDAO();
            DataTable dt = dao.ObtenerComparativo(dtpMes1.Value, dtpMes2.Value, listaIds);

            LlenarGrid(dt);
            LlenarGrafica(dt);
        }

        private void ReporteCambios_Load(object sender, EventArgs e)
        {
            // Solo mes y año
            dtpMes1.Format = DateTimePickerFormat.Custom;
            dtpMes1.CustomFormat = "MMMM yyyy"; 

            dtpMes2.Format = DateTimePickerFormat.Custom;
            dtpMes2.CustomFormat = "MMMM yyyy";

            dtpMes1.Value = DateTime.Now;
            dtpMes2.Value = DateTime.Now.AddMonths(-1);
            CargarListaProductos();
        }

        private void CargarListaProductos()
        {
            ReporteDAO dao = new ReporteDAO();
            DataTable dt = dao.ObtenerListaProductos();

            clbProductos.DataSource = dt;
            clbProductos.DisplayMember = "nombreProducto";
            clbProductos.ValueMember = "IdProducto";
        }

        private void LlenarGrid(DataTable dt)
        {
            dgvDatos.DataSource = dt;

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
            chartVentas.Series.Clear();
            chartVentas.ChartAreas.Clear();
            chartVentas.Legends.Clear(); 
            chartVentas.Titles.Clear();

            ChartArea area = new ChartArea();
            chartVentas.ChartAreas.Add(area);

            Legend miLeyenda = new Legend("LeyendaPrincipal");
            chartVentas.Legends.Add(miLeyenda);

            chartVentas.Titles.Add("Comparativo de Ventas");

            string serieMes1 = dtpMes1.Value.ToString("MMM yy");
            string serieMes2 = dtpMes2.Value.ToString("MMM yy");

            Series s1 = new Series(serieMes1);
            s1.ChartType = SeriesChartType.Column;
            s1.Color = Color.DarkOrange;
            s1.Legend = "LeyendaPrincipal";

            Series s2 = new Series(serieMes2);
            s2.ChartType = SeriesChartType.Column;
            s2.Color = Color.DarkGreen;
            s2.Legend = "LeyendaPrincipal";

            foreach (DataRow row in dt.Rows)
            {
                string producto = row["Producto"].ToString();
                decimal v1 = Convert.ToDecimal(row["VentasMes1"]);
                decimal v2 = Convert.ToDecimal(row["VentasMes2"]);

                s1.Points.AddXY(producto, v1);
                s2.Points.AddXY(producto, v2);
            }

            chartVentas.Series.Add(s1);
            chartVentas.Series.Add(s2);
        }


    }
}
