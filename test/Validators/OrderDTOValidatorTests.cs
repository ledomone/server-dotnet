using server_dotnet.Controllers.DTO;
using server_dotnet.Controllers.Validators;


namespace server_dotnet.tests.Validators
{
    public class OrderDTOValidatorTests
    {
        private readonly OrderDTOValidator _validator;
        public OrderDTOValidatorTests()
        {
            _validator = new OrderDTOValidator();
        }

        [Fact]
        public void Validate_ValidOrder_ReturnsSuccess()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                Id = 1,
                OrganizationId = 1,
                UserId = 1,
                TotalAmount = 100.00m, // Valid amount
                OrderDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = _validator.Validate(orderDTO);

            // Assert
            Assert.True(result.IsValid);
        }


        [Fact]
        public void Validate_InvalidOrder_ReturnsErrors()
        {
            // Arrange
            var orderDTO = new OrderDTO
            {
                Id = 1,
                OrganizationId = 1,
                UserId = 1,
                TotalAmount = -1.0m, // Invalid: negative amount
                OrderDate = DateTime.Now.AddDays(1) // Invalid: future date
            };

            // Act
            var result = _validator.Validate(orderDTO);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(OrderDTO.TotalAmount));
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(OrderDTO.OrderDate));
        }
    }
}
