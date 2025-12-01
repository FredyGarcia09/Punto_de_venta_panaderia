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
    public class VentaDAO
    {
        private ConexionDB conexion = new ConexionDB();

        public bool RealizarVenta(int idUsuario, List<ItemCarrito> items, out int idOrdenGenerada, out string mensaje)
        {
            idOrdenGenerada = 0;

            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();
                // 1. INICIAMOS LA TRANSACCIÓN
                MySqlTransaction transaccion = conn.BeginTransaction();

                try
                {
                    // ---------------------------------------------------
                    // PASO A: CREAR LA ORDEN (CABECERA)
                    // ---------------------------------------------------
                    using (MySqlCommand cmdOrden = new MySqlCommand("sp_CrearOrden", conn))
                    {
                        cmdOrden.Transaction = transaccion; // Vinculamos a la transacción
                        cmdOrden.CommandType = CommandType.StoredProcedure;
                        cmdOrden.Parameters.AddWithValue("p_IdUsuario", idUsuario);

                        // Parámetro de salida para recuperar el ID
                        cmdOrden.Parameters.Add("p_IdOrdenGenerada", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                        cmdOrden.ExecuteNonQuery();

                        // Recuperamos el ID creado (Ej. Ticket #500)
                        idOrdenGenerada = Convert.ToInt32(cmdOrden.Parameters["p_IdOrdenGenerada"].Value);
                    }

                    // ---------------------------------------------------
                    // PASO B: INSERTAR DETALLES (PRODUCTOS)
                    // ---------------------------------------------------
                    foreach (ItemCarrito item in items)
                    {
                        using (MySqlCommand cmdDetalle = new MySqlCommand("sp_AgregarDetalleOrden", conn))
                        {
                            cmdDetalle.Transaction = transaccion;
                            cmdDetalle.CommandType = CommandType.StoredProcedure;

                            cmdDetalle.Parameters.AddWithValue("p_IdOrden", idOrdenGenerada);
                            cmdDetalle.Parameters.AddWithValue("p_IdProducto", item.IdProducto);
                            cmdDetalle.Parameters.AddWithValue("p_Cantidad", item.Cantidad);

                            // Parámetros de salida del SP
                            cmdDetalle.Parameters.Add("p_Mensaje", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                            cmdDetalle.Parameters.Add("p_Exito", MySqlDbType.Bit).Direction = ParameterDirection.Output;

                            cmdDetalle.ExecuteNonQuery();

                            // VERIFICAMOS SI EL SP DIJO QUE HUBO ERROR (Ej. Falta de Stock)
                            bool exito = Convert.ToBoolean(cmdDetalle.Parameters["p_Exito"].Value);
                            string msgBD = cmdDetalle.Parameters["p_Mensaje"].Value.ToString();

                            if (!exito)
                            {
                                // Si falló un producto, cancelamos TODO
                                throw new Exception($"Error con el producto '{item.Nombre}': {msgBD}");
                            }
                        }
                    }

                    // ---------------------------------------------------
                    // PASO C: SI LLEGAMOS AQUÍ, TODO ESTÁ BIEN -> COMMIT
                    // ---------------------------------------------------
                    transaccion.Commit();
                    mensaje = "Venta realizada correctamente.";
                    return true;
                }
                catch (Exception ex)
                {
                    // SI ALGO FALLÓ, DESHACEMOS TODO -> ROLLBACK
                    transaccion.Rollback();
                    mensaje = "Fallo en la venta: " + ex.Message;
                    idOrdenGenerada = 0;
                    return false;
                }
            }
        }
    }
}
