using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        public User SignIn(RestaurantDatabaseSettings dbSettings, string username, string password)
        {
            User user = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"SELECT Id, RolId, Nombre, Apellido, Correo, Telefono, Direccion  FROM Usuario WHERE Correo = :username AND Contrasena = :password";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("username", username));
                cmd.Parameters.Add(new OracleParameter("password", password));

                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                reader.Read();
                user = new User()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Correo = reader["Correo"]?.ToString(),
                    Nombre = reader["Nombre"]?.ToString(),
                    Apellido = reader["Apellido"]?.ToString(),
                    Direccion = reader["Direccion"]?.ToString(),
                    Telefono = reader["Telefono"]?.ToString(),
                    RolId = Convert.ToInt32(reader["RolId"])
                };

                reader.Dispose();
            }

            return user;
        }
    }
}
