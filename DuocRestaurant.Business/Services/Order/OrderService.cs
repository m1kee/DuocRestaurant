using Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseSettings dbSettings;
        public OrderService(IOptions<DatabaseSettings> dbSettings)
        {
            this.dbSettings = dbSettings.Value;
        }

        public Order Add(Order order)
        {
            throw new NotImplementedException();
        }

        public OrderDetail Add(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int orderId)
        {
            throw new NotImplementedException();
        }

        public Order Edit(int orderId, Order order)
        {
            throw new NotImplementedException();
        }

        public IList<Order> Get()
        {
            throw new NotImplementedException();
        }

        public Order Get(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
