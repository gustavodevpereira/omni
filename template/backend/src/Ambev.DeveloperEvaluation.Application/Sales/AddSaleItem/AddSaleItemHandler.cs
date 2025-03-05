using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem
{
    /// <summary>
    /// Handler for processing AddSaleItemCommand requests.
    /// </summary>
    public class AddSaleItemHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSaleItemHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="uow">The unit of work.</param>
        public AddSaleItemHandler(ISaleRepository saleRepository, IUnitOfWork uow)
        {
            _saleRepository = saleRepository;
            _uow = uow;
        }

        /// <summary>
        /// Handles the AddSaleItemCommand by validating the command, retrieving the existing sale,
        /// adding a new item, and persisting the changes.
        /// </summary>
        /// <param name="command">The add sale item command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The result containing the sale ID, new sale item ID, and updated total amount.</returns>
        /// <exception cref="ValidationException">Thrown if command validation fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the sale is not found.</exception>
        public async Task<AddSaleItemResult> Handle(AddSaleItemCommand command, CancellationToken cancellationToken)
        {
            // Validate the command
            var validator = new AddSaleItemValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Retrieve the existing sale by its ID
            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
            if (sale == null)
                throw new InvalidOperationException($"Sale with ID {command.SaleId} not found.");

            // Add the sale item using the domain method.
            sale.AddItem(command.ProductExternalId, command.ProductName, command.Quantity, command.UnitPrice);

            // Update the sale in the repository (if necessary) and commit the changes.
            await _uow.CommitAsync(cancellationToken);

            // Assume the new sale item is the last added item.
            var newSaleItem = sale.Items.Last();

            // Build and return the result.
            return new AddSaleItemResult
            {
                SaleId = sale.Id,
                SaleItemId = newSaleItem.Id,
                NewTotalAmount = sale.TotalAmount
            };
        }
    }
}
