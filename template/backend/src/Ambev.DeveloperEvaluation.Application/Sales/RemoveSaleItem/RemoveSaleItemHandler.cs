using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Handler for processing RemoveSaleItemCommand requests
/// </summary>
public class RemoveSaleItemHandler : IRequestHandler<RemoveSaleItemCommand, RemoveSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of RemoveSaleItemHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public RemoveSaleItemHandler(
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the RemoveSaleItemCommand request
    /// </summary>
    /// <param name="request">The RemoveSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of removing the sale item</returns>
    public async Task<RemoveSaleItemResult> Handle(RemoveSaleItemCommand request, CancellationToken cancellationToken)
    {
        var validator = new RemoveSaleItemValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.SaleId} not found");

        sale.RemoveItem(request.SaleItemId);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new RemoveSaleItemResult
        {
            SaleId = sale.Id,
            SaleNumber = sale.SaleNumber,
            RemovedSaleItemId = request.SaleItemId,
            NewTotalAmount = sale.TotalAmount,
        };
    }
} 