using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface ITableService
    {
        Table Add(RestaurantDatabaseSettings ctx, Table table);
        bool Delete(RestaurantDatabaseSettings ctx, int tableId);
        Table Edit(RestaurantDatabaseSettings ctx, int tableId, Table table);
        IList<Table> Get(RestaurantDatabaseSettings ctx);
        Table Get(RestaurantDatabaseSettings ctx, int tableId);
        Table GetByNumber(RestaurantDatabaseSettings ctx, int tableNumber);
    }
}
