using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.MODELOS
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Existencias { get; set; }
        public byte[] FotoProducto { get; set; } // BLOB
        public bool Descontinuado { get; set; }

        // Ayuda en la interfaz, no es parte de la tabla
        public string NombreCategoria { get; set; }
    }
}
