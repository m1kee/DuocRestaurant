using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class MeasurementUnitService : IMeasurementUnitService
    {
        public IList<MeasurementUnit> Get(RestaurantDatabaseSettings ctx)
        {
            IList<MeasurementUnit> result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{MeasurementUnit.ColumnNames.Id}, " +
                    $"{MeasurementUnit.ColumnNames.Code}, " +
                    $"{MeasurementUnit.ColumnNames.Description} " +
                    $"FROM UnidadMedida";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (result == null)
                        result = new List<MeasurementUnit>();

                    result.Add(new MeasurementUnit()
                    {
                        Id = Convert.ToInt32(reader[MeasurementUnit.ColumnNames.Id]),
                        Code = reader[MeasurementUnit.ColumnNames.Code]?.ToString(),
                        Description = reader[MeasurementUnit.ColumnNames.Description]?.ToString()
                    });
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
