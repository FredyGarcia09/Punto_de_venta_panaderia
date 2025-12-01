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
        private string connectionString = "Server=localhost;Database=VENTAS;Uid=root;Pwd=tu_contraseña;"; // <--- Poner tu contraseña real

        public bool GuardarVentaCompleta(int idUsuario, List<ItemCarrito> productos, out string mensaje)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlTransaction transaccion = conn.BeginTransaction();

                try
                {
                    int idOrdenGenerada = 0;
                    using (MySqlCommand cmdOrden = new MySqlCommand("sp_CrearOrden", conn))
                    {
                        cmdOrden.Transaction = transaccion;
                        cmdOrden.CommandType = CommandType.StoredProcedure;
                        cmdOrden.Parameters.AddWithValue("p_IdUsuario", idUsuario);

                        MySqlParameter outParam = new MySqlParameter("p_IdOrdenGenerada", MySqlDbType.Int32);
                        outParam.Direction = ParameterDirection.Output;
                        cmdOrden.Parameters.Add(outParam);

                        cmdOrden.ExecuteNonQuery();
                        idOrdenGenerada = Convert.ToInt32(outParam.Value);
                    }

                    foreach (var item in productos)
                    {
                        using (MySqlCommand cmdDetalle = new MySqlCommand("sp_AgregarDetalleOrden", conn))
                        {
                            cmdDetalle.Transaction = transaccion;
                            cmdDetalle.CommandType = CommandType.StoredProcedure;
                            cmdDetalle.Parameters.AddWithValue("p_IdOrden", idOrdenGenerada);
                            cmdDetalle.Parameters.AddWithValue("p_IdProducto", item.IdProducto);
                            cmdDetalle.Parameters.AddWithValue("p_Cantidad", item.Cantidad);

                            MySqlParameter pMensaje = new MySqlParameter("p_Mensaje", MySqlDbType.VarChar, 100);
                            pMensaje.Direction = ParameterDirection.Output;
                            cmdDetalle.Parameters.Add(pMensaje);

                            MySqlParameter pExito = new MySqlParameter("p_Exito", MySqlDbType.Bit);
                            pExito.Direction = ParameterDirection.Output;
                            cmdDetalle.Parameters.Add(pExito);

                            cmdDetalle.ExecuteNonQuery();

                            if (!Convert.ToBoolean(pExito.Value))
                            {
                                transaccion.Rollback();
                                mensaje = "Error con " + item.Nombre + ": " + pMensaje.Value.ToString();
                                return false;
                            }
                        }
                    }

                    transaccion.Commit();
                    mensaje = "Compra realizada exitosamente. Ticket #" + idOrdenGenerada;
                    return true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    mensaje = "Error crítico: " + ex.Message;
                    return false;
                }
            }
        }
    }
}
