using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.AddCart;

/// <summary>
/// Validator for AddCartRequest
/// </summary>
public class AddCartRequestValidator : AbstractValidator<AddCartRequest>
{
    /// <summary>
    /// Initializes validators for AddCartRequest
    /// </summary>
    public AddCartRequestValidator()
    {
        RuleFor(x => x.CostumerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.CostumerName)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(100)
            .WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(x => x.BranchName)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MaximumLength(100)
            .WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("At least one product must be provided");

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
    /// Initializes validators for AddCartProductRequest
    /// </summary>
    public AddCartProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero");
    }
}