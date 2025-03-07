using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Handler for processing GetAllSalesCommand requests
/// </summary>
public class GetCartsHandler : IRequestHandler<GetCartsCommand, GetCartsResult>
{
    private readonly ICartRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCartsHandler
    /// </summary>
    /// <param name="saleRepository">The Cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCartsHandler(
        ICartRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
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

        var sales = await _saleRepository.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        var totalCount = await _saleRepository.CountAsync(cancellationToken);

        var result = new GetCartsResult
        {
            Items = _mapper.Map<List<CartResult>>(sales),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return result;
    }
}