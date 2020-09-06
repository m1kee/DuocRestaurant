using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IAuthService
    {
        User SignIn(RestaurantDatabaseSettings ctx, string username, string password);
    }
}
