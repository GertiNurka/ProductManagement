using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.GetProducts;

public sealed class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
{
    public GetProductsRequestValidator()
    {
        //no validations for now. Maybe add validation if more logic is been added to this request
    }
}
