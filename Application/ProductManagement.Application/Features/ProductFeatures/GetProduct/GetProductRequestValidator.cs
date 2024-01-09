using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.GetProduct;

public sealed class GetProductRequestValidator : AbstractValidator<GetProductRequest>
{
    public GetProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1);
    }
}
