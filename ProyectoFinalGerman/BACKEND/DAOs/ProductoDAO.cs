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
    /// Clase de Acceso a Datos (DAO) para la gestión de productos.
    /// Contiene métodos para consultar, insertar, actualizar y eliminar productos en la base de datos.
    /// </summary>
    public class ProductoDAO
    {
        /// <summary>
        /// Instancia para manejar la conexión con la base de datos MySQL.
        /// </summary>
        private ConexionDB conexion = new ConexionDB();

        /// <summary>
        /// Obtiene un listado de todas las categorías disponibles.
        /// Utilizado para llenar controles (Combo box por ejemplo) en las interfaces de usuario.
        /// </summary>
        /// <returns>
        /// Un objeto <see cref="DataTable"/> con las columnas IdCategoria y nombreCategoria.
        /// </returns>
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

        /// <summary>
        /// Obtiene una lista de productos para mostrar en tablas o reportes.
        /// Permite filtrar si se desean ver también los productos descontinuados.
        /// </summary>
        /// <param name="mostrarDescontinuados">
        /// Si es <c>true</c>, incluye productos descontinuados en el resultado.
        /// Si es <c>false</c>, solo muestra los activos.
        /// </param>
        /// <returns>
        /// Un <see cref="DataTable"/> con la información de los productos, formateada para mostrarse en listas.
        /// </returns>
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

        /// <summary>
        /// Busca y recupera toda la información detallada de un producto específico por su ID.
        /// </summary>
        /// <param name="id">Identificador único del producto a buscar.</param>
        /// <returns>
        /// Un objeto <see cref="Producto"/> con todos sus datos llenos si se encuentra; 
        /// de lo contrario, devuelve <c>null</c>.
        /// </returns>
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

        /// <summary>
        /// Inserta un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="prod">Objeto <see cref="Producto"/> con la información a guardar.</param>
        /// <param name="usuarioActual">Nombre del usuario que realiza la acción.</param>
        /// <param name="mensaje">Mensaje de salida con el resultado de la operación.</param>
        /// <returns><c>true</c> si la inserción fue exitosa; de lo contrario, <c>false</c>.</returns>
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

        /// <summary>
        /// Actualiza la información completa de un producto existente.
        /// </summary>
        /// <param name="prod">Objeto <see cref="Producto"/> con los datos actualizados.</param>
        /// <param name="usuarioActual">Usuario que realiza la modificación.</param>
        /// <param name="mensaje">Mensaje de resultado.</param>
        /// <returns><c>true</c> si se actualizó correctamente.</returns>
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

        /// <summary>
        /// Cambia el estado de un producto (Descontinuado/Activo).
        /// </summary>
        /// <param name="idProducto">ID del producto a modificar.</param>
        /// <param name="nuevoEstadoDescontinuado"><c>true</c> para descontinuar, <c>false</c> para reactivar.</param>
        /// <param name="usuario">Usuario responsable.</param>
        /// <param name="mensaje">Mensaje de resultado.</param>
        /// <returns><c>true</c> si el cambio fue exitoso.</returns>
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

        /// <summary>
        /// Elimina físicamente un producto y su historial de ventas de la base de datos.
        /// ¡CUIDADO! Esta es una acción irreversible, solo apta para administradores.
        /// </summary>
        /// <param name="idProducto">ID del producto a eliminar.</param>
        /// <param name="usuario">Usuario que ejecuta la eliminación.</param>
        /// <param name="mensaje">Mensaje de resultado.</param>
        /// <returns><c>true</c> si se eliminó correctamente.</returns>
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

        /// <summary>
        /// Obtiene una lista de objetos Producto optimizada para mostrar en el menú de compras (tarjetas visuales).
        /// Incluye la foto y el nombre de la categoría.
        /// </summary>
        /// <returns>Una lista de objetos <see cref="Producto"/>.</returns>
        public List<Producto> ObtenerProductosParaMenu()
        {
            List<Producto> lista = new List<Producto>();

            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            c.nombreCategoria, 
                            p.IdProducto, 
                            p.nombreProducto, 
                            p.precio, 
                            p.fotoProducto 
                        FROM productos p
                        INNER JOIN categorias c ON p.IdCategoria = c.IdCategoria
                        WHERE p.descontinuado = 0 
                        ORDER BY c.nombreCategoria, p.nombreProducto";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto prod = new Producto();

                            // Mapeamos los datos de la BD al Modelo
                            prod.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                            prod.NombreProducto = reader["nombreProducto"].ToString();
                            prod.Precio = Convert.ToDecimal(reader["precio"]);

                            prod.NombreCategoria = reader["nombreCategoria"].ToString();

                            if (reader["fotoProducto"] != DBNull.Value)
                            {
                                prod.FotoProducto = (byte[])reader["fotoProducto"];
                            }
                            else
                            {
                                prod.FotoProducto = null;
                            }

                            lista.Add(prod);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cargar menú: " + ex.Message);
                }
            }
            return lista;
        }

    }
}