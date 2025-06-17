using FluentValidation;
using server_dotnet.Controllers.DTO;

namespace server_dotnet.Controllers.Validators
{
    public class OrganizationDTOValidator : AbstractValidator<OrganizationDTO>
    {
        public OrganizationDTOValidator()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage("Organization name is required.")
                .NotNull().WithMessage("Organization name must not be null.");

            RuleFor(o => o.DateFounded)
                .Must(CommonRules.BeAValidDate).WithMessage("Date founded must be a valid date and not in the future.");
        }
    }
}
