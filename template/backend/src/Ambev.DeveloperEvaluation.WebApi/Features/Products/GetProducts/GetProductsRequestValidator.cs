using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Validator for <see cref="GetProductsRequest"/> that enforces validation rules for product list requests.
/// </summary>
/// <remarks>
/// Inherits common pagination validation rules from <see cref="PaginationRequestValidator{T}"/>.
/// </remarks>
public class GetProductsRequestValidator : PaginationRequestValidator<GetProductsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsRequestValidator"/> class.
    /// </summary>
    public GetProductsRequestValidator() : base()
    {
        // Additional product-specific validation rules can be added here
    }
} 