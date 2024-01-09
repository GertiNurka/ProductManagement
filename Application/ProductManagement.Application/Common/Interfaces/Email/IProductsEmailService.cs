namespace ProductManagement.Application.Common.Interfaces.Email;

public interface IProductsEmailService
{
    Task NotifySubscibersForProductQuantityChange(string productName, bool newInStock, bool newOutOfStock, CancellationToken cancellationToken);
    Task NotifySubscibersForNewSupportedProduct(string productName, CancellationToken cancellationToken);
    Task NotifySubscibersForProductChange(string productName, string changes, CancellationToken cancellationToken);
    Task NotifySubscibersForProductBeenUnavalable(string productName, CancellationToken cancellationToken);
    Task NotifySubscibersForProductBeenBackAvailable(string productName, CancellationToken cancellationToken);
    Task NotifySubscibersForProductNotBeenSupportedAnyMore(string productName, CancellationToken cancellationToken);
}
