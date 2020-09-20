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
            IList<MeasurementUnit> result = new List<MeasurementUnit>();

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

        public MeasurementUnit Get(RestaurantDatabaseSettings ctx, int measurementUnitId)
        {
            MeasurementUnit result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{MeasurementUnit.ColumnNames.Id}, " +
                    $"{MeasurementUnit.ColumnNames.Code}, " +
                    $"{MeasurementUnit.ColumnNames.Description} " +
                    $"FROM UnidadMedida " +
                    $"WHERE {MeasurementUnit.ColumnNames.Id} = {measurementUnitId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new MeasurementUnit()
                    {
                        Id = Convert.ToInt32(reader[MeasurementUnit.ColumnNames.Id]),
                        Code = reader[MeasurementUnit.ColumnNames.Code]?.ToString(),
                        Description = reader[MeasurementUnit.ColumnNames.Description]?.ToString()
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
