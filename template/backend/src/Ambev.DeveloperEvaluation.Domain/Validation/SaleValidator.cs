using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required.");
        
        RuleFor(sale => sale.CustomerExternalId)
            .NotEmpty()
            .WithMessage("Customer external ID is required.");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required.");

        RuleFor(sale => sale.BranchExternalId)
            .NotEmpty()
            .WithMessage("Branch external ID is required.");

        RuleFor(sale => sale.BranchName)
            .NotEmpty()
            .WithMessage("Branch name is required.");

        RuleFor(sale => sale.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future.");
    }
}
