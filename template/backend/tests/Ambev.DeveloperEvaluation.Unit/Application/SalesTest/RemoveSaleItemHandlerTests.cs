using Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading;
using System.Threading.Tasks;
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
        /// </summary>
        public RemoveSaleItemHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new RemoveSaleItemHandler(_saleRepository, _unitOfWork);
        }

        /// <summary>
        /// Tests that a valid request to remove a sale item succeeds.
        /// </summary>
        [Fact(DisplayName = "Given valid sale and item IDs When removing sale item Then returns success result")]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Given
            var saleId = Guid.NewGuid();
            var saleItemId = Guid.NewGuid();
            var command = new RemoveSaleItemCommand(saleId, saleItemId);
            
            var sale = new Sale(
                "SALE001", 
                DateTime.Now, 
                "CUSTOMER001", 
                "Test Customer", 
                "BRANCH001", 
                "Test Branch");
                
            // Add item with the specific ID we want to test against
            typeof(SaleItem).GetProperty(nameof(SaleItem.Id))?.SetValue(
                sale.AddItem("PRODUCT001", "Test Product", 2, 10.0M), 
                saleItemId);
            
            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.SaleId.Should().Be(saleId);
            result.SaleNumber.Should().Be("SALE001");
            result.RemovedSaleItemId.Should().Be(saleItemId);
            result.NewTotalAmount.Should().Be(sale.TotalAmount);
            result.Message.Should().Be("Sale item removed successfully");
            
            await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that when a sale is not found, the handler throws a KeyNotFoundException.
        /// </summary>
        [Fact(DisplayName = "Given non-existent sale ID When removing sale item Then throws KeyNotFoundException")]
        public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
        {
            // Given
            var saleId = Guid.NewGuid();
            var saleItemId = Guid.NewGuid();
            var command = new RemoveSaleItemCommand(saleId, saleItemId);
            
            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns((Sale)null);

            // When
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {saleId} not found");
            
            await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that when an error occurs during item removal, the handler returns a failure result.
        /// </summary>
        [Fact(DisplayName = "Given error in removing item When removing sale item Then returns failure result")]
        public async Task Handle_ErrorInRemovingItem_ReturnsFailureResult()
        {
            // Given
            var saleId = Guid.NewGuid();
            var saleItemId = Guid.NewGuid();
            var command = new RemoveSaleItemCommand(saleId, saleItemId);
            
            var sale = new Sale(
                "SALE001", 
                DateTime.Now, 
                "CUSTOMER001", 
                "Test Customer", 
                "BRANCH001", 
                "Test Branch");
                
            // Mock the sale to throw an exception when RemoveItem is called
            var exceptionMessage = "Sale item not found.";
            sale.When(s => s.RemoveItem(Arg.Any<Guid>()))
                .Do(_ => throw new Exception(exceptionMessage));
            
            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.SaleId.Should().Be(saleId);
            result.SaleNumber.Should().Be("SALE001");
            result.RemovedSaleItemId.Should().Be(saleItemId);
            result.Message.Should().Be(exceptionMessage);
            
            await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid request to remove a sale item throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given empty sale ID When removing sale item Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Given
            var command = new RemoveSaleItemCommand(Guid.Empty, Guid.NewGuid());

            // When
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }
    }
} 