using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class ProductTypeService : IProductTypeService
    {
        public ProductType Get(RestaurantDatabaseSettings ctx, int productTypeId)
        {
            ProductType result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{ProductType.ColumnNames.Id}, " +
                    $"{ProductType.ColumnNames.Description} " +
                    $"FROM TipoProducto " +
                    $"WHERE {ProductType.ColumnNames.Id} = {productTypeId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new ProductType()
                    {
                        Id = Convert.ToInt32(reader[ProductType.ColumnNames.Id]),
                        Description = reader[ProductType.ColumnNames.Description]?.ToString()
                    };
                }

                reader.Dispose();
            }

            return result;
        }

        IList<ProductType> IProductTypeService.Get(RestaurantDatabaseSettings ctx)
        {
            IList<ProductType> result = new List<ProductType>();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{ProductType.ColumnNames.Id}, " +
                    $"{ProductType.ColumnNames.Description} " +
                    $"FROM TipoProducto";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new ProductType()
                    {
                        Id = Convert.ToInt32(reader[ProductType.ColumnNames.Id]),
                        Description = reader[ProductType.ColumnNames.Description]?.ToString()
                    });
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
