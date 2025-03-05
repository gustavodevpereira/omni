using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.SalesTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="GetSaleHandler"/> class.
    /// </summary>
    public class GetSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSaleHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
        /// </summary>
        public GetSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSaleHandler(_saleRepository, _mapper);
        }

        /// <summary>
        /// Tests that a valid sale retrieval request returns the expected sale.
        /// </summary>
        [Fact(DisplayName = "Given valid sale ID When retrieving sale Then returns sale details")]
        public async Task Handle_ValidRequest_ReturnsSale()
        {
            // Given
            var saleId = Guid.NewGuid();
            var command = new GetSaleCommand(saleId);
            
            var sale = new Sale(
                "SALE001", 
                DateTime.Now, 
                "CUSTOMER001", 
                "Test Customer", 
                "BRANCH001", 
                "Test Branch");
            
            var expectedResult = new GetSaleResult
            {
                Id = saleId,
                SaleNumber = "SALE001",
                CustomerName = "Test Customer",
                BranchName = "Test Branch",
                Status = "Active"
            };

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);
                
            _mapper.Map<GetSaleResult>(sale)
                .Returns(expectedResult);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(saleId);
            result.SaleNumber.Should().Be("SALE001");
            result.CustomerName.Should().Be("Test Customer");
            result.BranchName.Should().Be("Test Branch");
            result.Status.Should().Be("Active");
            
            await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<GetSaleResult>(sale);
        }

        /// <summary>
        /// Tests that when a sale is not found, the handler throws a KeyNotFoundException.
        /// </summary>
        [Fact(DisplayName = "Given non-existent sale ID When retrieving sale Then throws KeyNotFoundException")]
        public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
        {
            // Given
            var saleId = Guid.NewGuid();
            var command = new GetSaleCommand(saleId);
            
            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns((Sale)null);

            // When
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {saleId} not found");
            
            await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid sale retrieval request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given empty sale ID When retrieving sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Given
            var command = new GetSaleCommand(Guid.Empty);

            // When
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }
    }
} 