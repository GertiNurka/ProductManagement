namespace ProductManagement.Application.Features.ProductFeatures.CreateProduct;

/// <summary>
/// Request to create a product.
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
public class CreateProductRequest : BaseRequest<CreateProductRequestValidator, CreateProductRequest>
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public int? Quantity { get; init; }

    public CreateProductRequest(string name, string? description, int? quantity)
    {
        Name = name;
        Description = description;
        Quantity = quantity;
    }
}
