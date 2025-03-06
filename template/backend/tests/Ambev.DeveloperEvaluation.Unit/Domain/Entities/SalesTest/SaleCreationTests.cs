using System;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.SalesTest;

/// <summary>
/// Unit tests for creating and validating the basic properties of a Sale.
/// </summary>
public class SaleCreationTests
{
    /// <summary>
    /// Tests that a valid Sale is created with an empty items collection.
    /// </summary>
    [Fact(DisplayName = "Should create a valid Sale with empty items")]
    public void CreateSale_WithValidData_ShouldCreateSaleWithEmptyItems()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act & Assert
        Assert.NotNull(sale);
        Assert.False(string.IsNullOrWhiteSpace(sale.SaleNumber));
        Assert.False(string.IsNullOrWhiteSpace(sale.CustomerExternalId));
        Assert.False(string.IsNullOrWhiteSpace(sale.CustomerName));
        Assert.False(string.IsNullOrWhiteSpace(sale.BranchExternalId));
        Assert.False(string.IsNullOrWhiteSpace(sale.BranchName));
        Assert.Equal(SaleStatus.Active, sale.Status);
        Assert.Empty(sale.Items);
        Assert.Equal(0m, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that a valid Sale passes validation.
    /// </summary>
    [Fact(DisplayName = "Valid Sale should pass validation")]
    public void Validate_ValidSale_ReturnsValidResult()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var validationResult = sale.Validate();

        // Assert
        Assert.True(validationResult.IsValid);
        Assert.Empty(validationResult.Errors);
    }

    /// <summary>
    /// Tests that an invalid Sale (empty sale number) fails validation.
    /// </summary>
    [Fact(DisplayName = "Invalid Sale (empty sale number) should fail validation")]
    public void Validate_InvalidSale_EmptySaleNumber_ReturnsInvalidResult()
    {
        // Arrange
        var sale = SaleTestData.GenerateInvalidSale("saleNumber");

        // Act
        var validationResult = sale.Validate();

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.NotEmpty(validationResult.Errors);
    }

    /// <summary>
    /// Tests for validating the behavior of the Sale aggregate's AddItem method,
    /// ensuring that the SaleItemValidator is correctly enforced.
    /// </summary>
    public class SaleAddItemValidatorTests
    {
        /// <summary>
        /// Tests that adding a valid SaleItem passes validation and is added to the sale.
        /// </summary>
        [Fact(DisplayName = "Adding a valid SaleItem should pass validation")]
        public void AddItem_WithValidData_ShouldPassValidator()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            // Use valid product data
            var productData = SaleTestData.GenerateProductData(quantity: 5, unitPrice: 100m);

            // Act
            sale.AddItem(productData.productExternalId, productData.productName, productData.quantity, productData.unitPrice);

            // Assert - the item should be added
            Assert.Single(sale.Items);
        }

        /// <summary>
        /// Tests that adding a SaleItem with an invalid product name (empty string) fails validation.
        /// This test validates that the SaleItemValidator is being invoked.
        /// </summary>
        [Fact(DisplayName = "Adding a SaleItem with an invalid product name should throw DomainException")]
        public void AddItem_WithInvalidProductName_ShouldThrowDomainException()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            // Invalid data: empty product name
            const string invalidProductName = "";
            const string validProductExternalId = "TestProductId";
            const int validQuantity = 5;
            const decimal validUnitPrice = 100m;

            // Act & Assert: Expect the validator to throw a DomainException because product name is required.
            var exception = Assert.Throws<DomainException>(() =>
                sale.AddItem(validProductExternalId, invalidProductName, validQuantity, validUnitPrice)
            );

            Assert.NotEmpty(exception.Message);
        }
    }
}
