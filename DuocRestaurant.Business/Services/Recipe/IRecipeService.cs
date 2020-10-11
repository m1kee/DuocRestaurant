using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IRecipeService
    {
        Recipe Add(RestaurantDatabaseSettings ctx, Recipe recipe);
        RecipeDetail Add(RestaurantDatabaseSettings ctx, RecipeDetail recipeDetail);
        Recipe Edit(RestaurantDatabaseSettings ctx, int recipeId, Recipe recipe);
        IList<Recipe> Get(RestaurantDatabaseSettings ctx);
        IList<RecipeDetail> Get(RestaurantDatabaseSettings ctx, Recipe recipe);
        Recipe Get(RestaurantDatabaseSettings ctx, int recipeId);
        bool Delete(RestaurantDatabaseSettings ctx, int recipeId);
        bool Delete(RestaurantDatabaseSettings ctx, RecipeDetail recipeDetail);
    }
}
