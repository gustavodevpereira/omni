using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CalculateDiscount;

/// <summary>
/// Request model for calculating cart discounts
/// </summary>
public class CalculateCartDiscountRequest
{
    /// <summary>
    /// Gets or sets the branch external ID
    /// </summary>
    public string BranchExternalId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch name
    /// </summary>
    public string BranchName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date of the cart
    /// </summary>
    public DateTime Date { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Gets or sets the products in the cart
    /// </summary>
    public List<CalculateCartDiscountProductRequest> Products { get; set; } = new();
}

/// <summary>
/// Request model for a product in the cart discount calculation
/// </summary>
public class CalculateCartDiscountProductRequest
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
} 