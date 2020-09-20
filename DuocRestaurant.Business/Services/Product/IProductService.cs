using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IProductService
    {
        IList<Product> Get(RestaurantDatabaseSettings ctx);
        Product Get(RestaurantDatabaseSettings ctx, int productId);
        Product Add(RestaurantDatabaseSettings ctx, Product product);
        Product Edit(RestaurantDatabaseSettings ctx, int productId, Product product);
        bool Delete(RestaurantDatabaseSettings ctx, int productId);
    }
}
