using Ambev.DeveloperEvaluation.Application.Common.Validators;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Validator for <see cref="GetProductsCommand"/> that enforces validation rules for product list requests.
/// </summary>
/// <remarks>
/// This validator inherits common pagination validation rules from <see cref="PaginatedRequestValidator{T}"/>
/// and can be extended with additional product-specific validation rules as needed.
/// </remarks>
public class GetProductsCommandValidator : PaginatedRequestValidator<GetProductsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsCommandValidator"/> class.
    /// </summary>
    public GetProductsCommandValidator() : base()
    {
        // Additional validation rules specific to GetProductsCommand can be added here
    }
} 