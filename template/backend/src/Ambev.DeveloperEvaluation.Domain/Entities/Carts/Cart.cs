using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Carts;

/// <summary>
/// Represents a cart transaction (aggregate root) in the system.
/// It encapsulates cart details such as cart number, cart date, customer and branch information,
/// and manages the collection of cart items. The aggregate also calculates the total cart amount.
/// </summary>
public class Cart : BaseEntity
{
    /// <summary>
    /// Gets the date and time when the cart was created.
    /// </summary>
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// Gets the external identifier for the customer involved in the cart.
    /// </summary>
    public Guid CustomerExternalId { get; private set; }

    /// <summary>
    /// Gets the customer name (denormalized data) used in the cart.
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Gets the external identifier for the branch where the cart was made.
    /// </summary>
    public Guid BranchExternalId { get; private set; }

    /// <summary>
    /// Gets the branch name (denormalized data) used in the cart.
    /// </summary>
    public string BranchName { get; private set; }

    /// <summary>
    /// Gets the current status of the cart (e.g., Active or Cancelled).
    /// </summary>
    public CartStatus Status { get; private set; }

    /// <summary>
    /// Gets the read-only collection of cart items included in this cart.
    /// </summary>
    public IReadOnlyCollection<CartProduct> Products => _products.AsReadOnly();

    /// <summary>
    /// Gets the total cart amount calculated by summing the total of each cart item.
    /// </summary>
    public decimal TotalAmount => _products.Sum(item => item.TotalAmount);

    // Internal collection of cart items.
    private readonly List<CartProduct> _products;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cart"/> class.
    /// Validates that essential cart information is provided.
    /// </summary>
    /// <param name="createdOn">The date when the cart was created.</param>
    /// <param name="customerExternalId">The external identifier for the customer.</param>
    /// <param name="customerName">The name of the customer.</param>
    /// <param name="branchExternalId">The external identifier for the branch.</param>
    /// <param name="branchName">The name of the branch.</param>
    /// <exception cref="DomainException">
    /// Thrown when any of the required fields is null or empty.
    /// </exception>
    public Cart(DateTime createdOn, Guid customerExternalId, string customerName, Guid branchExternalId, string branchName)
    {
        Id = Guid.NewGuid();
        CreatedOn = createdOn;
        CustomerExternalId = customerExternalId;
        CustomerName = customerName;
        BranchExternalId = branchExternalId;
        BranchName = branchName;
        Status = CartStatus.Active;

        _products = [];

        AddDomainEvent(new CartCreatedEvent(this));
    }

    public ValidationResultDetail Validate()
    {
        var validator = new CartValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }


    /// <summary>
    /// Adds a cart item to the cart by providing the product details, quantity, and unit price.
    /// The Cart aggregate creates the CartItem internally, ensuring all business rules are enforced.
    /// </summary>
    /// <param name="productExternalId">The external product identifier.</param>
    /// <param name="productName">The product name (denormalized).</param>
    /// <param name="quantity">The quantity of the product (must be between 1 and 20).</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <exception cref="DomainException">
    /// Thrown if the cart is cancelled or if the CartItem is invalid.
    /// </exception>
    public void AddProduct(Guid productExternalId, string productName, int quantity, decimal unitPrice)
    {
        EnsureCartModificationAllowed();
        var product = new CartProduct(productExternalId, productName, quantity, unitPrice);

        _products.Add(product);
        
        AddDomainEvent(new CartModifiedEvent(this));
    }


    /// <summary>
    /// Removes a cart item from the cart by its unique identifier.
    /// </summary>
    /// <param name="cartItemId">The unique identifier of the cart item to remove.</param>
    /// <exception cref="DomainException">
    /// Thrown if the cart is cancelled or if the cart item is not found.
    /// </exception>
    public void RemoveItem(Guid cartItemId)
    {
        EnsureCartModificationAllowed();

        var cartItem = _products.FirstOrDefault(item => item.Id == cartItemId);

        if (cartItem == null)
            throw new DomainException("Cart item not found.");

        _products.Remove(cartItem);
        AddDomainEvent(new CartProductCancelledEvent(this, cartItemId));
        AddDomainEvent(new CartModifiedEvent(this));
    }

    /// <summary>
    /// Ensures that modifications to the cart (adding or removing items) are allowed by verifying the cart is not cancelled.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown if the cart's status is <see cref="CartStatus.Cancelled"/>, meaning the cart cannot be modified.
    /// </exception>
    public void EnsureCartModificationAllowed()
    {
        if (Status == CartStatus.Cancelled)
            throw new DomainException("Cannot add or remove items from a cancelled cart.");
    }


    /// <summary>
    /// Cancels the cart by setting its status to Cancelled.
    /// </summary>
    public void CancelCart()
    {
        Status = CartStatus.Cancelled;
        AddDomainEvent(new CartCancelledEvent(this));
    }
}
