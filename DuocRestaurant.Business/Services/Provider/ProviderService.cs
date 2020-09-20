using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class ProviderService : IProviderService
    {
        public Provider Add(RestaurantDatabaseSettings ctx, Provider provider)
        {
            Provider result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"INSERT INTO Proveedor (" +
                    $"{Provider.ColumnNames.Name}, " +
                    $"{Provider.ColumnNames.Email}, " +
                    $"{Provider.ColumnNames.Phone}, " +
                    $"{Provider.ColumnNames.Address}, " +
                    $"{Provider.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"'{provider.Name}', " +
                    $"'{provider.Email}', " +
                    $"'{provider.Phone}', " +
                    $"'{provider.Address}', " +
                    $"{1} " +
                    $") RETURNING {Provider.ColumnNames.Id} INTO :{Provider.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{Provider.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                conn.Open();

                cmd.ExecuteNonQuery();

                provider.Id = Convert.ToInt32(cmd.Parameters[$":{Provider.ColumnNames.Id}"].Value.ToString());

                result = provider;
            }

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int providerId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Proveedor " +
                    $"SET {Provider.ColumnNames.Active} = 0 " +
                    $"WHERE {Provider.ColumnNames.Id} = {providerId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public Provider Edit(RestaurantDatabaseSettings ctx, int providerId, Provider provider)
        {
            Provider result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Proveedor " +
                    $"SET " +
                    $"{Provider.ColumnNames.Name} = '{provider.Name}', " +
                    $"{Provider.ColumnNames.Email} = '{provider.Email}', " +
                    $"{Provider.ColumnNames.Phone} = '{provider.Phone}', " +
                    $"{Provider.ColumnNames.Address} = '{provider.Address}' " +
                    $"WHERE {Provider.ColumnNames.Id} = {providerId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = provider;
            }

            return result;
        }

        public IList<Provider> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Provider> result = new List<Provider>();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Provider.ColumnNames.Id}, " +
                    $"{Provider.ColumnNames.Name}, " +
                    $"{Provider.ColumnNames.Email}, " +
                    $"{Provider.ColumnNames.Phone}, " +
                    $"{Provider.ColumnNames.Address}, " +
                    $"{Provider.ColumnNames.Active} " +
                    $"FROM Proveedor " +
                    $"WHERE {Provider.ColumnNames.Active} = 1";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Provider()
                    {
                        Id = Convert.ToInt32(reader[Provider.ColumnNames.Id]),
                        Email = reader[Provider.ColumnNames.Email]?.ToString(),
                        Name = reader[Provider.ColumnNames.Name]?.ToString(),
                        Address = reader[Provider.ColumnNames.Address]?.ToString(),
                        Phone = reader[Provider.ColumnNames.Phone]?.ToString(),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[Provider.ColumnNames.Active].ToString()))
                    });
                }

                reader.Dispose();
            }

            return result;
        }

        public Provider Get(RestaurantDatabaseSettings ctx, int providerId)
        {
            Provider result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Provider.ColumnNames.Id}, " +
                    $"{Provider.ColumnNames.Name}, " +
                    $"{Provider.ColumnNames.Email}, " +
                    $"{Provider.ColumnNames.Phone}, " +
                    $"{Provider.ColumnNames.Address}, " +
                    $"{Provider.ColumnNames.Active} " +
                    $"FROM Proveedor " +
                    $"WHERE {Provider.ColumnNames.Id} = {providerId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    result = new Provider()
                    {
                        Id = Convert.ToInt32(reader[Provider.ColumnNames.Id]),
                        Email = reader[Provider.ColumnNames.Email]?.ToString(),
                        Name = reader[Provider.ColumnNames.Name]?.ToString(),
                        Address = reader[Provider.ColumnNames.Address]?.ToString(),
                        Phone = reader[Provider.ColumnNames.Phone]?.ToString(),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[Provider.ColumnNames.Active].ToString()))
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
