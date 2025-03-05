using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

/// <summary>
/// Handler for processing GetAllSalesCommand requests
/// </summary>
public class GetAllSalesHandler : IRequestHandler<GetAllSalesCommand, GetAllSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetAllSalesHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetAllSalesHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllSalesCommand request
    /// </summary>
    /// <param name="request">The GetAllSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of sales</returns>
    public async Task<GetAllSalesResult> Handle(GetAllSalesCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetAllSalesValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sales = await _saleRepository.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        var totalCount = await _saleRepository.CountAsync(cancellationToken);

        var result = new GetAllSalesResult
        {
            Items = _mapper.Map<List<GetAllSalesResult.SaleDto>>(sales),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return result;
    }
} 