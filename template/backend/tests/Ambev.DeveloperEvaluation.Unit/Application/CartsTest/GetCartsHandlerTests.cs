using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartsTest;

/// <summary>
/// Contains unit tests for the <see cref="GetCartsHandler"/> class.
/// </summary>
public class GetCartsHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly GetCartsHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCartsHandlerTests"/> class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public GetCartsHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetCartsHandler(_cartRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid GetCarts command with a customer filter returns filtered paginated carts.
    /// </summary>
    [Fact(DisplayName = "Given valid command with customer When handling Then returns filtered paginated carts")]
    public async Task Handle_ValidCommandWithCustomer_ReturnsFilteredPaginatedCarts()
    {
        // Given
        var command = GetCartsTestData.GenerateValidCommandWithCustomer();
        var carts = new List<Cart> 
        { 
            new Cart(
                DateTime.UtcNow,
                command.CustomerId,
                "Customer Name",
                Guid.NewGuid(),
                "Branch 1"
            ),
            new Cart(
                DateTime.UtcNow,
                command.CustomerId,
                "Customer Name",
                Guid.NewGuid(),
                "Branch 2"
            )
        };
        var cartResults = new List<CartResult> { new CartResult(), new CartResult() };
        const int totalCount = 5;

        _cartRepository.GetAllPagedByCustomerAsync(command.CustomerId, command.PageNumber, command.PageSize, Arg.Any<CancellationToken>())
            .Returns(carts);
        _cartRepository.CountByCustomerAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(totalCount);
        _mapper.Map<List<CartResult>>(carts)
            .Returns(cartResults);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(cartResults);
        result.TotalCount.Should().Be(totalCount);
        result.PageNumber.Should().Be(command.PageNumber);
        result.PageSize.Should().Be(command.PageSize);
        
        await _cartRepository.Received(1).GetAllPagedByCustomerAsync(
            command.CustomerId, command.PageNumber, command.PageSize, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).CountByCustomerAsync(command.CustomerId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<CartResult>>(carts);
    }
} 