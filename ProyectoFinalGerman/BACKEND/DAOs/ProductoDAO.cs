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
    public class ProductoDAO
    {
        private ConexionDB conexion = new ConexionDB();

        public DataTable ObtenerCategorias()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IdCategoria, nombreCategoria FROM categorias";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception)
                {
                    // Tabla vacia
                }
            }
            return dt;
        }

        public bool InsertarProducto(Producto prod, string usuarioActual, out string mensaje)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_InsertarProducto", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_Nombre", prod.NombreProducto);

                        cmd.Parameters.AddWithValue("p_Desc", string.IsNullOrEmpty(prod.Descripcion) ? DBNull.Value : (object)prod.Descripcion);

                        cmd.Parameters.AddWithValue("p_IdCategoria", prod.IdCategoria);
                        cmd.Parameters.AddWithValue("p_Precio", prod.Precio);
                        cmd.Parameters.AddWithValue("p_Stock", prod.Existencias);

                        // Foto en longblob o null
                        cmd.Parameters.AddWithValue("p_Foto", prod.FotoProducto == null ? DBNull.Value : (object)prod.FotoProducto);

                        // Usuario para auditoría
                        cmd.Parameters.AddWithValue("p_UsuarioApp", usuarioActual);

                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0)
                        {
                            mensaje = "Producto registrado exitosamente.";
                            return true;
                        }
                        else
                        {
                            mensaje = "No se pudo registrar el producto.";
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error BD: " + ex.Message;
                    return false;
                }
            }
        }
    }
}
