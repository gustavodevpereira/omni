using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Handler for processing GetProductsCommand requests
/// </summary>
public class GetProductsHandler : IRequestHandler<GetProductsCommand, GetProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductsHandler
    /// </summary>
    /// <param name="productRepository">The Product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductsHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductsCommand request
    /// </summary>
    /// <param name="request">The GetProducts command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of products</returns>
    public async Task<GetProductsResult> Handle(GetProductsCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetProductsCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var products = await _productRepository.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        var totalCount = await _productRepository.CountAsync(cancellationToken);

        var result = new GetProductsResult
        {
            Items = _mapper.Map<List<ProductResult>>(products),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return result;
    }
} 