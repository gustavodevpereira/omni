using Bogus;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.SalesTest
{
    /// <summary>
    /// Contains unit tests for the SaleItem entity.
    /// Tests cover the calculation of discount and total amount based on quantity,
    /// as well as the validation of quantity constraints.
    /// </summary>
    public class SaleItemTests
    {
        /// <summary>
        /// Tests that a SaleItem is created with the correct discount and total amount
        /// for various valid quantities.
        /// </summary>
        [Theory(DisplayName = "Should create SaleItem with correct discount and total amount for valid quantities")]
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
            // Arrange: Generate a valid SaleItem using the test data helper with the specified quantity.
            var saleItem = SaleItemTestData.GenerateValidSaleItem(quantity);

            // Assert: Verify that the discount percentage is correct according to the business rules.
            var expectedDiscount = new DiscountPolicy().GetDiscountPercentage(quantity);
            Assert.Equal(expectedDiscount, saleItem.DiscountPercentage);

            // Assert: Validate the total amount calculation.
            decimal grossAmount = quantity * saleItem.UnitPrice;
            decimal discountAmount = grossAmount * expectedDiscount;
            decimal expectedTotal = grossAmount - discountAmount;

            Assert.Equal(expectedTotal, saleItem.TotalAmount);
        }

        /// <summary>
        /// Tests that a DomainException is thrown when an invalid quantity is provided.
        /// Quantities less than 1 or greater than 20 should not be accepted.
        /// </summary>
        [Theory(DisplayName = "Should throw DomainException for invalid quantity")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(21)]
        [InlineData(25)]
        public void CreateSaleItem_WithInvalidQuantity_ShouldThrowDomainException(int quantity)
        {
            // Arrange: Generate invalid SaleItem parameters for the specified quantity.
            var (productExternalId, productName, invalidQuantity, unitPrice) = SaleItemTestData.GenerateInvalidSaleItemParameters(quantity);

            // Act & Assert: Expect a DomainException to be thrown when creating a SaleItem with an invalid quantity.
            Assert.Throws<DomainException>(() => new SaleItem(productExternalId, productName, invalidQuantity, unitPrice));
        }
    }
}
