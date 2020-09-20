using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ProductType : RestaurantTable
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string Description = "Descripcion";
        }
    }
}
