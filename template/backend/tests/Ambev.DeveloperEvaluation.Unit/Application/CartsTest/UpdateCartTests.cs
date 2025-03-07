using Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Common.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartsTest;

/// <summary>
/// Contains unit tests for the <see cref="UpdateCartCommandHandler"/> class.
/// </summary>
public class UpdateCartCommandHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly UpdateCartCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCartCommandHandlerTests"/> class.
    /// Sets up the test dependencies.
    /// </summary>
    public UpdateCartCommandHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateCartCommandHandler(_cartRepository, _mapper);
    }

    /// <summary>
    /// Tests that updating a cart with valid data returns success.
    /// </summary>
    [Fact(DisplayName = "Given valid cart data When updating cart Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var command = UpdateCartTestData.GenerateValidCommand();

        // Create an existing cart to be updated
        var existingCart = new Cart(
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Original Customer",
            Guid.NewGuid(),
            "Original Branch"
        );

        // Setup repository mock to return the existing cart
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCart);

        // Create updated cart that will be returned by the repository
        var updatedCart = new Cart(
            existingCart.CreatedOn,
            command.CustomerExternalId,
            command.CustomerName,
            command.BranchExternalId,
            command.BranchName
        );

        // Add products to the updated cart
        foreach (var product in command.Products)
        {
            updatedCart.AddProduct(
                product.ProductExternalId,
                product.ProductName,
                product.Quantity,
                product.UnitPrice
            );
        }

        // Setup mapper to return the result
        var result = new UpdateCartResult
        {
            Id = updatedCart.Id,
            CustomerExternalId = command.CustomerExternalId,
            CustomerName = command.CustomerName,
            BranchExternalId = command.BranchExternalId,
            BranchName = command.BranchName,
            Status = CartStatus.Active.ToString(),
            Products = updatedCart.Products.Select(p => new UpdateCartProductResult
            {
                Id = p.Id,
                ProductExternalId = p.ProductExternalId,
                ProductName = p.ProductName,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                DiscountPercentage = p.DiscountPercentage,
                TotalAmount = p.TotalAmount
            }).ToList()
        };

        _mapper.Map<UpdateCartResult>(Arg.Any<Cart>())
            .Returns(result);

        // Act
        var cartResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        cartResult.Should().NotBeNull();
        cartResult.Id.Should().Be(result.Id);
        cartResult.CustomerExternalId.Should().Be(command.CustomerExternalId);
        cartResult.CustomerName.Should().Be(command.CustomerName);
        cartResult.BranchExternalId.Should().Be(command.BranchExternalId);
        cartResult.BranchName.Should().Be(command.BranchName);
        cartResult.Products.Should().HaveCount(command.Products.Count);

        await _cartRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that updating a non-existent cart throws NotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent cart ID When updating cart Then throws NotFoundException")]
    public async Task Handle_NonExistentCart_ThrowsNotFoundException()
    {
        // Arrange
        var command = UpdateCartTestData.GenerateValidCommand();

        // Setup repository to return null (cart not found)
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Cart)null);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Cart with ID {command.Id} not found");
    }

    /// <summary>
    /// Tests that updating a cancelled cart throws DomainException.
    /// </summary>
    [Fact(DisplayName = "Given cancelled cart When updating Then throws DomainException")]
    public async Task Handle_CancelledCart_ThrowsDomainException()
    {
        // Arrange
        var command = UpdateCartTestData.GenerateValidCommand();

        // Create an existing cancelled cart
        var existingCart = new Cart(
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Original Customer",
            Guid.NewGuid(),
            "Original Branch"
        );
        existingCart.CancelCart(); // Cancel the cart

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCart);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Cannot add or remove items from a cancelled cart.");
    }

    /// <summary>
    /// Tests that products are correctly updated in the cart.
    /// </summary>
    [Fact(DisplayName = "Given cart update with products When handling Then products are updated correctly")]
    public async Task Handle_RequestWithProducts_UpdatesProductsCorrectly()
    {
        // Arrange
        var command = UpdateCartTestData.GenerateValidCommand();

        // Create an existing cart with products
        var existingCart = new Cart(
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Original Customer",
            Guid.NewGuid(),
            "Original Branch"
        );

        // Add some initial products
        existingCart.AddProduct(Guid.NewGuid(), "Initial Product", 5, 10.0m);

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCart);

        // Capture the updated cart
        Cart capturedCart = null;
        await _cartRepository.UpdateAsync(Arg.Do<Cart>(c => capturedCart = c), Arg.Any<CancellationToken>());

        _mapper.Map<UpdateCartResult>(Arg.Any<Cart>())
            .Returns(new UpdateCartResult());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedCart.Should().NotBeNull();
        capturedCart.Products.Should().HaveCount(command.Products.Count);

        foreach (var expectedProduct in command.Products)
        {
            capturedCart.Products.Should().Contain(p =>
                p.ProductExternalId == expectedProduct.ProductExternalId &&
                p.ProductName == expectedProduct.ProductName &&
                p.Quantity == expectedProduct.Quantity &&
                p.UnitPrice == expectedProduct.UnitPrice);
        }
    }
}
