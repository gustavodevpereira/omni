using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.SalesTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="GetAllSalesHandler"/> class.
    /// </summary>
    public class GetAllSalesHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetAllSalesHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSalesHandlerTests"/> class.
        /// </summary>
        public GetAllSalesHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllSalesHandler(_saleRepository, _mapper);
        }

        /// <summary>
        /// Tests that a valid request for all sales returns the expected paginated sales.
        /// </summary>
        [Fact(DisplayName = "Given valid request When retrieving all sales Then returns paginated sales")]
        public async Task Handle_ValidRequest_ReturnsPaginatedSales()
        {
            // Given
            const int pageNumber = 1;
            const int pageSize = 10;
            const int totalCount = 25;
            
            var command = new GetAllSalesCommand(pageNumber, pageSize);
            
            var sales = CreateTestSales(pageSize);
            var salesDtos = sales.Select(s => new GetAllSalesResult.SaleDto 
            { 
                Id = s.Id,
                SaleNumber = s.SaleNumber,
                CustomerName = s.CustomerName,
                BranchName = s.BranchName,
                TotalAmount = s.TotalAmount,
                Status = s.Status.ToString(),
                ItemCount = s.Items.Count
            }).ToList();
            
            _saleRepository.GetAllPagedAsync(pageNumber, pageSize, Arg.Any<CancellationToken>())
                .Returns(sales);
                
            _saleRepository.CountAsync(Arg.Any<CancellationToken>())
                .Returns(totalCount);
                
            _mapper.Map<List<GetAllSalesResult.SaleDto>>(sales)
                .Returns(salesDtos);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Count.Should().Be(pageSize);
            result.TotalCount.Should().Be(totalCount);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalPages.Should().Be((int)Math.Ceiling(totalCount / (double)pageSize));
            
            await _saleRepository.Received(1).GetAllPagedAsync(pageNumber, pageSize, Arg.Any<CancellationToken>());
            await _saleRepository.Received(1).CountAsync(Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<List<GetAllSalesResult.SaleDto>>(sales);
        }

        /// <summary>
        /// Tests that an invalid request for all sales throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given invalid page parameters When retrieving all sales Then throws validation exception")]
        public async Task Handle_InvalidPageParameters_ThrowsValidationException()
        {
            // Given
            var command = new GetAllSalesCommand(0, 0);

            // When
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        /// <summary>
        /// Creates a list of test sales for use in tests.
        /// </summary>
        private IEnumerable<Sale> CreateTestSales(int count)
        {
            var sales = new List<Sale>();
            
            for (int i = 1; i <= count; i++)
            {
                var sale = new Sale(
                    $"SALE{i:000}",
                    DateTime.Now.AddDays(-i),
                    $"CUSTOMER{i:000}",
                    $"Customer {i}",
                    $"BRANCH{i:000}",
                    $"Branch {i}");
                    
                // Add a couple of items to each sale
                sale.AddItem($"PRODUCT{i:000}A", $"Product {i}A", 2, 10.0M);
                sale.AddItem($"PRODUCT{i:000}B", $"Product {i}B", 3, 15.0M);
                
                sales.Add(sale);
            }
            
            return sales;
        }
    }
} 