using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validator for each sale item in CreateSaleCommand.
    /// </summary>
    public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemCommand>
    {
        public CreateSaleItemValidator()
        {
            RuleFor(item => item.ProductExternalId)
                .NotEmpty().WithMessage("Product external ID is required.");
            RuleFor(item => item.ProductName)
                .NotEmpty().WithMessage("Product name is required.");
            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(item => item.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
