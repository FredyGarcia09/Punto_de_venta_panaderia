using MySql.Data.MySqlClient;
using ProyectoFinalGerman.MODELOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.BACKEND.DAOs
{
    /// <summary>
    /// Clase de Acceso a Datos (DAO) para la gestión de categorías de productos.
    /// Contiene métodos para insertar, consultar y administrar las categorías en la base de datos.
    /// </summary>
    public class CategoriaDao
    {
        /// <summary>
        /// Instancia para manejar la conexión con la base de datos MySQL.
        /// </summary>
        private ConexionDB conexion = new ConexionDB();

        /// <summary>
        /// Registra una nueva categoría en la base de datos.
        /// </summary>
        /// <param name="cat">Objeto de tipo <see cref="Categoria"/> que contiene los datos a insertar (Nombre y Descripción).</param>
        /// <param name="mensaje">Parámetro de salida que devolverá un mensaje de éxito o el detalle del error ocurrido.</param>
        /// <returns>
        /// <c>true</c> si la categoría se registró exitosamente; de lo contrario, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Este método utiliza el procedimiento almacenado 'sp_RegistrarCategoria'.
        /// La descripción es opcional, en xcaso de estar vacía se enviará como DBNull.
        /// </remarks>
        public bool RegistrarCategoria(Categoria cat, out string mensaje)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_RegistrarCategoria", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_Nombre", cat.NombreCategoria);

                        // Si esta vacio se guarda NULL en la BD
                        cmd.Parameters.AddWithValue("p_Descripcion",
                            string.IsNullOrEmpty(cat.Descripcion) ? DBNull.Value : (object)cat.Descripcion);

                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0)
                        {
                            mensaje = "Categoría registrada correctamente.";
                            return true;
                        }
                        else
                        {
                            mensaje = "No se pudo registrar la categoría.";
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error al guardar: " + ex.Message;
                    return false;
                }
            }
        }
    }
}