using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.UpdateCart;
public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    public UpdateCartRequestValidator()
    {
        RuleFor(x => x.CustomerExternalId)
            .NotEmpty().WithMessage("Customer external ID is required");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required");

        RuleFor(x => x.BranchExternalId)
            .NotEmpty().WithMessage("Branch external ID is required");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required");

        RuleFor(x => x.Products)
            .NotNull().WithMessage("Products cannot be null");

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(p => p.ProductExternalId)
                .NotEmpty().WithMessage("Product external ID is required");

            product.RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product name is required");

            product.RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20");

            product.RuleFor(p => p.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0");
        });
    }
}
