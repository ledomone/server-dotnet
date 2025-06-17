using server_dotnet.Controllers.DTO;
using server_dotnet.Controllers.Validators;
namespace server_dotnet.tests.Validators
{
    public class OrganizationDTOValidatorTests
    {
        private readonly OrganizationDTOValidator _validator;
        public OrganizationDTOValidatorTests()
        {
            _validator = new OrganizationDTOValidator();
        }

        [Fact]
        public void Validate_ValidOrganization_ReturnsSuccess()
        {
            // Arrange
            var organizationDTO = new OrganizationDTO
            {
                Name = "Valid Org",
                Industry = "Tech",
                DateFounded = DateTime.Now.AddYears(-1)
            };

            // Act
            var result = _validator.Validate(organizationDTO);

            // Assert
            Assert.True(result.IsValid);
        }
        [Fact]
        public void Validate_InvalidOrganization_ReturnsErrors()
        {
            // Arrange
            var organizationDTO = new OrganizationDTO
            {
                Name = "",
                Industry = "Tech",
                DateFounded = DateTime.Now.AddYears(1) // Future date
            };

            // Act
            var result = _validator.Validate(organizationDTO);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(OrganizationDTO.Name));
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(OrganizationDTO.DateFounded));
        }
    }
}
