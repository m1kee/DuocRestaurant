using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface ITableService
    {
        IList<Table> Get(RestaurantDatabaseSettings ctx);
        Table Get(RestaurantDatabaseSettings ctx, int table);
        Table Add(RestaurantDatabaseSettings ctx, Table table);
        Table Edit(RestaurantDatabaseSettings ctx, int tableId, Table table);
        Table Delete(RestaurantDatabaseSettings ctx, int tableId);
    }
}
