using Ambev.DeveloperEvaluation.Application.Carts.UseCases.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Common.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartsTest;

/// <summary>
/// Contains unit tests for the <see cref="DeleteCartCommandHandler"/> class.
/// </summary>
public class DeleteCartCommandHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly DeleteCartCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCartCommandHandlerTests"/> class.
    /// Sets up the test dependencies.
    /// </summary>
    public DeleteCartCommandHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _handler = new DeleteCartCommandHandler(_cartRepository);
    }

    /// <summary>
    /// Tests that deleting an existing cart cancels it and returns true.
    /// </summary>
    [Fact(DisplayName = "Given existing cart When deleting Then cancels the cart and returns true")]
    public async Task Handle_ExistingCart_CancelsCartAndReturnsTrue()
    {
        // Arrange
        var command = new DeleteCartCommand { Id = Guid.NewGuid() };

        // Create an existing cart
        var existingCart = new Cart(
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCart);

        // Capture the updated cart
        Cart capturedCart = null;
        await _cartRepository.UpdateAsync(Arg.Do<Cart>(c => capturedCart = c), Arg.Any<CancellationToken>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        capturedCart.Should().NotBeNull();
        capturedCart.Status.Should().Be(CartStatus.Cancelled);

        await _cartRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that deleting a non-existent cart throws NotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent cart ID When deleting Then throws NotFoundException")]
    public async Task Handle_NonExistentCart_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeleteCartCommand { Id = Guid.NewGuid() };

        // Setup repository to return null (cart not found)
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Cart)null);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Cart with ID {command.Id} not found");
    }

    /// <summary>
    /// Tests that deleting an already cancelled cart still returns true.
    /// </summary>
    [Fact(DisplayName = "Given already cancelled cart When deleting Then still returns true")]
    public async Task Handle_AlreadyCancelledCart_StillReturnsTrue()
    {
        // Arrange
        var command = new DeleteCartCommand { Id = Guid.NewGuid() };

        // Create an existing cart that is already cancelled
        var existingCart = new Cart(
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        existingCart.CancelCart(); // Cart is already cancelled

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCart);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        await _cartRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
    }
}
