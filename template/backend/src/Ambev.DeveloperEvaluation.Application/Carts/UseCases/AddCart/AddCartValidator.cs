using FluentValidation;
using System;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart
{
    public class AddCartValidator : AbstractValidator<AddCartCommand>
    {
        public AddCartValidator()
        {
            RuleFor(x => x.CostumerId)
                .NotEmpty()
                .WithMessage("Customer ID is required");

            // Customer details now fetched from repository based on CostumerId
            
            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage("Branch ID is required");

            RuleFor(x => x.BranchName)
                .NotEmpty()
                .WithMessage("Branch name is required")
                .MaximumLength(100)
                .WithMessage("Branch name cannot exceed 100 characters");

            RuleFor(x => x.CreatedOn)
                .NotEmpty()
                .WithMessage("Creation date is required")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Creation date cannot be in the future");

            RuleForEach(x => x.Products)
                .ChildRules(product =>
                {
                    product.RuleFor(p => p.Id)
                        .NotEmpty()
                        .WithMessage("Product ID is required");

                    product.RuleFor(p => p.Name)
                        .NotEmpty()
                        .WithMessage("Product name is required")
                        .MaximumLength(100)
                        .WithMessage("Product name cannot exceed 100 characters");

                    product.RuleFor(p => p.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than zero");

                    product.RuleFor(p => p.UnitPrice)
                        .GreaterThan(0)
                        .WithMessage("Unit price must be greater than zero");
                });
        }
    }
}