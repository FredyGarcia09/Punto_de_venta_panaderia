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
    public class UsuarioDAO
    {
        private ConexionDB conexion = new ConexionDB();

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


                        string passHash = EncriptarPass(user.Contrasenha);
                        cmd.Parameters.AddWithValue("p_PassHash", passHash);

                        // Parametros opcionales

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

        // Encriptar contraseña a SHA256
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
    }
}
