using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks.Product;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.ProductTest;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// Sets up the test dependencies.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _uow = Substitute.For<IUnitOfWork>();
        _handler = new CreateProductHandler(_productRepository, _mapper, _uow);
    }

    /// <summary>
    /// Tests that a valid product creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When creating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Sku = command.Sku,
            Price = command.Price,
            StockQuantity = command.StockQuantity,
            Category = command.Category,
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var result = new CreateProductResult
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

        _mapper.Map<Product>(command).Returns(product);
        _mapper.Map<CreateProductResult>(product).Returns(result);

        _productRepository.GetBySkuAsync(command.Sku, Arg.Any<CancellationToken>())
            .Returns((Product?)null);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // Act
        var createProductResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        createProductResult.Should().NotBeNull();
        createProductResult.Id.Should().Be(product.Id);
        await _productRepository.Received(1).GetBySkuAsync(command.Sku, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid product creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When creating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateProductCommand(); // Empty command will fail validation

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that an attempt to create a product with an existing SKU throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given existing SKU When creating product Then throws validation exception")]
    public async Task Handle_ExistingSku_ThrowsValidationException()
    {
        // Arrange
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var existingProduct = new Product
        {
            Id = Guid.NewGuid(),
            Sku = command.Sku,
            Name = "Existing Product"
        };

        _productRepository.GetBySkuAsync(command.Sku, Arg.Any<CancellationToken>())
            .Returns(existingProduct);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Message.Contains("SKU already exists"));
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to product entity")]
    public async Task Handle_ValidRequest_MapsCommandToProduct()
    {
        // Arrange
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Sku = command.Sku,
            Price = command.Price,
            StockQuantity = command.StockQuantity,
            Category = command.Category,
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.GetBySkuAsync(command.Sku, Arg.Any<CancellationToken>())
            .Returns((Product?)null);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<Product>(Arg.Is<CreateProductCommand>(c =>
            c.Name == command.Name &&
            c.Description == command.Description &&
            c.Sku == command.Sku &&
            c.Price == command.Price &&
            c.StockQuantity == command.StockQuantity &&
            c.Category == command.Category));
    }
} 