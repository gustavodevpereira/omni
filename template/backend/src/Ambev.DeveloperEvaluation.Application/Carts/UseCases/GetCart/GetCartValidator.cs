using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCart;

/// <summary>
/// Validator for GetSaleCommand
/// </summary>
public class GetCartValidator : AbstractValidator<GetCartCommand>
{
    /// <summary>
    /// Initializes a new instance of GetSaleValidator
    /// </summary>
    public GetCartValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sale ID is required.");
    }
}