using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;
using Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks.Product;

namespace Ambev.DeveloperEvaluation.Unit.Application.ProductsTest;

/// <summary>
/// Tests for the GetProductsHandler
/// </summary>
public class GetProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductsHandler _handler;

    /// <summary>
    /// Initializes a new instance of GetProductsHandlerTests
    /// </summary>
    public GetProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductsHandler(_productRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid command returns paginated products
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then returns paginated products")]
    public async Task Handle_ValidCommand_ReturnsPaginatedProducts()
    {
        // Arrange
        var command = GetProductsTestData.CreateValidCommand();
        var products = GetProductsTestData.CreateTestProducts();
        var totalCount = products.Count;

        _productRepository.GetAllPagedAsync(command.PageNumber, command.PageSize, Arg.Any<CancellationToken>())
            .Returns(products);
        _productRepository.CountAsync(Arg.Any<CancellationToken>())
            .Returns(totalCount);

        var productResults = products.Select(p => new ProductResult
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Sku = p.Sku,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            Category = p.Category,
            BranchExternalId = p.BranchExternalId,
            BranchName = p.BranchName,
            Status = p.Status.ToString(),
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        _mapper.Map<List<ProductResult>>(Arg.Any<List<Ambev.DeveloperEvaluation.Domain.Entities.Products.Product>>())
            .Returns(productResults);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(products.Count);
        result.TotalCount.Should().Be(totalCount);
        result.PageNumber.Should().Be(command.PageNumber);
        result.PageSize.Should().Be(command.PageSize);
        result.TotalPages.Should().Be((int)Math.Ceiling(totalCount / (double)command.PageSize));
    }

    /// <summary>
    /// Tests that an invalid command with negative page number throws validation exception
    /// </summary>
    [Fact(DisplayName = "Given invalid command with negative page number When handling Then throws validation exception")]
    public async Task Handle_InvalidCommandWithNegativePageNumber_ThrowsValidationException()
    {
        // Arrange
        var command = GetProductsTestData.CreateInvalidCommandWithNegativePageNumber();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests that an invalid command with negative page size throws validation exception
    /// </summary>
    [Fact(DisplayName = "Given invalid command with negative page size When handling Then throws validation exception")]
    public async Task Handle_InvalidCommandWithNegativePageSize_ThrowsValidationException()
    {
        // Arrange
        var command = GetProductsTestData.CreateInvalidCommandWithNegativePageSize();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests that an invalid command with excessive page size throws validation exception
    /// </summary>
    [Fact(DisplayName = "Given invalid command with excessive page size When handling Then throws validation exception")]
    public async Task Handle_InvalidCommandWithExcessivePageSize_ThrowsValidationException()
    {
        // Arrange
        var command = GetProductsTestData.CreateInvalidCommandWithExcessivePageSize();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
} 