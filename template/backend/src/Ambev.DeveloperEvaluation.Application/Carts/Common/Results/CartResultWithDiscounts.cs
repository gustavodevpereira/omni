using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common.Results
{
    /// <summary>
    /// Represents a cart result with discount information
    /// </summary>
    public class CartResultWithDiscounts
    {
        /// <summary>
        /// The cart ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The customer ID
        /// </summary>
        public Guid CustomerId { get; set; }
        
        /// <summary>
        /// The customer name
        /// </summary>
        public string CustomerName { get; set; }
        
        /// <summary>
        /// The customer email
        /// </summary>
        public string CustomerEmail { get; set; }
        
        /// <summary>
        /// The branch external ID
        /// </summary>
        public string BranchExternalId { get; set; }
        
        /// <summary>
        /// The branch name
        /// </summary>
        public string BranchName { get; set; }
        
        /// <summary>
        /// The cart date
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// The total amount of the cart before discounts
        /// </summary>
        public decimal TotalAmount { get; set; }
        
        /// <summary>
        /// The total discount applied to the cart
        /// </summary>
        public decimal TotalDiscount { get; set; }
        
        /// <summary>
        /// The final amount after discounts
        /// </summary>
        public decimal FinalAmount { get; set; }
        
        /// <summary>
        /// The products in the cart with discount information
        /// </summary>
        public List<CartProductResultWithDiscount> Products { get; set; } = new List<CartProductResultWithDiscount>();
    }

    /// <summary>
    /// Represents a cart product result with discount information
    /// </summary>
    public class CartProductResultWithDiscount
    {
        /// <summary>
        /// The product ID
        /// </summary>
        public Guid ProductId { get; set; }
        
        /// <summary>
        /// The product name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The product price
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// The quantity of the product
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// The subtotal amount before discount (Price * Quantity)
        /// </summary>
        public decimal Subtotal { get; set; }
        
        /// <summary>
        /// The discount percentage applied to the product
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        
        /// <summary>
        /// The discount amount applied to the product
        /// </summary>
        public decimal DiscountAmount { get; set; }
        
        /// <summary>
        /// The final amount after discount
        /// </summary>
        public decimal FinalAmount { get; set; }
    }
} 