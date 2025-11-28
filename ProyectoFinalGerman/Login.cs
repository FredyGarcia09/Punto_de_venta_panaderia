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
            Menu RE = new Menu();
            RE.Show();
            this.Hide();

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }
    }
}
