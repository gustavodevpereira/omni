using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.SalesTest
{
    /// <summary>
    /// Unit tests for behaviors related to sale cancellation.
    /// </summary>
    public class SaleCancellationTests
    {
        /// <summary>
        /// Tests that canceling the sale sets its status to Cancelled.
        /// </summary>
        [Fact(DisplayName = "Should cancel Sale and update status")]
        public void CancelSale_ShouldSetStatusToCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.CancelSale();

            // Assert
            Assert.Equal(SaleStatus.Cancelled, sale.Status);
        }

        /// <summary>
        /// Tests that adding an item after canceling the sale throws a DomainException.
        /// </summary>
        [Fact(DisplayName = "Adding an item after cancellation should throw exception")]
        public void AddItemAfterCancellation_ShouldThrowException()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.CancelSale();
            var productData = SaleTestData.GenerateProductData(quantity: 5);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                sale.AddItem(productData.productExternalId, productData.productName, productData.quantity, productData.unitPrice)
            );
            Assert.Contains("cancelled", exception.Message, System.StringComparison.OrdinalIgnoreCase);
            Assert.Equal(SaleStatus.Cancelled, sale.Status);
        }

        /// <summary>
        /// Tests that removing an item after canceling the sale throws a DomainException.
        /// </summary>
        [Fact(DisplayName = "Removing an item after cancellation should throw exception")]
        public void RemoveItemAfterCancellation_ShouldThrowException()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var productData = SaleTestData.GenerateProductData(quantity: 5);
            sale.AddItem(productData.productExternalId, productData.productName, productData.quantity, productData.unitPrice);
            var saleItem = sale.Items.First();
            sale.CancelSale();

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => sale.RemoveItem(saleItem.Id));
            Assert.Contains("cancelled", exception.Message, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
