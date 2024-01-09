namespace ProductManagement.Application.Features.ProductFeatures.UpdateProduct;

/// <summary>
/// Request to update product
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
public class UpdateProductRequest : BaseRequest<UpdateProductRequestValidator, UpdateProductRequest>
{
    public string Name { get; init; }
    public string? Description { get; init; }

    public UpdateProductRequest(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
