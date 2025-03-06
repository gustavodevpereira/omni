using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand.
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductValidator class.
    /// </summary>
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("Product SKU is required.")
            .MaximumLength(20).WithMessage("Product SKU cannot exceed 20 characters.")
            .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("Product SKU can only contain letters, numbers, and hyphens.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Product stock quantity cannot be negative.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Product category is required.")
            .MaximumLength(50).WithMessage("Product category cannot exceed 50 characters.");
    }
} 