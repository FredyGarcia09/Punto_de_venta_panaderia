using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.MODELOS
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreUsuario { get; set; }
        public string ContrasenhaHash { get; set; }
        public string TipoUsuario { get; set; } // Empleado o admin

        // Para interfaz
        public string NombreCompleto => $"{Nombre} {Apellidos}";
    }
}
