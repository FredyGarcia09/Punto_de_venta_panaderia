using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.MODELOS.ITEMS
{
    public class ReporteVentaItem
    {
        public int Clave { get; set; }      // IdProducto
        public string Nombre { get; set; }  // NombreProducto
        public int Unidades { get; set; }   // Suma de Cantidad
        public decimal Monto { get; set; }  // Suma de Importe
    }
}
