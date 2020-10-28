using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public class SupplyRequestService : ISupplyRequestService
    {
        public SupplyRequest Add(RestaurantDatabaseSettings ctx, SupplyRequest supplyRequest)
        {
            SupplyRequest result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                supplyRequest.CreationDate = DateTime.Now;
                supplyRequest.Code = $"{supplyRequest.ProviderId}{supplyRequest.CreationDate:yyMMddHHmmss}";

                string query = $"INSERT INTO {SupplyRequest.TableName} (" +
                    $"{SupplyRequest.ColumnNames.Code}, " +
                    $"{SupplyRequest.ColumnNames.ProviderId}, " +
                    $"{SupplyRequest.ColumnNames.StateId}, " +
                    $"{SupplyRequest.ColumnNames.CreationDate}, " +
                    $"{SupplyRequest.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"'{supplyRequest.Code}', " +
                    $"{supplyRequest.ProviderId}, " +
                    $"{supplyRequest.StateId}, " +
                    $"TO_DATE('{supplyRequest.CreationDate:ddMMyyyyHHmm}', 'DDMMYYYYHH24MI'), " +
                    $"{1} " +
                    $") RETURNING {SupplyRequest.ColumnNames.Id} INTO :{SupplyRequest.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn)
                {
                    Transaction = transaction
                };
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{SupplyRequest.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                try
                {
                    cmd.ExecuteNonQuery();

                    supplyRequest.Id = Convert.ToInt32(cmd.Parameters[$":{SupplyRequest.ColumnNames.Id}"].Value.ToString());

                    if (supplyRequest.SupplyRequestDetails != null)
                    {
                        List<SupplyRequestDetail> recipeDetails = new List<SupplyRequestDetail>();
                        foreach (var supplyRequestDetail in supplyRequest.SupplyRequestDetails)
                        {
                            SupplyRequestDetail createdRecipeDetail = this.Add(conn, transaction, supplyRequest.Id, supplyRequestDetail);
                            recipeDetails.Add(createdRecipeDetail);
                        }
                    }

                    transaction.Commit();

                    result = supplyRequest;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public SupplyRequestDetail Add(RestaurantDatabaseSettings ctx, SupplyRequestDetail supplyRequestDetail)
        {
            SupplyRequestDetail result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();

                string query = $"INSERT INTO {SupplyRequestDetail.TableName} (" +
                    $"{SupplyRequestDetail.ColumnNames.SupplyRequestId}, " +
                    $"{SupplyRequestDetail.ColumnNames.ProductId}, " +
                    $"{SupplyRequestDetail.ColumnNames.Count}, " +
                    $"{SupplyRequestDetail.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"{supplyRequestDetail.SupplyRequestId}, " +
                    $"{supplyRequestDetail.ProductId}, " +
                    $"{supplyRequestDetail.Count}, " +
                    $"{1} " +
                    $")";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.ExecuteNonQuery();

                result = supplyRequestDetail;
            }

            return result;
        }

        private SupplyRequestDetail Add(OracleConnection connection, OracleTransaction transaction, int supplyRequestId, SupplyRequestDetail supplyRequestDetail)
        {
            string query = $"INSERT INTO {SupplyRequestDetail.TableName} (" +
                $"{SupplyRequestDetail.ColumnNames.SupplyRequestId}, " +
                $"{SupplyRequestDetail.ColumnNames.ProductId}, " +
                $"{SupplyRequestDetail.ColumnNames.Count}, " +
                $"{SupplyRequestDetail.ColumnNames.Active} " +
                $") VALUES (" +
                $"{supplyRequestId}, " +
                $"{supplyRequestDetail.ProductId}, " +
                $"{supplyRequestDetail.Count}, " +
                $"{1} " +
                $")";
            OracleCommand cmd = new OracleCommand(query, connection)
            {
                Transaction = transaction
            };
            cmd.ExecuteNonQuery();

            supplyRequestDetail.SupplyRequestId = supplyRequestId;

            SupplyRequestDetail result = supplyRequestDetail;

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int supplyRequestId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE {SupplyRequest.TableName} " +
                    $"SET {SupplyRequest.ColumnNames.Active} = 0 " +
                    $"WHERE {SupplyRequest.ColumnNames.Id} = {supplyRequestId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        private bool Delete(OracleConnection conn, OracleTransaction transaction, SupplyRequestDetail supplyRequestDetail)
        {
            string query = $"DELETE FROM {SupplyRequestDetail.TableName} " +
                $"WHERE {SupplyRequestDetail.ColumnNames.SupplyRequestId} = {supplyRequestDetail.SupplyRequestId} " +
                $"AND {SupplyRequestDetail.ColumnNames.ProductId} = {supplyRequestDetail.ProductId}";
            OracleCommand deleteCommand = new OracleCommand(query, conn)
            {
                Transaction = transaction
            };
            deleteCommand.ExecuteNonQuery();

            bool result = true;

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, SupplyRequestDetail supplyRequestDetail)
        {
            bool result = false;
            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();

                string query = $"DELETE FROM {SupplyRequestDetail.TableName} " +
                $"WHERE {SupplyRequestDetail.ColumnNames.SupplyRequestId} = {supplyRequestDetail.SupplyRequestId} " +
                $"AND {SupplyRequestDetail.ColumnNames.ProductId} = {supplyRequestDetail.ProductId}";
                OracleCommand deleteCommand = new OracleCommand(query, conn);
                deleteCommand.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public SupplyRequest Edit(RestaurantDatabaseSettings ctx, int supplyRequestId, SupplyRequest supplyRequest)
        {
            SupplyRequest result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                string query = $"UPDATE {SupplyRequest.TableName} " +
                    $"SET " +
                    $"{SupplyRequest.ColumnNames.ProviderId} = {supplyRequest.ProviderId}, " +
                    $"{SupplyRequest.ColumnNames.StateId} = {supplyRequest.StateId} " +
                    $"WHERE {SupplyRequest.ColumnNames.Id} = {supplyRequestId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Transaction = transaction;

                try
                {
                    cmd.ExecuteNonQuery();

                    // check supplyRequest details
                    if (supplyRequest.SupplyRequestDetails != null)
                    {
                        if (supplyRequest.SupplyRequestDetails.Any(x => !x.Active))
                        {
                            // delete inactive supplyRequest details
                            foreach (var deletedSupplyRequestDetail in supplyRequest.SupplyRequestDetails.Where(x => !x.Active))
                            {
                                this.Delete(conn, transaction, deletedSupplyRequestDetail);
                            }
                        }

                        // get bd supplyRequest details to compare if exist or not
                        List<SupplyRequestDetail> currentSupplyRequestDetails = Get(conn, supplyRequest).ToList();

                        // edit active supplyRequest details
                        foreach (var editedSupplyRequestDetail in supplyRequest.SupplyRequestDetails.Where(x => x.Active && currentSupplyRequestDetails.Any(y => x.SupplyRequestId == y.SupplyRequestId && x.ProductId == y.ProductId)))
                        {
                            this.Edit(conn, transaction, editedSupplyRequestDetail);
                        }

                        // create new supplyRequest details
                        foreach (var createdRecipeDetail in supplyRequest.SupplyRequestDetails.Where(x => x.Active && !currentSupplyRequestDetails.Any(y => x.SupplyRequestId == y.SupplyRequestId && x.ProductId == y.ProductId)))
                        {
                            this.Add(conn, transaction, supplyRequestId, createdRecipeDetail);
                        }

                        foreach (var deletedSupplyRequestDetail in currentSupplyRequestDetails.Where(x => !supplyRequest.SupplyRequestDetails.Any(y => x.SupplyRequestId == y.SupplyRequestId && x.ProductId == y.ProductId)))
                        {
                            this.Delete(conn, transaction, deletedSupplyRequestDetail);
                        }
                    }

                    transaction.Commit();

                    result = supplyRequest;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        private SupplyRequestDetail Edit(OracleConnection conn, OracleTransaction transaction, SupplyRequestDetail supplyRequestDetail)
        {
            string query = $"UPDATE {SupplyRequestDetail.TableName} " +
                $"SET " +
                $"{SupplyRequestDetail.ColumnNames.Count} = {supplyRequestDetail.Count} " +
                $"WHERE {SupplyRequestDetail.ColumnNames.SupplyRequestId} = {supplyRequestDetail.SupplyRequestId} " +
                $"AND {SupplyRequestDetail.ColumnNames.ProductId} = {supplyRequestDetail.ProductId}";
            OracleCommand deleteCommand = new OracleCommand(query, conn);
            deleteCommand.Transaction = transaction;
            deleteCommand.ExecuteNonQuery();

            SupplyRequestDetail result = supplyRequestDetail;

            return result;
        }

        public IList<SupplyRequest> Get(RestaurantDatabaseSettings ctx)
        {
            IList<SupplyRequest> result = new List<SupplyRequest>();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"sr.{SupplyRequest.ColumnNames.Id}, " +
                    $"sr.{SupplyRequest.ColumnNames.Code}, " +
                    $"sr.{SupplyRequest.ColumnNames.ProviderId}, " +
                    $"sr.{SupplyRequest.ColumnNames.StateId}, " +
                    $"sr.{SupplyRequest.ColumnNames.CreationDate}, " +
                    $"sr.{SupplyRequest.ColumnNames.Active}, " +
                    $"p.{Provider.ColumnNames.Id} AS {Provider.TableName}{Provider.ColumnNames.Id}, " +
                    $"p.{Provider.ColumnNames.Name} AS {Provider.TableName}{Provider.ColumnNames.Name}, " +
                    $"p.{Provider.ColumnNames.Email} AS {Provider.TableName}{Provider.ColumnNames.Email}, " +
                    $"p.{Provider.ColumnNames.Phone} AS {Provider.TableName}{Provider.ColumnNames.Phone}, " +
                    $"p.{Provider.ColumnNames.Address} AS {Provider.TableName}{Provider.ColumnNames.Address}, " +
                    $"p.{Provider.ColumnNames.Active} AS {Provider.TableName}{Provider.ColumnNames.Active}, " +
                    $"sre.{SupplyRequestState.ColumnNames.Id} AS {SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Id}, " +
                    $"sre.{SupplyRequestState.ColumnNames.Description} AS {SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Description} " +
                    $"FROM {SupplyRequest.TableName} sr " +
                    $"JOIN {Provider.TableName} p on p.{Provider.ColumnNames.Id} = sr.{SupplyRequest.ColumnNames.ProviderId} " +
                    $"JOIN {SupplyRequestState.TableName} sre on sre.{SupplyRequestState.ColumnNames.Id} = {SupplyRequest.ColumnNames.StateId} " +
                    $"WHERE sr.{SupplyRequest.ColumnNames.Active} = 1";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SupplyRequest supplyRequest = new SupplyRequest()
                    {
                        Id = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.Id}"]),
                        Code = reader[$"{SupplyRequest.ColumnNames.Code}"]?.ToString(),
                        ProviderId = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.ProviderId}"]),
                        StateId = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.StateId}"]),
                        CreationDate = Convert.ToDateTime(reader[$"{SupplyRequest.ColumnNames.CreationDate}"]).ToLocalTime(),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[$"{SupplyRequest.ColumnNames.Active}"].ToString())),

                        Provider = new Provider()
                        {
                            Id = Convert.ToInt32(reader[$"{Provider.TableName}{Provider.ColumnNames.Id}"]),
                            Email = reader[$"{Provider.TableName}{Provider.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"{Provider.TableName}{Provider.ColumnNames.Name}"]?.ToString(),
                            Address = reader[$"{Provider.TableName}{Provider.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"{Provider.TableName}{Provider.ColumnNames.Phone}"]?.ToString(),
                            Active = Convert.ToBoolean(Convert.ToInt16(reader[$"{Provider.TableName}{Provider.ColumnNames.Active}"].ToString()))
                        },

                        State = new SupplyRequestState()
                        {
                            Id = Convert.ToInt32(reader[$"{SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Id}"]),
                            Description = reader[$"{SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Description}"]?.ToString()
                        }
                    };

                    supplyRequest.SupplyRequestDetails = this.Get(conn, supplyRequest).ToList();
                    result.Add(supplyRequest);
                }

                reader.Dispose();
            }

            return result;
        }

        private IList<SupplyRequestDetail> Get(OracleConnection connection, SupplyRequest supplyRequest)
        {
            IList<SupplyRequestDetail> result = new List<SupplyRequestDetail>();

            string query = $"SELECT " +
                $"{SupplyRequestDetail.ColumnNames.SupplyRequestId}, " +
                $"{SupplyRequestDetail.ColumnNames.ProductId}, " +
                $"{SupplyRequestDetail.ColumnNames.Count}, " +
                $"{SupplyRequestDetail.ColumnNames.Active} " +
                $"FROM {SupplyRequestDetail.TableName} " +
                $"WHERE {SupplyRequestDetail.ColumnNames.Active} = 1 " +
                $"AND {SupplyRequestDetail.ColumnNames.SupplyRequestId} = {supplyRequest.Id}";
            OracleCommand cmd = new OracleCommand(query, connection);

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SupplyRequestDetail supplyRequestDetail = new SupplyRequestDetail()
                {
                    SupplyRequestId = Convert.ToInt32(reader[SupplyRequestDetail.ColumnNames.SupplyRequestId]),
                    ProductId = Convert.ToInt32(reader[SupplyRequestDetail.ColumnNames.ProductId]),
                    Count = Convert.ToDecimal(reader[SupplyRequestDetail.ColumnNames.Count]),
                    Active = Convert.ToBoolean(Convert.ToInt16(reader[SupplyRequestDetail.ColumnNames.Active].ToString()))
                };

                result.Add(supplyRequestDetail);
            }

            reader.Dispose();

            return result;
        }

        public IList<SupplyRequestDetail> Get(RestaurantDatabaseSettings ctx, SupplyRequest supplyRequest)
        {
            IList<SupplyRequestDetail> result = new List<SupplyRequestDetail>();
            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();

                string query = $"SELECT " +
                $"{SupplyRequestDetail.ColumnNames.SupplyRequestId}, " +
                $"{SupplyRequestDetail.ColumnNames.ProductId}, " +
                $"{SupplyRequestDetail.ColumnNames.Count}, " +
                $"{SupplyRequestDetail.ColumnNames.Active} " +
                $"FROM {SupplyRequestDetail.TableName} " +
                $"WHERE {SupplyRequestDetail.ColumnNames.Active} = 1 " +
                $"AND {SupplyRequestDetail.ColumnNames.SupplyRequestId} = {supplyRequest.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SupplyRequestDetail supplyRequestDetail = new SupplyRequestDetail()
                    {
                        SupplyRequestId = Convert.ToInt32(reader[SupplyRequestDetail.ColumnNames.SupplyRequestId]),
                        ProductId = Convert.ToInt32(reader[SupplyRequestDetail.ColumnNames.ProductId]),
                        Count = Convert.ToDecimal(reader[SupplyRequestDetail.ColumnNames.Count]),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[SupplyRequestDetail.ColumnNames.Active].ToString()))
                    };

                    result.Add(supplyRequestDetail);
                }

                reader.Dispose();
            }

            return result;
        }

        public SupplyRequest Get(RestaurantDatabaseSettings ctx, int supplyRequestId)
        {
            SupplyRequest result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"sr.{SupplyRequest.ColumnNames.Id}, " +
                    $"sr.{SupplyRequest.ColumnNames.Code}, " +
                    $"sr.{SupplyRequest.ColumnNames.ProviderId}, " +
                    $"sr.{SupplyRequest.ColumnNames.StateId}, " +
                    $"sr.{SupplyRequest.ColumnNames.CreationDate}, " +
                    $"sr.{SupplyRequest.ColumnNames.Active}, " +
                    $"p.{Provider.ColumnNames.Id} AS {Provider.TableName}{Provider.ColumnNames.Id}, " +
                    $"p.{Provider.ColumnNames.Name} AS {Provider.TableName}{Provider.ColumnNames.Name}, " +
                    $"p.{Provider.ColumnNames.Email} AS {Provider.TableName}{Provider.ColumnNames.Email}, " +
                    $"p.{Provider.ColumnNames.Phone} AS {Provider.TableName}{Provider.ColumnNames.Phone}, " +
                    $"p.{Provider.ColumnNames.Address} AS {Provider.TableName}{Provider.ColumnNames.Address}, " +
                    $"p.{Provider.ColumnNames.Active} AS {Provider.TableName}{Provider.ColumnNames.Active}, " +
                    $"sre.{SupplyRequestState.ColumnNames.Id} AS {SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Id}, " +
                    $"sre.{SupplyRequestState.ColumnNames.Description} AS {SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Description} " +
                    $"FROM {SupplyRequest.TableName} sr " +
                    $"JOIN {Provider.TableName} p on p.{Provider.ColumnNames.Id} = {SupplyRequest.ColumnNames.ProviderId} " +
                    $"JOIN {SupplyRequestState.TableName} sre on sre.{SupplyRequestState.ColumnNames.Id} = {SupplyRequest.ColumnNames.StateId} " +
                    $"WHERE sr.{SupplyRequest.ColumnNames.Id} = {supplyRequestId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new SupplyRequest()
                    {
                        Id = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.Id}"]),
                        Code = reader[$"{SupplyRequest.ColumnNames.Code}"]?.ToString(),
                        ProviderId = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.ProviderId}"]),
                        StateId = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.StateId}"]),
                        CreationDate = Convert.ToDateTime(reader[$"{SupplyRequest.ColumnNames.CreationDate}"]).ToLocalTime(),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[$"{SupplyRequest.ColumnNames.Active}"].ToString())),

                        Provider = new Provider()
                        {
                            Id = Convert.ToInt32(reader[$"{Provider.TableName}{Provider.ColumnNames.Id}"]),
                            Email = reader[$"{Provider.TableName}{Provider.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"{Provider.TableName}{Provider.ColumnNames.Name}"]?.ToString(),
                            Address = reader[$"{Provider.TableName}{Provider.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"{Provider.TableName}{Provider.ColumnNames.Phone}"]?.ToString(),
                            Active = Convert.ToBoolean(Convert.ToInt16(reader[$"{Provider.TableName}{Provider.ColumnNames.Active}"].ToString()))
                        },

                        State = new SupplyRequestState()
                        {
                            Id = Convert.ToInt32(reader[$"{SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Id}"]),
                            Description = reader[$"{SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Description}"]?.ToString()
                        }
                    };

                    result.SupplyRequestDetails = this.Get(conn, result).ToList();
                }

                reader.Dispose();
            }

            return result;
        }

        public SupplyRequestDetail Get(RestaurantDatabaseSettings ctx, int supplyRequestId, int productId)
        {
            SupplyRequestDetail result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                $"{SupplyRequestDetail.ColumnNames.SupplyRequestId}, " +
                $"{SupplyRequestDetail.ColumnNames.ProductId}, " +
                $"{SupplyRequestDetail.ColumnNames.Count}, " +
                $"{SupplyRequestDetail.ColumnNames.Active} " +
                $"FROM {SupplyRequestDetail.TableName} " +
                $"WHERE {SupplyRequestDetail.ColumnNames.SupplyRequestId} = {supplyRequestId} " +
                $"AND {SupplyRequestDetail.ColumnNames.ProductId} = {productId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new SupplyRequestDetail()
                    {
                        SupplyRequestId = Convert.ToInt32(reader[SupplyRequestDetail.ColumnNames.SupplyRequestId]),
                        ProductId = Convert.ToInt32(reader[SupplyRequestDetail.ColumnNames.ProductId]),
                        Count = Convert.ToDecimal(reader[SupplyRequestDetail.ColumnNames.Count]),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[SupplyRequestDetail.ColumnNames.Active].ToString()))
                    };
                }

                reader.Dispose();
            }

            return result;
        }

        public SupplyRequest GetByCode(RestaurantDatabaseSettings ctx, string supplyRequestCode)
        {
            SupplyRequest result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"sr.{SupplyRequest.ColumnNames.Id}, " +
                    $"sr.{SupplyRequest.ColumnNames.Code}, " +
                    $"sr.{SupplyRequest.ColumnNames.ProviderId}, " +
                    $"sr.{SupplyRequest.ColumnNames.StateId}, " +
                    $"sr.{SupplyRequest.ColumnNames.CreationDate}, " +
                    $"sr.{SupplyRequest.ColumnNames.Active}, " +
                    $"p.{Provider.ColumnNames.Id} AS {Provider.TableName}{Provider.ColumnNames.Id}, " +
                    $"p.{Provider.ColumnNames.Name} AS {Provider.TableName}{Provider.ColumnNames.Name}, " +
                    $"p.{Provider.ColumnNames.Email} AS {Provider.TableName}{Provider.ColumnNames.Email}, " +
                    $"p.{Provider.ColumnNames.Phone} AS {Provider.TableName}{Provider.ColumnNames.Phone}, " +
                    $"p.{Provider.ColumnNames.Address} AS {Provider.TableName}{Provider.ColumnNames.Address}, " +
                    $"p.{Provider.ColumnNames.Active} AS {Provider.TableName}{Provider.ColumnNames.Active}, " +
                    $"sre.{SupplyRequestState.ColumnNames.Id} AS {SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Id}, " +
                    $"sre.{SupplyRequestState.ColumnNames.Description} AS {SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Description} " +
                    $"FROM {SupplyRequest.TableName} sr " +
                    $"JOIN {Provider.TableName} p on p.{Provider.ColumnNames.Id} = {SupplyRequest.ColumnNames.ProviderId} " +
                    $"JOIN {SupplyRequestState.TableName} sre on sre.{SupplyRequestState.ColumnNames.Id} = {SupplyRequest.ColumnNames.StateId} " +
                    $"WHERE {SupplyRequest.ColumnNames.Code} = '{supplyRequestCode}'";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new SupplyRequest()
                    {
                        Id = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.Id}"]),
                        Code = reader[$"{SupplyRequest.ColumnNames.Code}"]?.ToString(),
                        ProviderId = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.ProviderId}"]),
                        StateId = Convert.ToInt32(reader[$"{SupplyRequest.ColumnNames.StateId}"]),
                        CreationDate = Convert.ToDateTime(reader[$"{SupplyRequest.ColumnNames.CreationDate}"]).ToLocalTime(),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[$"{SupplyRequest.ColumnNames.Active}"].ToString())),

                        Provider = new Provider()
                        {
                            Id = Convert.ToInt32(reader[$"{Provider.TableName}{Provider.ColumnNames.Id}"]),
                            Email = reader[$"{Provider.TableName}{Provider.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"{Provider.TableName}{Provider.ColumnNames.Name}"]?.ToString(),
                            Address = reader[$"{Provider.TableName}{Provider.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"{Provider.TableName}{Provider.ColumnNames.Phone}"]?.ToString(),
                            Active = Convert.ToBoolean(Convert.ToInt16(reader[$"{Provider.TableName}{Provider.ColumnNames.Active}"].ToString()))
                        },

                        State = new SupplyRequestState()
                        {
                            Id = Convert.ToInt32(reader[$"{SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Id}"]),
                            Description = reader[$"{SupplyRequestState.TableName}{SupplyRequestState.ColumnNames.Description}"]?.ToString()
                        }
                    };

                    result.SupplyRequestDetails = this.Get(conn, result).ToList();
                }

                reader.Dispose();
            }

            return result;
        }

        public SupplyRequestState GetState(RestaurantDatabaseSettings ctx, int supplyRequestStateId)
        {
            SupplyRequestState result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{SupplyRequestState.ColumnNames.Id}, " +
                    $"{SupplyRequestState.ColumnNames.Description} " +
                    $"FROM {SupplyRequestState.TableName} sr " +
                    $"WHERE {SupplyRequestState.ColumnNames.Id} = {supplyRequestStateId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new SupplyRequestState()
                    {
                        Id = Convert.ToInt32(reader[$"{SupplyRequestState.ColumnNames.Id}"]),
                        Description = reader[$"{SupplyRequestState.ColumnNames.Description}"]?.ToString()
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
