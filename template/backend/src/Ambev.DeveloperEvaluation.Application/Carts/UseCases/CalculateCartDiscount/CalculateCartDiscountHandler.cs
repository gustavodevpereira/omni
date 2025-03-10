using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Entities.Users;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount
{
    /// <summary>
    /// Handles the calculation of discounts for a cart based on the quantity of products.
    /// </summary>
    public class CalculateCartDiscountHandler : IRequestHandler<CalculateCartDiscountCommand, CartResultWithDiscounts>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateCartDiscountHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository to fetch user information.</param>
        public CalculateCartDiscountHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the <see cref="CalculateCartDiscountCommand"/> and returns the cart with discount information
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Cart with discount information</returns>
        public async Task<CartResultWithDiscounts> Handle(CalculateCartDiscountCommand request, CancellationToken cancellationToken)
        {
            var validator = new CalculateCartDiscountCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Fetch user data from repository
            var user = await _userRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (user == null)
            {
                throw new DomainException("User", $"User with ID {request.CustomerId} not found");
            }

            // Create a cart domain entity to apply discount rules
            var cart = new Cart(
                DateTime.UtcNow,
                request.CustomerId,
                user.Username, // Use the username from the repository
                request.BranchExternalId != null ? Guid.Parse(request.BranchExternalId) : Guid.Empty,
                request.BranchName ?? "Default Branch"
            );

            // Add products to the cart (this will apply discount rules based on quantity)
            foreach (var productRequest in request.Products)
            {
                cart.AddProduct(
                    productRequest.ProductId,
                    productRequest.Name,
                    productRequest.Quantity,
                    productRequest.Price
                );
            }

            // Calculate and map discount information
            var result = new CartResultWithDiscounts
            {
                Id = cart.Id,
                CustomerId = request.CustomerId,
                CustomerName = user.Username,
                CustomerEmail = user.Email,
                BranchExternalId = request.BranchExternalId ?? Guid.Empty.ToString(),
                BranchName = request.BranchName ?? "Default Branch",
                Date = request.Date,
                TotalAmount = cart.Products.Sum(item => item.UnitPrice * item.Quantity),
                TotalDiscount = cart.Products.Sum(item => item.UnitPrice * item.Quantity * item.DiscountPercentage),
                FinalAmount = cart.Products.Sum(item => item.UnitPrice * item.Quantity * (1 - item.DiscountPercentage))
            };

            // Map cart items with discount information
            foreach (var item in cart.Products)
            {
                var subtotal = item.UnitPrice * item.Quantity;
                var discountAmount = subtotal * item.DiscountPercentage;
                var finalAmount = subtotal - discountAmount;

                result.Products.Add(new CartProductResultWithDiscount
                {
                    ProductId = item.ProductExternalId,
                    Name = item.ProductName,
                    Price = item.UnitPrice,
                    Quantity = item.Quantity,
                    Subtotal = subtotal,
                    DiscountPercentage = item.DiscountPercentage,
                    DiscountAmount = discountAmount,
                    FinalAmount = finalAmount
                });
            }

            return result;
        }
    }
}