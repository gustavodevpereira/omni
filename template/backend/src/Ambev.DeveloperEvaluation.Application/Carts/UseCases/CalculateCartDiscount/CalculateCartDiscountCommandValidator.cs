using System;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount
{
    /// <summary>
    /// Validator for <see cref="CalculateCartDiscountCommand"/>
    /// </summary>
    public class CalculateCartDiscountCommandValidator : AbstractValidator<CalculateCartDiscountCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateCartDiscountCommandValidator"/> class
        /// </summary>
        public CalculateCartDiscountCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required");
                
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required")
                .Must(date => date <= DateTime.Now).WithMessage("Date cannot be in the future");
                
            RuleFor(x => x.Products)
                .NotEmpty().WithMessage("At least one product is required");
                
            RuleForEach(x => x.Products).ChildRules(product => {
                product.RuleFor(p => p.ProductId)
                    .NotEmpty().WithMessage("Product ID is required");
                    
                product.RuleFor(p => p.Name)
                    .NotEmpty().WithMessage("Product name is required");
                    
                product.RuleFor(p => p.Price)
                    .GreaterThan(0).WithMessage("Product price must be greater than 0");
                    
                product.RuleFor(p => p.Quantity)
                    .GreaterThan(0).WithMessage("Product quantity must be greater than 0")
                    .LessThanOrEqualTo(20).WithMessage("Product quantity cannot exceed 20");
            });
        }
    }
} 