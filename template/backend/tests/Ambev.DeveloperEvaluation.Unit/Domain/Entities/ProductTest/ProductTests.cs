using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Mocks.ProductMock;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.ProductTest;

/// <summary>
/// Contains unit tests for the Product entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Tests that when a discontinued product is activated, its status changes to Active.
    /// </summary>
    [Fact(DisplayName = "Product status should change to Active when activated")]
    public void Given_DiscontinuedProduct_When_Activated_Then_StatusShouldBeActive()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Status = ProductStatus.Discontinued;

        // Act
        product.Activate();

        // Assert
        Assert.Equal(ProductStatus.Active, product.Status);
        Assert.NotNull(product.UpdatedAt);
    }

    /// <summary>
    /// Tests that when an active product is discontinued, its status changes to Discontinued.
    /// </summary>
    [Fact(DisplayName = "Product status should change to Discontinued when discontinued")]
    public void Given_ActiveProduct_When_Discontinued_Then_StatusShouldBeDiscontinued()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Status = ProductStatus.Active;

        // Act
        product.Discontinue();

        // Assert
        Assert.Equal(ProductStatus.Discontinued, product.Status);
        Assert.NotNull(product.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdateStock method updates the stock quantity and updatedAt date.
    /// </summary>
    [Fact(DisplayName = "UpdateStock should update stock quantity and updatedAt")]
    public void Given_Product_When_UpdateStock_Then_StockQuantityShouldBeUpdated()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var originalStock = product.StockQuantity;
        var newStock = originalStock + 10;
        product.UpdatedAt = null; // Reset to ensure we can detect the update

        // Act
        product.UpdateStock(newStock);

        // Assert
        Assert.Equal(newStock, product.StockQuantity);
        Assert.NotNull(product.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdateStock throws an exception when a negative quantity is provided.
    /// </summary>
    [Fact(DisplayName = "UpdateStock should throw exception for negative quantity")]
    public void Given_Product_When_UpdateStockWithNegativeValue_Then_ShouldThrowException()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => product.UpdateStock(-10));
        Assert.Contains("cannot be negative", exception.Message);
    }

    /// <summary>
    /// Tests that a newly created product has Active status by default.
    /// </summary>
    [Fact(DisplayName = "New product should have Active status by default")]
    public void Given_NewProduct_When_Created_Then_StatusShouldBeActive()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Equal(ProductStatus.Active, product.Status);
    }

    /// <summary>
    /// Tests that a newly created product has CreatedAt date set.
    /// </summary>
    [Fact(DisplayName = "New product should have CreatedAt date set")]
    public void Given_NewProduct_When_Created_Then_CreatedAtShouldBeSet()
    {
        // Act
        var product = new Product();
        var now = DateTime.UtcNow;

        // Assert
        Assert.NotEqual(default(DateTime), product.CreatedAt);
        Assert.True(now >= product.CreatedAt);
        Assert.True(now.AddSeconds(-5) <= product.CreatedAt); // Allow 5 seconds tolerance
    }
} 