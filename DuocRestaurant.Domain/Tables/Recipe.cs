using Newtonsoft.Json.Linq;
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
        public string Details { get; set; }
        public decimal PreparationTime { get; set; }
        public string ImageBase64 { get; set; }
        public byte[] Image 
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ImageBase64))
                    return Convert.FromBase64String(ImageBase64);

                return null;
            }
        }
        public bool Active { get; set; }
        public List<RecipeDetail> RecipeDetails { get; set; }

        public const string TableName = "Receta";
        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string Name = "Nombre";
            public const string Price = "Precio";
            public const string Details = "Detalle";
            public const string PreparationTime = "TiempoPreparacion";
            public const string Image = "Imagen";
            public const string Active = "Activa";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.RecipeDetails != null)
                    result.RecipeDetails = this.RecipeDetails.MapAll(ctx, true);
            }

            return result;
        }
    }
}
