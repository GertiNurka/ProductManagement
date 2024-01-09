using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Common.DTOs;
using ProductManagement.Application.Common.Interfaces.Email;
using ProductManagement.Application.Common.Interfaces.Persistence;
using ProductManagement.Application.Features.ProductFeatures.CreateProduct;
using ProductManagement.Application.Features.ProductFeatures.GetProduct;
using ProductManagement.Application.Features.ProductFeatures.GetProducts;
using ProductManagement.Application.Features.ProductFeatures.HardDeleteProduct;
using ProductManagement.Application.Features.ProductFeatures.RestoreProduct;
using ProductManagement.Application.Features.ProductFeatures.SetProductQuantity;
using ProductManagement.Application.Features.ProductFeatures.SoftDeleteProduct;
using ProductManagement.Application.Features.ProductFeatures.UpdateProduct;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Services.Products;

/// <summary>
/// Service to interact with product and to handle application logic.
/// Here we assume that each method is a unit of work. So each method, that makes domain changes, commits the transaction.
/// TODO: 
///     Handle permissions... 
///         1. if the application is multi-tenant then discover the tenant id (or owner id)
///         2. if the application has different permissions or roles then handle these cases
/// </summary>
public class ProductsService : IProductsService
{
    private readonly ILogger<ProductsService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductsRepository _productsRepository;
    private readonly IProductsEmailService _productsEmailService;
    private readonly IMapper _mapper;

    public ProductsService(ILogger<ProductsService> logger, IUnitOfWork unitOfWork,
        IProductsRepository productsRepository, IProductsEmailService productsEmailService,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _productsRepository = productsRepository;
        _productsEmailService = productsEmailService;
        _mapper = mapper;

        _logger.LogInformation($"{nameof(ProductsService)} created.");        
    }

    /// <summary>
    /// Get all products.
    /// TODO: Support pagination and filtering
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProductDto>?> GetProducts(GetProductsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.GetProducts)} handling request.");

        request.Validate();

        var products = await _productsRepository.GetAll(cancellationToken);

        var response = _mapper.Map<IEnumerable<ProductDto>>(products);

        _logger.LogInformation($"{nameof(ProductsService.GetProducts)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto?> GetProduct(GetProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.GetProduct)} handling request.");

        request.Validate();

        var product = await _productsRepository.Get(request.Id, cancellationToken);

        var response = _mapper.Map<ProductDto>(product);

        _logger.LogInformation($"{nameof(ProductsService.GetProduct)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Set the quantity of the product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto?> SetQuantity(int id, SetProductQuantityRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.SetQuantity)} handling request.");

        request.Validate();

        var product = await _productsRepository.Get(id, cancellationToken);

        if (product == null)
            return null;

        product.SetQuantity(request.Quantity!.Value); //here request.Quantity isn't null because of the validation step earlier.

        await _productsRepository.Update(product);

        await _unitOfWork.Save(cancellationToken);

        await _productsEmailService.NotifySubscibersForProductQuantityChange(product.Name, product.NewInStock, product.NewOutOfStock, cancellationToken);

        var response = _mapper.Map<ProductDto>(product);

        _logger.LogInformation($"{nameof(ProductsService.SetQuantity)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.Create)} handling request.");

        request.Validate();

        var product = new Product(request.Name, request.Description, request.Quantity!.Value); //here request.Quantity isn't null because of the validation step earlier.

        await _productsRepository.Create(product);

        await _unitOfWork.Save(cancellationToken);

        await _productsEmailService.NotifySubscibersForNewSupportedProduct(product.Name, cancellationToken);

        var response = _mapper.Map<ProductDto>(product);

        _logger.LogInformation($"{nameof(ProductsService.Create)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto?> Update(int id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.Update)} handling request.");

        request.Validate();

        var product = await _productsRepository.Get(id, cancellationToken);

        if (product == null)
            return null;

        product.Update(request.Name, request.Description);

        await _productsRepository.Update(product);

        await _unitOfWork.Save(cancellationToken);

        await _productsEmailService.NotifySubscibersForProductChange(product.Name, "", cancellationToken);

        var response = _mapper.Map<ProductDto>(product);

        _logger.LogInformation($"{nameof(ProductsService.Update)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Soft delete product
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto?> SoftDelete(SoftDeleteProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.SoftDelete)} handling request.");

        request.Validate();

        var product = await _productsRepository.Get(request.Id, cancellationToken);

        if (product == null)
            return null;

        product.SoftDelete();

        await _productsRepository.Update(product);

        await _unitOfWork.Save(cancellationToken);

        await _productsEmailService.NotifySubscibersForProductBeenUnavalable(product.Name, cancellationToken);

        var response = _mapper.Map<ProductDto>(product);

        _logger.LogInformation($"{nameof(ProductsService.SoftDelete)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Restore soft deleted product
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto?> Restore(RestoreProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.Restore)} handling request.");

        request.Validate();

        var product = await _productsRepository.Get(request.Id, cancellationToken);

        if (product == null)
            return null;

        product.Restore();

        await _productsRepository.Update(product);

        await _unitOfWork.Save(cancellationToken);

        await _productsEmailService.NotifySubscibersForProductBeenBackAvailable(product.Name, cancellationToken);

        var response = _mapper.Map<ProductDto>(product);

        _logger.LogInformation($"{nameof(ProductsService.Restore)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Hard delete product
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductDto?> HardDelete(HardDeleteProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsService.HardDelete)} handling request.");

        request.Validate();

        var product = await _productsRepository.Get(request.Id, cancellationToken);

        if (product == null)
            return null;

        _productsRepository.Delete(product);

        await _unitOfWork.Save(cancellationToken);

        await _productsEmailService.NotifySubscibersForProductNotBeenSupportedAnyMore(product.Name, cancellationToken);

        var response = _mapper.Map<ProductDto>(product);

        _logger.LogInformation($"{nameof(ProductsService.HardDelete)} handled request successfully.");

        return response;
    }

    /// <summary>
    /// Save unit of work.
    /// TODO: 
    ///     The current implementation of 'ProductsService' assumes that each method is a unit of work. That's way we call 'await _unitOfWork.Save(cancellationToken);' in each of the methods.
    ///     If on the other hand you want to allow the 'caller' to control when to save the unit of work then:
    ///         1. Uncomment this this method
    ///         2. Remove 'await _unitOfWork.Save(cancellationToken);' from all other methods.
    ///     By uncommenting this code - the developer needs to consider how to manage transaction for example not to send any emails out before this method is called.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    //public async Task Save(CancellationToken cancellationToken)
    //{
    //    await _unitOfWork.Save(cancellationToken);
    //}
}
