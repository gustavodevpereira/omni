using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCarts;

/// <summary>
/// Validator for GetCartsRequest
/// </summary>
public class GetCartsRequestValidator : AbstractValidator<GetCartsRequest>
{
    /// <summary>
    /// Initializes validation rules
    /// </summary>
    public GetCartsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");
    }
}