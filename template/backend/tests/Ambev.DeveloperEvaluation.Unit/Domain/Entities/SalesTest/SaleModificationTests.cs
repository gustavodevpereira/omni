using System.Collections.ObjectModel;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;
using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.SalesTest;

/// <summary>
/// Unit tests for modifying a Sale, including adding/removing items and updating totals.
/// </summary>
public class SaleModificationTests
{
    /// <summary>
    /// Tests that adding a SaleItem increases the item count and updates the total amount.
    /// </summary>
    [Fact(DisplayName = "Should add SaleItem and update total amount")]
    public void AddSaleItem_ShouldIncreaseItemCountAndUpdateTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productData = SaleTestData.GenerateProductData(quantity: 5);
        decimal initialTotal = sale.TotalAmount;

        // Act
        sale.AddItem(productData.productExternalId, productData.productName, productData.quantity, productData.unitPrice);
        var saleItem = sale.Items.Last();

        // Calculate the expected total applying the discount policy
        decimal expectedDiscount = new DiscountPolicy().GetDiscountPercentage(productData.quantity);
        decimal expectedItemTotal = (productData.quantity * productData.unitPrice) - (productData.quantity * productData.unitPrice * expectedDiscount);

        // Assert
        Assert.Single(sale.Items);
        Assert.Equal(initialTotal + expectedItemTotal, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that removing a SaleItem decreases the item count and updates the total amount.
    /// </summary>
    [Fact(DisplayName = "Should remove SaleItem and update total amount")]
    public void RemoveSaleItem_ShouldDecreaseItemCountAndUpdateTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productData = SaleTestData.GenerateProductData(quantity: 5);
        sale.AddItem(productData.productExternalId, productData.productName, productData.quantity, productData.unitPrice);
        var saleItem = sale.Items.Last();
        decimal totalAfterAddition = sale.TotalAmount;

        // Act
        sale.RemoveItem(saleItem.Id);

        // Assert
        Assert.Empty(sale.Items);
        Assert.Equal(totalAfterAddition - saleItem.TotalAmount, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that adding multiple SaleItems correctly updates the item count and total amount.
    /// </summary>
    [Fact(DisplayName = "Should add multiple SaleItems and update total amount correctly")]
    public void AddMultipleSaleItems_ShouldUpdateTotalAndCountCorrectly()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product1 = SaleTestData.GenerateProductData(quantity: 3);
        var product2 = SaleTestData.GenerateProductData(quantity: 5);
        var product3 = SaleTestData.GenerateProductData(quantity: 12);

        // Act
        sale.AddItem(product1.productExternalId, product1.productName, product1.quantity, product1.unitPrice);
        sale.AddItem(product2.productExternalId, product2.productName, product2.quantity, product2.unitPrice);
        sale.AddItem(product3.productExternalId, product3.productName, product3.quantity, product3.unitPrice);

        // Assert
        Assert.Equal(3, sale.Items.Count);
        decimal totalExpected = sale.Items.Sum(item => item.TotalAmount);
        Assert.Equal(totalExpected, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that removing one SaleItem from multiple items updates the item count and total amount correctly.
    /// </summary>
    [Fact(DisplayName = "Should remove one SaleItem from multiple and update total amount correctly")]
    public void RemoveMultipleSaleItems_ShouldUpdateTotalAndCountCorrectly()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product1 = SaleTestData.GenerateProductData(quantity: 3);
        var product2 = SaleTestData.GenerateProductData(quantity: 5);
        var product3 = SaleTestData.GenerateProductData(quantity: 12);

        sale.AddItem(product1.productExternalId, product1.productName, product1.quantity, product1.unitPrice);
        sale.AddItem(product2.productExternalId, product2.productName, product2.quantity, product2.unitPrice);
        sale.AddItem(product3.productExternalId, product3.productName, product3.quantity, product3.unitPrice);

        var items = sale.Items.ToList();
        decimal totalBeforeRemoval = sale.TotalAmount;

        // Act: Remove the second item
        sale.RemoveItem(items[1].Id);

        // Assert
        Assert.Equal(2, sale.Items.Count);
        decimal expectedTotal = sale.Items.Sum(item => item.TotalAmount);
        Assert.Equal(expectedTotal, sale.TotalAmount);
        Assert.Equal(totalBeforeRemoval - items[1].TotalAmount, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that the sale's items collection is read-only.
    /// </summary>
    [Fact(DisplayName = "Sale items collection should be read-only")]
    public void SaleItemsCollection_ShouldBeReadOnly()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act & Assert
        Assert.IsType<ReadOnlyCollection<SaleItem>>(sale.Items);
    }
}
