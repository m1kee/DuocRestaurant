using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuocRestaurant.API.Hubs
{
    public class OrdersHub : Hub<IOrderClient>
    {
        public async Task ReloadOrders()
        {
            await Clients.All.ReloadOrders();
        }
    }

    public interface IOrderClient
    {
        Task ReloadOrders();
    }
}
