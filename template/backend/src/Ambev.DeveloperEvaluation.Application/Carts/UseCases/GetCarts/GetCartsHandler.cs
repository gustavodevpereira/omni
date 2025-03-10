using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Handler for processing <see cref="GetCartsCommand"/> requests to retrieve a paginated list of carts.
/// </summary>
/// <remarks>
/// This handler is responsible for:
/// <list type="bullet">
/// <item><description>Validating the request using <see cref="GetCartsCommandValidator"/></description></item>
/// <item><description>Retrieving carts from the repository with pagination and optional customer filtering</description></item>
/// <item><description>Mapping the domain entities to DTO objects</description></item>
/// <item><description>Constructing the paginated response with metadata</description></item>
/// </list>
/// </remarks>
public class GetCartsHandler : IRequestHandler<GetCartsCommand, GetCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCartsHandler"/> class.
    /// </summary>
    /// <param name="cartRepository">The cart repository for data access</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public GetCartsHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the <see cref="GetCartsCommand"/> request to retrieve a paginated list of carts.
    /// </summary>
    /// <param name="request">The get carts command with pagination parameters and optional customer filter</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>A <see cref="GetCartsResult"/> containing the paginated list of carts</returns>
    /// <exception cref="ValidationException">Thrown when the request fails validation</exception>
    /// <exception cref="InvalidOperationException">Thrown when the customer ID is required but not provided</exception>
    public async Task<GetCartsResult> Handle(GetCartsCommand request, CancellationToken cancellationToken)
    {
        // Validate request
        var validator = new GetCartsCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Retrieve data with pagination
        IEnumerable<Domain.Entities.Carts.Cart> carts;
        int totalCount;

        // Get carts for the specific customer
        carts = await _cartRepository.GetAllPagedByCustomerAsync(request.CustomerId, request.PageNumber, request.PageSize, cancellationToken);
        totalCount = await _cartRepository.CountByCustomerAsync(request.CustomerId, cancellationToken);

        // Map domain entities to DTOs
        var cartDtos = _mapper.Map<List<CartResult>>(carts);

        // Create and return paginated result
        return new GetCartsResult(cartDtos, totalCount, request.PageNumber, request.PageSize);
    }
}