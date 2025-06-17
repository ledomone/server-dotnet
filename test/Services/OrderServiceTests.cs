using FluentValidation;
using FluentValidation.Results;
using Moq;
using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;
using server_dotnet.Services;

namespace server_dotnet.tests.Services
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IValidator<OrderDTO>> _mockOrderValidator;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockOrderValidator = new Mock<IValidator<OrderDTO>>();
            _orderService = new OrderService(_mockOrderRepository.Object, _mockOrderValidator.Object);

            _mockOrderValidator
                .Setup(v => v.ValidateAsync(It.IsAny<OrderDTO>(), default))
                .ReturnsAsync(new ValidationResult());
        }

        [Fact]
        public async Task CreateAsync_ValidOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                OrderDate = DateTime.Now,
                TotalAmount = 100.0m,
                UserId = 1,
                OrganizationId = 1
            };
            _mockOrderRepository
                .Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(1);

            // Act
            var result = await _orderService.CreateAsync(orderDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(orderDTO.OrderDate, result.OrderDate);
            Assert.Equal(orderDTO.TotalAmount, result.TotalAmount);
            Assert.Equal(orderDTO.UserId, result.UserId);
            Assert.Equal(orderDTO.OrganizationId, result.OrganizationId);
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once());
            _mockOrderValidator.Verify(v => v.ValidateAsync(orderDTO, default), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidOrder_ThrowsValidationException()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                OrderDate = DateTime.Now,
                TotalAmount = -100.0m,
                UserId = 1,
                OrganizationId = 1
            };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("TotalAmount", "Total amount must be greater than zero") });
            _mockOrderValidator
                .Setup(v => v.ValidateAsync(orderDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _orderService.CreateAsync(orderDTO));
            _mockOrderValidator.Verify(v => v.ValidateAsync(orderDTO, default), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_OrganizationNotExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                OrderDate = DateTime.Now,
                TotalAmount = 100.0m,
                UserId = 1,
                OrganizationId = 999 // Assuming this organization does not exist
            };
            _mockOrderRepository
                .Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Throws(new InvalidOperationException("Organization not found"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.CreateAsync(orderDTO));
            _mockOrderValidator.Verify(v => v.ValidateAsync(orderDTO, default), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidOrder_Success()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                Id = 1,
                OrderDate = DateTime.Now.AddDays(-1),
                TotalAmount = 150.0m,
                UserId = 1,
                OrganizationId = 1
            };
            _mockOrderRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Order
                {
                    Id = 1,
                    OrderDate = DateTime.Now.AddDays(-1),
                    TotalAmount = 200.0m,
                    UserId = 1,
                    OrganizationId = 1
                });
            _mockOrderRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            // Act
            await _orderService.UpdateAsync(1, orderDTO);

            // Assert
            _mockOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once());
            _mockOrderValidator.Verify(v => v.ValidateAsync(orderDTO, default), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_OrderNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                Id = 999, // Assuming this order does not exist
                OrderDate = DateTime.Now,
                TotalAmount = 100.0m,
                UserId = 1,
                OrganizationId = 1
            };
            _mockOrderRepository
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.UpdateAsync(999, orderDTO));
            _mockOrderValidator.Verify(v => v.ValidateAsync(orderDTO, default), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidOrder_ThrowsValidationException()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                Id = 1,
                OrderDate = DateTime.Now,
                TotalAmount = -50.0m, // Invalid total amount
                UserId = 1,
                OrganizationId = 1
            };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("TotalAmount", "Total amount must be greater than zero") });
            _mockOrderValidator
                .Setup(v => v.ValidateAsync(orderDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _orderService.UpdateAsync(1, orderDTO));
            _mockOrderValidator.Verify(v => v.ValidateAsync(orderDTO, default), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_IdMismatch_ThrowsArgumentException()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                Id = 2, // Mismatched ID
                OrderDate = DateTime.Now,
                TotalAmount = 100.0m,
                UserId = 1,
                OrganizationId = 1
            };
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _orderService.UpdateAsync(1, orderDTO));
            _mockOrderValidator.Verify(v => v.ValidateAsync(orderDTO, default), Times.Once());
        }
    }
}
