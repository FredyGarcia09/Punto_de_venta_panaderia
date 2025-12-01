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

namespace ProyectoFinalGerman.FRONTEND
{
    public partial class AuditoriaProductos : Form
    {
        public AuditoriaProductos()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#DAA897");
            groupBox2.BackColor = ColorTranslator.FromHtml("#BFB6AD");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            EstilosUI.AplicarEstiloGrid(dgvAuditoriaProductos);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AuditoriaProductos_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                AuditoriaDAO dao = new AuditoriaDAO();
                DataTable dt = dao.ObtenerHistorial();

                dgvAuditoriaProductos.DataSource = dt;

                if (dgvAuditoriaProductos.Columns.Count > 0)
                {
                    dgvAuditoriaProductos.Columns["IdAuditoria"].Visible = false;

                    dgvAuditoriaProductos.Columns["FechaHora"].HeaderText = "Fecha y Hora";
                    dgvAuditoriaProductos.Columns["FechaHora"].Width = 120;

                    dgvAuditoriaProductos.Columns["UsuarioDB"].HeaderText = "Usuario";
                    dgvAuditoriaProductos.Columns["UsuarioDB"].Width = 100;

                    dgvAuditoriaProductos.Columns["TipoCambio"].HeaderText = "Acción";
                    dgvAuditoriaProductos.Columns["TipoCambio"].Width = 80;

                    dgvAuditoriaProductos.Columns["NombreProducto"].HeaderText = "Producto Afectado";
                    dgvAuditoriaProductos.Columns["NombreProducto"].Width = 150;

                    dgvAuditoriaProductos.Columns["ValorAnterior"].HeaderText = "Antes";
                    dgvAuditoriaProductos.Columns["ValorAnterior"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvAuditoriaProductos.Columns["ValorNuevo"].HeaderText = "Después";
                    dgvAuditoriaProductos.Columns["ValorNuevo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar historial: " + ex.Message);
            }
        }
    }
}
