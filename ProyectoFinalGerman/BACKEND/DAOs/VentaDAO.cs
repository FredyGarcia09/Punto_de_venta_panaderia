using MySql.Data.MySqlClient;
using ProyectoFinalGerman.MODELOS.ITEMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.BACKEND.DAOs
{
    /// <summary>
    /// Clase de Acceso a Datos (DAO) para la gestión del proceso de ventas.
    /// Maneja transacciones complejas que involucran la creación de órdenes, 
    /// inserción de detalles y actualización de inventarios.
    /// </summary>
    public class VentaDAO
    {
        /// <summary>
        /// Instancia para manejar la conexión con la base de datos MySQL.
        /// </summary>
        private ConexionDB conexion = new ConexionDB();

        /// <summary>
        /// Ejecuta una transacción completa de venta: crea la orden, guarda los detalles de los productos 
        /// y actualiza el stock. Si algún paso falla, se revierten todos los cambios (ROLLBACK).
        /// </summary>
        /// <param name="idUsuario">ID del empleado o usuario que realiza la venta.</param>
        /// <param name="items">Lista de objetos <see cref="ItemCarrito"/> que representan los productos vendidos.</param>
        /// <param name="idOrdenGenerada">Parámetro de salida que devolverá el ID único de la orden creada (Ticket).</param>
        /// <param name="mensaje">Mensaje de salida con el resultado de la operación o detalle del error.</param>
        /// <returns>
        /// <c>true</c> si la venta se procesó correctamente y se confirmó (Commit); 
        /// de lo contrario, <c>false</c> (Rollback).
        /// </returns>
        public bool RealizarVenta(int idUsuario, List<ItemCarrito> items, out int idOrdenGenerada, out string mensaje)
        {
            idOrdenGenerada = 0;

            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();
                // Transacción
                MySqlTransaction transaccion = conn.BeginTransaction();

                try
                {
                    using (MySqlCommand cmdOrden = new MySqlCommand("sp_CrearOrden", conn))
                    {
                        cmdOrden.Transaction = transaccion; // Vinculamos a la transacción activa
                        cmdOrden.CommandType = CommandType.StoredProcedure;
                        cmdOrden.Parameters.AddWithValue("p_IdUsuario", idUsuario);

                        // Parámetro de salida para obtener el ID de la orden creada
                        cmdOrden.Parameters.Add("p_IdOrdenGenerada", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                        cmdOrden.ExecuteNonQuery();

                        // Recuperamos el ID creado
                        idOrdenGenerada = Convert.ToInt32(cmdOrden.Parameters["p_IdOrdenGenerada"].Value);
                    }

                    foreach (ItemCarrito item in items)
                    {
                        using (MySqlCommand cmdDetalle = new MySqlCommand("sp_AgregarDetalleOrden", conn))
                        {
                            cmdDetalle.Transaction = transaccion;
                            cmdDetalle.CommandType = CommandType.StoredProcedure;

                            cmdDetalle.Parameters.AddWithValue("p_IdOrden", idOrdenGenerada);
                            cmdDetalle.Parameters.AddWithValue("p_IdProducto", item.IdProducto);
                            cmdDetalle.Parameters.AddWithValue("p_Cantidad", item.Cantidad);

                            // Parámetros de salida del SP para validación de Stock
                            cmdDetalle.Parameters.Add("p_Mensaje", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                            cmdDetalle.Parameters.Add("p_Exito", MySqlDbType.Bit).Direction = ParameterDirection.Output;

                            cmdDetalle.ExecuteNonQuery();

                            // VFalta de stock u otro error
                            bool exito = Convert.ToBoolean(cmdDetalle.Parameters["p_Exito"].Value);
                            string msgBD = cmdDetalle.Parameters["p_Mensaje"].Value.ToString();

                            if (!exito)
                            {
                                // Si falló un producto, lanzamos excepción para cancelar TODA la venta
                                throw new Exception($"Error con el producto '{item.Nombre}': {msgBD}");
                            }
                        }
                    }

                    transaccion.Commit();
                    mensaje = "Venta realizada correctamente.";
                    return true;
                }
                catch (Exception ex)
                {
                    // Se hace rollback
                    transaccion.Rollback();
                    mensaje = "Fallo en la venta: " + ex.Message;
                    idOrdenGenerada = 0;
                    return false;
                }
            }
        }
    }
}