using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class CartValidator : AbstractValidator<Cart>
{
    public CartValidator()
    {
        RuleFor(cart => cart.CustomerExternalId)
            .NotEmpty()
            .WithMessage("Customer external ID is required.");

        RuleFor(cart => cart.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required.");

        RuleFor(cart => cart.BranchExternalId)
            .NotEmpty()
            .WithMessage("Branch external ID is required.");

        RuleFor(cart => cart.BranchName)
            .NotEmpty()
            .WithMessage("Branch name is required.");

        RuleFor(cart => cart.CreatedOn)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future.");
    }
}
