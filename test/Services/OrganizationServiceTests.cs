using FluentValidation;
using FluentValidation.Results;
using Moq;
using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;
using server_dotnet.Services;

namespace server_dotnet.tests.Services
{
    public class OrganizationServiceTests
    {
        private readonly OrganizationService _organizationService;
        private readonly Mock<IRepository<Organization>> _mockOrganizationRepository;
        private readonly Mock<IValidator<OrganizationDTO>> _mockOrganizationValidator;

        public OrganizationServiceTests()
        {
            _mockOrganizationRepository = new Mock<IRepository<Organization>>();
            _mockOrganizationValidator = new Mock<IValidator<OrganizationDTO>>();

            _organizationService = new OrganizationService(_mockOrganizationRepository.Object, _mockOrganizationValidator.Object);

            // Setup the mock validator to return a successful validation result
            _mockOrganizationValidator
                .Setup(v => v.ValidateAsync(It.IsAny<OrganizationDTO>(), default))
                .ReturnsAsync(new ValidationResult());
        }

        [Fact]
        public async Task CreateAsync_ValidOrganization_ReturnsCreatedOrganization()
        {
            // Arrange
            var organizationDTO = new OrganizationDTO
            {
                Id = 0,
                Name = "Test Org",
                Industry = "Tech",
                DateFounded = DateTime.Now.AddDays(-1)
            };
            
            _mockOrganizationRepository
                .Setup(r => r.AddAsync(It.IsAny<Organization>()))
                .ReturnsAsync(1);

            // Act
            var result = await _organizationService.CreateAsync(organizationDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(organizationDTO.Name, result.Name);
            Assert.Equal(organizationDTO.Industry, result.Industry);
            Assert.Equal(organizationDTO.DateFounded, result.DateFounded);
            _mockOrganizationRepository.Verify(r => r.AddAsync(It.IsAny<Organization>()), Times.Once());
            _mockOrganizationValidator.Verify(v => v.ValidateAsync(organizationDTO, default), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidOrganization_ThrowsValidationException()
        {
            // Arrange
            var organizationDTO = new OrganizationDTO
            {
                Id = 0,
                Name = "", // Invalid name
                Industry = "Tech",
                DateFounded = DateTime.Now.AddDays(-1)
            };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Name cannot be empty") });
            _mockOrganizationValidator
                .Setup(v => v.ValidateAsync(organizationDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _organizationService.CreateAsync(organizationDTO));
            _mockOrganizationValidator.Verify(v => v.ValidateAsync(organizationDTO, default), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidOrganization_Success()
        {
            // Arrange
            var organizationDTO = new OrganizationDTO
            {
                Id = 1,
                Name = "Updated Org",
                Industry = "Tech",
                DateFounded = DateTime.Now.AddDays(-1)
            };
            
            _mockOrganizationRepository
                .Setup(r => r.ExistsAsync(1))
                .ReturnsAsync(true);
            _mockOrganizationRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Organization { Id = 1, Name = "Old Org", Industry = "Tech", DateFounded = DateTime.Now.AddDays(-2) });
            _mockOrganizationRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Organization>()))
                .Returns(Task.CompletedTask);

            // Act
            await _organizationService.UpdateAsync(1, organizationDTO);

            // Assert
            _mockOrganizationRepository.Verify(r => r.UpdateAsync(It.IsAny<Organization>()), Times.Once());
            _mockOrganizationValidator.Verify(v => v.ValidateAsync(organizationDTO, default), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidOrganization_ThrowsValidationException()
        {
            // Arrange
            var organizationDTO = new OrganizationDTO
            {
                Id = 1,
                Name = "", // Invalid name
                Industry = "Tech",
                DateFounded = DateTime.Now.AddDays(-1)
            };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Name cannot be empty") });
            _mockOrganizationValidator
                .Setup(v => v.ValidateAsync(organizationDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _organizationService.UpdateAsync(1, organizationDTO));
            _mockOrganizationValidator.Verify(v => v.ValidateAsync(organizationDTO, default), Times.Once());
            _mockOrganizationRepository.Verify(r => r.UpdateAsync(It.IsAny<Organization>()), Times.Never());
        }

        [Fact]
        public async Task UpdateAsync_OrganizationNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var organizationDTO = new OrganizationDTO
            {
                Id = 1,
                Name = "Updated Org",
                Industry = "Tech",
                DateFounded = DateTime.Now.AddDays(-1)
            };
            
            _mockOrganizationRepository
                .Setup(r => r.ExistsAsync(1))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _organizationService.UpdateAsync(1, organizationDTO));
            _mockOrganizationRepository.Verify(r => r.UpdateAsync(It.IsAny<Organization>()), Times.Never());
        }
    }
}
