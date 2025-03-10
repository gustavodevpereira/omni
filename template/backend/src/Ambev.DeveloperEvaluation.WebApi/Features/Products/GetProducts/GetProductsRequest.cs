namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Request model for retrieving products with pagination
/// </summary>
public class GetProductsRequest
{
    /// <summary>
    /// The page number (starting from 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// The number of items per page
    /// </summary>
    public int PageSize { get; set; } = 10;
} 