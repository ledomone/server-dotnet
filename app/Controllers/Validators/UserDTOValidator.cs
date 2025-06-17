using FluentValidation;
using server_dotnet.Controllers.DTO;

namespace server_dotnet.Controllers.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(user => user.FirstName)
                .NotNull().WithMessage("First name must not be null.")
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(user => user.LastName)
                .NotNull().WithMessage("Last name must not be null.")
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(user => user.DateCreated)
                .Must(CommonRules.BeAValidDate).WithMessage("Date created must be a valid date and not in the future.");
        }
    }
}
