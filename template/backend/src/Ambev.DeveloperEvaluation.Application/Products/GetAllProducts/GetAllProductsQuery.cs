using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

/// <summary>
/// Query to retrieve all products.
/// </summary>
public class GetAllProductsQuery : IRequest<List<GetAllProductsResult>>
{
} 