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
    public partial class MenuPan : Form
    {
        public MenuPan()
        {
            InitializeComponent();
            groupBox1.BackColor = ColorTranslator.FromHtml("#C9C7B6");
            groupBox3.BackColor = ColorTranslator.FromHtml("#D7BFA8");
            groupBox2.BackColor = ColorTranslator.FromHtml("#C89B84");
            btnMenu.FlatAppearance.BorderSize = 0;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.BorderSize = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }
        
        private void panelMenuOpciones_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
