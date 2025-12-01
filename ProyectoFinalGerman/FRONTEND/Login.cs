using ProyectoFinalGerman.BACKEND.DAOs;
using ProyectoFinalGerman.MODELOS;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#D8C4B3");
            groupBox1.BackColor = ColorTranslator.FromHtml("#F5E6D7");
            btnEntrar.BackColor = ColorTranslator.FromHtml("#C3A295");
            btnEntrar.FlatAppearance.BorderSize = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Por favor ingresa usuario y contraseña.");
                return;
            }

            UsuarioDAO dao = new UsuarioDAO();
            Usuario usuarioLogueado = dao.Login(user, pass);

            if (usuarioLogueado != null)
            {
                // Guardar datos en la sesión
                Sesion.IdUsuario = usuarioLogueado.IdUsuario;
                Sesion.Nombre = usuarioLogueado.Nombre;
                Sesion.Apellidos = usuarioLogueado.Apellidos;
                Sesion.UsuarioAcceso = usuarioLogueado.UsuarioAcceso;
                Sesion.Rol = usuarioLogueado.TipoUsuario;

                MessageBox.Show($"Bienvenido, {Sesion.NombreCompleto} ({Sesion.Rol})", "Acceso Correcto");
                
                Menu menu = new Menu();
                menu.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }
    }
}
