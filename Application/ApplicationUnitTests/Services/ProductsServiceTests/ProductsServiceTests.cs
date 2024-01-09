namespace ApplicationUnitTests.Services.ProductsServiceTests;

/// <summary>
/// Products service tests
/// </summary>
public class ProductsServiceTests
{
    private readonly ITestOutputHelper _output;
    private readonly ProductsService _productsService;
    private readonly FakeLogger<ProductsService> _fakeLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductsRepository> _mockProductsRepository;
    private readonly Mock<IProductsEmailService> _mockProductsEmailService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// Set here all global arrangements
    /// </summary>
    public ProductsServiceTests(ITestOutputHelper output)
    {        
        _output = output;

        _fakeLogger = new FakeLogger<ProductsService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductsRepository = new Mock<IProductsRepository>();
        _mockProductsEmailService = new Mock<IProductsEmailService>();
        _mockMapper = new Mock<IMapper>();

        _productsService = new ProductsService(_fakeLogger, _mockUnitOfWork.Object, _mockProductsRepository.Object, _mockProductsEmailService.Object, _mockMapper.Object);
    }

    [Fact]
    [Trait("Category", "Create")]
    public async Task Create_Returns_ProductDto()
    {
        // Arrange        
        var request = new Mock<CreateProductRequest>("Test", "Test", 1);
        request.Setup(x => x.IsValid());

        var expectedResponse = new ProductDto(1, "Test", "Test", 1, false, QuantityStatus.InStock);

        _mockProductsRepository
            .Setup(x => x.Create(It.IsAny<Product>()));
        _mockUnitOfWork
            .Setup(x => x.Save(It.IsAny<CancellationToken>()));
        _mockProductsEmailService
            .Setup(x => x.NotifySubscibersForNewSupportedProduct(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        _mockMapper
            .Setup(x => x.Map<ProductDto>(It.IsAny<Product>()))
            .Returns(expectedResponse);

        // Act
        var result = await _productsService.Create(request.Object, _cancellationToken);

        // Assert
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsService.Create)} handling request.", 1);
        Assert.IsAssignableFrom<ProductDto>(result);
        Assert.Equal(expectedResponse, result!);
        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsService.Create)} handled request successfully.", 2);

        Assert.Equal(3, _fakeLogger.Collector.Count);
    }

    [Theory]
    [Trait("Category", "Create")]
    [MemberData(nameof(ProductsServiceTestData.CreateProductBadRequestTheoryData), MemberType = typeof(ProductsServiceTestData))]
    public async Task Create_ValidationFailed_ThrowException(string? name, string? description, int quantity, string expectedExceptionMessage)
    {
        // Arrange        
        var request = new Mock<CreateProductRequest>(name, description, quantity);
        request.Setup(x => x.Validate()).Throws(new ValidationException(expectedExceptionMessage));

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await _productsService.Create(request.Object, _cancellationToken);
        });

        AssertLoggerMessage(LogLevel.Information, $"{nameof(ProductsService.Create)} handling request.", 1);

        Assert.Equal(expectedExceptionMessage, exception.Message);

        Assert.Equal(2, _fakeLogger.Collector.Count);
    }

    private void AssertLoggerMessage(LogLevel loglevel, string msg, int index)
    {
        Assert.Equal(msg, _fakeLogger.Collector.GetSnapshot()[index].Message);
        Assert.Equal(loglevel, _fakeLogger.Collector.GetSnapshot()[index].Level);
    }
}
