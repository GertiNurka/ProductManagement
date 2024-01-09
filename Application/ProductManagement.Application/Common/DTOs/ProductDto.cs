using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Common.DTOs;

public record ProductDto 
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public int Quantity { get; init; }
    public bool IsDeleted { get; init; } 
    public QuantityStatus QuantityStatus { get; init; }
    public string QuantityStatusName { get; private set; }

    public ProductDto(int id, string? name, string? description, int quantity, bool isDeleted, QuantityStatus quantityStatus)
    {
        Id = id;
        Name = name;
        Description = description;
        Quantity = quantity;
        IsDeleted = isDeleted;
        QuantityStatus = quantityStatus;

        QuantityStatusName = Enum.GetName(typeof(QuantityStatus), quantityStatus)!;
    }
}
