using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.ProductTest;

/// <summary>
/// Contains unit tests for the <see cref="GetProductHandler"/> class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductHandlerTests"/> class.
    /// Sets up the test dependencies.
    /// </summary>
    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductHandler(_productRepository, _mapper);
    }

    /// <summary>
    /// Tests that when a product exists, it is returned successfully.
    /// </summary>
    [Fact(DisplayName = "Given existing product ID When getting product Then returns product data")]
    public async Task Handle_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var query = new GetProductQuery { Id = productId };
        
        var product = new Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Sku = "SKU-TEST123",
            Price = 19.99m,
            StockQuantity = 10,
            Category = "Test Category",
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
        
        var expectedResult = new GetProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Sku = product.Sku,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Category = product.Category,
            Status = product.Status.ToString(),
            CreatedAt = product.CreatedAt
        };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Name.Should().Be(product.Name);
        result.Sku.Should().Be(product.Sku);
        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when a product does not exist, null is returned.
    /// </summary>
    [Fact(DisplayName = "Given non-existent product ID When getting product Then returns null")]
    public async Task Handle_NonExistentProduct_ReturnsNull()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var query = new GetProductQuery { Id = productId };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }
} 