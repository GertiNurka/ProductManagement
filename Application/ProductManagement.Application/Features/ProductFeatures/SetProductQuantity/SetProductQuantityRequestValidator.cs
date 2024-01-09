using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.SetProductQuantity;

public sealed class SetProductQuantityRequestValidator : AbstractValidator<SetProductQuantityRequest>
{
    public SetProductQuantityRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}
