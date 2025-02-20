using MockAssigment;
using Moq;

public class UserServiceTests
{
    [Fact]
    public void CreateUser_SendsWelcomeEmail()
    {
        // 1. Creatig mock object from IEmailService
        var emailServiceMock = new Mock<IEmailService>();

        // 2. Injecting mock to UserService
        var userService = new UserService(emailServiceMock.Object);

        // 3. Runs method we want to test 
        userService.CreateUser("matti", "matti@example.com");

        // 4. Verify that the SendEmail method was called once with given parameters
        emailServiceMock.Verify(
            es => es.SendEmail(
                "matti@example.com",
                "Tervetuloa!",
                "Hei matti, tervetuloa palveluun!"
            ),
            Times.Once
        );
    }

    public class BadOrderService
    {
        private readonly OrderRepository _repository;

        public BadOrderService()
        {
            _repository = new OrderRepository();
        }

        public void PlaceOrder(string product, int quantity)
        {
            _repository.SaveOrder(new Order { ProductName = product, Quantity = quantity });
        }
    }

    public class OrderServiceTests
    {
        [Fact]
        public void PlaceOrder_ShouldSaveOrder()
        {
            // Arrange
            var service = new BadOrderService();
            var order = new Order { ProductName = "test-product", Quantity = 1 };

            // Act
            service.PlaceOrder(order.ProductName, order.Quantity);

            // Assert
            // It is not easy to verify whether SaveOrder was called here
            // because the repository cannot be mocked.
            // We could read the stored values directly from the OrderRepository class,
            // but that is not a good practice in unit testing.

        }
    }

    [Fact]
    public void PlaceOrder_ShouldSaveOrdery()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);
        var order = new Order() { ProductName = "Test", Quantity = 1 };

        service.PlaceOrder(order.ProductName, order.Quantity);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.PlaceOrder("", 1));
        mockRepo.Verify(repo => repo.SaveOrder(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public void PlaceOrder_ShouldSaveOrder()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);
        var order = new Order { ProductName = "test-product", Quantity = 1 };

        // Act
        service.PlaceOrder(order.ProductName, order.Quantity);

        // Assert
        mockRepo.Verify(repo => repo.SaveOrder(It.Is<Order>(
            o => o.ProductName == order.ProductName && o.Quantity == order.Quantity)), Times.Once);
    }

    [Fact]
    public void PlaceOrder_ShouldNotSaveOrder_WhenProductNameIsEmpty()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.PlaceOrder("", 1));
        mockRepo.Verify(repo => repo.SaveOrder(It.IsAny<Order>()), Times.Never);
    }


    [Theory]
    [InlineData("", 1)]  // Empty order name
    [InlineData("ValidProduct", -5)]  // Negative amount
    [InlineData(null, 3)]  // Null oder name
    public void PlaceOrder_ShouldThrowArgumentException_WhenInvalidInput(string productName, int quantity)
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.PlaceOrder(productName, quantity));
        mockRepo.Verify(repo => repo.SaveOrder(It.IsAny<Order>()), Times.Never);
    }


    [Fact]
    public void GetOrders_ShouldReturnExpectedOrders()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var expectedOrders = new List<Order>
        {
            new Order { ProductName = "Product1", Quantity = 2 },
            new Order { ProductName = "Product2", Quantity = 3 }
        };

        mockRepo.Setup(repo => repo.GetOrders()).Returns(expectedOrders);
        var service = new OrderService(mockRepo.Object);

        // Act
        var actualOrders = service.GetOrders();

        // Assert
        Assert.Equal(expectedOrders.Count, actualOrders.Count);
        Assert.Equal(expectedOrders[0].ProductName, actualOrders[0].ProductName);
        Assert.Equal(expectedOrders[1].Quantity, actualOrders[1].Quantity);
    }
}