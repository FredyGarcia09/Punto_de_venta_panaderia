using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.BACKEND.DAOs
{
    /// <summary>
    /// Clase de Acceso a Datos (DAO) para la generación de reportes.
    /// Proporciona métodos para extraer información resumida y detallada sobre las ventas y productos
    /// para su visualización en gráficos y tablas.
    /// </summary>
    public class ReporteDAO
    {
        /// <summary>
        /// Instancia para manejar la conexión con la base de datos MySQL.
        /// </summary>
        private ConexionDB conexion = new ConexionDB();

        /// <summary>
        /// Obtiene un reporte detallado de las ventas agrupadas por producto dentro de un rango de fechas.
        /// </summary>
        /// <param name="inicio">Fecha de inicio del periodo a consultar.</param>
        /// <param name="fin">Fecha de fin del periodo a consultar.</param>
        /// <returns>
        /// Un <see cref="DataTable"/> con las columnas: Clave (Id), Nombre, Unidades vendidas y Monto total.
        /// </returns>
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

                        // Se envían las fechas formateadas como string 'YYYY-MM-DD' para compatibilidad con MySQL
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

        /// <summary>
        /// Obtiene una lista simple de productos activos para llenar controles de selección (como CheckList).
        /// </summary>
        /// <returns>
        /// Un <see cref="DataTable"/> con las columnas IdProducto y nombreProducto.
        /// </returns>
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

        /// <summary>
        /// Genera un reporte comparativo de ventas entre dos meses específicos para una lista de productos seleccionados.
        /// </summary>
        /// <param name="fecha1">Fecha que determina el primer mes y año a comparar.</param>
        /// <param name="fecha2">Fecha que determina el segundo mes y año a comparar.</param>
        /// <param name="listaIds">Cadena de texto con los IDs de los productos separados por coma (ej. "1,5,10").</param>
        /// <returns>
        /// Un <see cref="DataTable"/> con las ventas de cada producto en el Mes 1 y en el Mes 2 para su comparación.
        /// </returns>
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
                    // Tabla vacia
                }
            }
            return dt;
        }
    }
}