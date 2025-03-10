using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Entities.Users;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartsTest;

/// <summary>
/// Contains unit tests for the <see cref="CalculateCartDiscountHandler"/> class.
/// </summary>
public class CalculateCartDiscountHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly CalculateCartDiscountHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalculateCartDiscountHandlerTests"/> class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public CalculateCartDiscountHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new CalculateCartDiscountHandler(_userRepository);
    }

    /// <summary>
    /// Tests that a valid CalculateCartDiscount command is handled successfully
    /// and produces appropriate discount calculations.
    /// </summary>
    [Fact(DisplayName = "Given valid cart command When handling Then returns cart with correct discounts")]
    public async Task Handle_ValidCommand_ReturnsCartWithCorrectDiscounts()
    {
        // Given
        var command = CalculateCartDiscountTestData.GenerateCommandWithVariedDiscounts();
        
        // Setup mock user
        var mockUser = new User
        {
            Id = command.CustomerId,
            Username = "Test User",
            Email = "test@example.com"
        };
        
        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(mockUser);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.CustomerId.Should().Be(command.CustomerId);
        result.CustomerName.Should().Be(mockUser.Username); // Should use user from repository
        result.CustomerEmail.Should().Be(mockUser.Email); // Should use email from repository
        result.BranchExternalId.Should().Be(command.BranchExternalId);
        result.BranchName.Should().Be(command.BranchName);
        result.Date.Should().Be(command.Date);
        
        // Check that products are mapped correctly
        result.Products.Should().HaveCount(command.Products.Count);
        
        // Verify specific discount calculations
        var noDiscountProduct = result.Products.First(p => p.Name == "Product with no discount");
        noDiscountProduct.DiscountPercentage.Should().Be(0m);
        noDiscountProduct.Subtotal.Should().Be(20m); // 2 * 10
        noDiscountProduct.DiscountAmount.Should().Be(0m);
        noDiscountProduct.FinalAmount.Should().Be(20m);
        
        var tenPercentDiscountProduct = result.Products.First(p => p.Name == "Product with 10% discount");
        tenPercentDiscountProduct.DiscountPercentage.Should().Be(0.1m);
        tenPercentDiscountProduct.Subtotal.Should().Be(120m); // 6 * 20
        tenPercentDiscountProduct.DiscountAmount.Should().Be(12m); // 120 * 0.1
        tenPercentDiscountProduct.FinalAmount.Should().Be(108m); // 120 - 12
        
        var twentyPercentDiscountProduct = result.Products.First(p => p.Name == "Product with 20% discount");
        twentyPercentDiscountProduct.DiscountPercentage.Should().Be(0.2m);
        twentyPercentDiscountProduct.Subtotal.Should().Be(450m); // 15 * 30
        twentyPercentDiscountProduct.DiscountAmount.Should().Be(90m); // 450 * 0.2
        twentyPercentDiscountProduct.FinalAmount.Should().Be(360m); // 450 - 90

        // Verify cart totals
        result.TotalAmount.Should().Be(590m); // 20 + 120 + 450
        result.TotalDiscount.Should().Be(102m); // 0 + 12 + 90
        result.FinalAmount.Should().Be(488m); // 590 - 102
    }

    /// <summary>
    /// Tests that an invalid CalculateCartDiscount command with validation errors
    /// throws a ValidationException.
    /// </summary>
    [Fact(DisplayName = "Given invalid cart command When handling Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = CalculateCartDiscountTestData.GenerateInvalidCommandWithMissingCustomerData();
        
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// Tests that a command with non-existent user ID throws a DomainException.
    /// </summary>
    [Fact(DisplayName = "Given command with non-existent user When handling Then throws DomainException")]
    public async Task Handle_NonExistentUser_ThrowsDomainException()
    {
        // Given
        var command = CalculateCartDiscountTestData.GenerateValidCommand();
        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns((User)null);
            
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        
        // Then
        await act.Should().ThrowAsync<DomainException>()
            .Where(ex => ex.Message.Contains($"User with ID {command.CustomerId} not found"));
    }
} 