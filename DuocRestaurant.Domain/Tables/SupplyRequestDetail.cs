using Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class SupplyRequestDetail : RestaurantTable
    {
        public int SupplyRequestId { get; set; }
        public int ProductId { get; set; }
        public decimal Count { get; set; }
        public bool Active { get; set; }

        public Product Product { get; set; }

        public const string TableName = "DetallePedidoInsumo";

        public struct ColumnNames
        {
            public const string SupplyRequestId = "PedidoInsumoId";
            public const string ProductId = "ProductoId";
            public const string Count = "Cantidad";
            public const string Active = "Activo";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.Product != null)
                    result.Product = this.Product.Map(ctx, true);
            }

            return result;
        }
    }
}
