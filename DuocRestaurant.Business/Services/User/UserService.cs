using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class UserService : IUserService
    {
        public User Add(RestaurantDatabaseSettings ctx, User user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int userId)
        {
            throw new NotImplementedException();
        }

        public User Edit(RestaurantDatabaseSettings ctx, int userId, User user)
        {
            throw new NotImplementedException();
        }

        public IList<User> Get(RestaurantDatabaseSettings ctx)
        {
            IList<User> users = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"u.{User.ColumnNames.Id}, " +
                    $"u.{User.ColumnNames.RoleId}, " +
                    $"u.{User.ColumnNames.Name}, " +
                    $"u.{User.ColumnNames.LastName}, " +
                    $"u.{User.ColumnNames.Email}, " +
                    $"u.{User.ColumnNames.Phone}, " +
                    $"u.{User.ColumnNames.Address}, " +
                    $"r.{Role.ColumnNames.Description} " +
                    $"FROM Usuario u " +
                    $"join Rol r on u.{User.ColumnNames.RoleId} = r.{Role.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (users == null)
                        users = new List<User>();

                    users.Add(new User()
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
                            Id = Convert.ToInt32(reader[User.ColumnNames.RoleId]),
                            Description = reader[Role.ColumnNames.Description]?.ToString()
                        }
                    });
                }

                reader.Dispose();
            }

            return users;
        }

        public User Get(RestaurantDatabaseSettings ctx, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
