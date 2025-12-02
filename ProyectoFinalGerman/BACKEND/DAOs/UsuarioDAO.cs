using MySql.Data.MySqlClient;
using ProyectoFinalGerman.MODELOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalGerman.BACKEND.DAOs
{
    /// <summary>
    /// Clase de Acceso a Datos (DAO) para la gestión de usuarios del sistema.
    /// Proporciona métodos para registrar, actualizar, consultar y autenticar usuarios (empleados y administradores).
    /// </summary>
    public class UsuarioDAO
    {
        /// <summary>
        /// Instancia para manejar la conexión con la base de datos MySQL.
        /// </summary>
        private ConexionDB conexion = new ConexionDB();

        /// <summary>
        /// Registra un nuevo usuario en la base de datos con toda su información personal y de acceso.
        /// </summary>
        /// <param name="user">Objeto <see cref="Usuario"/> que contiene los datos del empleado a registrar.</param>
        /// <param name="mensaje">Mensaje de salida que indica el resultado de la operación (éxito o error).</param>
        /// <returns>
        /// <c>true</c> si el usuario se registró correctamente; de lo contrario, <c>false</c>.
        /// </returns>
        public bool RegistrarUsuario(Usuario user, out string mensaje)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_RegistrarUsuarioCompleto", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parametros obligatorios
                        cmd.Parameters.AddWithValue("p_Nombre", user.Nombre);
                        cmd.Parameters.AddWithValue("p_Apellidos", user.Apellidos);
                        cmd.Parameters.AddWithValue("p_Usuario", user.UsuarioAcceso);

                        // Encriptación de contraseña
                        string passHash = EncriptarPass(user.Contrasenha);
                        cmd.Parameters.AddWithValue("p_PassHash", passHash);

                        // Parametros opcionales (Manejo de nulos)
                        cmd.Parameters.AddWithValue("p_Rfc", string.IsNullOrEmpty(user.Rfc) ? DBNull.Value : (object)user.Rfc);
                        cmd.Parameters.AddWithValue("p_Curp", string.IsNullOrEmpty(user.Curp) ? DBNull.Value : (object)user.Curp);
                        cmd.Parameters.AddWithValue("p_Email1", string.IsNullOrEmpty(user.Email) ? DBNull.Value : (object)user.Email);
                        cmd.Parameters.AddWithValue("p_NSS", string.IsNullOrEmpty(user.Nss) ? DBNull.Value : (object)user.Nss);
                        cmd.Parameters.AddWithValue("p_Direccion", string.IsNullOrEmpty(user.Direccion) ? DBNull.Value : (object)user.Direccion);
                        cmd.Parameters.AddWithValue("p_Tel1", user.Telefono1);
                        cmd.Parameters.AddWithValue("p_Tel2", string.IsNullOrEmpty(user.Telefono2) ? DBNull.Value : (object)user.Telefono2);
                        cmd.Parameters.AddWithValue("p_TipoSangre", string.IsNullOrEmpty(user.TipoSangre) ? DBNull.Value : (object)user.TipoSangre);
                        cmd.Parameters.AddWithValue("p_TipoUsuario", user.TipoUsuario);

                        // Ejecutar
                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0)
                        {
                            mensaje = "Empleado registrado correctamente.";
                            return true;
                        }
                        else
                        {
                            mensaje = "No se pudo registrar el empleado.";
                            return false;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Manejo para duplicados
                    if (ex.Number == 1062)
                    {
                        mensaje = "El nombre de usuario ya existe. Por favor elige otro.";
                    }
                    else
                    {
                        mensaje = "Error de base de datos: " + ex.Message;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    mensaje = "Error: " + ex.Message;
                    return false;
                }
            }
        }

        /// <summary>
        /// Encripta una cadena de texto utilizando el algoritmo SHA256.
        /// </summary>
        /// <param name="textoPlano">La contraseña o texto a encriptar.</param>
        /// <returns>Una cadena hexadecimal que representa el hash SHA256 del texto.</returns>
        private string EncriptarPass(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return "";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(textoPlano));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Obtiene una lista resumen de todos los usuarios registrados.
        /// </summary>
        /// <returns>Un <see cref="DataTable"/> con la información resumida de los usuarios.</returns>
        public DataTable ObtenerUsuariosResumen()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ObtenerUsuariosResumen", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception)
                {
                    // Tabla vacia
                }
            }
            return dt;
        }

        /// <summary>
        /// Obtiene toda la información detallada de un usuario específico por su ID.
        /// </summary>
        /// <param name="id">El ID único del usuario a buscar.</param>
        /// <returns>
        /// Un objeto <see cref="Usuario"/> con todos los datos cargados si se encuentra; 
        /// de lo contrario, devuelve <c>null</c>.
        /// </returns>
        public Usuario ObtenerPorId(int id)
        {
            Usuario u = null;
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ObtenerUsuarioPorId", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_Id", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                u = new Usuario();
                                // Conversión segura de datos
                                u.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                                u.Nombre = reader["nombre"].ToString();
                                u.Apellidos = reader["apellidos"].ToString();
                                u.UsuarioAcceso = reader["usuario"].ToString();

                                // Valuida nulos
                                u.Rfc = reader["rfc"] == DBNull.Value ? "" : reader["rfc"].ToString();
                                u.Curp = reader["curp"].ToString();
                                u.Nss = reader["NSS"] == DBNull.Value ? "" : reader["NSS"].ToString();
                                u.Email = reader["email1"] == DBNull.Value ? "" : reader["email1"].ToString();
                                u.Direccion = reader["direccion"] == DBNull.Value ? "" : reader["direccion"].ToString();
                                u.Telefono1 = reader["telefono1"].ToString();
                                u.Telefono2 = reader["telefono2"] == DBNull.Value ? "" : reader["telefono2"].ToString();
                                u.TipoSangre = reader["tipoSangre"].ToString();
                                u.TipoUsuario = reader["tipoUsuario"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al leer usuario (DAO): " + ex.Message);
                }
            }
            return u;
        }

        /// <summary>
        /// Actualiza la información de un usuario existente en la base de datos.
        /// Permite actualizar la contraseña solo si se proporciona una nueva.
        /// </summary>
        /// <param name="user">Objeto <see cref="Usuario"/> con los datos actualizados.</param>
        /// <param name="mensaje">Mensaje de salida con el resultado de la operación.</param>
        /// <returns><c>true</c> si la actualización fue exitosa.</returns>
        public bool ActualizarUsuario(Usuario user, out string mensaje)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizarUsuarioCompleto", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_IdUsuario", user.IdUsuario);
                        cmd.Parameters.AddWithValue("p_Nombre", user.Nombre);
                        cmd.Parameters.AddWithValue("p_Apellidos", user.Apellidos);
                        cmd.Parameters.AddWithValue("p_Usuario", user.UsuarioAcceso);

                        // Si no está vacía, se encripta y actualiza.
                        // Si está vacía, se envía DBNull para mantener la actual.
                        if (!string.IsNullOrEmpty(user.Contrasenha))
                        {
                            string hash = EncriptarPass(user.Contrasenha);
                            cmd.Parameters.AddWithValue("p_PassHash", hash);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("p_PassHash", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("p_Rfc", string.IsNullOrEmpty(user.Rfc) ? DBNull.Value : (object)user.Rfc);
                        cmd.Parameters.AddWithValue("p_Curp", string.IsNullOrEmpty(user.Curp) ? DBNull.Value : (object)user.Curp);
                        cmd.Parameters.AddWithValue("p_Email1", string.IsNullOrEmpty(user.Email) ? DBNull.Value : (object)user.Email);
                        cmd.Parameters.AddWithValue("p_NSS", string.IsNullOrEmpty(user.Nss) ? DBNull.Value : (object)user.Nss);
                        cmd.Parameters.AddWithValue("p_Direccion", string.IsNullOrEmpty(user.Direccion) ? DBNull.Value : (object)user.Direccion);
                        cmd.Parameters.AddWithValue("p_Tel1", user.Telefono1);
                        cmd.Parameters.AddWithValue("p_Tel2", string.IsNullOrEmpty(user.Telefono2) ? DBNull.Value : (object)user.Telefono2);
                        cmd.Parameters.AddWithValue("p_TipoSangre", string.IsNullOrEmpty(user.TipoSangre) ? DBNull.Value : (object)user.TipoSangre);
                        cmd.Parameters.AddWithValue("p_TipoUsuario", user.TipoUsuario);

                        cmd.ExecuteNonQuery();
                        mensaje = "Usuario actualizado correctamente.";
                        return true;
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1062) mensaje = "Ese nombre de usuario ya está ocupado.";
                    else mensaje = "Error BD: " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    mensaje = "Error: " + ex.Message;
                    return false;
                }
            }
        }

        /// <summary>
        /// Valida las credenciales de un usuario para permitir el acceso al sistema.
        /// </summary>
        /// <param name="user">Nombre de usuario.</param>
        /// <param name="passTextoPlano">Contraseña en texto plano ingresada por el usuario.</param>
        /// <returns>
        /// Un objeto <see cref="Usuario"/> con la información básica (ID, Nombre, Rol) si las credenciales son correctas;
        /// de lo contrario, devuelve <c>null</c>.
        /// </returns>
        public Usuario Login(string user, string passTextoPlano)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ValidarLogin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_User", user);

                        // Encriptar contraseña para comparar
                        string passHash = EncriptarPass(passTextoPlano);
                        cmd.Parameters.AddWithValue("p_Pass", passHash);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Usuario u = new Usuario();
                                u.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                                u.Nombre = reader["nombre"].ToString();
                                u.Apellidos = reader["apellidos"].ToString();
                                u.UsuarioAcceso = reader["usuario"].ToString();
                                u.TipoUsuario = reader["tipoUsuario"].ToString();
                                return u;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null; // Incorrecto o no encontrado
        }
    }
}