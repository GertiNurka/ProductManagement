using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.HardDeleteProduct;

public sealed class HardDeleteProductRequestValidator : AbstractValidator<HardDeleteProductRequest>
{
    public HardDeleteProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1);
    }
}
