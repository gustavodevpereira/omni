namespace Ambev.DeveloperEvaluation.Application.Carts.Common.Results;

/// <summary>
/// Represents the response returned after successfully retrieving a cart.
/// </summary>
public class CartResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the date when the cart was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the external identifier for the customer.
    /// </summary>
    public string CustomerExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external identifier for the branch.
    /// </summary>
    public string BranchExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the cart.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount of the cart.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the items in the cart.
    /// </summary>
    public List<CartProductResult> Products { get; set; } = [];
}
