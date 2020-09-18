using Business.Services;
using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class RoleService : IRoleService
    {
        public IList<Role> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Role> result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Role.ColumnNames.Id}, " +
                    $"{Role.ColumnNames.Description} " +
                    $"FROM Rol";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (result == null)
                        result = new List<Role>();

                    result.Add(new Role()
                    {
                        Id = Convert.ToInt32(reader[Role.ColumnNames.Id]),
                        Description = reader[Role.ColumnNames.Description]?.ToString()
                    });
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
