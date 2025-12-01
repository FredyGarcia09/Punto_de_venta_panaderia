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
    public partial class RegistrarEmpleado : Form
    {
        public RegistrarEmpleado()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#DAA897");
            groupBox2.BackColor = ColorTranslator.FromHtml("#BFB6AD");
            groupBox1.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            btnRegistrarUsuario.BackColor = ColorTranslator.FromHtml("#F7E1D5");
            btnRegistrarUsuario.FlatAppearance.BorderSize = 0;
            btnRegresar.FlatAppearance.BorderSize = 0;
            cboTipoSangre.SelectedIndex = 8;
            cboTipoUsuario.SelectedIndex = 0;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void RegistrarEmpleado_Load(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Menu RE = new Menu();
            RE.Show();
            this.Hide();
        }

        private void btnRegistrarUsuario_Click(object sender, EventArgs e)
        {
            // Validaciones campos not null
            if (EsVacio(txtNombre.Text) || EsVacio(txtApellido.Text) ||
                EsVacio(txtUsuario.Text) || EsVacio(txtContrasenha.Text) ||
                EsVacio(txtTelefono1.Text))
            {
                MostrarError("Por favor, llena los campos obligatorios:\n- Nombre\n- Apellido\n- Usuario\n- Contraseña\n- Teléfono Principal");
                return;
            }

            // Validaciones de formato

            // Contraseña
            if (txtContrasenha.Text != txtConfirmarContrasenha.Text)
            {
                MostrarError("Las contraseñas no coinciden.");
                return;
            }

            if (!Regex.IsMatch(txtContrasenha.Text, @"^[a-zA-Z0-9]{8,20}$"))
            {
                MostrarError("La contraseña no cumple con los requisitos de seguridad:\n" +
                             "- Mínimo 8 caracteres\n" +
                             "- Máximo 20 caracteres\n" +
                             "- Solo letras y números (sin espacios ni símbolos)");
                return;
            }

            // Telefonos
            if (!EsTelefonoValido(txtTelefono1.Text))
            {
                MostrarError("El teléfono principal debe tener 10 dígitos numéricos.");
                return;
            }

            // Secundario (Opcional)
            if (!EsVacio(txtTelefono2.Text) && !EsTelefonoValido(txtTelefono2.Text))
            {
                MostrarError("El teléfono secundario tiene un formato inválido.\nDebe tener 10 dígitos numéricos o déjalo vacío.");
                return;
            }


            //Validaciones opcionales
            if (!EsVacio(txtEmail.Text) && !EsEmailValido(txtEmail.Text))
            {
                MostrarError("El Email no tiene un formato válido (ejemplo@dominio.com).");
                return;
            }

            // NSS (11 dígitos)
            if (!EsVacio(txtNSS.Text) && !Regex.IsMatch(txtNSS.Text, @"^\d{11}$"))
            {
                MostrarError("El NSS debe contener exactamente 11 dígitos numéricos.");
                return;
            }

            // RFC
            if (!EsVacio(txtRFC.Text) && !Regex.IsMatch(txtRFC.Text.ToUpper(), @"^[A-ZÑ&]{3,4}\d{6}[A-Z0-9]{3}$"))
            {
                MostrarError("El formato del RFC es incorrecto.");
                return;
            }

            // CURP
            if (!EsVacio(txtCURP.Text) && !Regex.IsMatch(txtCURP.Text.ToUpper(), @"^[A-Z]{4}\d{6}[HM][A-Z]{5}[A-Z0-9]{2}$"))
            {
                MostrarError("El formato de la CURP es incorrecto.");
                return;
            }

            
            // Guardar en DB
            try
            {
                Usuario nuevoUser = new Usuario();

                nuevoUser.Nombre = txtNombre.Text.Trim();
                nuevoUser.Apellidos = txtApellido.Text.Trim();
                nuevoUser.UsuarioAcceso = txtUsuario.Text.Trim();
                nuevoUser.Contrasenha = txtContrasenha.Text.Trim();
                nuevoUser.Telefono1 = txtTelefono1.Text.Trim();

                // Nulos para opcionales
                nuevoUser.Email = EsVacio(txtEmail.Text) ? null : txtEmail.Text.Trim();
                nuevoUser.Nss = EsVacio(txtNSS.Text) ? null : txtNSS.Text.Trim();
                nuevoUser.Telefono2 = EsVacio(txtTelefono2.Text) ? null : txtTelefono2.Text.Trim();
                nuevoUser.Direccion = EsVacio(txtDireccion.Text) ? null : txtDireccion.Text.Trim();

                // Mayusculas 
                nuevoUser.Rfc = EsVacio(txtRFC.Text) ? null : txtRFC.Text.ToUpper().Trim();
                nuevoUser.Curp = EsVacio(txtCURP.Text) ? null : txtCURP.Text.ToUpper().Trim();

                // Valores de cbos
                nuevoUser.TipoSangre = cboTipoSangre.Text;
                nuevoUser.TipoUsuario = cboTipoUsuario.Text;

                // Llamada al DAO
                UsuarioDAO dao = new UsuarioDAO();
                string mensajeDB;

                if (dao.RegistrarUsuario(nuevoUser, out mensajeDB))
                {
                    MessageBox.Show("¡Empleado registrado con éxito!", "Registro Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                }
                else
                {
                    MostrarError("No se pudo registrar: " + mensajeDB);
                }
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error inesperado en el sistema:\n" + ex.Message);
            }
        }

        private void LimpiarFormulario()
        {
            foreach (Control c in this.Controls) if (c is TextBox) ((TextBox)c).Clear();
            if (cboTipoUsuario.Items.Count > 0) cboTipoUsuario.SelectedIndex = 0;
            if (cboTipoSangre.Items.Count > 0) cboTipoSangre.SelectedIndex = 8;
            txtNombre.Focus();
        }

        private void MostrarError(string msg) => MessageBox.Show(msg, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private bool EsVacio(string txt) => string.IsNullOrWhiteSpace(txt);
        private bool EsEmailValido(string email) => Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        private bool EsTelefonoValido(string tel) => Regex.IsMatch(tel, @"^\d{10}$");
        private void btnCerrar_Click(object sender, EventArgs e) => this.Close();
    }
}
