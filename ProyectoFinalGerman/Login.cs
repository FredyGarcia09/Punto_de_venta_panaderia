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
            this.BackColor = ColorTranslator.FromHtml("#D2B48C");
            groupBox1.BackColor = ColorTranslator.FromHtml("#F5D7B3");
            btnEntar.FlatAppearance.BorderSize = 0;
            txtUsuario.BorderStyle = BorderStyle.None;
            txtContrasenha.BorderStyle = BorderStyle.None;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu RE = new Menu();
            RE.Show();
            this.Hide();

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }
    }
}
