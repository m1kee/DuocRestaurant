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

        public const string TableName = "DetalleOrden";

        public struct ColumnNames
        {
            public const string OrderId = "Id";
            public const string ProductId = "ProductoId";
            public const string RecipeId = "RecetaId";
            public const string Count = "Cantidad";
            public const string Price = "Precio";
            public const string Active = "Activa";
        }
    }
}
