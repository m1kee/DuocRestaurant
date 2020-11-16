using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Order : RestaurantTable
    {
        public int Id { get; set; }
        public int? PurchaseId { get; set; }
        public int StateId { get; set; }
        public string Note { get; set; }
        public int TableId { get; set; }
        public int UserId { get; set; }

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
        public Table Table { get; set; }
        public User User { get; set; }

        public const string TableName = "Orden";

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string PurchaseId = "CompraId";
            public const string StateId = "EstadoId";
            public const string Note = "Nota";
            public const string TableId = "MesaId";
            public const string UserId = "UsuarioId";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.OrderDetails != null)
                    result.OrderDetails = this.OrderDetails.MapAll(ctx, true);
            }

            return result;
        }
    }
}
