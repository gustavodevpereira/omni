using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddCart;

/// <summary>
/// Validator for AddCartRequest
/// </summary>
public class AddCartRequestValidator : AbstractValidator<AddCartRequest>
{
    /// <summary>
    /// Initializes a new instance of AddCartRequestValidator
    /// </summary>
    public AddCartRequestValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required")
            .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("At least one product is required")
            .Must(products => products.Count <= 20).WithMessage("Cart cannot have more than 20 products");

        RuleForEach(x => x.Products)
            .SetValidator(new AddCartProductRequestValidator());
    }
}

/// <summary>
/// Validator for AddCartProductRequest
/// </summary>
public class AddCartProductRequestValidator : AbstractValidator<AddCartProductRequest>
{
    /// <summary>
    /// Initializes a new instance of AddCartProductRequestValidator
    /// </summary>
    public AddCartProductRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Quantity must not exceed 20");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");
    }
} 