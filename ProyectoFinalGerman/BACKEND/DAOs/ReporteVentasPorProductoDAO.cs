using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.BACKEND.DAOs
{
    public class ReporteVentasPorProductoDAO
    {
        private ConexionDB conexion = new ConexionDB();

        public DataTable ObtenerVentasPorProducto(DateTime inicio, DateTime fin)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ReporteVentaPorProducto", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_FechaInicio", inicio.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("p_FechaFin", fin.ToString("yyyy-MM-dd"));

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch { }
            }
            return dt;
        }
    }
}
