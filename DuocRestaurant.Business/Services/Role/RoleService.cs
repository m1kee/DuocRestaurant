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
            IList<Role> result = new List<Role>();

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

        public Role Get(RestaurantDatabaseSettings ctx, int roleId)
        {
            Role result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Role.ColumnNames.Id}, " +
                    $"{Role.ColumnNames.Description} " +
                    $"FROM Rol " +
                    $"WHERE {Role.ColumnNames.Id} = {roleId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Role()
                    {
                        Id = Convert.ToInt32(reader[Role.ColumnNames.Id]),
                        Description = reader[Role.ColumnNames.Description]?.ToString()
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
