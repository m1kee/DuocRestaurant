using Domain;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
                    $"{Purchase.ColumnNames.StateId}, " +
                    $"{Purchase.ColumnNames.TableId}, " +
                    $"{Purchase.ColumnNames.UserId}, " +
                    $"{Purchase.ColumnNames.CreationDate} " +
                    $") VALUES (" +
                    $"{purchase.StateId}, " +
                    $"{purchase.TableId}, " +
                    $"{purchase.UserId}, " +
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
                    $"{Purchase.ColumnNames.StateId} = {purchase.StateId} " +
                    $"WHERE {Purchase.ColumnNames.Id} = {purchaseId}";
                OracleCommand cmd = new OracleCommand(query, conn);
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
                    $"{Purchase.ColumnNames.TableId}, " +
                    $"{Purchase.ColumnNames.UserId}, " +
                    $"{Purchase.ColumnNames.CreationDate} " +
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
                        TableId = Convert.ToInt32(reader[$"{Purchase.ColumnNames.TableId}"]),
                        UserId = Convert.ToInt32(reader[$"{Purchase.ColumnNames.UserId}"]),
                        CreationDate = Convert.ToDateTime(reader[$"{Purchase.ColumnNames.CreationDate}"]).ToLocalTime()
                    };

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
                    $"{Purchase.ColumnNames.TableId}, " +
                    $"{Purchase.ColumnNames.UserId}, " +
                    $"{Purchase.ColumnNames.CreationDate} " +
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
                        TableId = Convert.ToInt32(reader[$"{Purchase.ColumnNames.TableId}"]),
                        UserId = Convert.ToInt32(reader[$"{Purchase.ColumnNames.UserId}"]),
                        CreationDate = Convert.ToDateTime(reader[$"{Purchase.ColumnNames.CreationDate}"]).ToLocalTime()
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
