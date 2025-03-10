using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Validator for GetProductsCommand
/// </summary>
public class GetProductsCommandValidator : AbstractValidator<GetProductsCommand>
{
    /// <summary>
    /// Initializes a new instance of GetProductsCommandValidator
    /// </summary>
    public GetProductsCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
} 