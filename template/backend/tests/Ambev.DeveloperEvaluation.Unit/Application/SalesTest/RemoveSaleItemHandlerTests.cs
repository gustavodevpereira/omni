using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.SalesTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="RemoveSaleItemHandler"/> class.
    /// </summary>
    public class RemoveSaleItemHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RemoveSaleItemHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveSaleItemHandlerTests"/> class.
        /// Sets up the test dependencies.
        /// </summary>
        public RemoveSaleItemHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new RemoveSaleItemHandler(_saleRepository, _unitOfWork);
        }

        /// <summary>
        /// Tests that a valid remove sale item request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid remove sale item command When handling Then removes item successfully")]
        public async Task Handle_ValidCommand_RemovesItemSuccessfully()
        {
            // Given
            var saleId = Guid.NewGuid();
            var saleItemId = Guid.NewGuid();
            var saleNumber = "S001";

            var sale = CreateSaleWithItem(saleNumber, saleItemId);
            var initialTotal = sale.TotalAmount;

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);

            var command = new RemoveSaleItemCommand(saleId, saleItemId);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.SaleNumber.Should().Be(saleNumber);
            result.RemovedSaleItemId.Should().Be(saleItemId);
            result.NewTotalAmount.Should().Be(0);
            
            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that a command with non-existent sale throws KeyNotFoundException.
        /// </summary>
        [Fact(DisplayName = "Given non-existent sale When handling remove item Then throws KeyNotFoundException")]
        public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
        {
            // Given
            var saleId = Guid.NewGuid();
            var saleItemId = Guid.NewGuid();

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            var command = new RemoveSaleItemCommand(saleId, saleItemId);

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {saleId} not found");
        }

        /// <summary>
        /// Tests that a command with non-existent sale item throws DomainException.
        /// </summary>
        [Fact(DisplayName = "Given non-existent sale item When handling remove item Then throws DomainException")]
        public async Task Handle_NonExistentSaleItem_ThrowsDomainException()
        {
            // Given
            var saleId = Guid.NewGuid();
            var validSaleItemId = Guid.NewGuid();
            var nonExistentSaleItemId = Guid.NewGuid();

            var sale = CreateSaleWithItem("S001", validSaleItemId);

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);

            var command = new RemoveSaleItemCommand(saleId, nonExistentSaleItemId);

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Sale item not found.");
        }

        /// <summary>
        /// Tests that an invalid command throws ValidationException.
        /// </summary>
        [Fact(DisplayName = "Given invalid command When handling remove item Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Given
            var command = new RemoveSaleItemCommand(Guid.Empty, Guid.Empty); // Empty command will fail validation

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ValidationException>();
        }

        /// <summary>
        /// Tests that a command on a cancelled sale throws DomainException.
        /// </summary>
        [Fact(DisplayName = "Given cancelled sale When handling remove item Then throws DomainException")]
        public async Task Handle_CancelledSale_ThrowsDomainException()
        {
            // Given
            var saleId = Guid.NewGuid();
            var saleItemId = Guid.NewGuid();

            var sale = CreateSaleWithItem("S001", saleItemId);
            sale.CancelSale(); // Cancel the sale

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);

            var command = new RemoveSaleItemCommand(saleId, saleItemId);

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Cannot add or remove items from a cancelled sale.");
        }

        /// <summary>
        /// Helper method to create a sale with a single item.
        /// </summary>
        private Sale CreateSaleWithItem(string saleNumber, Guid saleItemId)
        {
            // Create a new sale
            var sale = new Sale(
                saleNumber,
                DateTime.Now,
                "C001",
                "Customer Test",
                "B001",
                "Branch Test"
            );

            // Use Reflection to set the ID of the added sale item
            sale.AddItem("P001", "Test Product", 1, 100m);
            var items = typeof(Sale).GetField("_items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(sale) as List<SaleItem>;
            var item = items[0];

            // Use Reflection to set the ID of the added sale item
            typeof(SaleItem).GetProperty("Id").SetValue(item, saleItemId);

            return sale;
        }
    }
}
