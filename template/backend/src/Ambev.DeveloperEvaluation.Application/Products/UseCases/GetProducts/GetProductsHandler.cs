using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Handler for processing <see cref="GetProductsCommand"/> requests to retrieve a paginated list of products.
/// </summary>
/// <remarks>
/// This handler is responsible for:
/// <list type="bullet">
/// <item><description>Validating the request using <see cref="GetProductsCommandValidator"/></description></item>
/// <item><description>Retrieving products from the repository with pagination</description></item>
/// <item><description>Mapping the domain entities to DTO objects</description></item>
/// <item><description>Constructing the paginated response with metadata</description></item>
/// </list>
/// </remarks>
public class GetProductsHandler : IRequestHandler<GetProductsCommand, GetProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsHandler"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository for data access</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public GetProductsHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the <see cref="GetProductsCommand"/> request to retrieve a paginated list of products.
    /// </summary>
    /// <param name="request">The get products command with pagination parameters</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>A <see cref="GetProductsResult"/> containing the paginated list of products</returns>
    /// <exception cref="ValidationException">Thrown when the request fails validation</exception>
    public async Task<GetProductsResult> Handle(GetProductsCommand request, CancellationToken cancellationToken)
    {
        // Validate request
        var validator = new GetProductsCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Retrieve data with pagination
        var products = await _productRepository.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        var totalCount = await _productRepository.CountAsync(cancellationToken);

        // Map domain entities to DTOs
        var productDtos = _mapper.Map<List<ProductResult>>(products);

        // Create and return paginated result
        return new GetProductsResult(productDtos, totalCount, request.PageNumber, request.PageSize);
    }
} 