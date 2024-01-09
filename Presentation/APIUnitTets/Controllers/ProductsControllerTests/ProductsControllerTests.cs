namespace APIUnitTets.Controllers.ProductsControllerTests;

/// <summary>
/// Products controller tests
/// </summary>
public partial class ProductsControllerTests
{
    private readonly ITestOutputHelper _output;
    private readonly ProductsController _productsController;
    private readonly FakeLogger<ProductsController> _fakeLogger;
    private readonly Mock<IProductsService> _mockProductsService;

    /// <summary>
    /// Set here all global arrangements
    /// </summary>
    public ProductsControllerTests(ITestOutputHelper output)
    {
        _output = output;

        _fakeLogger = new FakeLogger<ProductsController>();

        _mockProductsService = new Mock<IProductsService>();

        _productsController = new ProductsController(_fakeLogger, _mockProductsService.Object);
    }

    [Fact]
    [Trait("Category", "GetProducts")]
    public async Task GetProducts_ReturnsOk_WithValidData()
    {
        // Arrange
        var mockProductDto = new Mock<ProductDto>(1, "Test", "Test", 1, false, QuantityStatus.InStock).Object;
        var expectedResponse = new List<ProductDto> { mockProductDto };

        _mockProductsService
            .Setup(x => x.GetProducts(It.IsAny<GetProductsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.GetProducts(It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProducts)} handling request.", 1);

        Assert.IsAssignableFrom<IActionResult>(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);

        Assert.Equal(expectedResponse, actualResponse);

        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProducts)} handled request successfully.", 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "GetProducts")]
    public async Task GetProducts_ReturnsNoContent_WhenServiceReturnsEmptyList()
    {
        // Arrange
        var expectedResponse = new List<ProductDto>();

        _mockProductsService
            .Setup(x => x.GetProducts(It.IsAny<GetProductsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.GetProducts(It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProducts)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NoContentResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProducts)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "GetProduct")]
    public async Task GetProduct_ReturnsOk_WithValidData()
    {
        // Arrange
        var expectedResponse = new ProductDto(1, "Test", "Test", 1, false, QuantityStatus.InStock);

        _mockProductsService
            .Setup(x => x.GetProduct(It.IsAny<GetProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.GetProduct(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProduct)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(expectedResponse, actualResponse);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProduct)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "GetProduct")]
    public async Task GetProduct_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        _mockProductsService
            .Setup(x => x.GetProduct(It.IsAny<GetProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _productsController.GetProduct(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProduct)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NotFoundResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.GetProduct)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "CreateProduct")]
    public async Task CreateProduct_ReturnsCreatedAtRoute_WithValidData()
    {
        // Arrange
        var createProductRequest = new Mock<CreateProductRequest>("Test", "Test", 1);
        createProductRequest.Setup(x => x.IsValid())
            .Returns((true, null));

        var expectedResponse = new Mock<ProductDto>(1, "Test", "Test", 1, false, QuantityStatus.InStock).Object;

        _mockProductsService
            .Setup(x => x.Create(It.IsAny<CreateProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.CreateProduct(createProductRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.CreateProduct)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);

        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);

        Assert.Equal(nameof(ProductsController.GetProduct), createdAtRouteResult.RouteName);
        Assert.Equal(expectedResponse.Id, createdAtRouteResult.RouteValues["id"]);
        Assert.Equal(expectedResponse.Id, createdAtRouteResult.Value);

        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.CreateProduct)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Theory]
    [Trait("Category", "CreateProduct")]
    [MemberData(nameof(ProductControllerTestData.CreateProductBadRequestTheoryData), MemberType = typeof(ProductControllerTestData))]
    public async Task CreateProduct_ReturnsBadRequest_WhenRequestHasInvalidData(string? name, string? description, int quantity, string expectedMessageToReturn, string loggerExpectedMessage)
    {
        // Arrange
        var createProductRequest = new Mock<CreateProductRequest>(name, description, quantity);
        createProductRequest.Setup(x => x.IsValid())
            .Returns((false, expectedMessageToReturn));

        // Act
        var result = await _productsController.CreateProduct(createProductRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.CreateProduct)} handling request.", 1);

        Assert.IsAssignableFrom<IActionResult>(result);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(expectedMessageToReturn, badRequest.Value);

        AssertLoggerMessage(LogLevel.Information, loggerExpectedMessage, 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "SetQuantity")]
    public async Task SetQuantity_ReturnsOk_WithValidData()
    {
        // Arrange
        var setProductQuantityRequest = new Mock<SetProductQuantityRequest>(1);
        setProductQuantityRequest.Setup(x => x.IsValid())
            .Returns((true, null));
        var expectedResponse = new ProductDto(1, "Test", "Test", 1, false, QuantityStatus.InStock);

        _mockProductsService
            .Setup(x => x.SetQuantity(It.IsAny<int>(), It.IsAny<SetProductQuantityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.SetQuantity(1, setProductQuantityRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SetQuantity)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(expectedResponse, actualResponse);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SetQuantity)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "SetQuantity")]
    public async Task SetQuantity_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var setProductQuantityRequest = new Mock<SetProductQuantityRequest>(1);
        setProductQuantityRequest.Setup(x => x.IsValid())
            .Returns((true, null));

        _mockProductsService
            .Setup(x => x.SetQuantity(It.IsAny<int>(), It.IsAny<SetProductQuantityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _productsController.SetQuantity(1, setProductQuantityRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SetQuantity)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NotFoundResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SetQuantity)} handled request successfully.", 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Theory]
    [Trait("Category", "SetQuantity")]
    [MemberData(nameof(ProductControllerTestData.SetProductQuantityBadRequestTheoryData), MemberType = typeof(ProductControllerTestData))]
    public async Task SetQuantity_ReturnsBadRequest_WhenRequestHasInvalidData(int quantity, string expectedMessageToReturn, string loggerExpectedMessage)
    {
        // Arrange
        var setProductQuantityRequest = new Mock<SetProductQuantityRequest>(quantity);
        setProductQuantityRequest.Setup(x => x.IsValid())
            .Returns((false, expectedMessageToReturn));

        // Act
        var result = await _productsController.SetQuantity(1, setProductQuantityRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SetQuantity)} handling request.", 1);

        Assert.IsAssignableFrom<IActionResult>(result);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(expectedMessageToReturn, badRequest.Value);

        AssertLoggerMessage(LogLevel.Information, loggerExpectedMessage, 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }


    [Fact]
    [Trait("Category", "UpdateProduct")]
    public async Task UpdateProduct_ReturnsOk_WithValidData()
    {
        // Arrange
        var updateProductRequest = new Mock<UpdateProductRequest>("Test", "Test");
        updateProductRequest.Setup(x => x.IsValid())
            .Returns((true, null));

        var expectedResponse = new ProductDto(1, "Test", "Test", 1, false, QuantityStatus.InStock);

        _mockProductsService
            .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.UpdateProduct(1, updateProductRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.UpdateProduct)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(expectedResponse, actualResponse);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.UpdateProduct)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "UpdateProduct")]
    public async Task UpdateProduct_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var updateProductRequest = new Mock<UpdateProductRequest>("Test", "Test");
        updateProductRequest.Setup(x => x.IsValid())
            .Returns((true, null));

        _mockProductsService
            .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _productsController.UpdateProduct(1, updateProductRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.UpdateProduct)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NotFoundResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.UpdateProduct)} handled request successfully.", 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Theory]
    [Trait("Category", "UpdateProduct")]
    [MemberData(nameof(ProductControllerTestData.UpdateProductBadRequestTheoryData), MemberType = typeof(ProductControllerTestData))]
    public async Task UpdateProduct_ReturnsBadRequest_WhenRequestHasInvalidData(string? name, string? description, string expectedMessageToReturn, string loggerExpectedMessage)
    {
        // Arrange
        var updateProductRequest = new Mock<UpdateProductRequest>(name, description);
        updateProductRequest.Setup(x => x.IsValid())
            .Returns((false, expectedMessageToReturn));

        // Act
        var result = await _productsController.UpdateProduct(1, updateProductRequest.Object, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.UpdateProduct)} handling request.", 1);

        Assert.IsAssignableFrom<IActionResult>(result);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(expectedMessageToReturn, badRequest.Value);

        AssertLoggerMessage(LogLevel.Information, loggerExpectedMessage, 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "HardDelete")]
    public async Task HardDelete_ReturnsNoContent_WhenServiceReturnsProductDto()
    {
        // Arrange
        var expectedResponse = new Mock<ProductDto>(1, "Test", "Test", 1, false, QuantityStatus.InStock).Object;

        _mockProductsService
            .Setup(x => x.HardDelete(It.IsAny<HardDeleteProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.HardDelete(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.HardDelete)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NoContentResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.HardDelete)} handled request successfully.", 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "HardDelete")]
    public async Task HardDelete_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var expectedResponse = new Mock<ProductDto>(1, "Test", "Test", 1, false, QuantityStatus.InStock).Object;

        _mockProductsService
            .Setup(x => x.HardDelete(It.IsAny<HardDeleteProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _productsController.HardDelete(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.HardDelete)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NotFoundResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.HardDelete)} handled request successfully.", 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "SoftDelete")]
    public async Task SoftDelete_ReturnsOk_WithValidData()
    {
        // Arrange
        var expectedResponse = new ProductDto(1, "Test", "Test", 1, false, QuantityStatus.InStock);

        _mockProductsService
            .Setup(x => x.SoftDelete(It.IsAny<SoftDeleteProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.SoftDelete(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SoftDelete)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(expectedResponse, actualResponse);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SoftDelete)} handled request successfully.", 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "SoftDelete")]
    public async Task SoftDelete_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        _mockProductsService
            .Setup(x => x.SoftDelete(It.IsAny<SoftDeleteProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _productsController.SoftDelete(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SoftDelete)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NotFoundResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.SoftDelete)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "Restore")]
    public async Task Restore_ReturnsOk_WithValidData()
    {
        // Arrange
        var expectedResponse = new ProductDto(1, "Test", "Test", 1, false, QuantityStatus.InStock);

        _mockProductsService
            .Setup(x => x.Restore(It.IsAny<RestoreProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productsController.Restore(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.Restore)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(expectedResponse, actualResponse);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.Restore)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Fact]
    [Trait("Category", "Restore")]
    public async Task Restore_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        _mockProductsService
            .Setup(x => x.Restore(It.IsAny<RestoreProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _productsController.Restore(1, It.IsAny<CancellationToken>());

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.Restore)} handling request.", 1);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<NotFoundResult>(result);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsController.Restore)} handled request successfully.", 2);
        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    private void AssertLoggerMessage(LogLevel loglevel, string msg, int index)
    {
        Assert.Equal(msg, _fakeLogger.Collector.GetSnapshot()[index].Message);
        Assert.Equal(loglevel, _fakeLogger.Collector.GetSnapshot()[index].Level);
    }
}