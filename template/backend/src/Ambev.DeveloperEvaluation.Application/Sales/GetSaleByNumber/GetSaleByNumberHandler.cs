using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;

/// <summary>
/// Handler for processing GetSaleByNumberCommand requests
/// </summary>
public class GetSaleByNumberHandler : IRequestHandler<GetSaleByNumberCommand, GetSaleByNumberResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleByNumberHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSaleByNumberHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSaleByNumberCommand request
    /// </summary>
    /// <param name="request">The GetSaleByNumber command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    public async Task<GetSaleByNumberResult> Handle(GetSaleByNumberCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetSaleByNumberValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByNumberAsync(request.SaleNumber, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with number {request.SaleNumber} not found");

        return _mapper.Map<GetSaleByNumberResult>(sale);
    }
} 