using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.BACKEND.DAOs
{
    public class ReporteDAO
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


        public DataTable ObtenerListaProductos()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter("sp_ObtenerListaProductosSimple", conn))
                    {
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.Fill(dt);
                    }
                }
                catch { }
            }
            return dt;
        }

        // REPORTE COMPARATIVO
        public DataTable ObtenerComparativo(DateTime fecha1, DateTime fecha2, string listaIds)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ReporteComparativoFiltrado", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_Mes1", fecha1.Month);
                        cmd.Parameters.AddWithValue("p_Anio1", fecha1.Year);

                        cmd.Parameters.AddWithValue("p_Mes2", fecha2.Month);
                        cmd.Parameters.AddWithValue("p_Anio2", fecha2.Year);

                        cmd.Parameters.AddWithValue("p_ListaIDs", listaIds);

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Error
                }
            }
            return dt;
        }
    }
}
