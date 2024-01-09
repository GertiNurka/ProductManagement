using FluentValidation;

namespace ProductManagement.Domain.Entities.Validators;

public class ProductValidator : BaseValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Length(3, 50);
        RuleFor(x => x.Description)
            .MaximumLength(500);
        RuleFor(x => x.Quantity)
           .GreaterThanOrEqualTo(0);
    }
}
