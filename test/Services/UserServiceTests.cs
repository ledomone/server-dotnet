using FluentValidation;
using FluentValidation.Results;
using Moq;
using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;
using server_dotnet.Services;

namespace server_dotnet.tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IValidator<UserDTO>> _mockUserValidator;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUserValidator = new Mock<IValidator<UserDTO>>();
            _userService = new UserService(_mockUserRepository.Object, _mockUserValidator.Object);

            _mockUserValidator
                .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), default))
                .ReturnsAsync(new ValidationResult());
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                DateCreated = DateTime.Now.AddDays(-1),
                Email = "john@doe.com",
                OrganizationId = 1
            };
            _mockUserRepository
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(1);

            // Act
            var result = await _userService.CreateAsync(userDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(userDTO.FirstName, result.FirstName);
            Assert.Equal(userDTO.LastName, result.LastName);
            Assert.Equal(userDTO.Email, result.Email);
            Assert.Equal(userDTO.OrganizationId, result.OrganizationId);
            Assert.Equal(userDTO.DateCreated, result.DateCreated);
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _mockUserValidator.Verify(v => v.ValidateAsync(userDTO, default), Times.Once);
        }

        [Fact]
        public async Task CreateUser_InvalidUser_ThrowsValidationException()
        {
            // Arrange
            var userDTO = new UserDTO { FirstName = "", LastName = "Doe", DateCreated = DateTime.Now };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("FirstName", "First name is required") });
            _mockUserValidator.Setup(v => v.ValidateAsync(userDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(userDTO));
        }

        [Fact]
        public async Task UpdateAsync_ValidUser_Success()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                DateCreated = DateTime.Now.AddDays(-1),
                Email = "john@doe.com",
                OrganizationId = 1
            };
            _mockUserRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@doe.com",
                    OrganizationId = 1,
                    DateCreated = DateTime.Now.AddDays(-1)
                });

            //Act
            await _userService.UpdateAsync(1, userDTO);

            // Assert
            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
            _mockUserValidator.Verify(v => v.ValidateAsync(userDTO, default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidUser_ThrowsValidationException()
        {
            // Arrange
            var userDTO = new UserDTO { Id = 1, FirstName = "", LastName = "Doe", DateCreated = DateTime.Now };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("FirstName", "First name is required") });
            _mockUserValidator.Setup(v => v.ValidateAsync(userDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _userService.UpdateAsync(1, userDTO));
        }

        [Fact]
        public async Task UpdateAsync_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userDTO = new UserDTO { Id = 1, FirstName = "Jane", LastName = "Doe", DateCreated = DateTime.Now };
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateAsync(1, userDTO));
        }
    }
}
