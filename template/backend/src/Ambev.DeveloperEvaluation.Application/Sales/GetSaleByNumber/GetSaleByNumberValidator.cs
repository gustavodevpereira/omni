using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;

/// <summary>
/// Validator for GetSaleByNumberCommand
/// </summary>
public class GetSaleByNumberValidator : AbstractValidator<GetSaleByNumberCommand>
{
    /// <summary>
    /// Initializes a new instance of GetSaleByNumberValidator
    /// </summary>
    public GetSaleByNumberValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");
    }
} 