using System;
using System.Collections.Generic;
using MediatR;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount
{
    /// <summary>
    /// Command for calculating the discounts for a cart
    /// </summary>
    public class CalculateCartDiscountCommand : IRequest<CartResultWithDiscounts>
    {
        /// <summary>
        /// The customer ID for the cart
        /// </summary>
        public Guid CustomerId { get; set; }
        
        /// <summary>
        /// The customer name for the cart
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;
        
        /// <summary>
        /// The customer email for the cart
        /// </summary>
        public string CustomerEmail { get; set; } = string.Empty;
        
        /// <summary>
        /// The branch external ID for the cart
        /// </summary>
        public string BranchExternalId { get; set; } = string.Empty;
        
        /// <summary>
        /// The branch name for the cart
        /// </summary>
        public string BranchName { get; set; } = string.Empty;
        
        /// <summary>
        /// The date for the cart
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// The products in the cart
        /// </summary>
        public List<CalculateCartDiscountProductCommand> Products { get; set; } = [];
    }
    
    /// <summary>
    /// Command for a product in the cart discount calculation
    /// </summary>
    public class CalculateCartDiscountProductCommand
    {
        /// <summary>
        /// The product ID
        /// </summary>
        public Guid ProductId { get; set; }
        
        /// <summary>
        /// The product name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// The product price
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// The quantity of the product
        /// </summary>
        public int Quantity { get; set; }
    }
} 