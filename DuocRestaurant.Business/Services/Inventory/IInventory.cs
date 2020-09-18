using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IInventoryService
    {
        IList<Inventory> Get(RestaurantDatabaseSettings ctx);
        Inventory Get(RestaurantDatabaseSettings ctx, int inventoryId);
        Inventory Add(RestaurantDatabaseSettings ctx, Inventory inventory);
        Inventory Edit(RestaurantDatabaseSettings ctx, int inventoryId, Inventory inventory);
        bool Delete(RestaurantDatabaseSettings ctx, int inventoryId);
    }
}
