using Ambev.DeveloperEvaluation.Application.Common.Validators;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Validator for <see cref="GetCartsCommand"/> that enforces validation rules for cart list requests.
/// </summary>
/// <remarks>
/// This validator inherits common pagination validation rules from <see cref="PaginatedRequestValidator{T}"/>
/// and adds specific validation for the customer ID filter when present.
/// </remarks>
public class GetCartsCommandValidator : PaginatedRequestValidator<GetCartsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCartsCommandValidator"/> class.
    /// </summary>
    public GetCartsCommandValidator() : base()
    {
        RuleFor(x => x.CustomerId)
            .NotNull().WithMessage("Customer ID is required")
            .NotEqual(Guid.Empty).WithMessage("Customer ID cannot be empty when specified");
    }
}