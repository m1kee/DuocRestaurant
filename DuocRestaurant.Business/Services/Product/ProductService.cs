using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        public Product Add(RestaurantDatabaseSettings ctx, Product product)
        {
            Product result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"INSERT INTO Producto (" +
                    $"{Product.ColumnNames.Name}, " +
                    $"{Product.ColumnNames.Details}, " +
                    $"{Product.ColumnNames.ProductTypeId}, " +
                    $"{Product.ColumnNames.Count}, " +
                    $"{Product.ColumnNames.MeasurementUnitId} " +
                    $"{Product.ColumnNames.Price} " +
                    $"{Product.ColumnNames.ProviderId} " +
                    $") VALUES (" +
                    $"'{product.Name}', " +
                    $"'{product.Details}', " +
                    $"{product.ProductTypeId}, " +
                    $"{product.Count}, " +
                    $"{product.MeasurementUnitId}, " +
                    $"{product.Price}, " +
                    $"{product.ProviderId} " +
                    $") RETURNING {Product.ColumnNames.Id} INTO :{Product.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{Product.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                conn.Open();

                cmd.ExecuteNonQuery();

                product.Id = Convert.ToInt32(cmd.Parameters[$":{Product.ColumnNames.Id}"].Value.ToString());

                result = product;
            }

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int productId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Producto " +
                    $"SET {Product.ColumnNames.Active} = 0 " +
                    $"WHERE {Product.ColumnNames.Id} = {productId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public Product Edit(RestaurantDatabaseSettings ctx, int productId, Product product)
        {
            Product result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Producto " +
                    $"SET " +
                    $"{Product.ColumnNames.Name} = '{product.Name}', " +
                    $"{Product.ColumnNames.Details} = '{product.Details}', " +
                    $"{Product.ColumnNames.ProductTypeId} = {product.ProductTypeId}, " +
                    $"{Product.ColumnNames.Count} = {product.Count}, " +
                    $"{Product.ColumnNames.MeasurementUnitId} = {product.MeasurementUnitId}, " +
                    $"{Product.ColumnNames.Price} = {product.Price}, " +
                    $"{Product.ColumnNames.ProviderId} = {product.ProviderId} " +
                    $"WHERE {Product.ColumnNames.Id} = {productId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = product;
            }

            return result;
        }

        public IList<Product> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Product> result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"p.{Product.ColumnNames.Id}, " +
                    $"p.{Product.ColumnNames.Name}, " +
                    $"p.{Product.ColumnNames.Details}, " +
                    $"p.{Product.ColumnNames.ProductTypeId}, " +
                    $"p.{Product.ColumnNames.Count}, " +
                    $"p.{Product.ColumnNames.MeasurementUnitId}, " +
                    $"p.{Product.ColumnNames.Price}, " +
                    $"p.{Product.ColumnNames.ProviderId}, " +
                    $"p.{Product.ColumnNames.Active}, " +
                    $"um.{MeasurementUnit.ColumnNames.Id}, " +
                    $"um.{MeasurementUnit.ColumnNames.Description}, " +
                    $"um.{MeasurementUnit.ColumnNames.Code}, " +
                    $"tp.{ProductType.ColumnNames.Id}, " +
                    $"tp.{ProductType.ColumnNames.Description} " +
                    $"FROM Producto p " +
                    $"JOIN UnidadMedida um ON um.Id = p.UnidadMedidaId " +
                    $"JOIN TipoProducto tp ON tp.Id = p.TipoProductoId " +
                    $"WHERE {Product.ColumnNames.Active} = 1";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (result == null)
                        result = new List<Product>();

                    result.Add(new Product()
                    {
                        Id = Convert.ToInt32(reader[Product.ColumnNames.Id]),
                        Name = reader[Product.ColumnNames.Name]?.ToString(),
                        Details = reader[Product.ColumnNames.Details]?.ToString(),
                        ProductTypeId = Convert.ToInt32(reader[Product.ColumnNames.ProductTypeId]),
                        Count = Convert.ToInt32(reader[Product.ColumnNames.Count]),
                        MeasurementUnitId = Convert.ToInt32(reader[Product.ColumnNames.MeasurementUnitId]),
                        Price = Convert.ToDecimal(reader[Product.ColumnNames.Price]),
                        ProviderId = Convert.ToInt32(reader[Product.ColumnNames.ProviderId]),
                        Active = Convert.ToBoolean(reader[Product.ColumnNames.Active].ToString()),
                        ProductType = new ProductType()
                        {
                            Id = Convert.ToInt32(reader[ProductType.ColumnNames.Id]),
                            Description = reader[ProductType.ColumnNames.Description]?.ToString()
                        },
                        MeasurementUnit = new MeasurementUnit()
                        {
                            Id = Convert.ToInt32(reader[MeasurementUnit.ColumnNames.Id]),
                            Code = reader[MeasurementUnit.ColumnNames.Code]?.ToString(),
                            Description = reader[MeasurementUnit.ColumnNames.Description]?.ToString()
                        }
                    });
                }

                reader.Dispose();
            }

            return result;
        }

        public Product Get(RestaurantDatabaseSettings ctx, int productId)
        {
            Product result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"p.{Product.ColumnNames.Id}, " +
                    $"p.{Product.ColumnNames.Name}, " +
                    $"p.{Product.ColumnNames.Details}, " +
                    $"p.{Product.ColumnNames.ProductTypeId}, " +
                    $"p.{Product.ColumnNames.Count}, " +
                    $"p.{Product.ColumnNames.MeasurementUnitId}, " +
                    $"p.{Product.ColumnNames.Price}, " +
                    $"p.{Product.ColumnNames.ProviderId}, " +
                    $"p.{Product.ColumnNames.Active}, " +
                    $"um.{MeasurementUnit.ColumnNames.Id}, " +
                    $"um.{MeasurementUnit.ColumnNames.Description}, " +
                    $"um.{MeasurementUnit.ColumnNames.Code}, " +
                    $"tp.{ProductType.ColumnNames.Id}, " +
                    $"tp.{ProductType.ColumnNames.Description} " +
                    $"FROM Producto p " +
                    $"JOIN UnidadMedida um ON um.Id = p.UnidadMedidaId " +
                    $"JOIN TipoProducto tp ON tp.Id = p.TipoProductoId " +
                    $"WHERE {Product.ColumnNames.Id} = {productId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    result = new Product()
                    {
                        Id = Convert.ToInt32(reader[Product.ColumnNames.Id]),
                        Name = reader[Product.ColumnNames.Name]?.ToString(),
                        Details = reader[Product.ColumnNames.Details]?.ToString(),
                        ProductTypeId = Convert.ToInt32(reader[Product.ColumnNames.ProductTypeId]),
                        Count = Convert.ToInt32(reader[Product.ColumnNames.Count]),
                        MeasurementUnitId = Convert.ToInt32(reader[Product.ColumnNames.MeasurementUnitId]),
                        Price = Convert.ToDecimal(reader[Product.ColumnNames.Price]),
                        ProviderId = Convert.ToInt32(reader[Product.ColumnNames.ProviderId]),
                        Active = Convert.ToBoolean(reader[Product.ColumnNames.Active].ToString()),
                        ProductType = new ProductType()
                        {
                            Id = Convert.ToInt32(reader[ProductType.ColumnNames.Id]),
                            Description = reader[ProductType.ColumnNames.Description]?.ToString()
                        },
                        MeasurementUnit = new MeasurementUnit()
                        {
                            Id = Convert.ToInt32(reader[MeasurementUnit.ColumnNames.Id]),
                            Code = reader[MeasurementUnit.ColumnNames.Code]?.ToString(),
                            Description = reader[MeasurementUnit.ColumnNames.Description]?.ToString()
                        }
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
