using Domain;
using Microsoft.AspNetCore.Mvc.Formatters;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class TableService : ITableService
    {
        public Table Add(RestaurantDatabaseSettings ctx, Table table)
        {
            Table result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"INSERT INTO Mesa " +
                    $"({Table.ColumnNames.Number}, " +
                    $"{Table.ColumnNames.Capacity}, " +
                    $"{Table.ColumnNames.Description}, " +
                    $"{Table.ColumnNames.Active}, " +
                    $"{Table.ColumnNames.InUse}) " +
                    $"VALUES " +
                    $"({table.Number}, " +
                    $"{table.Capacity}, " +
                    $"'{table.Description}', " +
                    $"{(table.Active ? 1 : 0)}, " +
                    $"0) " +
                    $"RETURNING {Table.ColumnNames.Id} INTO :{Table.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{Table.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                conn.Open();

                cmd.ExecuteNonQuery();

                table.Id = Convert.ToInt32(cmd.Parameters[$":{Table.ColumnNames.Id}"].Value.ToString());

                result = table;
            }

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int tableId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Mesa " +
                    $"SET {Table.ColumnNames.Active} = 0 " +
                    $"WHERE {Table.ColumnNames.Id} = {tableId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public Table Edit(RestaurantDatabaseSettings ctx, int tableId, Table table)
        {
            Table result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Mesa " +
                    $"SET {Table.ColumnNames.Number} = {table.Number}, " +
                    $"{Table.ColumnNames.Capacity} = {table.Capacity}, " +
                    $"{Table.ColumnNames.Description} = '{table.Description}', " +
                    $"{Table.ColumnNames.Active} = {(table.Active ? 1 : 0)}, " +
                    $"{Table.ColumnNames.InUse} = {(table.InUse ? 1 : 0)}, " +
                    $"{Table.ColumnNames.UserId} = :{Table.ColumnNames.UserId} " +
                    $"WHERE {Table.ColumnNames.Id} = {tableId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter() { 
                    Value = table.UserId,
                    ParameterName = $":{Table.ColumnNames.UserId}",
                    DbType = System.Data.DbType.Int32
                });
                conn.Open();

                cmd.ExecuteNonQuery();

                result = table;
            }

            return result;
        }

        public IList<Table> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Table> result = new List<Table>();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"m.{Table.ColumnNames.Id}, " +
                    $"m.{Table.ColumnNames.Number}, " +
                    $"m.{Table.ColumnNames.Capacity}, " +
                    $"m.{Table.ColumnNames.Description}, " +
                    $"m.{Table.ColumnNames.Active}, " +
                    $"m.{Table.ColumnNames.InUse}, " +
                    $"u.{User.ColumnNames.Id} AS {User.TableName}{User.ColumnNames.Id}, " +
                    $"u.{User.ColumnNames.RoleId} AS {User.TableName}{User.ColumnNames.RoleId}, " +
                    $"u.{User.ColumnNames.Name} AS {User.TableName}{User.ColumnNames.Name}, " +
                    $"u.{User.ColumnNames.LastName} AS {User.TableName}{User.ColumnNames.LastName}, " +
                    $"u.{User.ColumnNames.Email} AS {User.TableName}{User.ColumnNames.Email}, " +
                    $"u.{User.ColumnNames.Phone} AS {User.TableName}{User.ColumnNames.Phone}, " +
                    $"u.{User.ColumnNames.Address} AS {User.TableName}{User.ColumnNames.Address}, " +
                    $"u.{User.ColumnNames.Password} AS {User.TableName}{User.ColumnNames.Password}, " +
                    $"u.{User.ColumnNames.Active}  AS {User.TableName}{User.ColumnNames.Active} " +
                    $"FROM {Table.TableName} m " +
                    $"LEFT JOIN {User.TableName} u ON u.{User.ColumnNames.Id} = m.{Table.ColumnNames.UserId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var table = new Table()
                    {
                        Id = Convert.ToInt32(reader[Table.ColumnNames.Id]),
                        Number = Convert.ToInt32(reader[Table.ColumnNames.Number]),
                        Capacity = Convert.ToInt32(reader[Table.ColumnNames.Capacity]),
                        Description = reader[Table.ColumnNames.Description]?.ToString(),
                        Active = Convert.ToBoolean(reader[Table.ColumnNames.Active]),
                        InUse = Convert.ToBoolean(reader[Table.ColumnNames.InUse])
                    };

                    if (reader[Table.ColumnNames.UserId] != DBNull.Value)
                    {
                        table.UserId = Convert.ToInt32(reader[Table.ColumnNames.UserId]);
                        table.User = new User()
                        {
                            Id = Convert.ToInt32(reader[$"{User.TableName}{User.ColumnNames.Id}"]),
                            Email = reader[$"{User.TableName}{User.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"{User.TableName}{User.ColumnNames.Name}"]?.ToString(),
                            LastName = reader[$"{User.TableName}{User.ColumnNames.LastName}"]?.ToString(),
                            Address = reader[$"{User.TableName}{User.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"{User.TableName}{User.ColumnNames.Phone}"]?.ToString(),
                            RoleId = Convert.ToInt32(reader[$"{User.TableName}{User.ColumnNames.RoleId}"])
                        };
                    }

                    result.Add(table);
                }

                reader.Dispose();
            }

            return result;
        }

        public Table Get(RestaurantDatabaseSettings ctx, int tableId)
        {
            Table result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"m.{Table.ColumnNames.Id}, " +
                    $"m.{Table.ColumnNames.Number}, " +
                    $"m.{Table.ColumnNames.Capacity}, " +
                    $"m.{Table.ColumnNames.Description}, " +
                    $"m.{Table.ColumnNames.Active}, " +
                    $"m.{Table.ColumnNames.InUse}, " +
                    $"u.{User.ColumnNames.RoleId} AS {User.TableName}{User.ColumnNames.RoleId}, " +
                    $"u.{User.ColumnNames.Name} AS {User.TableName}{User.ColumnNames.Name}, " +
                    $"u.{User.ColumnNames.LastName} AS {User.TableName}{User.ColumnNames.LastName}, " +
                    $"u.{User.ColumnNames.Email} AS {User.TableName}{User.ColumnNames.Email}, " +
                    $"u.{User.ColumnNames.Phone} AS {User.TableName}{User.ColumnNames.Phone}, " +
                    $"u.{User.ColumnNames.Address} AS {User.TableName}{User.ColumnNames.Address}, " +
                    $"u.{User.ColumnNames.Password} AS {User.TableName}{User.ColumnNames.Password}, " +
                    $"u.{User.ColumnNames.Active}  AS {User.TableName}{User.ColumnNames.Active} " +
                    $"FROM {Table.TableName} m " +
                    $"LEFT JOIN {User.TableName} u ON u.{User.ColumnNames.Id} = m.{Table.ColumnNames.UserId} " +
                    $"WHERE {Table.ColumnNames.Id} = {tableId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Table()
                    {
                        Id = Convert.ToInt32(reader[Table.ColumnNames.Id]),
                        Number = Convert.ToInt32(reader[Table.ColumnNames.Number]),
                        Capacity = Convert.ToInt32(reader[Table.ColumnNames.Capacity]),
                        Description = reader[Table.ColumnNames.Description]?.ToString(),
                        Active = Convert.ToBoolean(reader[Table.ColumnNames.Active]),
                        InUse = Convert.ToBoolean(reader[Table.ColumnNames.InUse]),
                        UserId = Convert.ToInt32(reader[Table.ColumnNames.UserId])
                    };

                    if (reader[Table.ColumnNames.UserId] != DBNull.Value)
                    {
                        result.UserId = Convert.ToInt32(reader[Table.ColumnNames.UserId]);
                        result.User = new User()
                        {
                            Id = Convert.ToInt32(reader[$"{User.TableName}{User.ColumnNames.Id}"]),
                            Email = reader[$"{User.TableName}{User.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"{User.TableName}{User.ColumnNames.Name}"]?.ToString(),
                            LastName = reader[$"{User.TableName}{User.ColumnNames.LastName}"]?.ToString(),
                            Address = reader[$"{User.TableName}{User.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"{User.TableName}{User.ColumnNames.Phone}"]?.ToString(),
                            RoleId = Convert.ToInt32(reader[$"{User.TableName}{User.ColumnNames.RoleId}"])
                        };
                    }
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
