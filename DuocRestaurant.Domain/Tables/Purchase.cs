using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Purchase : RestaurantTable
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public DateTime CreationDate { get; set; }
        public int StateId { get; set; }
        public Enums.PurchaseState PurchaseState
        {
            get
            {
                var result = Enums.PurchaseState.NotAssigned;
                switch (StateId)
                {
                    case (int)Enums.PurchaseState.PendingPayment:
                        result = Enums.PurchaseState.PendingPayment;
                        break;
                    case (int)Enums.PurchaseState.PaidInCash:
                        result = Enums.PurchaseState.PaidInCash;
                        break;
                    case (int)Enums.PurchaseState.PaidByCredit:
                        result = Enums.PurchaseState.PaidByCredit;
                        break;
                    case (int)Enums.PurchaseState.PaidByDebit:
                        result = Enums.PurchaseState.PaidByDebit;
                        break;
                }

                return result;
            }
        }
        public List<Order> Orders { get; set; }

        public const string TableName = "Compra";

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string Total = "Total";
            public const string CreationDate = "Fecha";
            public const string StateId = "EstadoId";
        }
    }
}
