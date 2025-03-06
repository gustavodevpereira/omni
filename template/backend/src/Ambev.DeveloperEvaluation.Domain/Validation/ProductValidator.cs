using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Product entity.
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    /// <summary>
    /// Initializes a new instance of the ProductValidator class.
    /// </summary>
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters.");

        RuleFor(p => p.Sku)
            .NotEmpty().WithMessage("Product SKU is required.")
            .MaximumLength(20).WithMessage("Product SKU cannot exceed 20 characters.")
            .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("Product SKU can only contain letters, numbers, and hyphens.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Product stock quantity cannot be negative.");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Product category is required.")
            .MaximumLength(50).WithMessage("Product category cannot exceed 50 characters.");

        RuleFor(p => p.Status)
            .IsInEnum().WithMessage("Invalid product status.");
    }
} 