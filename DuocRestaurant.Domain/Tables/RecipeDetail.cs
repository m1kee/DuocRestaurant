using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class RecipeDetail : RestaurantTable
    {
        public int RecipeId { get; set; }
        public int ProductId { get; set; }
        public decimal Count { get; set; }
        public bool Active { get; set; }

        public Product Product { get; set; }

        public const string TableName = "DetalleReceta";
        public struct ColumnNames
        {
            public const string RecipeId = "RecetaId";
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
