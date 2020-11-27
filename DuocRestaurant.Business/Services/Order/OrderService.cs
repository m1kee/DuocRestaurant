using Domain;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseSettings dbSettings;
        public OrderService(IOptions<DatabaseSettings> dbSettings)
        {
            this.dbSettings = dbSettings.Value;
        }

        public Order Add(Order order)
        {
            Order result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                order.StateId = (int)Enums.OrderState.Pending;

                string query = $"INSERT INTO {Order.TableName} (" +
                    $"{Order.ColumnNames.PurchaseId}, " +
                    $"{Order.ColumnNames.StateId}, " +
                    $"{Order.ColumnNames.TableId}, " +
                    $"{Order.ColumnNames.UserId}, " +
                    $"{Order.ColumnNames.Note} " +
                    $") VALUES (" +
                    $":{Order.ColumnNames.PurchaseId}, " +
                    $"{order.StateId}, " +
                    $"{order.TableId}, " +
                    $"{order.UserId}, " +
                    $"'{order.Note}' " +
                    $") RETURNING {Order.ColumnNames.Id} INTO :{Order.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn)
                {
                    Transaction = transaction
                };
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = order.PurchaseId,
                    ParameterName = $":{Order.ColumnNames.PurchaseId}",
                    DbType = System.Data.DbType.Int32
                });
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{Order.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });


                try
                {
                    cmd.ExecuteNonQuery();

                    order.Id = Convert.ToInt32(cmd.Parameters[$":{Order.ColumnNames.Id}"].Value.ToString());

                    if (order.OrderDetails != null)
                    {
                        List<OrderDetail> orderDetails = new List<OrderDetail>();
                        foreach (var orderDetail in order.OrderDetails)
                        {
                            OrderDetail createdRecipeDetail = this.Add(conn, transaction, order.Id, orderDetail);
                            orderDetails.Add(createdRecipeDetail);
                        }
                    }

                    transaction.Commit();

                    result = order;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public OrderDetail Add(OrderDetail orderDetail)
        {
            OrderDetail result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();

                string query = $"INSERT INTO {OrderDetail.TableName} (" +
                    $"{OrderDetail.ColumnNames.OrderId}, " +
                    $"{OrderDetail.ColumnNames.ProductId}, " +
                    $"{OrderDetail.ColumnNames.RecipeId}, " +
                    $"{OrderDetail.ColumnNames.Count}, " +
                    $"{OrderDetail.ColumnNames.Price} " +
                    $") VALUES (" +
                    $"{orderDetail.OrderId}, " +
                    $"{orderDetail.ProductId}, " +
                    $"{orderDetail.RecipeId}, " +
                    $"{orderDetail.Count}, " +
                    $"{orderDetail.Price} " +
                    $")";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.ExecuteNonQuery();

                result = orderDetail;
            }

            return result;
        }

        private OrderDetail Add(OracleConnection connection, OracleTransaction transaction, int orderId, OrderDetail orderDetail)
        {
            string query = $"INSERT INTO {OrderDetail.TableName} (" +
                $"{OrderDetail.ColumnNames.OrderId}, " +
                $"{OrderDetail.ColumnNames.ProductId}, " +
                $"{OrderDetail.ColumnNames.RecipeId}, " +
                $"{OrderDetail.ColumnNames.Count}, " +
                $"{OrderDetail.ColumnNames.Price} " +
                $") VALUES (" +
                $"{orderId}, " +
                $":{OrderDetail.ColumnNames.ProductId}, " +
                $":{OrderDetail.ColumnNames.RecipeId}, " +
                $"{orderDetail.Count}, " +
                $"{orderDetail.Price} " +
                $")";
            OracleCommand cmd = new OracleCommand(query, connection)
            {
                Transaction = transaction
            };
            cmd.Parameters.Add(new OracleParameter()
            {
                Value = orderDetail.ProductId,
                ParameterName = $":{OrderDetail.ColumnNames.ProductId}",
                DbType = System.Data.DbType.Int32
            });
            cmd.Parameters.Add(new OracleParameter()
            {
                Value = orderDetail.RecipeId,
                ParameterName = $":{OrderDetail.ColumnNames.RecipeId}",
                DbType = System.Data.DbType.Int32
            });
            cmd.ExecuteNonQuery();

            orderDetail.OrderId = orderId;

            OrderDetail result = orderDetail;

            return result;
        }

        public bool Delete(int orderId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"UPDATE {Order.TableName} " +
                    $"SET {Order.ColumnNames.StateId} = {(int)Enums.OrderState.Canceled} " +
                    $"WHERE {Order.ColumnNames.Id} = {orderId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        private bool Delete(OracleConnection conn, OracleTransaction transaction, OrderDetail orderDetail)
        {
            string query = $"DELETE FROM {OrderDetail.TableName} " +
                $"WHERE {OrderDetail.ColumnNames.OrderId} = {orderDetail.OrderId} " +
                $"AND {OrderDetail.ColumnNames.ProductId} = :{OrderDetail.ColumnNames.ProductId} " +
                $"AND {OrderDetail.ColumnNames.RecipeId} = :{OrderDetail.ColumnNames.RecipeId}";
            OracleCommand deleteCommand = new OracleCommand(query, conn)
            {
                Transaction = transaction
            };
            deleteCommand.Parameters.Add(new OracleParameter()
            {
                Value = orderDetail.ProductId,
                ParameterName = $":{OrderDetail.ColumnNames.ProductId}",
                DbType = System.Data.DbType.Int32
            });
            deleteCommand.Parameters.Add(new OracleParameter()
            {
                Value = orderDetail.RecipeId,
                ParameterName = $":{OrderDetail.ColumnNames.RecipeId}",
                DbType = System.Data.DbType.Int32
            });
            deleteCommand.ExecuteNonQuery();

            bool result = true;

            return result;
        }

        public bool Delete(OrderDetail orderDetail)
        {
            bool result = false;
            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();

                string query = $"DELETE FROM {OrderDetail.TableName} " +
                $"WHERE {OrderDetail.ColumnNames.OrderId} = {orderDetail.OrderId} " +
                $"AND {OrderDetail.ColumnNames.ProductId} = :{OrderDetail.ColumnNames.ProductId} " +
                $"AND {OrderDetail.ColumnNames.RecipeId} = :{OrderDetail.ColumnNames.RecipeId}";
                OracleCommand deleteCommand = new OracleCommand(query, conn);
                deleteCommand.Parameters.Add(new OracleParameter()
                {
                    Value = orderDetail.ProductId,
                    ParameterName = $":{OrderDetail.ColumnNames.ProductId}",
                    DbType = System.Data.DbType.Int32
                });
                deleteCommand.Parameters.Add(new OracleParameter()
                {
                    Value = orderDetail.RecipeId,
                    ParameterName = $":{OrderDetail.ColumnNames.RecipeId}",
                    DbType = System.Data.DbType.Int32
                });
                deleteCommand.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public Order Edit(int orderId, Order order)
        {
            Order result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                string query = $"UPDATE {Order.TableName} " +
                    $"SET " +
                    $"{Order.ColumnNames.StateId} = {order.StateId}, " +
                    $"{Order.ColumnNames.PurchaseId} = :{Order.ColumnNames.PurchaseId} " +
                    $"WHERE {Order.ColumnNames.Id} = {orderId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = order.PurchaseId,
                    ParameterName = $":{Order.ColumnNames.PurchaseId}",
                    DbType = System.Data.DbType.Int32
                });

                try
                {
                    cmd.ExecuteNonQuery();

                    // check order details
                    if (order.OrderDetails != null)
                    {
                        if (order.OrderDetails.Any(x => !x.Active))
                        {
                            // delete inactive order details
                            foreach (var deletedOrderDetail in order.OrderDetails.Where(x => !x.Active))
                            {
                                this.Delete(conn, transaction, deletedOrderDetail);
                            }
                        }

                        // get bd order details to compare if exist or not
                        List<OrderDetail> currentOrderDetails = Get(conn, order).ToList();

                        // edit active order details
                        foreach (var editedOrderDetail in order.OrderDetails.Where(x => x.Active && currentOrderDetails.Any(y => x.OrderId == y.OrderId && x.ProductId == y.ProductId && x.RecipeId == y.RecipeId)))
                        {
                            this.Edit(conn, transaction, editedOrderDetail);
                        }

                        // create new order details
                        foreach (var createdOrderDetail in order.OrderDetails.Where(x => x.Active && !currentOrderDetails.Any(y => x.OrderId == y.OrderId && x.ProductId == y.ProductId && x.RecipeId == y.RecipeId)))
                        {
                            this.Add(conn, transaction, orderId, createdOrderDetail);
                        }

                        foreach (var deletedOrderDetail in currentOrderDetails.Where(x => !order.OrderDetails.Any(y => x.OrderId == y.OrderId && x.ProductId == y.ProductId && x.RecipeId == y.RecipeId)))
                        {
                            this.Delete(conn, transaction, deletedOrderDetail);
                        }
                    }

                    transaction.Commit();

                    result = order;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }
        public Order Edit(int orderId, Order order, OracleTransaction transaction)
        {
            Order result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();

                string query = $"UPDATE {Order.TableName} " +
                    $"SET " +
                    $"{Order.ColumnNames.PurchaseId} = :{Order.ColumnNames.PurchaseId} " +
                    $"WHERE {Order.ColumnNames.Id} = {orderId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = order.PurchaseId,
                    ParameterName = $":{Order.ColumnNames.PurchaseId}",
                    DbType = System.Data.DbType.Int32
                });

                cmd.ExecuteNonQuery();

                result = order;
            }

            return result;
        }

        private OrderDetail Edit(OracleConnection conn, OracleTransaction transaction, OrderDetail orderDetail)
        {
            string query = $"UPDATE {OrderDetail.TableName} " +
                $"SET " +
                $"{OrderDetail.ColumnNames.Count} = {orderDetail.Count} " +
                $"WHERE {OrderDetail.ColumnNames.OrderId} = {orderDetail.OrderId} " +
                $"AND {OrderDetail.ColumnNames.ProductId} = :{OrderDetail.ColumnNames.ProductId} " +
                $"AND {OrderDetail.ColumnNames.RecipeId} = :{OrderDetail.ColumnNames.RecipeId}";
            OracleCommand editCommand = new OracleCommand(query, conn);
            editCommand.Transaction = transaction;
            editCommand.Parameters.Add(new OracleParameter()
            {
                Value = orderDetail.ProductId,
                ParameterName = $":{OrderDetail.ColumnNames.ProductId}",
                DbType = System.Data.DbType.Int32
            });
            editCommand.Parameters.Add(new OracleParameter()
            {
                Value = orderDetail.RecipeId,
                ParameterName = $":{OrderDetail.ColumnNames.RecipeId}",
                DbType = System.Data.DbType.Int32
            });

            editCommand.ExecuteNonQuery();

            OrderDetail result = orderDetail;

            return result;
        }

        public IList<Order> Get()
        {
            IList<Order> result = new List<Order>();

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"SELECT " +
                    $"o.{Order.ColumnNames.Id}, " +
                    $"o.{Order.ColumnNames.PurchaseId}, " +
                    $"o.{Order.ColumnNames.StateId}, " +
                    $"o.{Order.ColumnNames.Note}, " +
                    $"o.{Order.ColumnNames.TableId}, " +
                    $"o.{Order.ColumnNames.UserId}, " +
                    $"p.{Purchase.ColumnNames.Id} AS {Purchase.TableName}{Purchase.ColumnNames.Id}, " +
                    $"p.{Purchase.ColumnNames.StateId} AS {Purchase.TableName}{Purchase.ColumnNames.StateId}, " +
                    $"p.{Purchase.ColumnNames.CreationDate} AS {Purchase.TableName}{Purchase.ColumnNames.CreationDate} " +
                    $"FROM {Order.TableName} o " +
                    $"LEFT JOIN {Purchase.TableName} p on p.{Purchase.ColumnNames.Id} = o.{Order.ColumnNames.PurchaseId} " +
                    $"WHERE o.{Order.ColumnNames.StateId} <> {(int)Enums.OrderState.Canceled}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Order order = new Order()
                    {
                        Id = Convert.ToInt32(reader[$"{Order.ColumnNames.Id}"]),
                        StateId = Convert.ToInt32(reader[$"{Order.ColumnNames.StateId}"]),
                        Note = reader[$"{Order.ColumnNames.Note}"]?.ToString(),
                        TableId = Convert.ToInt32(reader[$"{Order.ColumnNames.TableId}"]),
                        UserId = Convert.ToInt32(reader[$"{Order.ColumnNames.UserId}"]),
                    };

                    if (reader[$"{Order.ColumnNames.PurchaseId}"] != DBNull.Value)
                    {
                        order.PurchaseId = Convert.ToInt32(reader[$"{Order.ColumnNames.PurchaseId}"]);
                        order.Purchase = new Purchase()
                        {
                            Id = Convert.ToInt32(reader[$"{Purchase.TableName}{Purchase.ColumnNames.Id}"]),
                            StateId = Convert.ToInt32(reader[$"{Purchase.TableName}{Purchase.ColumnNames.StateId}"]),
                            CreationDate = Convert.ToDateTime(reader[$"{Purchase.TableName}{Purchase.ColumnNames.CreationDate}"]).ToLocalTime()
                        };
                    }

                    order.OrderDetails = this.Get(conn, order).ToList();
                    result.Add(order);
                }

                reader.Dispose();
            }

            return result;
        }

        private IList<OrderDetail> Get(OracleConnection connection, Order order)
        {
            IList<OrderDetail> result = new List<OrderDetail>();

            string query = $"SELECT " +
                $"{OrderDetail.ColumnNames.OrderId}, " +
                $"{OrderDetail.ColumnNames.ProductId}, " +
                $"{OrderDetail.ColumnNames.RecipeId}, " +
                $"{OrderDetail.ColumnNames.Price}, " +
                $"{OrderDetail.ColumnNames.Count} " +
                $"FROM {OrderDetail.TableName} " +
                $"WHERE {OrderDetail.ColumnNames.OrderId} = {order.Id} ";
            OracleCommand cmd = new OracleCommand(query, connection);

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = Convert.ToInt32(reader[OrderDetail.ColumnNames.OrderId]),
                    ProductId = Convert.ToInt32(reader[OrderDetail.ColumnNames.ProductId] == DBNull.Value ? null : reader[OrderDetail.ColumnNames.ProductId]),
                    RecipeId = Convert.ToInt32(reader[OrderDetail.ColumnNames.RecipeId] == DBNull.Value ? null : reader[OrderDetail.ColumnNames.RecipeId]),
                    Count = Convert.ToInt32(reader[OrderDetail.ColumnNames.Count]),
                    Price = Convert.ToInt32(reader[OrderDetail.ColumnNames.Price])
                };

                result.Add(orderDetail);
            }

            reader.Dispose();

            return result;
        }

        public IList<OrderDetail> Get(Order order)
        {
            IList<OrderDetail> result = new List<OrderDetail>();
            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();

                string query = $"SELECT " +
                $"{OrderDetail.ColumnNames.OrderId}, " +
                $"{OrderDetail.ColumnNames.ProductId}, " +
                $"{OrderDetail.ColumnNames.RecipeId}, " +
                $"{OrderDetail.ColumnNames.Price}, " +
                $"{OrderDetail.ColumnNames.Count} " +
                $"FROM {OrderDetail.TableName} " +
                $"WHERE {OrderDetail.ColumnNames.OrderId} = {order.Id} ";
                OracleCommand cmd = new OracleCommand(query, conn);

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderId = Convert.ToInt32(reader[OrderDetail.ColumnNames.OrderId]),
                        ProductId = Convert.ToInt32(reader[OrderDetail.ColumnNames.ProductId] == DBNull.Value ? null : reader[OrderDetail.ColumnNames.ProductId]),
                        RecipeId = Convert.ToInt32(reader[OrderDetail.ColumnNames.RecipeId] == DBNull.Value ? null : reader[OrderDetail.ColumnNames.RecipeId]),
                        Count = Convert.ToInt32(reader[OrderDetail.ColumnNames.Count]),
                        Price = Convert.ToInt32(reader[OrderDetail.ColumnNames.Price])
                    };

                    result.Add(orderDetail);
                }

                reader.Dispose();
            }

            return result;
        }

        public Order Get(int orderId)
        {
            Order result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"SELECT " +
                    $"o.{Order.ColumnNames.Id}, " +
                    $"o.{Order.ColumnNames.PurchaseId}, " +
                    $"o.{Order.ColumnNames.StateId}, " +
                    $"o.{Order.ColumnNames.Note}, " +
                    $"o.{Order.ColumnNames.TableId}, " +
                    $"o.{Order.ColumnNames.UserId}, " +
                    $"p.{Purchase.ColumnNames.Id} AS {Purchase.TableName}{Purchase.ColumnNames.Id}, " +
                    $"p.{Purchase.ColumnNames.StateId} AS {Purchase.TableName}{Purchase.ColumnNames.StateId}, " +
                    $"p.{Purchase.ColumnNames.CreationDate} AS {Purchase.TableName}{Purchase.ColumnNames.CreationDate} " +
                    $"FROM {Order.TableName} o " +
                    $"LEFT JOIN {Purchase.TableName} p on p.{Purchase.ColumnNames.Id} = o.{Order.ColumnNames.PurchaseId} " +
                    $"WHERE o.{Order.ColumnNames.Id} = {orderId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Order()
                    {
                        Id = Convert.ToInt32(reader[$"{Order.ColumnNames.Id}"]),
                        StateId = Convert.ToInt32(reader[$"{Order.ColumnNames.StateId}"]),
                        Note = reader[$"{Order.ColumnNames.Note}"]?.ToString(),
                        TableId = Convert.ToInt32(reader[$"{Order.ColumnNames.TableId}"]),
                        UserId = Convert.ToInt32(reader[$"{Order.ColumnNames.UserId}"]),
                    };

                    if (reader[$"{Order.ColumnNames.PurchaseId}"] != DBNull.Value)
                    {
                        result.PurchaseId = Convert.ToInt32(reader[$"{Order.ColumnNames.PurchaseId}"]);
                        result.Purchase = new Purchase()
                        {
                            Id = Convert.ToInt32(reader[$"{Purchase.TableName}{Purchase.ColumnNames.Id}"]),
                            StateId = Convert.ToInt32(reader[$"{Purchase.TableName}{Purchase.ColumnNames.StateId}"]),
                            CreationDate = Convert.ToDateTime(reader[$"{Purchase.TableName}{Purchase.ColumnNames.CreationDate}"]).ToLocalTime()
                        };
                    }

                    result.OrderDetails = this.Get(conn, result).ToList();
                }

                reader.Dispose();
            }

            return result;
        }

        public OrderDetail Get(int orderId, int? productId, int? recipeId)
        {
            OrderDetail result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"SELECT " +
                $"{OrderDetail.ColumnNames.OrderId}, " +
                $"{OrderDetail.ColumnNames.ProductId}, " +
                $"{OrderDetail.ColumnNames.RecipeId}, " +
                $"{OrderDetail.ColumnNames.Price}, " +
                $"{OrderDetail.ColumnNames.Count} " +
                $"FROM {OrderDetail.TableName} " +
                $"WHERE {OrderDetail.ColumnNames.OrderId} = {orderId} " +
                $"AND {OrderDetail.ColumnNames.ProductId} = :{OrderDetail.ColumnNames.ProductId} " +
                $"AND {OrderDetail.ColumnNames.RecipeId} = :{OrderDetail.ColumnNames.RecipeId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = productId,
                    ParameterName = $":{OrderDetail.ColumnNames.ProductId}",
                    DbType = System.Data.DbType.Int32
                });
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = recipeId,
                    ParameterName = $":{OrderDetail.ColumnNames.RecipeId}",
                    DbType = System.Data.DbType.Int32
                });
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new OrderDetail()
                    {
                        OrderId = Convert.ToInt32(reader[OrderDetail.ColumnNames.OrderId]),
                        ProductId = Convert.ToInt32(reader[OrderDetail.ColumnNames.ProductId] == DBNull.Value ? null : reader[OrderDetail.ColumnNames.ProductId]),
                        RecipeId = Convert.ToInt32(reader[OrderDetail.ColumnNames.RecipeId] == DBNull.Value ? null : reader[OrderDetail.ColumnNames.RecipeId]),
                        Count = Convert.ToInt32(reader[OrderDetail.ColumnNames.Count]),
                        Price = Convert.ToInt32(reader[OrderDetail.ColumnNames.Price])
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
