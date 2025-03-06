using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales;

/// <summary>
/// Represents a sale transaction (aggregate root) in the system.
/// It encapsulates sale details such as sale number, sale date, customer and branch information,
/// and manages the collection of sale items. The aggregate also calculates the total sale amount.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the sale number, a unique identifier for the sale transaction.
    /// </summary>
    public string SaleNumber { get; private set; }

    /// <summary>
    /// Gets the date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the external identifier for the customer involved in the sale.
    /// </summary>
    public string CustomerExternalId { get; private set; }

    /// <summary>
    /// Gets the customer name (denormalized data) used in the sale.
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Gets the external identifier for the branch where the sale was made.
    /// </summary>
    public string BranchExternalId { get; private set; }

    /// <summary>
    /// Gets the branch name (denormalized data) used in the sale.
    /// </summary>
    public string BranchName { get; private set; }

    /// <summary>
    /// Gets the current status of the sale (e.g., Active or Cancelled).
    /// </summary>
    public SaleStatus Status { get; private set; }

    /// <summary>
    /// Gets the read-only collection of sale items included in this sale.
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Gets the total sale amount calculated by summing the total of each sale item.
    /// </summary>
    public decimal TotalAmount => _items.Sum(item => item.TotalAmount);

    // Internal collection of sale items.
    private readonly List<SaleItem> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// Validates that essential sale information is provided.
    /// </summary>
    /// <param name="saleNumber">The unique sale number.</param>
    /// <param name="saleDate">The date when the sale was made.</param>
    /// <param name="customerExternalId">The external identifier for the customer.</param>
    /// <param name="customerName">The name of the customer.</param>
    /// <param name="branchExternalId">The external identifier for the branch.</param>
    /// <param name="branchName">The name of the branch.</param>
    /// <exception cref="DomainException">
    /// Thrown when any of the required fields is null or empty.
    /// </exception>
    public Sale(string saleNumber, DateTime saleDate, string customerExternalId, string customerName, string branchExternalId, string branchName)
    {
        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerExternalId = customerExternalId;
        CustomerName = customerName;
        BranchExternalId = branchExternalId;
        BranchName = branchName;
        Status = SaleStatus.Active;

        _items = [];

        AddDomainEvent(new SaleCreatedEvent(this));
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }


    /// <summary>
    /// <summary>
    /// Adds a sale item to the sale by providing the product details, quantity, and unit price.
    /// The Sale aggregate creates the SaleItem internally, ensuring all business rules are enforced.
    /// </summary>
    /// <param name="productExternalId">The external product identifier.</param>
    /// <param name="productName">The product name (denormalized).</param>
    /// <param name="quantity">The quantity of the product (must be between 1 and 20).</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <exception cref="DomainException">
    /// Thrown if the sale is cancelled or if the SaleItem is invalid.
    /// </exception>
    public void AddItem(string productExternalId, string productName, int quantity, decimal unitPrice)
    {
        EnsureSaleModificationAllowed();

        var saleItem = new SaleItem(productExternalId, productName, quantity, unitPrice);
        var validationResult = saleItem.Validate();

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.Error));
            throw new DomainException(errorMessages);
        }

        _items.Add(saleItem);
    }


    /// <summary>
    /// Removes a sale item from the sale by its unique identifier.
    /// </summary>
    /// <param name="saleItemId">The unique identifier of the sale item to remove.</param>
    /// <exception cref="DomainException">
    /// Thrown if the sale is cancelled or if the sale item is not found.
    /// </exception>
    public void RemoveItem(Guid saleItemId)
    {
        EnsureSaleModificationAllowed();

        var saleItem = _items.FirstOrDefault(item => item.Id == saleItemId);

        if (saleItem == null)
            throw new DomainException("Sale item not found.");

        _items.Remove(saleItem);
        AddDomainEvent(new ItemCancelledEvent(this, saleItemId));
    }

    /// <summary>
    /// Ensures that modifications to the sale (adding or removing items) are allowed by verifying the sale is not cancelled.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown if the sale's status is <see cref="SaleStatus.Cancelled"/>, meaning the sale cannot be modified.
    /// </exception>
    public void EnsureSaleModificationAllowed()
    {
        if (Status == SaleStatus.Cancelled)
            throw new DomainException("Cannot add or remove items from a cancelled sale.");
    }


    /// <summary>
    /// Cancels the sale by setting its status to Cancelled.
    /// </summary>
    public void CancelSale()
    {
        Status = SaleStatus.Cancelled;
    }
}
