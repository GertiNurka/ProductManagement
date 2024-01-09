using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Features.ProductFeatures.CreateProduct;
using ProductManagement.Application.Features.ProductFeatures.GetProduct;
using ProductManagement.Application.Features.ProductFeatures.GetProducts;
using ProductManagement.Application.Features.ProductFeatures.HardDeleteProduct;
using ProductManagement.Application.Features.ProductFeatures.RestoreProduct;
using ProductManagement.Application.Features.ProductFeatures.SetProductQuantity;
using ProductManagement.Application.Features.ProductFeatures.SoftDeleteProduct;
using ProductManagement.Application.Features.ProductFeatures.UpdateProduct;
using ProductManagement.Application.Services.Products;

namespace ProductManagement.API.Controllers;

/// <summary>
/// API Controller to interact with products
/// TODO: 
///     Instead of exposing requests for application layer (i.e CreateProductRequest) directly to the client. 
///     Consider creating models in the presentation layer to expose to the client. Then map those models to requests. 
///     The mapping should take place in presentation layer.
///     I didn't do it here because of the simplicity of this project and restriction of time.
/// </summary>
[Route("api/[controller]")]
[ApiController]
//TODO: uncomment authorize attribute for test/live environment
//[Authorize("User")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductsService _productsService;

    public ProductsController(ILogger<ProductsController> logger, IProductsService productsService)
    {
        _logger = logger;
        _productsService = productsService;

        _logger.LogInformation($"{nameof(ProductsController)} created.");
    }

    /// <summary>
    /// Get all products
    /// TODO: Support pagination and filtering
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.GetProducts)} handling request.");

        var request = new GetProductsRequest();

        var response = await _productsService.GetProducts(request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.GetProducts)} handled request successfully.");

        if (response == null || !response.Any())
            return NoContent();

        return Ok(response);
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.GetProduct)} handling request.");

        var request = new GetProductRequest(id);

        var response = await _productsService.GetProduct(request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.GetProduct)} handled request successfully.");

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    //TODO: uncomment authorize attribute for test/live environment
    //[Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.CreateProduct)} handling request.");

        //* Explanation Comment *
        //This is not expected to fail when called by the client (i.e swagger).
        // When the method is called from the client the validation of the request takes place in the model biding validation pipeline. And if the validation fails the method will never be called.
        // We achieve that by adding 'AddFluentValidationAutoValidation' and 'AddValidatorsFromAssemblyContaining' in IServiceCollection in program.cs.
        //
        // Given the above, I include these validation lines here for 2 reasons
        //  1. To show an other way how to validate if we don't want to inject fluent validation in the pipeline
        //  2. So we can easily unit test the return of bad request.
        var (isValid, errors) = request.IsValid();
        if (!isValid)
        {
            _logger.LogInformation($"The following validation errors have occurred on {nameof(CreateProductRequest)}: {errors}");
            return BadRequest(errors);
        }

        var response = await _productsService.Create(request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.CreateProduct)} handled request successfully.");

        return CreatedAtRoute(nameof(this.GetProduct), new { id = response.Id }, response.Id);
    }

    /// <summary>
    /// Set quantity of a product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}/setQuantity")]
    //TODO: uncomment authorize attribute for test/live environment
    //[Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetQuantity(int id, [FromBody] SetProductQuantityRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.SetQuantity)} handling request.");

        //* Explanation Comment *
        //This validation is not expected to fail when called by the client (i.e swagger).
        // When the method is called from the client the validation of the request takes place in the model biding validation pipeline. And if the validation fails the method will never be called.
        // We achieve that by adding 'AddFluentValidationAutoValidation' and 'AddValidatorsFromAssemblyContaining' in IServiceCollection in program.cs.
        //
        // Given the above, I include these validation lines here for 2 reasons
        //  1. To show an other way how to validate if we don't want to inject fluent validation in the pipeline
        //  2. So we can easily unit test the return of bad request.
        var (isValid, errors) = request.IsValid();
        if (!isValid)
        {
            _logger.LogInformation($"The following validation errors have occurred on {nameof(SetProductQuantityRequest)}: {errors}");
            return BadRequest(errors);
        }

        var response = await _productsService.SetQuantity(id, request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.SetQuantity)} handled request successfully.");

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    //TODO: uncomment authorize attribute for test/live environment
    //[Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.UpdateProduct)} handling request.");

        //* Explanation Comment *
        //This is not expected to fail when called by the client (i.e swagger).
        // When the method is called from the client the validation of the request takes place in the model biding validation pipeline. And if the validation fails the method will never be called.
        // We achieve that by adding 'AddFluentValidationAutoValidation' and 'AddValidatorsFromAssemblyContaining' in IServiceCollection in program.cs.
        //
        // Given the above, I include these validation lines here for 2 reasons
        //  1. To show an other way how to validate if we don't want to inject fluent validation in the pipeline
        //  2. So we can unit test the return of bad request.
        var (isValid, errors) = request.IsValid();
        if (!isValid)
        {
            _logger.LogInformation($"The following validation errors have occurred on {nameof(UpdateProductRequest)}: {errors}");
            return BadRequest(errors);
        }

        var response = await _productsService.Update(id, request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.UpdateProduct)} handled request successfully.");

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Hard delete a product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(1)}")]
    //TODO: uncomment authorize attribute for test/live environment
    //[Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> HardDelete(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.HardDelete)} handling request.");

        var request = new HardDeleteProductRequest(id);

        var response = await _productsService.HardDelete(request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.HardDelete)} handled request successfully.");

        if (response == null)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Soft delete a product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}/softDelete")]
    //TODO: uncomment authorize attribute for test/live environment
    //[Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoftDelete(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.SoftDelete)} handling request.");

        var request = new SoftDeleteProductRequest(id);

        var response = await _productsService.SoftDelete(request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.SoftDelete)} handled request successfully.");

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Restore a product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}/restore")]
    //TODO: uncomment authorize attribute for test/live environment
    //[Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Restore(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ProductsController.Restore)} handling request.");

        var request = new RestoreProductRequest(id);

        var response = await _productsService.Restore(request, cancellationToken);

        _logger.LogInformation($"{nameof(ProductsController.Restore)} handled request successfully.");

        if (response == null)
            return NotFound();

        return Ok(response);
    }
}
