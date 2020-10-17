using Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class SupplyRequest : RestaurantTable
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public DateTime CreationDate { get; set; }
        public int StateId { get; set; }
        public bool Active { get; set; }

        public Provider Provider { get; set; }
        public List<SupplyRequestDetail> SupplyRequestDetails { get; set; }

        public const string TableName = "PedidoInsumo";

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string ProviderId = "ProveedorId";
            public const string StateId = "EstadoPedidoId";
            public const string Active = "Activo";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.Provider != null)
                    result.Provider = this.Provider.Map(ctx, true);
                if (this.SupplyRequestDetails != null)
                    result.SupplyRequestDetails = this.SupplyRequestDetails.MapAll(ctx, true);
            }

            return result;
        }
    }
}
