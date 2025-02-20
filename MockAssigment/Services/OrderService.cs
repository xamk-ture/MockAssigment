using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockAssigment.Models;
using MockAssigment.Repository;

namespace MockAssigment.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public void PlaceOrder(string product, int quantity)
        {
            if (string.IsNullOrWhiteSpace(product) || product.Length > 150 || quantity <= 0 || quantity >= 250)
            {
                throw new ArgumentException("Invalid order details");
            }

            _repository.SaveOrder(new Order { ProductName = product, Quantity = quantity });
        }

        public List<Order> GetOrders()
        {
            return _repository.GetOrders();
        }
    }

}
