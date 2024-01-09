namespace ProductManagement.Application.Features.ProductFeatures.GetProduct;

/// <summary>
/// Request to create product
/// </summary>
public class GetProductRequest : BaseRequest<GetProductRequestValidator, GetProductRequest>
{
    public int Id { get; init; }

    public GetProductRequest(int id)
    {
        Id = id;
    }
}
