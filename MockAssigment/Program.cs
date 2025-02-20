using MockAssigment.Models;
using MockAssigment.Repository;
using MockAssigment.Services;

namespace MockAssigment
{
    internal class Program
    {
        static void Main()
        {
            IOrderRepository orderRepository = new OrderRepository(); // Replace with actual implementation
            OrderService orderService = new OrderService(orderRepository);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Order Management System");
                Console.WriteLine("1. Place an order");
                Console.WriteLine("2. View all orders");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PlaceOrder(orderService);
                        break;
                    case "2":
                        ViewOrders(orderService);
                        break;
                    case "3":
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void PlaceOrder(OrderService orderService)
        {
            Console.Write("\nEnter product name: ");
            string productName = Console.ReadLine();

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a number.");
                return;
            }

            try
            {
                orderService.PlaceOrder(productName, quantity);
                Console.WriteLine("Order placed successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ViewOrders(OrderService orderService)
        {
            List<Order> orders = orderService.GetOrders();

            if (orders.Count == 0)
            {
                Console.WriteLine("\nNo orders found.");
                return;
            }

            Console.WriteLine("\nCurrent Orders:");
            foreach (var order in orders)
            {
                Console.WriteLine($"- {order.ProductName} (Quantity: {order.Quantity})");
            }
        }
    }
}
