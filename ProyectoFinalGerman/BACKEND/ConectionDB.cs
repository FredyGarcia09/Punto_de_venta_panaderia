using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.BACKEND
{
    public class ConexionDB
    {
        private readonly string connectionString = "Server=localhost;Database=VENTAS;Uid=root;Pwd=root;Port=3306;";

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(connectionString);
        }

        public bool ProbarConexion()
        {
            using (MySqlConnection conn = ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error de conexión: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
