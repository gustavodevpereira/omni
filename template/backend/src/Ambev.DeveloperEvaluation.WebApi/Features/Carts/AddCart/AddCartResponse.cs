namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddCart;

/// <summary>
/// Response model after successfully creating a cart
/// </summary>
public class AddCartResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the created cart
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the date when the cart was created
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier
    /// </summary>
    public Guid CustomerExternalId { get; set; }

    /// <summary>
    /// Gets or sets the customer name
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch identifier
    /// </summary>
    public Guid BranchExternalId { get; set; }
    
    /// <summary>
    /// Gets or sets the branch name
    /// </summary>
    public string BranchName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the status of the cart
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total amount of the cart
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the products in the cart
    /// </summary>
    public List<AddCartProductResponse> Products { get; set; } = [];
}

/// <summary>
/// Response model for a product in a newly created cart
/// </summary>
public class AddCartProductResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart product
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the product identifier
    /// </summary>
    public Guid ProductExternalId { get; set; }
    
    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the discount percentage applied to the product
    /// </summary>
    public decimal DiscountPercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the total amount for this product after discount
    /// </summary>
    public decimal TotalAmount { get; set; }
} 