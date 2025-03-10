using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Handler for processing GetCartsCommand requests
/// </summary>
public class GetCartsHandler : IRequestHandler<GetCartsCommand, GetCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCartsHandler
    /// </summary>
    /// <param name="cartRepository">The Cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCartsHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCartsCommand request
    /// </summary>
    /// <param name="request">The GetCarts command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of carts</returns>
    public async Task<GetCartsResult> Handle(GetCartsCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetCartsCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        IEnumerable<Domain.Entities.Carts.Cart> carts;
        int totalCount;

        if (!request.CustomerId.HasValue)
        {
            throw new InvalidOperationException("CustomerId doesn't have value");
        }


        // Get carts for the specific customer
        carts = await _cartRepository.GetAllPagedByCustomerAsync(
        request.CustomerId.Value, request.PageNumber, request.PageSize, cancellationToken);
        totalCount = await _cartRepository.CountByCustomerAsync(request.CustomerId.Value, cancellationToken);


        var result = new GetCartsResult
        {
            Items = _mapper.Map<List<CartResult>>(carts),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return result;
    }
}