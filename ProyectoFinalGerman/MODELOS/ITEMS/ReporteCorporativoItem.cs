using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.MODELOS.ITEMS
{
    public class ReporteComparativoItem
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public decimal Precio { get; set; }
        public decimal VentasMes1 { get; set; } 
        public decimal VentasMes2 { get; set; } 
    }
}
