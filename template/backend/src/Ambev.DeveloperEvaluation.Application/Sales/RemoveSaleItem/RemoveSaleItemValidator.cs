using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Validator for RemoveSaleItemCommand
/// </summary>
public class RemoveSaleItemValidator : AbstractValidator<RemoveSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of RemoveSaleItemValidator
    /// </summary>
    public RemoveSaleItemValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale ID is required");

        RuleFor(x => x.SaleItemId)
            .NotEmpty().WithMessage("Sale Item ID is required");
    }
} 