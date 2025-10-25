using FluentValidation;
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(command => command.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(command => command.Status)
            .NotEmpty().WithMessage("Status is required.");

        RuleFor(command => command.TotalCost)
            .GreaterThan(0).WithMessage("Total cost should be greater than 0.");
    }
}