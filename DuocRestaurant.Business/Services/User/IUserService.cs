using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IUserService
    {
        IList<User> Get(RestaurantDatabaseSettings ctx);
        User Get(RestaurantDatabaseSettings ctx, int userId);
        User Add(RestaurantDatabaseSettings ctx, User user);
        User Edit(RestaurantDatabaseSettings ctx, int userId, User user);
        bool Delete(RestaurantDatabaseSettings ctx, int userId);
    }
}
