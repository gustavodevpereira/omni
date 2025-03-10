using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Validator for GetProductsRequest
/// </summary>
public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
{
    /// <summary>
    /// Initializes a new instance of GetProductsRequestValidator
    /// </summary>
    public GetProductsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
} 