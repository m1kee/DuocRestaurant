using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class RecipeDetail : RestaurantTable
    {
        public int RecipeId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public bool Active { get; set; }

        public struct ColumnNames
        {
            public const string RecipeId = "RecetaId";
            public const string ProductId = "ProductoId";
            public const string Count = "Cantidad";
            public const string Active = "Activo";
        }
    }
}
