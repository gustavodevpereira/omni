using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

/// <summary>
/// Validator for GetCartsRequest
/// </summary>
public class GetCartsRequestValidator : AbstractValidator<GetCartsRequest>
{
    /// <summary>
    /// Initializes a new instance of GetCartsRequestValidator
    /// </summary>
    public GetCartsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
} 