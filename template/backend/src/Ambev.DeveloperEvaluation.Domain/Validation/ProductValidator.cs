using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Product entity.
/// Contains validation rules for product properties to ensure data integrity and business rules compliance.
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductValidator"/> class with validation rules for products.
    /// </summary>
    public ProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot be longer than 100 characters.");

        RuleFor(product => product.Description)
            .MaximumLength(500).WithMessage("Product description cannot be longer than 500 characters.");

        RuleFor(product => product.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .Matches(@"^[A-Z0-9]{8,12}$").WithMessage("SKU must be 8-12 alphanumeric characters (uppercase).");

        RuleFor(product => product.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

        RuleFor(product => product.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(50).WithMessage("Category cannot be longer than 50 characters.");

        RuleFor(product => product.BranchExternalId)
            .NotEmpty().WithMessage("Branch external ID is required.");

        RuleFor(product => product.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100).WithMessage("Branch name cannot be longer than 100 characters.");

        RuleFor(product => product.Status)
            .IsInEnum().WithMessage("Invalid product status.");
    }
} 