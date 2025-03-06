using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem
{
    /// <summary>
    /// Validator for the AddSaleItemCommand.
    /// </summary>
    public class AddSaleItemValidator : AbstractValidator<AddSaleItemCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddSaleItemValidator"/> class.
        /// </summary>
        public AddSaleItemValidator()
        {
            RuleFor(c => c.SaleId)
                .NotEmpty().WithMessage("Sale ID is required.");

            RuleFor(c => c.ProductExternalId)
                .NotEmpty().WithMessage("Product external ID is required.");

            RuleFor(c => c.ProductName)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Quantity must be 20 or less.");

            RuleFor(c => c.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
