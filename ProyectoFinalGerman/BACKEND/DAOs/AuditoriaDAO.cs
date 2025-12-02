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
    /// Clase de Acceso a Datos (DAO) para la gestión de auditorías.
    /// Proporciona métodos para consultar el historial de cambios registrados en el sistema.
    /// </summary>
    public class AuditoriaDAO
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private ConexionDB conexion = new ConexionDB();

        /// <summary>
        /// Obtiene el historial completo de auditoría desde la base de datos.
        /// Ejecuta el procedimiento almacenado 'sp_ObtenerAuditoria' para recuperar
        /// los registros de cambios realizados en los productos.
        /// </summary>
        /// <returns>
        /// Un objeto <see cref="DataTable"/> que contiene las filas de la tabla de auditoría
        /// (Id, Fecha, Usuario, TipoCambio, Producto, Valores Anterior y Nuevo).
        /// Devuelve una tabla vacía si ocurre una excepción durante la consulta.
        /// </returns>
        public DataTable ObtenerHistorial()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("sp_ObtenerAuditoria", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch
                {
                    // Tabla vacía
                }
            }
            return dt;
        }
    }
}