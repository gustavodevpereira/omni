using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Handler for processing GetProductQuery requests.
/// </summary>
public class GetProductHandler : IRequestHandler<GetProductQuery, GetProductResult?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetProductHandler class.
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the GetProductQuery.
    /// </summary>
    /// <param name="query">The query to retrieve a product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product result if found, null otherwise</returns>
    public async Task<GetProductResult?> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.Id, cancellationToken);
        
        if (product == null)
            return null;

        return _mapper.Map<GetProductResult>(product);
    }
} 