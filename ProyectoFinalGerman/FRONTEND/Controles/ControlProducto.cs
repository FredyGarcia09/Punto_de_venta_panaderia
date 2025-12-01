using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProyectoFinalGerman // Asegúrate que este namespace coincida con el de tu proyecto
{
    public partial class ControlProducto : UserControl
    {
        // Propiedades para guardar datos
        public int IdProducto { get; set; }
        public decimal Precio { get; set; }
        public string Nombre { get; set; }

        // Evento personalizado
        public event EventHandler AlHacerClick;

        public ControlProducto()
        {
            InitializeComponent();
            // Forzar estilos al iniciar
            this.DoubleBuffered = true;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        public void CargarDatos(int id, string nombre, decimal precio, byte[] foto)
        {
            this.IdProducto = id;
            this.Nombre = nombre;
            this.Precio = precio;

            lblNombre.Text = nombre;
            lblPrecio.Text = precio.ToString("C2");

            if (foto != null && foto.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(foto))
                {
                    pbFoto.Image = Image.FromStream(ms);
                    pbFoto.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            else
            {
                pbFoto.Image = null; // Imagen por defecto si quieres
            }

            this.pbFoto.Click += (s, e) => this.OnClick(e);
            this.lblNombre.Click += (s, e) => this.OnClick(e);
            this.lblPrecio.Click += (s, e) => this.OnClick(e);
        }

        private void ControlProducto_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Bisque;
            this.Cursor = Cursors.Hand;
        }

        private void ControlProducto_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.Cursor = Cursors.Default;
        }
    }
}