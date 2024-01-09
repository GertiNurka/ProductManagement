namespace ProductManagement.Application.Features.ProductFeatures.HardDeleteProduct;

/// <summary>
/// Request to hard delete product
/// </summary>
public class HardDeleteProductRequest : BaseRequest<HardDeleteProductRequestValidator, HardDeleteProductRequest>
{
    public int Id { get; init; }

    public HardDeleteProductRequest(int id)
    {
        Id = id;
    }
}
