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
        public List<RecipeDetail> RecipeDetails { get; set; }
    }
}
