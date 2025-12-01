using ProyectoFinalGerman.BACKEND.DAOs;
using ProyectoFinalGerman.MODELOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ProyectoFinalGerman
{
    public partial class EditarDatosEmpleado : Form
    {
        private int _idUsuarioEditar;
        public EditarDatosEmpleado(int idUsuario)
        {
            InitializeComponent();
            _idUsuarioEditar = idUsuario;

            txtContrasenha.UseSystemPasswordChar = true;
            txtConfirmarContrasenha.UseSystemPasswordChar = true;

            this.BackColor = ColorTranslator.FromHtml("#DAA897");
            groupBox2.BackColor = ColorTranslator.FromHtml("#BFB6AD");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            btnGuardar.BackColor = ColorTranslator.FromHtml("#F7E1D5");
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnRegresar.FlatAppearance.BorderSize = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtTelefono1.Text))
            {
                MessageBox.Show("Nombre, Apellido, Usuario y Teléfono son obligatorios.");
                return;
            }

            // Solo validamos si el usuario escribió algo en la caja de contraseña
            if (!string.IsNullOrEmpty(txtContrasenha.Text))
            {
                if (txtContrasenha.Text != txtConfirmarContrasenha.Text)
                {
                    MessageBox.Show("Las nuevas contraseñas no coinciden.");
                    return;
                }
                if (!Regex.IsMatch(txtContrasenha.Text, @"^[a-zA-Z0-9]{8,20}$"))
                {
                    MessageBox.Show("La nueva contraseña debe ser alfanumérica de 8 a 20 caracteres.");
                    return;
                }
            }

            try
            {
                Usuario u = new Usuario();
                u.IdUsuario = _idUsuarioEditar; 
                u.Nombre = txtNombre.Text.Trim();
                u.Apellidos = txtApellido.Text.Trim();
                u.UsuarioAcceso = txtUsuario.Text.Trim();

                // Si dejó vacía la contraseña se envia NULL
                u.Contrasenha = txtContrasenha.Text.Trim();

                u.Email = txtEmail.Text.Trim();
                u.Direccion = txtDireccion.Text.Trim();
                u.Nss = txtNSS.Text.Trim();
                u.Rfc = txtRFC.Text.Trim();
                u.Curp = txtCURP.Text.Trim();
                u.Telefono1 = txtTelefono1.Text.Trim();
                u.Telefono2 = txtTelefono2.Text.Trim();
                u.TipoSangre = cboTipoSangre.Text;
                u.TipoUsuario = cboTipoUsuario.Text;

                UsuarioDAO dao = new UsuarioDAO();
                string msg;
                if (dao.ActualizarUsuario(u, out msg))
                {
                    MessageBox.Show(msg, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void EditarDatosEmpleado_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            UsuarioDAO dao = new UsuarioDAO();
            Usuario u = dao.ObtenerPorId(_idUsuarioEditar);

            if (u != null)
            {
                txtNombre.Text = u.Nombre;
                txtApellido.Text = u.Apellidos;
                txtUsuario.Text = u.UsuarioAcceso;
                // Contraseñas se dejan en blanco por seguridad

                txtEmail.Text = u.Email;
                txtDireccion.Text = u.Direccion;
                txtNSS.Text = u.Nss;
                txtRFC.Text = u.Rfc;
                txtCURP.Text = u.Curp;
                txtTelefono1.Text = u.Telefono1;
                txtTelefono2.Text = u.Telefono2;

                cboTipoSangre.Text = u.TipoSangre;
                cboTipoUsuario.Text = u.TipoUsuario;
            }
            else
            {
                MessageBox.Show("No se encontró el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
