using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class OrderDetail : RestaurantTable
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? RecipeId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }

        public Product Product { get; set; }
        public Recipe Recipe { get; set; }

        public const string TableName = "DetalleOrden";

        public struct ColumnNames
        {
            public const string OrderId = "OrdenId";
            public const string ProductId = "ProductoId";
            public const string RecipeId = "RecetaId";
            public const string Count = "Cantidad";
            public const string Price = "Precio";
            public const string Active = "Activa";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.Product != null)
                    result.Product = this.Product.Map(ctx, true);
                if (this.Recipe != null)
                    result.Recipe = this.Recipe.Map(ctx, true);
            }

            return result;
        }
    }
}
