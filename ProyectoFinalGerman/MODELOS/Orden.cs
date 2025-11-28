using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.MODELOS
{
    public class Orden
    {
        public int IdOrden { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaOrden { get; set; }
        public string Estatus { get; set; }
    }
}
