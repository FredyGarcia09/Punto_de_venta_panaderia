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
            // Hacer que todo el control sea clicable
            this.Click += (s, e) => AlHacerClick?.Invoke(this, EventArgs.Empty);
            pbFoto.Click += (s, e) => AlHacerClick?.Invoke(this, EventArgs.Empty);
            lblNombre.Click += (s, e) => AlHacerClick?.Invoke(this, EventArgs.Empty);
            lblPrecio.Click += (s, e) => AlHacerClick?.Invoke(this, EventArgs.Empty);
        }

        public void CargarDatos(int id, string nombre, decimal precio, byte[] fotoBytes)
        {
            IdProducto = id;
            Nombre = nombre;
            Precio = precio;

            lblNombre.Text = nombre;
            lblPrecio.Text = precio.ToString("C2"); // Formato moneda

            if (fotoBytes != null && fotoBytes.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(fotoBytes))
                {
                    pbFoto.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pbFoto.Image = null;
                pbFoto.BackColor = Color.LightGray;
            }
        }
    }
}