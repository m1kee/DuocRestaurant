using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IProviderService
    {
        IList<Provider> Get(RestaurantDatabaseSettings ctx);
        Provider Get(RestaurantDatabaseSettings ctx, int providerId);
        Provider Add(RestaurantDatabaseSettings ctx, Provider provider);
        Provider Edit(RestaurantDatabaseSettings ctx, int providerId, Provider provider);
        bool Delete(RestaurantDatabaseSettings ctx, int providerId);
    }
}
