﻿using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Carts;

/// <summary>
/// Represents an item in a sale, encapsulating product details, quantity, pricing, discount calculation, and total amount.
/// </summary>
public class CartProduct
{
    /// <summary>
    /// Gets the unique identifier for the cart product.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the external product identifier provided by the external system.
    /// </summary>
    public Guid ProductExternalId { get; private set; }

    /// <summary>
    /// Gets the product name (denormalized) for the sale.
    /// </summary>
    public string ProductName { get; private set; }

    /// <summary>
    /// Gets the quantity of the product in this sale item.
    /// Must be between 1 and 20.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount percentage applied to this sale item.
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Gets the total amount for this sale item after discount.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CartProduct"/> class.
    /// Validates that the quantity is between 1 and 20 and calculates the discount and total amount.
    /// </summary>
    /// <param name="productExternalId">The external product identifier.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="quantity">The quantity of the product sold.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <exception cref="DomainException">Thrown when the quantity is not between 1 and 20.</exception>
    internal CartProduct(Guid productExternalId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity < 1 || quantity > 20)
            throw new DomainException("Quantity must be between 1 and 20.");

        Id = Guid.NewGuid();
        ProductExternalId = productExternalId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercentage = GetDiscountPercentage(quantity);
        TotalAmount = CalculateTotalAmount();
    }

    /// <summary>
    /// Performs validation of the cart product entity using the CartProductValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing validation results and any errors.
    /// </returns>
    /// <remarks>
    /// <para>The validation includes checking:</para>
    /// <list type="bullet">
    /// <item><description>Product external ID is not empty</description></item>
    /// <item><description>Product name is not empty</description></item>
    /// <item><description>Quantity is between 1 and 20</description></item>
    /// <item><description>Unit price is greater than zero</description></item>
    /// <item><description>Discount percentage is within valid range</description></item>
    /// </list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new CartProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Retrieves the discount percentage for the given quantity using the DiscountPolicy.
    /// </summary>
    /// <param name="quantity">The quantity of the product.</param>
    /// <returns>The discount percentage as a decimal.</returns>
    private decimal GetDiscountPercentage(int quantity)
    {
        var discountPolicy = new DiscountPolicy();
        return discountPolicy.GetDiscountPercentage(quantity);
    }

    /// <summary>
    /// Calculates the total amount for this sale item after applying the discount.
    /// </summary>
    /// <returns>The calculated total amount.</returns>
    private decimal CalculateTotalAmount()
    {
        decimal grossAmount = Quantity * UnitPrice;
        decimal discountAmount = grossAmount * DiscountPercentage;
        return grossAmount - discountAmount;
    }
}
