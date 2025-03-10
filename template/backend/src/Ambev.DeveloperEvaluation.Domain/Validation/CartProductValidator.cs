using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the CartProduct entity.
/// Contains validation rules for cart product properties to ensure data integrity and business rules compliance.
/// </summary>
public class CartProductValidator : AbstractValidator<CartProduct>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CartProductValidator"/> class with validation rules for cart products.
    /// </summary>
    public CartProductValidator()
    {
        RuleFor(item => item.ProductExternalId)
            .NotEmpty().WithMessage("Product external ID is required.");

        RuleFor(item => item.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot be longer than 100 characters.");

        RuleFor(item => item.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Quantity must be at least 1.")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

        RuleFor(item => item.DiscountPercentage)
            .InclusiveBetween(0, 0.9m).WithMessage("Discount percentage must be between 0% and 90%.");

        RuleFor(item => item.TotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Total amount cannot be negative.");
    }
} 