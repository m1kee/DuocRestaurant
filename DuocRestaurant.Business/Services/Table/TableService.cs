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
                    $"{Table.ColumnNames.InUse} = {(table.InUse ? 1 : 0)} " +
                    $"WHERE {Table.ColumnNames.Id} = {tableId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = table;
            }

            return result;
        }

        public IList<Table> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Table> result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Table.ColumnNames.Id}, " +
                    $"{Table.ColumnNames.Number}, " +
                    $"{Table.ColumnNames.Capacity}, " +
                    $"{Table.ColumnNames.Description}, " +
                    $"{Table.ColumnNames.Active}, " +
                    $"{Table.ColumnNames.InUse} " +
                    $"FROM Mesa";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (result == null)
                        result = new List<Table>();

                    result.Add(new Table()
                    {
                        Id = Convert.ToInt32(reader[Table.ColumnNames.Id]),
                        Number = Convert.ToInt32(reader[Table.ColumnNames.Number]),
                        Capacity = Convert.ToInt32(reader[Table.ColumnNames.Capacity]),
                        Description = reader[Table.ColumnNames.Description]?.ToString(),
                        Active = Convert.ToBoolean(reader[Table.ColumnNames.Active]),
                        InUse = Convert.ToBoolean(reader[Table.ColumnNames.InUse])
                    });
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
                    $"{Table.ColumnNames.Id}, " +
                    $"{Table.ColumnNames.Number}, " +
                    $"{Table.ColumnNames.Capacity}, " +
                    $"{Table.ColumnNames.Description}, " +
                    $"{Table.ColumnNames.Active}, " +
                    $"{Table.ColumnNames.InUse} " +
                    $"FROM Mesa " +
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
                        InUse = Convert.ToBoolean(reader[Table.ColumnNames.InUse])
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
