using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IRoleService
    {
        IList<Role> Get(RestaurantDatabaseSettings ctx);
        Role Get(RestaurantDatabaseSettings ctx, int roleId);
    }
}
