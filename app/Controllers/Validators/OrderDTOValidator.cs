using FluentValidation;
using server_dotnet.Controllers.DTO;

namespace server_dotnet.Controllers.Validators
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {
        public OrderDTOValidator()
        {
            RuleFor(o => o.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.")
                .NotNull().WithMessage("Total amount must not be null.");

            RuleFor(o => o.OrderDate)
                .Must(CommonRules.BeAValidDate).WithMessage("Order date must be a valid date and not in the future.");
        }
    }
}
