using Microsoft.Extensions.Logging;
using ProductManagement.Application.Common.Interfaces.Email;
using ProductManagement.Application.Services.Products;

namespace ProductManagement.Infrastructure.Email;

/// <summary>
/// Responsible to send emails out.
/// TODO: 
///     1. Extract this service in its own library together with ISmtpClientHelper.
///     2. Inject/retrieve email templates and subscribers.
/// </summary>
public class ProductsEmailService : IProductsEmailService
{
    private readonly ILogger<ProductsEmailService> _logger;
    private readonly ISmtpClientHelper _smtpClientHelper;

    public ProductsEmailService(ILogger<ProductsEmailService> logger, ISmtpClientHelper smtpClientHelper)
    {
        _logger = logger;
        _smtpClientHelper = smtpClientHelper;

        _logger.LogInformation($"{nameof(ProductsEmailService)} created.");
    }

    public async Task NotifySubscibersForNewSupportedProduct(string productName, CancellationToken cancellationToken)
    {
        //TODO:
        //Call service responsible to find all users who have subscribed to be notified when this product is back in stock.
        //Call service responsible to gather 'InStock' email templates, i.e from azure storage account or cosmos DB
        //Call smtp server or Microsoft graph to send the email out
    }

    public async Task NotifySubscibersForProductBeenBackAvailable(string productName, CancellationToken cancellationToken)
    {
        //TODO:
        //Call service responsible to find all users who have subscribed to be notified when this product is offered again.
        //Call service responsible to gather 'InStock' email templates, i.e from azure storage account or cosmos DB
        //Call smtp server or Microsoft graph to send the email out
    }

    public async Task NotifySubscibersForProductBeenUnavalable(string productName, CancellationToken cancellationToken)
    {
        //TODO:
        //Call service responsible to find all users who have subscribed to be notified when this product is not offered anymore.
        //Call service responsible to gather 'InStock' email templates, i.e from azure storage account or cosmos DB
        //Call smtp server or Microsoft graph to send the email out
    }

    public async Task NotifySubscibersForProductChange(string productName, string changes, CancellationToken cancellationToken)
    {
        //TODO:
        //Call service responsible to find all users who have subscribed to be notified when this product has changed.
        //Call service responsible to gather 'InStock' email templates, i.e from azure storage account or cosmos DB
        //Call smtp server or Microsoft graph to send the email out
    }

    public async Task NotifySubscibersForProductNotBeenSupportedAnyMore(string productName, CancellationToken cancellationToken)
    {
        //TODO:
        //Call service responsible to find all users who have subscribed to be notified when this product is not supported any more.
        //Call service responsible to gather 'InStock' email templates, i.e from azure storage account or cosmos DB
        //Call smtp server or Microsoft graph to send the email out
    }

    public async Task NotifySubscibersForProductQuantityChange(string productName, bool newInStock, bool newOutOfStock, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsEmailService.NotifySubscibersForProductQuantityChange)} handling request.");

        //If there has been no change in stock availability then no action is needed.
        if (!newInStock && !newOutOfStock)
            return;

        if (newInStock)
        {
            //TODO:
            //Call service responsible to find all users who have subscribed to be notified when this product is back in stock.
            //Call service responsible to gather 'InStock' email templates, i.e from azure storage account or cosmos DB
            //Call smtp server or Microsoft graph to send the email out
            
            //await _smtpClientHelper.SendEmail(to: "Subscriber", subject: "Product back in stock", $"Good news - {productName} is back in stock", cancellationToken);
        }
        else if (newOutOfStock)
        {
            //TODO:
            //Call service responsible to find all users who have subscribed to be notified when this product is out of stock.
            //Call service responsible to gather 'OutOfStock' email templates, i.e from azure storage account or cosmos DB
            //Call smtp server or Microsoft graph to send the email out
            
            //await _smtpClientHelper.SendEmail(to: "Subscriber", subject: "Product out of stock", $"{productName} is out of stock. We will notify you once is back in stock.", cancellationToken);
        }

        _logger.LogInformation($"{nameof(ProductsService.GetProducts)} handled request successfully.");
    }
}
