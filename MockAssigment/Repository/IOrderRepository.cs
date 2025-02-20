using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockAssigment.Models;

namespace MockAssigment.Repository
{
    public interface IOrderRepository
    {
        void SaveOrder(Order order);
        List<Order> GetOrders();
    }
}


