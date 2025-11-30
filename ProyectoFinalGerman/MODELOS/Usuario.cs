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
        public string UsuarioAcceso { get; set; } // Usuario en DB
        public string Contrasenha { get; set; }   // Texto plano (solo para transporte)
        public string Rfc { get; set; }
        public string Curp { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Nss { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string TipoSangre { get; set; }
        public string TipoUsuario { get; set; } = "Empleado";
    }
}
