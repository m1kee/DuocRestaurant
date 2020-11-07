using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Order : RestaurantTable
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int StateId { get; set; }
        public string Note { get; set; }

        public Purchase Purchase { get; set; }
        public Enums.OrderState OrderState
        {
            get
            {
                var result = Enums.OrderState.NotAssigned;
                switch (StateId)
                {
                    case (int)Enums.OrderState.Pending:
                        result = Enums.OrderState.Pending;
                        break;
                    case (int)Enums.OrderState.InPreparation:
                        result = Enums.OrderState.Pending;
                        break;
                    case (int)Enums.OrderState.Ready:
                        result = Enums.OrderState.Pending;
                        break;
                }

                return result;
            }
        }
        public List<OrderDetail> OrderDetails { get; set; }

        public const string TableName = "Orden";

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string PurchaseId = "CompraId";
            public const string StateId = "EstadoId";
            public const string Note = "Nota";
        }
    }
}
