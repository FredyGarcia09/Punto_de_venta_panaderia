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
    public class CategoriaDao
    {
        private ConexionDB conexion = new ConexionDB();

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

                        // Manejo de nulos para descripción
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
