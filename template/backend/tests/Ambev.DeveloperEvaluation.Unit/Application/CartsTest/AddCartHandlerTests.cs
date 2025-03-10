using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Entities.Users;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartsTest;

/// <summary>
/// Contains unit tests for the <see cref="AddCartHandler"/> class.
/// </summary>
public class AddCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly AddCartHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddCartHandlerTests"/> class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public AddCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new AddCartHandler(_cartRepository, _userRepository, _unitOfWork, _mapper);
    }

    /// <summary>
    /// Tests that a valid AddCart command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid cart command When handling Then creates cart and returns result")]
    public async Task Handle_ValidCommand_ReturnsCartResult()
    {
        // Given
        var command = AddCartTestData.GenerateValidCommand();
        var cartResult = new CartResult { Id = Guid.NewGuid() };
        
        // Mock user
        var user = new User
        {
            Id = command.CostumerId,
            Username = "Test User",
            Email = "test@example.com"
        };
        
        _userRepository.GetByIdAsync(command.CostumerId, Arg.Any<CancellationToken>())
            .Returns(user);
            
        var cart = new Cart(
            DateTime.UtcNow,
            command.CostumerId,
            user.Username,
            command.BranchId,
            command.BranchName
        );
        
        _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cart));
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));
        _mapper.Map<CartResult>(Arg.Any<Cart>())
            .Returns(cartResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(cartResult);

        await _cartRepository.Received(1).CreateAsync(
            Arg.Is<Cart>(c => 
                c.CustomerExternalId == command.CostumerId && 
                c.CustomerName == user.Username &&  // Should use username from user repository
                c.BranchExternalId == command.BranchId &&
                c.BranchName == command.BranchName),
            Arg.Any<CancellationToken>());
        
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CartResult>(Arg.Any<Cart>());
    }

    /// <summary>
    /// Tests that products are added to the cart when handling a valid command.
    /// </summary>
    [Fact(DisplayName = "Given valid command with products When handling Then adds products to cart")]
    public async Task Handle_ValidCommandWithProducts_AddsProductsToCart()
    {
        // Given
        var command = AddCartTestData.GenerateValidCommand();
        var products = command.Products;
        var cartResult = new CartResult { Id = Guid.NewGuid() };
        
        // Mock user
        var user = new User
        {
            Id = command.CostumerId,
            Username = "Test User",
            Email = "test@example.com"
        };
        
        _userRepository.GetByIdAsync(command.CostumerId, Arg.Any<CancellationToken>())
            .Returns(user);
            
        var cart = new Cart(
            DateTime.UtcNow,
            command.CostumerId,
            user.Username,
            command.BranchId,
            command.BranchName
        );
        
        _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cart));
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));
        _mapper.Map<CartResult>(Arg.Any<Cart>())
            .Returns(cartResult);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _cartRepository.Received(1).CreateAsync(
            Arg.Is<Cart>(c => 
                c.Products.Count == products.Count &&
                c.Products.All(p => products.Any(cp => 
                    p.ProductExternalId == cp.Id && 
                    p.ProductName == cp.Name && 
                    p.Quantity == cp.Quantity && 
                    p.UnitPrice == cp.UnitPrice))),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid AddCart command with missing customer data throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid command with missing customer data When handling Then throws validation exception")]
    public async Task Handle_InvalidCommandWithMissingCustomerData_ThrowsValidationException()
    {
        // Given
        var command = AddCartTestData.GenerateInvalidCommandWithMissingCustomerData();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.ErrorMessage.Contains("Customer ID is required")));
        
        await _cartRepository.DidNotReceive().CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
    
    /// <summary>
    /// Tests that command with non-existent user ID throws a DomainException.
    /// </summary>
    [Fact(DisplayName = "Given command with non-existent user When handling Then throws DomainException")]
    public async Task Handle_NonExistentUser_ThrowsDomainException()
    {
        // Given
        var command = AddCartTestData.GenerateValidCommand();
        _userRepository.GetByIdAsync(command.CostumerId, Arg.Any<CancellationToken>())
            .Returns((User)null);
            
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);
        
        // Then
        await act.Should().ThrowAsync<DomainException>()
            .Where(ex => ex.Message.Contains($"User with ID {command.CostumerId} not found"));
            
        await _cartRepository.DidNotReceive().CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid AddCart command with missing branch data throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid command with missing branch data When handling Then throws validation exception")]
    public async Task Handle_InvalidCommandWithMissingBranchData_ThrowsValidationException()
    {
        // Given
        var command = AddCartTestData.GenerateInvalidCommandWithMissingBranchData();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.ErrorMessage.Contains("Branch ID is required") || 
                                          e.ErrorMessage.Contains("Branch name is required")));
        
        await _cartRepository.DidNotReceive().CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid AddCart command with a future date throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid command with future date When handling Then throws validation exception")]
    public async Task Handle_InvalidCommandWithFutureDate_ThrowsValidationException()
    {
        // Given
        var command = AddCartTestData.GenerateInvalidCommandWithFutureDate();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.ErrorMessage.Contains("Creation date cannot be in the future")));
        
        await _cartRepository.DidNotReceive().CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid AddCart command with invalid product data throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid command with invalid product data When handling Then throws validation exception")]
    public async Task Handle_InvalidCommandWithInvalidProductData_ThrowsValidationException()
    {
        // Given
        var command = AddCartTestData.GenerateInvalidCommandWithInvalidProductData();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Any(e => 
                e.ErrorMessage.Contains("Product ID is required") || 
                e.ErrorMessage.Contains("Product name is required") ||
                e.ErrorMessage.Contains("Quantity must be greater than zero") ||
                e.ErrorMessage.Contains("Unit price must be greater than zero")));
        
        await _cartRepository.DidNotReceive().CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
} 