using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Recipe : RestaurantTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Detail { get; set; }
        public bool Active { get; set; }
        public List<RecipeDetail> RecipeDetails { get; set; }

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string Name = "Nombre";
            public const string Price = "Precio";
            public const string Detail = "Detalle";
            public const string Active = "Activa";
        }
    }
}
