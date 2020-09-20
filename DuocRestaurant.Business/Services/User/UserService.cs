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
            User result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"INSERT INTO Usuario (" +
                    $"{User.ColumnNames.RoleId}, " +
                    $"{User.ColumnNames.Name}, " +
                    $"{User.ColumnNames.LastName}, " +
                    $"{User.ColumnNames.Email}, " +
                    $"{User.ColumnNames.Phone}, " +
                    $"{User.ColumnNames.Address}, " +
                    $"{User.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"{user.RoleId}, " +
                    $"'{user.Name}', " +
                    $"'{user.LastName}', " +
                    $"'{user.Email}', " +
                    $"'{user.Phone}', " +
                    $"'{user.Address}', " +
                    $"{1} " +
                    $") RETURNING {User.ColumnNames.Id} INTO :{User.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{User.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                conn.Open();

                cmd.ExecuteNonQuery();

                user.Id = Convert.ToInt32(cmd.Parameters[$":{User.ColumnNames.Id}"].Value.ToString());

                result = user;
            }

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int userId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Usuario " +
                    $"SET {User.ColumnNames.Active} = 0 " +
                    $"WHERE {User.ColumnNames.Id} = {userId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public User Edit(RestaurantDatabaseSettings ctx, int userId, User user)
        {
            User result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Usuario " +
                    $"SET " +
                    $"{User.ColumnNames.RoleId} = {user.RoleId}, " +
                    $"{User.ColumnNames.Name} = '{user.Name}', " +
                    $"{User.ColumnNames.LastName} = '{user.LastName}', " +
                    $"{User.ColumnNames.Email} = '{user.Email}', " +
                    $"{User.ColumnNames.Phone} = '{user.Phone}', " +
                    $"{User.ColumnNames.Address} = '{user.Address}' " +
                    $"WHERE {User.ColumnNames.Id} = {userId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = user;
            }

            return result;
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
                    $"join Rol r on u.{User.ColumnNames.RoleId} = r.{Role.ColumnNames.Id} " +
                    $"WHERE {User.ColumnNames.Active} = 1";
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
            User result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"u.{User.ColumnNames.Id}, " +
                    $"u.{User.ColumnNames.RoleId}, " +
                    $"u.{User.ColumnNames.Name}, " +
                    $"u.{User.ColumnNames.LastName}, " +
                    $"u.{User.ColumnNames.Email}, " +
                    $"u.{User.ColumnNames.Phone}, " +
                    $"u.{User.ColumnNames.Address} " +
                    $"r.{Role.ColumnNames.Description} " +
                    $"FROM Usuario u " +
                    $"join Rol r on u.{User.ColumnNames.RoleId} = r.{Role.ColumnNames.Id} " +
                    $"WHERE {User.ColumnNames.Id} = {userId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new User()
                    {
                        Id = Convert.ToInt32(reader[User.ColumnNames.Id]),
                        Email = reader[User.ColumnNames.Email]?.ToString(),
                        Name = reader[User.ColumnNames.Name]?.ToString(),
                        LastName = reader[User.ColumnNames.LastName]?.ToString(),
                        Address = reader[User.ColumnNames.Address]?.ToString(),
                        Phone = reader[User.ColumnNames.Phone]?.ToString(),
                        RoleId = Convert.ToInt32(reader[User.ColumnNames.RoleId])
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
