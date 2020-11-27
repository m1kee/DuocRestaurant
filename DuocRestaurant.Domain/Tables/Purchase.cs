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
        public string URL { get; set; }
        public string Token { get; set; }
        public int FlowOrder { get; set; }

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
                    case (int)Enums.PurchaseState.Paid:
                        result = Enums.PurchaseState.Paid;
                        break;
                    case (int)Enums.PurchaseState.Rejected:
                        result = Enums.PurchaseState.Rejected;
                        break;
                    case (int)Enums.PurchaseState.Canceled:
                        result = Enums.PurchaseState.Canceled;
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
            public const string URL = "URL";
            public const string Token = "Token";
            public const string FlowOrder = "FlowOrder";
        }
    }
}
