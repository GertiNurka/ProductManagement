using ProductManagement.Application.Common.DTOs;
using ProductManagement.Application.Features.ProductFeatures.CreateProduct;
using ProductManagement.Application.Features.ProductFeatures.GetProduct;
using ProductManagement.Application.Features.ProductFeatures.GetProducts;
using ProductManagement.Application.Features.ProductFeatures.HardDeleteProduct;
using ProductManagement.Application.Features.ProductFeatures.RestoreProduct;
using ProductManagement.Application.Features.ProductFeatures.SetProductQuantity;
using ProductManagement.Application.Features.ProductFeatures.SoftDeleteProduct;
using ProductManagement.Application.Features.ProductFeatures.UpdateProduct;

namespace ProductManagement.Application.Services.Products;

public interface IProductsService
{
    Task<ProductDto?> GetProduct(GetProductRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<ProductDto>?> GetProducts(GetProductsRequest request, CancellationToken cancellationToken);
    Task<ProductDto?> SetQuantity(int id, SetProductQuantityRequest request, CancellationToken cancellationToken);
    Task<ProductDto> Create(CreateProductRequest request, CancellationToken cancellationToken);
    Task<ProductDto?> Update(int id, UpdateProductRequest request, CancellationToken cancellationToken);
    Task<ProductDto?> SoftDelete(SoftDeleteProductRequest request, CancellationToken cancellationToken);
    Task<ProductDto?> Restore(RestoreProductRequest request, CancellationToken cancellationToken);
    Task<ProductDto?> HardDelete(HardDeleteProductRequest request, CancellationToken cancellationToken);
}
