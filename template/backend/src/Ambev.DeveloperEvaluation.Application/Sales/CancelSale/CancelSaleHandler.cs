using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests.
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _uow;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">The sale repository.</param>
    /// <param name="uow">The unit of work.</param>
    public CancelSaleHandler(ISaleRepository saleRepository, IUnitOfWork uow)
    {
        _saleRepository = saleRepository;
        _uow = uow;
    }

    /// <summary>
    /// Handles the CancelSaleCommand by validating the command, retrieving the existing sale,
    /// and setting its status to Cancelled.
    /// </summary>
    /// <param name="command">The cancel sale command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result indicating the sale ID and whether the cancellation succeeded.</returns>
    /// <exception cref="ValidationException">Thrown if command validation fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the sale is not found.</exception>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Retrieve the existing sale by its ID
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {command.SaleId} not found.");

        // Cancel the sale via the domain method
        sale.CancelSale();

        // Update the sale in the repository (if needed) and commit changes
        await _uow.CommitAsync(cancellationToken);

        // Build and return the result
        return new CancelSaleResult
        {
            SaleId = sale.Id,
            IsCancelled = sale.Status == SaleStatus.Cancelled
        };
    }
}
