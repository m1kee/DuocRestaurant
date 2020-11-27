using Domain;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly DatabaseSettings dbSettings;

        public PurchaseService(IOptions<DatabaseSettings> dbSettings)
        {
            this.dbSettings = dbSettings.Value;

        }

        public Purchase Add(Purchase purchase)
        {
            Purchase result = null;


            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();

                purchase.StateId = (int)Enums.PurchaseState.PendingPayment;
                purchase.CreationDate = DateTime.Now;

                string query = $"INSERT INTO {Purchase.TableName} (" +
                    $"{Purchase.ColumnNames.Total}, " +
                    $"{Purchase.ColumnNames.StateId}, " +
                    $"{Purchase.ColumnNames.CreationDate} " +
                    $") VALUES (" +
                    $"{purchase.Total}, " +
                    $"{purchase.StateId}, " +
                    $"TO_DATE('{purchase.CreationDate:ddMMyyyyHHmm}', 'DDMMYYYYHH24MI') " +
                    $") RETURNING {Purchase.ColumnNames.Id} INTO :{Purchase.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{Purchase.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                cmd.ExecuteNonQuery();

                purchase.Id = Convert.ToInt32(cmd.Parameters[$":{Purchase.ColumnNames.Id}"].Value.ToString());

                result = purchase;
            }

            return result;
        }

        public bool Delete(int purchaseId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"UPDATE {Purchase.TableName} " +
                    $"SET {Purchase.ColumnNames.StateId} = {(int)Enums.PurchaseState.Canceled} " +
                    $"WHERE {Purchase.ColumnNames.Id} = {purchaseId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public Purchase Edit(int purchaseId, Purchase purchase)
        {
            Purchase result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                conn.Open();

                string query = $"UPDATE {Purchase.TableName} " +
                    $"SET " +
                    $"{Purchase.ColumnNames.StateId} = {purchase.StateId}, " +
                    $"{Purchase.ColumnNames.URL} = :{Purchase.ColumnNames.URL}, " +
                    $"{Purchase.ColumnNames.Token} = :{Purchase.ColumnNames.Token}, " +
                    $"{Purchase.ColumnNames.FlowOrder} = :{Purchase.ColumnNames.FlowOrder} " +
                    $"WHERE {Purchase.ColumnNames.Id} = {purchaseId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = purchase.URL,
                    ParameterName = $":{Purchase.ColumnNames.URL}",
                    DbType = System.Data.DbType.String
                });
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = purchase.Token,
                    ParameterName = $":{Purchase.ColumnNames.Token}",
                    DbType = System.Data.DbType.String
                });
                cmd.Parameters.Add(new OracleParameter()
                {
                    Value = purchase.FlowOrder,
                    ParameterName = $":{Purchase.ColumnNames.FlowOrder}",
                    DbType = System.Data.DbType.Int32
                });
                cmd.ExecuteNonQuery();

                result = purchase;
            }

            return result;
        }

        public IList<Purchase> Get()
        {
            IList<Purchase> result = new List<Purchase>();

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Purchase.ColumnNames.Id}, " +
                    $"{Purchase.ColumnNames.StateId}, " +
                    $"{Purchase.ColumnNames.Total}, " +
                    $"{Purchase.ColumnNames.CreationDate}, " +
                    $"{Purchase.ColumnNames.URL}, " +
                    $"{Purchase.ColumnNames.Token}, " +
                    $"{Purchase.ColumnNames.FlowOrder} " +
                    $"FROM {Purchase.TableName} ";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Purchase purchase = new Purchase()
                    {
                        Id = Convert.ToInt32(reader[$"{Purchase.ColumnNames.Id}"]),
                        StateId = Convert.ToInt32(reader[$"{Purchase.ColumnNames.StateId}"]),
                        Total = Convert.ToInt32(reader[$"{Purchase.ColumnNames.Total}"]),
                        CreationDate = Convert.ToDateTime(reader[$"{Purchase.ColumnNames.CreationDate}"]).ToLocalTime()
                    };

                    if (reader[Purchase.ColumnNames.URL] != DBNull.Value)
                        purchase.URL = reader[Purchase.ColumnNames.URL]?.ToString();
                    if (reader[Purchase.ColumnNames.Token] != DBNull.Value)
                        purchase.Token = reader[Purchase.ColumnNames.Token]?.ToString();
                    if (reader[Purchase.ColumnNames.FlowOrder] != DBNull.Value)
                        purchase.FlowOrder = Convert.ToInt32(reader[Purchase.ColumnNames.FlowOrder]);

                    result.Add(purchase);
                }

                reader.Dispose();
            }

            return result;
        }

        public Purchase Get(int purchaseId)
        {
            Purchase result = null;

            using (OracleConnection conn = new OracleConnection(dbSettings.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Purchase.ColumnNames.Id}, " +
                    $"{Purchase.ColumnNames.StateId}, " +
                    $"{Purchase.ColumnNames.Total}, " +
                    $"{Purchase.ColumnNames.CreationDate}, " +
                    $"{Purchase.ColumnNames.URL}, " +
                    $"{Purchase.ColumnNames.Token}, " +
                    $"{Purchase.ColumnNames.FlowOrder} " +
                    $"FROM {Purchase.TableName} " +
                    $"WHERE {Purchase.ColumnNames.Id} = {purchaseId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Purchase()
                    {
                        Id = Convert.ToInt32(reader[$"{Purchase.ColumnNames.Id}"]),
                        StateId = Convert.ToInt32(reader[$"{Purchase.ColumnNames.StateId}"]),
                        Total = Convert.ToInt32(reader[$"{Purchase.ColumnNames.Total}"]),
                        CreationDate = Convert.ToDateTime(reader[$"{Purchase.ColumnNames.CreationDate}"]).ToLocalTime()
                    };

                    if (reader[Purchase.ColumnNames.URL] != DBNull.Value)
                        result.URL = reader[Purchase.ColumnNames.URL]?.ToString();
                    if (reader[Purchase.ColumnNames.Token] != DBNull.Value)
                        result.Token = reader[Purchase.ColumnNames.Token]?.ToString();
                    if (reader[Purchase.ColumnNames.FlowOrder] != DBNull.Value)
                        result.FlowOrder = Convert.ToInt32(reader[Purchase.ColumnNames.FlowOrder]);
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
