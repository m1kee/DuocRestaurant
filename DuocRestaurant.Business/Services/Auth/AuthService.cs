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
                string query = $"SELECT {User.ColumnNames.Id}, {User.ColumnNames.RoleId}, {User.ColumnNames.Name}, {User.ColumnNames.LastName}, {User.ColumnNames.Email}, {User.ColumnNames.Phone}, {User.ColumnNames.Address} FROM Usuario WHERE {User.ColumnNames.Email} = :username AND {User.ColumnNames.Password} = :password";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("username", username));
                cmd.Parameters.Add(new OracleParameter("password", password));

                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Email = reader["Correo"]?.ToString(),
                        Name = reader["Nombre"]?.ToString(),
                        LastName = reader["Apellido"]?.ToString(),
                        Address = reader["Direccion"]?.ToString(),
                        Phone = reader["Telefono"]?.ToString(),
                        RoleId = Convert.ToInt32(reader["RolId"])
                    };
                }

                reader.Dispose();
            }

            return user;
        }
    }
}
