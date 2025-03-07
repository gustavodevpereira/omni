using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Mocks.CartMock;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.CartTest;

/// <summary>
/// Contains unit tests for the Cart entity class.
/// Tests cover cart creation, validation, item management, and cart status operations.
/// </summary>
public class CartTests
{
    /// <summary>
    /// Tests that a cart is properly initialized with correct properties.
    /// </summary>
    [Fact(DisplayName = "Cart should be initialized with correct properties")]
    public void Given_ValidData_When_CartIsCreated_Then_PropertiesShouldBeCorrectlyInitialized()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var customerExternalId = Guid.NewGuid();
        var customerName = "Test Customer";
        var branchExternalId = Guid.NewGuid();
        var branchName = "Test Branch";

        // Act
        var cart = new Cart(now, customerExternalId, customerName, branchExternalId, branchName);

        // Assert
        cart.CreatedOn.Should().Be(now);
        cart.CustomerExternalId.Should().Be(customerExternalId);
        cart.CustomerName.Should().Be(customerName);
        cart.BranchExternalId.Should().Be(branchExternalId);
        cart.BranchName.Should().Be(branchName);
        cart.Status.Should().Be(CartStatus.Active);
        cart.Products.Should().BeEmpty();
        cart.TotalAmount.Should().Be(0);
    }

    /// <summary>
    /// Tests that a cart can be validated successfully.
    /// </summary>
    [Fact(DisplayName = "Valid cart should pass validation")]
    public void Given_ValidCart_When_Validate_Then_ValidationShouldPass()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Act
        var validationResult = cart.Validate();

        // Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that a product can be added to the cart successfully.
    /// </summary>
    [Fact(DisplayName = "Product should be added to cart successfully")]
    public void Given_ValidProduct_When_AddProduct_Then_ProductShouldBeAdded()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 1;
        var unitPrice = 10.0m;

        // Act
        cart.AddProduct(productExternalId, productName, quantity, unitPrice);

        // Assert
        cart.Products.Should().HaveCount(1);
        var product = cart.Products.First();
        product.ProductExternalId.Should().Be(productExternalId);
        product.ProductName.Should().Be(productName);
        product.Quantity.Should().Be(quantity);
        product.UnitPrice.Should().Be(unitPrice);
        product.DiscountPercentage.Should().Be(0); // No discount for quantity < 4
        product.TotalAmount.Should().Be(quantity * unitPrice);
        cart.TotalAmount.Should().Be(quantity * unitPrice);
    }

    /// <summary>
    /// Tests that a product with 5 items receives a 10% discount.
    /// </summary>
    [Fact(DisplayName = "Product with 5 items should have 10% discount")]
    public void Given_ProductWith5Items_When_AddProduct_Then_ShouldApply10PercentDiscount()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 5;
        var unitPrice = 10.0m;
        var expectedDiscount = 0.1m; // 10%
        var expectedTotal = quantity * unitPrice * (1 - expectedDiscount);

        // Act
        cart.AddProduct(productExternalId, productName, quantity, unitPrice);

        // Assert
        cart.Products.Should().HaveCount(1);
        var product = cart.Products.First();
        product.Quantity.Should().Be(quantity);
        product.DiscountPercentage.Should().Be(expectedDiscount);
        product.TotalAmount.Should().Be(expectedTotal);
        cart.TotalAmount.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests that a product with 15 items receives a 20% discount.
    /// </summary>
    [Fact(DisplayName = "Product with 15 items should have 20% discount")]
    public void Given_ProductWith15Items_When_AddProduct_Then_ShouldApply20PercentDiscount()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 15;
        var unitPrice = 10.0m;
        var expectedDiscount = 0.2m; // 20%
        var expectedTotal = quantity * unitPrice * (1 - expectedDiscount);

        // Act
        cart.AddProduct(productExternalId, productName, quantity, unitPrice);

        // Assert
        cart.Products.Should().HaveCount(1);
        var product = cart.Products.First();
        product.Quantity.Should().Be(quantity);
        product.DiscountPercentage.Should().Be(expectedDiscount);
        product.TotalAmount.Should().Be(expectedTotal);
        cart.TotalAmount.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests that adding more than 20 items throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding more than 20 items should throw exception")]
    public void Given_ProductWithMoreThan20Items_When_AddProduct_Then_ShouldThrowException()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 21;
        var unitPrice = 10.0m;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            cart.AddProduct(productExternalId, productName, quantity, unitPrice));
        exception.Message.Should().Contain("between 1 and 20");
    }

    /// <summary>
    /// Tests that adding less than 1 item throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding less than 1 item should throw exception")]
    public void Given_ProductWithZeroItems_When_AddProduct_Then_ShouldThrowException()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 0;
        var unitPrice = 10.0m;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            cart.AddProduct(productExternalId, productName, quantity, unitPrice));
        exception.Message.Should().Contain("between 1 and 20");
    }

    /// <summary>
    /// Tests that multiple products can be added and the total is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Multiple products should be added and total calculated correctly")]
    public void Given_MultipleProducts_When_Added_Then_TotalAmountShouldBeCorrect()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Product 1: 2 items, no discount
        var product1ExternalId = Guid.NewGuid();
        var product1Name = "Product 1";
        var product1Quantity = 2;
        var product1UnitPrice = 10.0m;
        var product1Total = product1Quantity * product1UnitPrice;

        // Product 2: 5 items, 10% discount
        var product2ExternalId = Guid.NewGuid();
        var product2Name = "Product 2";
        var product2Quantity = 5;
        var product2UnitPrice = 20.0m;
        var product2Discount = 0.1m;
        var product2Total = product2Quantity * product2UnitPrice * (1 - product2Discount);

        // Product 3: 15 items, 20% discount
        var product3ExternalId = Guid.NewGuid();
        var product3Name = "Product 3";
        var product3Quantity = 15;
        var product3UnitPrice = 5.0m;
        var product3Discount = 0.2m;
        var product3Total = product3Quantity * product3UnitPrice * (1 - product3Discount);

        var expectedTotal = product1Total + product2Total + product3Total;

        // Act
        cart.AddProduct(product1ExternalId, product1Name, product1Quantity, product1UnitPrice);
        cart.AddProduct(product2ExternalId, product2Name, product2Quantity, product2UnitPrice);
        cart.AddProduct(product3ExternalId, product3Name, product3Quantity, product3UnitPrice);

        // Assert
        cart.Products.Should().HaveCount(3);
        cart.TotalAmount.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests that a product can be removed from the cart.
    /// </summary>
    [Fact(DisplayName = "Product should be removed from cart successfully")]
    public void Given_CartWithProduct_When_RemoveItem_Then_ProductShouldBeRemoved()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 1;
        var unitPrice = 10.0m;

        cart.AddProduct(productExternalId, productName, quantity, unitPrice);
        var productId = cart.Products.First().Id;

        // Act
        cart.RemoveItem(productId);

        // Assert
        cart.Products.Should().BeEmpty();
        cart.TotalAmount.Should().Be(0);
    }

    /// <summary>
    /// Tests that trying to remove a non-existent product throws an exception.
    /// </summary>
    [Fact(DisplayName = "Removing non-existent product should throw exception")]
    public void Given_CartWithoutProduct_When_RemoveNonExistentItem_Then_ShouldThrowException()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var nonExistentProductId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => cart.RemoveItem(nonExistentProductId));
        exception.Message.Should().Contain("not found");
    }

    /// <summary>
    /// Tests that a cart can be cancelled.
    /// </summary>
    [Fact(DisplayName = "Cart should be cancelled successfully")]
    public void Given_ActiveCart_When_CancelCart_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Act
        cart.CancelCart();

        // Assert
        cart.Status.Should().Be(CartStatus.Cancelled);
    }

    /// <summary>
    /// Tests that adding a product to a cancelled cart throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding product to cancelled cart should throw exception")]
    public void Given_CancelledCart_When_AddProduct_Then_ShouldThrowException()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.CancelCart();

        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 1;
        var unitPrice = 10.0m;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            cart.AddProduct(productExternalId, productName, quantity, unitPrice));
        exception.Message.Should().Contain("cancelled cart");
    }

    /// <summary>
    /// Tests that removing a product from a cancelled cart throws an exception.
    /// </summary>
    [Fact(DisplayName = "Removing product from cancelled cart should throw exception")]
    public void Given_CancelledCart_When_RemoveItem_Then_ShouldThrowException()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var productExternalId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 1;
        var unitPrice = 10.0m;

        cart.AddProduct(productExternalId, productName, quantity, unitPrice);
        var productId = cart.Products.First().Id;
        cart.CancelCart();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => cart.RemoveItem(productId));
        exception.Message.Should().Contain("cancelled cart");
    }
}