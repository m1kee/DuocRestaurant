using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuocRestaurant.API.Hubs
{
    public class NotificationsHub : Hub<INotificationClient>
    {
        public async Task Notify(string message)
        {
            await Clients.All.Notify(message);
        }

        public async Task NotifyUser(int userId, string message)
        {
            await Clients.All.Notify(userId, message);
        }
    }

    public interface INotificationClient
    {
        Task Notify(string message);
        Task Notify(int userId, string message);
    }
}
