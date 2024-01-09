using APIUnitTets.Extensions;

namespace APIUnitTets.Controllers.ProductsControllerTests;

internal static class ProductControllerTestData
{
    public static IEnumerable<object[]> CreateProductBadRequestTheoryData =>
            new List<object[]>()
            {
                new object[] { null, "X", 1, $"'{nameof(CreateProductRequest.Name)}' must not be empty.", $"The following validation errors have occurred on {nameof(CreateProductRequest)}: '{nameof(CreateProductRequest.Name)}' must not be empty." },
                new object[] { "X", "X", 1, $"'{nameof(CreateProductRequest.Name)}' must be between 3 and 50 characters. You entered 1 characters.", $"The following validation errors have occurred on {nameof(CreateProductRequest)}: '{nameof(CreateProductRequest.Name)}' must be between 3 and 50 characters. You entered 1 characters." },
                new object[] { RandomStringGenerator.GenerateRandomString(51), "X", 1, $"'{nameof(CreateProductRequest.Name)}' must be between 3 and 50 characters. You entered 51 characters.", $"The following validation errors have occurred on {nameof(CreateProductRequest)}: '{nameof(CreateProductRequest.Name)}' must be between 3 and 50 characters. You entered 51 characters."},
                new object[] { "XXX", RandomStringGenerator.GenerateRandomString(501), 1, $"The length of '{nameof(CreateProductRequest.Description)}' must be 500 characters or fewer. You entered 501 characters.", $"The following validation errors have occurred on {nameof(CreateProductRequest)}: The length of '{nameof(CreateProductRequest.Description)}' must be 500 characters or fewer. You entered 501 characters." }
                //TODO: Add more use cases
            };

    public static IEnumerable<object[]> SetProductQuantityBadRequestTheoryData =>
            new List<object[]>()
            {
                new object[] { -1, $"'{nameof(SetProductQuantityRequest.Quantity)}' must be greater than or equal to '0'.", $"The following validation errors have occurred on {nameof(SetProductQuantityRequest)}: '{nameof(SetProductQuantityRequest.Quantity)}' must be greater than or equal to '0'." },
                //TODO: Add more use cases
            };

    public static IEnumerable<object[]> UpdateProductBadRequestTheoryData =>
            new List<object[]>()
            {
                new object[] { null, "X", $"'{nameof(UpdateProductRequest.Name)}' must not be empty.", $"The following validation errors have occurred on {nameof(UpdateProductRequest)}: '{nameof(UpdateProductRequest.Name)}' must not be empty." },
                new object[] { "X", "X", $"'{nameof(UpdateProductRequest.Name)}' must be between 3 and 50 characters. You entered 1 characters.", $"The following validation errors have occurred on {nameof(UpdateProductRequest)}: '{nameof(UpdateProductRequest.Name)}' must be between 3 and 50 characters. You entered 1 characters." },
                new object[] { RandomStringGenerator.GenerateRandomString(51), "X", $"'{nameof(UpdateProductRequest.Name)}' must be between 3 and 50 characters. You entered 51 characters.", $"The following validation errors have occurred on {nameof(UpdateProductRequest)}: '{nameof(UpdateProductRequest.Name)}' must be between 3 and 50 characters. You entered 51 characters."},
                new object[] { "XXX", RandomStringGenerator.GenerateRandomString(501), $"The length of '{nameof(UpdateProductRequest.Description)}' must be 500 characters or fewer. You entered 501 characters.", $"The following validation errors have occurred on {nameof(UpdateProductRequest)}: The length of '{nameof(UpdateProductRequest.Description)}' must be 500 characters or fewer. You entered 501 characters." }
                //TODO: Add more use cases
            };
}