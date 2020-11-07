using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IOrderService
    {
        IList<Order> Get();
        Order Get(int orderId);
        Order Add(Order order);
        Order Edit(int orderId, Order order);
        bool Delete(int orderId);
    }
}
