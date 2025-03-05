using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

/// <summary>
/// Validator for GetAllSalesCommand
/// </summary>
public class GetAllSalesValidator : AbstractValidator<GetAllSalesCommand>
{
    /// <summary>
    /// Initializes a new instance of GetAllSalesValidator
    /// </summary>
    public GetAllSalesValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
} 