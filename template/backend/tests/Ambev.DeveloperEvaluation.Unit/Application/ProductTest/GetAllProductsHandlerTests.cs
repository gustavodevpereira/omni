using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.ProductTest;

/// <summary>
/// Contains unit tests for the <see cref="GetAllProductsHandler"/> class.
/// </summary>
public class GetAllProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetAllProductsHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllProductsHandlerTests"/> class.
    /// Sets up the test dependencies.
    /// </summary>
    public GetAllProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllProductsHandler(_productRepository, _mapper);
    }

    /// <summary>
    /// Tests that when products exist, they are returned successfully.
    /// </summary>
    [Fact(DisplayName = "Given existing products When getting all products Then returns all products")]
    public async Task Handle_ExistingProducts_ReturnsAllProducts()
    {
        // Arrange
        var query = new GetAllProductsQuery();
        
        var products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Sku = "SKU-001",
                Price = 10.99m,
                StockQuantity = 50,
                Category = "Category 1",
                Status = ProductStatus.Active
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Sku = "SKU-002",
                Price = 20.99m,
                StockQuantity = 25,
                Category = "Category 2",
                Status = ProductStatus.Discontinued
            }
        };
        
        var expectedResults = products.Select(p => new GetAllProductsResult
        {
            Id = p.Id,
            Name = p.Name,
            Sku = p.Sku,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            Category = p.Category,
            Status = p.Status.ToString()
        }).ToList();

        _productRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(products);
        _mapper.Map<List<GetAllProductsResult>>(products)
            .Returns(expectedResults);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(products.Count);
        result.Should().BeEquivalentTo(expectedResults);
    }
} 