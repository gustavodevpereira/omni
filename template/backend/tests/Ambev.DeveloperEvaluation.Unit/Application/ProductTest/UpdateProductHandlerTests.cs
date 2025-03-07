using Ambev.DeveloperEvaluation.Application.Common.Exceptions;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
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
/// Contains unit tests for the <see cref="UpdateProductHandler"/> class.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly UpdateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductHandlerTests"/> class.
    /// Sets up the test dependencies.
    /// </summary>
    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _uow = Substitute.For<IUnitOfWork>();
        _handler = new UpdateProductHandler(_productRepository, _mapper, _uow);
    }

    /// <summary>
    /// Tests that a valid product update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When updating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var existingProduct = new Product
        {
            Id = command.Id,
            Name = "Old Name",
            Description = "Old Description",
            Sku = "SKU-OLD12345",
            Price = 9.99m,
            StockQuantity = 5,
            Category = "Old Category",
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };

        var updatedProduct = new Product
        {
            Id = command.Id,
            Name = command.Name,
            Description = command.Description,
            Sku = "SKU-OLD12345", // SKU should not be changed
            Price = command.Price,
            StockQuantity = command.StockQuantity,
            Category = command.Category,
            Status = command.Status == "Active" ? ProductStatus.Active : ProductStatus.Discontinued,
            CreatedAt = existingProduct.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var result = new UpdateProductResult
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Sku = updatedProduct.Sku,
            Price = updatedProduct.Price,
            StockQuantity = updatedProduct.StockQuantity,
            Category = updatedProduct.Category,
            Status = updatedProduct.Status.ToString(),
            CreatedAt = updatedProduct.CreatedAt,
            UpdatedAt = updatedProduct.UpdatedAt
        };

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(updatedProduct);
        _mapper.Map<UpdateProductResult>(updatedProduct).Returns(result);

        // Act
        var updateProductResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        updateProductResult.Should().NotBeNull();
        updateProductResult.Id.Should().Be(command.Id);
        updateProductResult.Name.Should().Be(command.Name);
        updateProductResult.Description.Should().Be(command.Description);
        updateProductResult.Price.Should().Be(command.Price);
        updateProductResult.StockQuantity.Should().Be(command.StockQuantity);
        updateProductResult.Category.Should().Be(command.Category);
        updateProductResult.Status.Should().Be(command.Status);
        await _productRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid product update request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When updating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "", // Invalid: empty name
            Price = -10 // Invalid: negative price
        };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that an attempt to update a non-existent product throws a NotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent product ID When updating product Then throws NotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsNotFoundException()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(ex => ex.Message.Contains($"ID {command.Id}"));
    }

    /// <summary>
    /// Tests that when updating a product's status from Active to Discontinued, 
    /// the Discontinue method is called.
    /// </summary>
    [Fact(DisplayName = "Given status change to Discontinued When updating product Then calls Discontinue method")]
    public async Task Handle_StatusChangeToDiscontinued_CallsDiscontinueMethod()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        command.Status = "Discontinued";

        var existingProduct = new Product
        {
            Id = command.Id,
            Name = "Test Product",
            Description = "Test Description",
            Sku = "SKU-TEST123",
            Price = 19.99m,
            StockQuantity = 10,
            Category = "Test Category",
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        };

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(existingProduct);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<Product>(p => 
                p.Status == ProductStatus.Discontinued && 
                p.UpdatedAt != null),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when updating a product's status from Discontinued to Active, 
    /// the Activate method is called.
    /// </summary>
    [Fact(DisplayName = "Given status change to Active When updating product Then calls Activate method")]
    public async Task Handle_StatusChangeToActive_CallsActivateMethod()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        command.Status = "Active";

        var existingProduct = new Product
        {
            Id = command.Id,
            Name = "Test Product",
            Description = "Test Description",
            Sku = "SKU-TEST123",
            Price = 19.99m,
            StockQuantity = 10,
            Category = "Test Category",
            Status = ProductStatus.Discontinued,
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        };

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(existingProduct);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<Product>(p => 
                p.Status == ProductStatus.Active && 
                p.UpdatedAt != null),
            Arg.Any<CancellationToken>());
    }
} 