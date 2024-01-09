using ProductManagement.Application.Features.ProductFeatures.CreateProduct;

namespace ApplicationUnitTests.Services.ProductsServiceTests;

internal static class ProductsServiceTestData
{
    public static IEnumerable<object[]> CreateProductBadRequestTheoryData =>
            new List<object[]>()
            {
                new object[] { null, "X", 1, $"'{nameof(CreateProductRequest.Name)}' must not be empty." },
                new object[] { "X", "X", 1, $"'{nameof(CreateProductRequest.Name)}' must be between 3 and 50 characters. You entered 1 characters." }
                //TODO: Add more use cases
            };
}
