using ProyectoFinalGerman.BACKEND.DAOs;
using ProyectoFinalGerman.FRONTEND.Helpers;
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
    public partial class ReporteVentas : Form
    {
        private bool cargando = false;
        public ReporteVentas()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#CD9B81");
            groupBox1.BackColor = ColorTranslator.FromHtml("#E9CEBB");
            groupBox2.BackColor = ColorTranslator.FromHtml("#A8B29B");
            btnRegresar.FlatAppearance.BorderSize = 0;
            EstilosUI.AplicarEstiloFecha(dtpInicio);
            EstilosUI.AplicarEstiloFecha(dtpFin);
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            CargarReporte();
        }

        private void ReporteVentas_Load(object sender, EventArgs e)
        {
            cargando = true;
            EstilosUI.AplicarEstiloFecha(dtpInicio);
            EstilosUI.AplicarEstiloFecha(dtpFin);
            EstilosUI.AplicarEstiloGrid(dgvVentas);

            DateTime hoy = DateTime.Now;
            dtpInicio.Value = new DateTime(hoy.Year, hoy.Month, 1);
            dtpFin.Value = hoy;

            cargando = false;

            CargarReporte();
        }

        private void CargarReporte()
        {
            if (cargando) return;

            // Fecha Fin no puede ser menor a Inicio
            if (dtpFin.Value.Date < dtpInicio.Value.Date)
            {
                return;
            }

            try
            {
                ReporteVentasPorProductoDAO dao = new ReporteVentasPorProductoDAO();
                DataTable dt = dao.ObtenerVentasPorProducto(dtpInicio.Value, dtpFin.Value);

                dgvVentas.DataSource = dt;
                if (dgvVentas.Columns.Count > 0)
                {
                    // Encabezados
                    dgvVentas.Columns["Clave"].HeaderText = "Clave";
                    dgvVentas.Columns["Clave"].Width = 60;

                    dgvVentas.Columns["Nombre"].HeaderText = "Nombre";
                    dgvVentas.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 

                    dgvVentas.Columns["Unidades"].HeaderText = "Unidades";
                    dgvVentas.Columns["Unidades"].Width = 80;
                    dgvVentas.Columns["Unidades"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dgvVentas.Columns["Monto"].HeaderText = "Monto";
                    dgvVentas.Columns["Monto"].Width = 100;

                    EstilosUI.FormatearMoneda(dgvVentas, "Monto");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte: " + ex.Message);
            }
        }

        private void dtpInicio_ValueChanged(object sender, EventArgs e)
        {
            CargarReporte();
        }
    }
}
