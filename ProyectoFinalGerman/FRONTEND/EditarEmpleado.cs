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

namespace ProyectoFinalGerman
{
    public partial class EditarEmpleado : Form
    {
        public EditarEmpleado()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#DAA897");
            groupBox2.BackColor = ColorTranslator.FromHtml("#BFB6AD");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            EstilosUI.AplicarEstiloGrid(dgvEmpleados);
        }
        private void CargarGridUsuarios()
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                DataTable dt = dao.ObtenerUsuariosResumen();

                dgvEmpleados.DataSource = dt;

                // Responsividad
                dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                
                if (dgvEmpleados.Columns.Count > 0)
                {
                    dgvEmpleados.Columns["IdUsuario"].HeaderText = "ID";
                    dgvEmpleados.Columns["NombreCompleto"].HeaderText = "Nombre Completo";
                    dgvEmpleados.Columns["tipoUsuario"].HeaderText = "Rol / Puesto";
                    dgvEmpleados.Columns["curp"].HeaderText = "CURP";

                    dgvEmpleados.Columns[dgvEmpleados.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                
                dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvEmpleados.ReadOnly = true;
                dgvEmpleados.AllowUserToAddRows = false;
                dgvEmpleados.RowHeadersVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void EditarEmpleado_Load(object sender, EventArgs e)
        {
            CargarGridUsuarios();
        }

        private void dgvEmpleados_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    var valorCelda = dgvEmpleados.Rows[e.RowIndex].Cells[0].Value;

                    if (valorCelda == null)
                    {
                        MessageBox.Show("El ID es nulo. Revisa la consulta.");
                        return;
                    }

                    int idSeleccionado = Convert.ToInt32(valorCelda);

                    EditarDatosEmpleado frm = new EditarDatosEmpleado(idSeleccionado);
                    frm.ShowDialog();

                    CargarGridUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir edición: " + ex.Message);
                }
            }
        }

        private void dgvEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
