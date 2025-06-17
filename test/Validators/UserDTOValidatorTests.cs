using server_dotnet.Controllers.DTO;
using server_dotnet.Controllers.Validators;

namespace server_dotnet.tests.Validators
{
    public class UserDTOValidatorTests
    {
        private readonly UserDTOValidator _validator;
        public UserDTOValidatorTests()
        {
            _validator = new UserDTOValidator();
        }

        [Fact]
        public void Validate_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                DateCreated = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = _validator.Validate(userDTO);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_InvalidUser_ReturnsErrors()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                FirstName = null, // Invalid: null first name
                LastName = "", // Invalid: empty last name
                DateCreated = DateTime.Now.AddDays(1) // Invalid: future date
            };

            // Act
            var result = _validator.Validate(userDTO);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserDTO.FirstName));
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserDTO.LastName));
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(UserDTO.DateCreated));
        }
    }
}