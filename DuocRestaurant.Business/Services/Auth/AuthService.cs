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
                string query = $"SELECT " +
                    $"u.{User.ColumnNames.Id}, " +
                    $"u.{User.ColumnNames.RoleId}, " +
                    $"u.{User.ColumnNames.Name}, " +
                    $"u.{User.ColumnNames.LastName}, " +
                    $"u.{User.ColumnNames.Email}, " +
                    $"u.{User.ColumnNames.Phone}, " +
                    $"u.{User.ColumnNames.Address}, " +
                    $"r.{Role.ColumnNames.Id} AS Role{Role.ColumnNames.Id}, " +
                    $"r.{Role.ColumnNames.Description} AS Role{Role.ColumnNames.Description} " +
                    $"FROM Usuario u " +
                    $"join Rol r on u.{User.ColumnNames.RoleId} = r.{Role.ColumnNames.Id} " +
                    $"WHERE LOWER({User.ColumnNames.Email}) = :username AND {User.ColumnNames.Password} = :password";

                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("username", username.ToLower()));
                cmd.Parameters.Add(new OracleParameter("password", password));

                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User()
                    {
                        Id = Convert.ToInt32(reader[User.ColumnNames.Id]),
                        Email = reader[User.ColumnNames.Email]?.ToString(),
                        Name = reader[User.ColumnNames.Name]?.ToString(),
                        LastName = reader[User.ColumnNames.LastName]?.ToString(),
                        Address = reader[User.ColumnNames.Address]?.ToString(),
                        Phone = reader[User.ColumnNames.Phone]?.ToString(),
                        RoleId = Convert.ToInt32(reader[User.ColumnNames.RoleId]),
                        Role = new Role()
                        {
                            Id = Convert.ToInt32(reader[$"Role{Role.ColumnNames.Id}"]),
                            Description = reader[$"Role{Role.ColumnNames.Description}"]?.ToString()
                        }
                    };
                }

                reader.Dispose();
            }

            return user;
        }
    }
}
