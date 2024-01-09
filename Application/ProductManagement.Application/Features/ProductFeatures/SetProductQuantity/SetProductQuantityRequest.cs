namespace ProductManagement.Application.Features.ProductFeatures.SetProductQuantity;

/// <summary>
/// Request to set product quantity
/// Defining 'Quantity' as nullable int because we want to show that a null quantity is not allowed. And that a null quantity doesn't equal to 0.
/// </summary>
/// <param name="Quantity"></param>
public class SetProductQuantityRequest : BaseRequest<SetProductQuantityRequestValidator, SetProductQuantityRequest>
{
    public int? Quantity { get; init; }

    public SetProductQuantityRequest(int? quantity)
    {
        this.Quantity = quantity;
    }
}