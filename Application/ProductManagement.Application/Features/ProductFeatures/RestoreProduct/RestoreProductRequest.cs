namespace ProductManagement.Application.Features.ProductFeatures.RestoreProduct;

public class RestoreProductRequest : BaseRequest<RestoreProductRequestValidator, RestoreProductRequest>
{
    public int Id { get; init; }

    public RestoreProductRequest(int id)
    {
        Id = id;
    }
}
