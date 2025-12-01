using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.MODELOS
{
    public static class Sesion
    {
        public static int IdUsuario { get; set; }
        public static string Nombre { get; set; }
        public static string Apellidos { get; set; }
        public static string UsuarioAcceso { get; set; } 
        public static string Rol { get; set; } 

        public static string NombreCompleto
        {
            get { return Nombre + " " + Apellidos; }
        }

        public static void CerrarSesion()
        {
            IdUsuario = 0;
            Nombre = string.Empty;
            Apellidos = string.Empty;
            UsuarioAcceso = string.Empty;
            Rol = string.Empty;
        }
    }
}
