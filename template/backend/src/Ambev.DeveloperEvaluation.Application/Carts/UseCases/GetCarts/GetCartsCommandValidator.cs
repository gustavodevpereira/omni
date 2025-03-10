using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Validator for GetCartsCommand
/// </summary>
public class GetCartsCommandValidator : AbstractValidator<GetCartsCommand>
{
    /// <summary>
    /// Initializes a new instance of GetCartsCommandValidator
    /// </summary>
    public GetCartsCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
}