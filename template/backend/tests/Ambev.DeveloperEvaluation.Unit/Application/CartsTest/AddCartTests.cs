using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartsTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="AddCartHandler"/> class.
    /// </summary>
    public class AddCartHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AddCartHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCartHandlerTests"/> class.
        /// Sets up the test dependencies.
        /// </summary>
        public AddCartHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new AddCartHandler(_cartRepository, _unitOfWork, _mapper);
        }

        /// <summary>
        /// Tests that a valid cart creation request with no products is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid cart data with no products When creating cart Then returns success response")]
        public async Task Handle_ValidRequestNoProducts_ReturnsSuccessResponse()
        {
            // Arrange
            var command = AddCartTestData.GenerateValidCommand();

            // Create a cart instance that will be returned by the repository
            var cart = new Cart(
                command.CreatedOn,
                command.CostumerId,
                command.CostumerName,
                command.BranchId,
                command.BranchName
            );

            // Create a result that will be returned by the mapper
            var result = new CartResult
            {
                Id = cart.Id,
                CustomerExternalId = command.CostumerId.ToString(),
                CustomerName = command.CostumerName,
                BranchExternalId = command.BranchId.ToString(),
                BranchName = command.BranchName,
                Products = new List<CartProductResult>()
            };

            // Configure mocks
            _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
                .Returns(cart);

            _mapper.Map<CartResult>(Arg.Any<Cart>())
                .Returns(result);

            // Act
            var cartResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            cartResult.Should().NotBeNull();
            cartResult.Id.Should().Be(cart.Id);
            cartResult.CustomerExternalId.Should().Be(command.CostumerId.ToString());
            cartResult.CustomerName.Should().Be(command.CostumerName);
            cartResult.BranchExternalId.Should().Be(command.BranchId.ToString());
            cartResult.BranchName.Should().Be(command.BranchName);
            cartResult.Products.Should().BeEmpty();

            await _cartRepository.Received(1).CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that a valid cart creation request with products is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid cart data with products When creating cart Then returns success response")]
        public async Task Handle_ValidRequestWithProducts_ReturnsSuccessResponse()
        {
            // Arrange
            var command = AddCartTestData.GenerateValidCommandWithProducts();

            // Create a cart instance that will be returned by the repository
            var cart = new Cart(
                command.CreatedOn,
                command.CostumerId,
                command.CostumerName,
                command.BranchId,
                command.BranchName
            );

            // Add products to the cart
            foreach (var product in command.Products)
            {
                cart.AddProduct(
                    product.Id,
                    product.Name,
                    product.Quantity,
                    product.UnitPrice
                );
            }

            // Create a result that will be returned by the mapper
            var result = new CartResult
            {
                Id = cart.Id,
                CustomerExternalId = command.CostumerId.ToString(),
                CustomerName = command.CostumerName,
                BranchExternalId = command.BranchId.ToString(),
                BranchName = command.BranchName,
                Products = new List<CartProductResult>()
            };

            // Configure mocks
            _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
                .Returns(cart);

            _mapper.Map<CartResult>(Arg.Any<Cart>())
                .Returns(result);

            // Act
            var cartResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            cartResult.Should().NotBeNull();
            cartResult.Id.Should().Be(cart.Id);
            cartResult.CustomerExternalId.Should().Be(command.CostumerId.ToString());
            cartResult.CustomerName.Should().Be(command.CostumerName);
            cartResult.BranchExternalId.Should().Be(command.BranchId.ToString());
            cartResult.BranchName.Should().Be(command.BranchName);

            await _cartRepository.Received(1).CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid cart creation request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given invalid cart data When creating cart Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = AddCartTestData.GenerateInvalidCommand();

            // Act & Assert
            var act = () => _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<ValidationException>();
        }

        /// <summary>
        /// Tests that products are correctly added to the cart entity.
        /// </summary>
        [Fact(DisplayName = "Given cart with products When handling Then products are added correctly")]
        public async Task Handle_RequestWithProducts_AddsProductsCorrectly()
        {
            // Arrange
            var command = AddCartTestData.GenerateValidCommandWithProducts(3);
            Cart capturedCart = null;

            _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
                .Returns(callInfo => {
                    capturedCart = callInfo.Arg<Cart>();
                    return capturedCart;
                });

            _mapper.Map<CartResult>(Arg.Any<Cart>())
                .Returns(new CartResult());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedCart.Should().NotBeNull();
            capturedCart.Products.Should().HaveCount(command.Products.Count);

            foreach (var product in command.Products)
            {
                capturedCart.Products.Should().Contain(p =>
                    p.ProductExternalId == product.Id &&
                    p.ProductName == product.Name &&
                    p.Quantity == product.Quantity &&
                    p.UnitPrice == product.UnitPrice);
            }

            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that the mapper is called to convert the entity to result.
        /// </summary>
        [Fact(DisplayName = "Given valid request When handling Then maps entity to result")]
        public async Task Handle_ValidRequest_MapsEntityToResult()
        {
            // Arrange
            var command = AddCartTestData.GenerateValidCommand();

            var cart = new Cart(
                command.CreatedOn,
                command.CostumerId,
                command.CostumerName,
                command.BranchId,
                command.BranchName
            );

            _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
                .Returns(cart);

            _mapper.Map<CartResult>(Arg.Any<Cart>())
                .Returns(new CartResult());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapper.Received(1).Map<CartResult>(Arg.Is<Cart>(c =>
                c.CustomerExternalId == command.CostumerId &&
                c.CustomerName == command.CostumerName &&
                c.BranchExternalId == command.BranchId &&
                c.BranchName == command.BranchName
            ));
        }
    }
}