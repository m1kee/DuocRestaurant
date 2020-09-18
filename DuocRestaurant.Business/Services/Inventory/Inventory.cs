using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class InventoryService : IInventoryService
    {
        public Inventory Add(RestaurantDatabaseSettings ctx, Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int inventoryId)
        {
            throw new NotImplementedException();
        }

        public Inventory Edit(RestaurantDatabaseSettings ctx, int inventoryId, Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public IList<Inventory> Get(RestaurantDatabaseSettings ctx)
        {
            throw new NotImplementedException();
        }

        public Inventory Get(RestaurantDatabaseSettings ctx, int inventoryId)
        {
            throw new NotImplementedException();
        }
    }
}
