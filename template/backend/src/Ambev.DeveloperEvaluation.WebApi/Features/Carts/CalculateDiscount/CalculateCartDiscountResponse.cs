using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CalculateDiscount;

/// <summary>
/// Response model for cart discount calculation
/// </summary>
public class CalculateCartDiscountResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the customer identifier
    /// </summary>
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// Gets or sets the customer name
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer email
    /// </summary>
    public string CustomerEmail { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch external ID
    /// </summary>
    public string BranchExternalId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch name
    /// </summary>
    public string BranchName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the cart date
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Gets or sets the total amount before discounts
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the total discount amount
    /// </summary>
    public decimal TotalDiscount { get; set; }
    
    /// <summary>
    /// Gets or sets the final amount after discounts
    /// </summary>
    public decimal FinalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the products in the cart with discount information
    /// </summary>
    public List<CalculateCartDiscountProductResponse> Products { get; set; } = new();
}

/// <summary>
/// Response model for a product in the cart discount calculation
/// </summary>
public class CalculateCartDiscountProductResponse
{
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product price
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the subtotal before discount
    /// </summary>
    public decimal Subtotal { get; set; }
    
    /// <summary>
    /// Gets or sets the discount percentage
    /// </summary>
    public decimal DiscountPercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the final amount after discount
    /// </summary>
    public decimal FinalAmount { get; set; }
} 