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
                    $"{Product.ColumnNames.MeasurementUnitId}, " +
                    $"{Product.ColumnNames.CostPrice}, " +
                    $"{Product.ColumnNames.SalePrice}, " +
                    $"{Product.ColumnNames.ProviderId}, " +
                    $"{Product.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"'{product.Name}', " +
                    $"'{product.Details}', " +
                    $"{product.ProductTypeId}, " +
                    $"{product.Count}, " +
                    $"{product.MeasurementUnitId}, " +
                    $"{product.CostPrice}, " +
                    $":{Product.ColumnNames.SalePrice}, " +
                    $"{product.ProviderId}, " +
                    $"{1} " +
                    $") RETURNING {Product.ColumnNames.Id} INTO :{Product.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = product.SalePrice,
                    ParameterName = $":{Product.ColumnNames.SalePrice}",
                    DbType = System.Data.DbType.Int32
                });
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
                    $"{Product.ColumnNames.CostPrice} = {product.CostPrice}, " +
                    $"{Product.ColumnNames.SalePrice} = :{Product.ColumnNames.SalePrice}, " +
                    $"{Product.ColumnNames.ProviderId} = {product.ProviderId} " +
                    $"WHERE {Product.ColumnNames.Id} = {productId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = product.SalePrice,
                    ParameterName = $":{Product.ColumnNames.SalePrice}",
                    DbType = System.Data.DbType.Int32
                });

                cmd.ExecuteNonQuery();

                result = product;
            }

            return result;
        }

        public IList<Product> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Product> result = new List<Product>();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"p.{Product.ColumnNames.Id}, " +
                    $"p.{Product.ColumnNames.Name}, " +
                    $"p.{Product.ColumnNames.Details}, " +
                    $"p.{Product.ColumnNames.ProductTypeId}, " +
                    $"p.{Product.ColumnNames.Count}, " +
                    $"p.{Product.ColumnNames.MeasurementUnitId}, " +
                    $"p.{Product.ColumnNames.SalePrice}, " +
                    $"p.{Product.ColumnNames.CostPrice}, " +
                    $"p.{Product.ColumnNames.ProviderId}, " +
                    $"p.{Product.ColumnNames.Active}, " +
                    $"um.{MeasurementUnit.ColumnNames.Id} AS MeasurementUnit{MeasurementUnit.ColumnNames.Id}, " +
                    $"um.{MeasurementUnit.ColumnNames.Description} AS MeasurementUnit{MeasurementUnit.ColumnNames.Description}, " +
                    $"um.{MeasurementUnit.ColumnNames.Code} AS MeasurementUnit{MeasurementUnit.ColumnNames.Code}, " +
                    $"tp.{ProductType.ColumnNames.Id} AS ProductType{ProductType.ColumnNames.Id}, " +
                    $"tp.{ProductType.ColumnNames.Description} AS ProductType{ProductType.ColumnNames.Description}, " +
                    $"pv.{Provider.ColumnNames.Id} AS Provider{Provider.ColumnNames.Id}, " +
                    $"pv.{Provider.ColumnNames.Name} AS Provider{Provider.ColumnNames.Name}, " +
                    $"pv.{Provider.ColumnNames.Email} AS Provider{Provider.ColumnNames.Email}, " +
                    $"pv.{Provider.ColumnNames.Phone} AS Provider{Provider.ColumnNames.Phone}, " +
                    $"pv.{Provider.ColumnNames.Address} AS Provider{Provider.ColumnNames.Address}, " +
                    $"pv.{Provider.ColumnNames.Active} AS Provider{Provider.ColumnNames.Active} " +
                    $"FROM Producto p " +
                    $"JOIN UnidadMedida um ON um.Id = p.{Product.ColumnNames.MeasurementUnitId} " +
                    $"JOIN TipoProducto tp ON tp.Id = p.{Product.ColumnNames.ProductTypeId} " +
                    $"JOIN Proveedor pv ON pv.Id = p.{Product.ColumnNames.ProviderId} " +
                    $"WHERE p.{Product.ColumnNames.Active} = 1";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = Convert.ToInt32(reader[Product.ColumnNames.Id]),
                        Name = reader[Product.ColumnNames.Name]?.ToString(),
                        Details = reader[Product.ColumnNames.Details]?.ToString(),
                        ProductTypeId = Convert.ToInt32(reader[Product.ColumnNames.ProductTypeId]),
                        Count = Convert.ToInt32(reader[Product.ColumnNames.Count]),
                        MeasurementUnitId = Convert.ToInt32(reader[Product.ColumnNames.MeasurementUnitId]),
                        CostPrice = Convert.ToDecimal(reader[Product.ColumnNames.CostPrice]),
                        ProviderId = Convert.ToInt32(reader[Product.ColumnNames.ProviderId]),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[Product.ColumnNames.Active].ToString())),
                        ProductType = new ProductType()
                        {
                            Id = Convert.ToInt32(reader[$"ProductType{ProductType.ColumnNames.Id}"]),
                            Description = reader[$"ProductType{ProductType.ColumnNames.Description}"]?.ToString()
                        },
                        MeasurementUnit = new MeasurementUnit()
                        {
                            Id = Convert.ToInt32(reader[$"MeasurementUnit{MeasurementUnit.ColumnNames.Id}"]),
                            Code = reader[$"MeasurementUnit{MeasurementUnit.ColumnNames.Code}"]?.ToString(),
                            Description = reader[$"MeasurementUnit{MeasurementUnit.ColumnNames.Description}"]?.ToString()
                        },
                        Provider = new Provider()
                        {
                            Id = Convert.ToInt32(reader[$"Provider{Provider.ColumnNames.Id}"]),
                            Email = reader[$"Provider{Provider.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"Provider{Provider.ColumnNames.Name}"]?.ToString(),
                            Address = reader[$"Provider{Provider.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"Provider{Provider.ColumnNames.Phone}"]?.ToString(),
                            Active = Convert.ToBoolean(Convert.ToInt16(reader[$"Provider{Provider.ColumnNames.Active}"].ToString()))
                        }
                    };

                    if (reader[Product.ColumnNames.SalePrice] != DBNull.Value)
                    {
                        product.SalePrice = Convert.ToDecimal(reader[Product.ColumnNames.SalePrice]);
                    }

                    result.Add(product);
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
                    $"p.{Product.ColumnNames.SalePrice}, " +
                    $"p.{Product.ColumnNames.CostPrice}, " +
                    $"p.{Product.ColumnNames.ProviderId}, " +
                    $"p.{Product.ColumnNames.Active}, " +
                    $"um.{MeasurementUnit.ColumnNames.Id} AS MeasurementUnit{MeasurementUnit.ColumnNames.Id}, " +
                    $"um.{MeasurementUnit.ColumnNames.Description} AS MeasurementUnit{MeasurementUnit.ColumnNames.Description}, " +
                    $"um.{MeasurementUnit.ColumnNames.Code} AS MeasurementUnit{MeasurementUnit.ColumnNames.Code}, " +
                    $"tp.{ProductType.ColumnNames.Id} AS ProductType{ProductType.ColumnNames.Id}, " +
                    $"tp.{ProductType.ColumnNames.Description} AS ProductType{ProductType.ColumnNames.Description}, " +
                    $"pv.{Provider.ColumnNames.Id} AS Provider{Provider.ColumnNames.Id}, " +
                    $"pv.{Provider.ColumnNames.Name} AS Provider{Provider.ColumnNames.Name}, " +
                    $"pv.{Provider.ColumnNames.Email} AS Provider{Provider.ColumnNames.Email}, " +
                    $"pv.{Provider.ColumnNames.Phone} AS Provider{Provider.ColumnNames.Phone}, " +
                    $"pv.{Provider.ColumnNames.Address} AS Provider{Provider.ColumnNames.Address}, " +
                    $"pv.{Provider.ColumnNames.Active} AS Provider{Provider.ColumnNames.Active} " +
                    $"FROM Producto p " +
                    $"JOIN UnidadMedida um ON um.Id = p.{Product.ColumnNames.MeasurementUnitId} " +
                    $"JOIN TipoProducto tp ON tp.Id = p.{Product.ColumnNames.ProductTypeId} " +
                    $"JOIN Proveedor pv ON pv.Id = p.{Product.ColumnNames.ProviderId} " +
                    $"WHERE p.{Product.ColumnNames.Id} = {productId}";
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
                        CostPrice = Convert.ToDecimal(reader[Product.ColumnNames.SalePrice]),
                        ProviderId = Convert.ToInt32(reader[Product.ColumnNames.ProviderId]),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[Product.ColumnNames.Active].ToString())),
                        ProductType = new ProductType()
                        {
                            Id = Convert.ToInt32(reader[$"ProductType{ProductType.ColumnNames.Id}"]),
                            Description = reader[$"ProductType{ProductType.ColumnNames.Description}"]?.ToString()
                        },
                        MeasurementUnit = new MeasurementUnit()
                        {
                            Id = Convert.ToInt32(reader[$"MeasurementUnit{MeasurementUnit.ColumnNames.Id}"]),
                            Code = reader[$"MeasurementUnit{MeasurementUnit.ColumnNames.Code}"]?.ToString(),
                            Description = reader[$"MeasurementUnit{MeasurementUnit.ColumnNames.Description}"]?.ToString()
                        },
                        Provider = new Provider()
                        {
                            Id = Convert.ToInt32(reader[$"Provider{Provider.ColumnNames.Id}"]),
                            Email = reader[$"Provider{Provider.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"Provider{Provider.ColumnNames.Name}"]?.ToString(),
                            Address = reader[$"Provider{Provider.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"Provider{Provider.ColumnNames.Phone}"]?.ToString(),
                            Active = Convert.ToBoolean(Convert.ToInt16(reader[$"Provider{Provider.ColumnNames.Active}"].ToString()))
                        }
                    };

                    if (reader[Product.ColumnNames.SalePrice] != DBNull.Value)
                    {
                        result.SalePrice = Convert.ToDecimal(reader[Product.ColumnNames.SalePrice]);
                    }
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
