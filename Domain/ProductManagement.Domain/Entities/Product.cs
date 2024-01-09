using ProductManagement.Domain.Common;
using ProductManagement.Domain.Common.Audit;

namespace ProductManagement.Domain.Entities;

public class Product : BaseEntity, IAuditable
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsDeleted { get; private set; }
    public int Quantity { get; private set; }

    public QuantityStatus QuantityStatus { get; private set; }
    public bool NewInStock { get; private set; } = false;
    public bool NewOutOfStock { get; private set; } = false;

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? UpdatedOnUtc { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }

    public Product(string name, string? description, int quantity)
    {
        this.Name = name;
        this.Description = description;

        SetQuantity(quantity);
    }

    /// <summary>
    /// Update product.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public void Update(string name, string? description)
    {
        this.Name = name;
        this.Description = description;
    }

    /// <summary>
    /// The only method responsible for setting the quantity
    /// </summary>
    /// <param name="quantity"></param>
    public void SetQuantity(int quantity) 
    { 
        if(quantity <= 0)
            this.Quantity = 0;
        else
            this.Quantity = quantity;

        UpdateQuantityStatus();
    }

    /// <summary>
    /// Soft delete
    /// </summary>
    public void SoftDelete()
    {
        //If already deleted then return
        // in the future this method may trigger events,
        // we don't want to send out any events if is already been deleted.
        if (this.IsDeleted)
            return;

        this.IsDeleted = true;
    }

    /// <summary>
    /// Restore
    /// </summary>
    public void Restore()
    {
        //If is not deleted then return
        // in the future this method may trigger events,
        // we don't want to send out any events if is already active.
        if (!this.IsDeleted) 
            return;

        this.IsDeleted = false;
    }

    /// <summary>
    /// The only method responsible for updating the quantity status
    /// </summary>
    private void UpdateQuantityStatus()
    {
        var currentQuantityStatus = this.QuantityStatus;

        #region Cases that don't require action

        //product was already in stock, no change is required
        if (this.Quantity > 0 && currentQuantityStatus.Equals(QuantityStatus.InStock))
            return;
        //product was already out of stock, no change is required
        if(this.Quantity == 0 && currentQuantityStatus.Equals(QuantityStatus.OutOfStock))
            return;

        #endregion

        #region Cases that require action

        //Product came back in stock
        if (this.Quantity > 0 && currentQuantityStatus.Equals(QuantityStatus.OutOfStock))
        {
            this.QuantityStatus = QuantityStatus.InStock;
            this.NewInStock = true;
            this.NewOutOfStock = false;
        }
        //Product out of stock
        else if(this.Quantity == 0 && currentQuantityStatus.Equals(QuantityStatus.InStock))
        {
            this.QuantityStatus = QuantityStatus.OutOfStock;
            this.NewInStock = false;
            this.NewOutOfStock = true;
        }

        #endregion


    }
}
