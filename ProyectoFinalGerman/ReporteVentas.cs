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
        public ReporteVentas()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#CD9B81");
            groupBox1.BackColor = ColorTranslator.FromHtml("#E9CEBB");
            groupBox2.BackColor = ColorTranslator.FromHtml("#A8B29B");
            groupBox1.FlatAppearance.BorderSize = 0;
            groupBox2.FlatAppearance.BorderSize = 0;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }
    }
}
