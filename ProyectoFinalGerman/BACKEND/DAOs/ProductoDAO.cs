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
                catch { }
            }
            return dt;
        }

        public DataTable ObtenerTodos(bool mostrarDescontinuados)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ObtenerTodosLosProductos", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_MostrarDescontinuados", mostrarDescontinuados);

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

        public Producto ObtenerPorId(int id)
        {
            Producto prod = null;
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ObtenerProductoPorId", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_Id", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prod = new Producto();
                                prod.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                                prod.NombreProducto = reader["nombreProducto"].ToString();
                                prod.Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : "";
                                prod.Precio = Convert.ToDecimal(reader["precio"]);
                                prod.Existencias = Convert.ToInt32(reader["existencias"]);
                                prod.IdCategoria = Convert.ToInt32(reader["IdCategoria"]);
                                prod.Descontinuado = Convert.ToBoolean(reader["descontinuado"]);

                                if (reader["fotoProducto"] != DBNull.Value)
                                    prod.FotoProducto = (byte[])reader["fotoProducto"];
                            }
                        }
                    }
                }
                catch { }
            }
            return prod;
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
                        cmd.Parameters.AddWithValue("p_Foto", prod.FotoProducto == null ? DBNull.Value : (object)prod.FotoProducto);

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

        public bool ActualizarProducto(Producto prod, string usuarioActual, out string mensaje)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizarProductoCompleto", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_IdProducto", prod.IdProducto);
                        cmd.Parameters.AddWithValue("p_Nombre", prod.NombreProducto);
                        cmd.Parameters.AddWithValue("p_Desc", string.IsNullOrEmpty(prod.Descripcion) ? DBNull.Value : (object)prod.Descripcion);
                        cmd.Parameters.AddWithValue("p_IdCategoria", prod.IdCategoria);
                        cmd.Parameters.AddWithValue("p_Precio", prod.Precio);
                        cmd.Parameters.AddWithValue("p_Stock", prod.Existencias);
                        cmd.Parameters.AddWithValue("p_Foto", prod.FotoProducto == null ? DBNull.Value : (object)prod.FotoProducto);
                        cmd.Parameters.AddWithValue("p_UsuarioApp", usuarioActual);

                        cmd.ExecuteNonQuery();
                        mensaje = "Producto actualizado correctamente.";
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error: " + ex.Message;
                    return false;
                }
            }
        }

        // DESCONTINUAR/REACTIVAR
        public bool CambiarEstado(int idProducto, bool nuevoEstadoDescontinuado, string usuario, out string mensaje)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_CambiarEstadoProducto", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_IdProducto", idProducto);
                        cmd.Parameters.AddWithValue("p_Estado", nuevoEstadoDescontinuado);
                        cmd.Parameters.AddWithValue("p_UsuarioApp", usuario);
                        cmd.ExecuteNonQuery();

                        mensaje = nuevoEstadoDescontinuado ? "Producto DESCONTINUADO correctamente." : "Producto REACTIVADO correctamente.";
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error: " + ex.Message;
                    return false;
                }
            }
        }

        public bool EliminarProducto(int idProducto, string usuario, out string mensaje)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_EliminarProductoFisico", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_IdProducto", idProducto);
                        cmd.Parameters.AddWithValue("p_UsuarioApp", usuario);

                        cmd.ExecuteNonQuery();
                        mensaje = "Producto y su historial de ventas eliminados permanentemente.";
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error crítico al eliminar: " + ex.Message;
                    return false;
                }
            }
        }
    }
}
