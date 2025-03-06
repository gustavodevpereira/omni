using Xunit;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.SalesTest;

/// <summary>
/// Unit tests to verify correct discount and total calculations for SaleItems based on quantity.
/// </summary>
public class SaleDiscountTests
{
    /// <summary>
    /// Tests creating a SaleItem with valid quantities and verifies the discount and total calculations.
    /// Various quantities are tested to validate the discount rules.
    /// </summary>
    /// <param name="quantity">The quantity for the sale item.</param>
    [Theory(DisplayName = "Should create SaleItem with correct discount and total for valid quantities")]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void CreateSaleItem_WithValidQuantity_ShouldCalculateDiscountAndTotal(int quantity)
    {
        // Arrange
        const decimal unitPrice = 100m;
        var sale = SaleTestData.GenerateValidSale();
        var productData = SaleTestData.GenerateProductData(quantity, unitPrice);

        // Act
        sale.AddItem(productData.productExternalId, productData.productName, productData.quantity, productData.unitPrice);
        var saleItem = sale.Items.First();

        // Calculate expected discount and total
        decimal expectedDiscount = new DiscountPolicy().GetDiscountPercentage(quantity);
        decimal expectedTotal = (quantity * unitPrice) - (quantity * unitPrice * expectedDiscount);

        // Assert
        Assert.Equal(expectedDiscount, saleItem.DiscountPercentage);
        Assert.Equal(expectedTotal, saleItem.TotalAmount);
        Assert.Equal(expectedTotal, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that adding a SaleItem with an invalid quantity throws a DomainException.
    /// Quantities outside the allowed range (1 to 20) should cause an error.
    /// </summary>
    /// <param name="quantity">The invalid quantity for the sale item.</param>
    [Theory(DisplayName = "Should throw DomainException for invalid quantity")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(21)]
    [InlineData(25)]
    public void CreateSaleItem_WithInvalidQuantity_ShouldThrowDomainException(int quantity)
    {
        // Arrange
        const decimal unitPrice = 100m;
        var sale = SaleTestData.GenerateValidSale();

        // Act & Assert: Expect a DomainException when adding an item with an invalid quantity.
        Assert.Throws<DomainException>(() =>
            sale.AddItem("TestProductId", "TestProduct", quantity, unitPrice)
        );
    }
}
