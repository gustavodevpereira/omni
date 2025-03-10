using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

/// <summary>
/// Validator for <see cref="GetCartsRequest"/> that enforces validation rules for cart list requests.
/// </summary>
/// <remarks>
/// Inherits common pagination validation rules from <see cref="PaginationRequestValidator{T}"/>.
/// </remarks>
public class GetCartsRequestValidator : PaginationRequestValidator<GetCartsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCartsRequestValidator"/> class.
    /// </summary>
    public GetCartsRequestValidator() : base()
    {
        // Additional cart-specific validation rules can be added here
    }
} 