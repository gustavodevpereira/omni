using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Validator for the CancelSaleCommand.
/// </summary>
public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleValidator"/> class.
    /// </summary>
    public CancelSaleValidator()
    {
        RuleFor(c => c.SaleId)
            .NotEmpty().WithMessage("Sale ID is required for cancellation.");
    }
}
