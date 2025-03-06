using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validator for CreateSaleCommand.
    /// </summary>
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(sale => sale.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.");

            RuleFor(sale => sale.SaleDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(sale => sale.CustomerExternalId)
                .NotEmpty().WithMessage("Customer external ID is required.");

            RuleFor(sale => sale.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.");

            RuleFor(sale => sale.BranchExternalId)
                .NotEmpty().WithMessage("Branch external ID is required.");

            RuleFor(sale => sale.BranchName)
                .NotEmpty().WithMessage("Branch name is required.");

            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("At least one sale item is required.");

            RuleForEach(sale => sale.Items)
                .SetValidator(new CreateSaleItemValidator());
        }
    }
}
