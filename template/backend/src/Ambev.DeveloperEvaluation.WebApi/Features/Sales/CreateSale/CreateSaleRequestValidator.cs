using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters");

        RuleFor(x => x.SaleDate)
            .NotEmpty().WithMessage("Sale date is required");

        RuleFor(x => x.BranchExternalId)
            .NotEmpty().WithMessage("Branch external ID is required")
            .MaximumLength(100).WithMessage("Branch external ID cannot exceed 100 characters");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required")
            .MaximumLength(200).WithMessage("Branch name cannot exceed 200 characters");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required");

        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemRequestValidator());
    }
}

/// <summary>
/// Validator for CreateSaleItemRequest
/// </summary>
public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    public CreateSaleItemRequestValidator()
    {
        RuleFor(x => x.ProductExternalId)
            .NotEmpty().WithMessage("Product external ID is required")
            .MaximumLength(100).WithMessage("Product external ID cannot exceed 100 characters");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");
    }
} 