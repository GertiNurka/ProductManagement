namespace ProductManagement.Application.Features.ProductFeatures.SoftDeleteProduct;

/// <summary>
/// Request to soft delete product
/// </summary>
/// <param name="Id"></param>
public class SoftDeleteProductRequest : BaseRequest<SoftDeleteProductRequestValidator, SoftDeleteProductRequest>
{
    public int Id { get; init; }

    public SoftDeleteProductRequest(int id)
    {
        Id = id;
    }
}
