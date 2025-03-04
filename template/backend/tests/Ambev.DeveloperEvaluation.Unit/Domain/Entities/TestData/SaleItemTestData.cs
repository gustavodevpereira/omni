using Bogus;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods for generating test data for the SaleItem entity using the Bogus library.
    /// This class centralizes test data generation for SaleItem to ensure consistency across tests,
    /// offering both valid and invalid data scenarios.
    /// </summary>
    public static class SaleItemTestData
    {
        /// <summary>
        /// Generates a valid SaleItem instance with random data, using the provided quantity.
        /// </summary>
        /// <param name="quantity">The desired quantity for the SaleItem (between 1 and 20).</param>
        /// <returns>A valid SaleItem instance with randomly generated data.</returns>
        public static SaleItem GenerateValidSaleItem(int quantity)
        {
            var faker = new Faker();
            string productExternalId = faker.Commerce.Ean13();
            string productName = faker.Commerce.ProductName();
            decimal unitPrice = faker.Random.Decimal(10, 100);

            return new SaleItem(productExternalId, productName, quantity, unitPrice);
        }

        /// <summary>
        /// Generates a valid SaleItem instance with random data,
        /// where the quantity is randomly generated between 1 and 20.
        /// </summary>
        /// <returns>A valid SaleItem instance with randomly generated data.</returns>
        public static SaleItem GenerateValidSaleItem()
        {
            var faker = new Faker();
            int quantity = faker.Random.Int(1, 20);
            return GenerateValidSaleItem(quantity);
        }

        /// <summary>
        /// Generates a set of parameters for an invalid SaleItem.
        /// Useful for testing scenarios where the quantity is outside the allowed range (less than 1 or greater than 20).
        /// </summary>
        /// <param name="quantity">The invalid quantity to be used.</param>
        /// <returns>
        /// A tuple containing productExternalId, productName, quantity, and unitPrice.
        /// </returns>
        public static (string productExternalId, string productName, int quantity, decimal unitPrice)
            GenerateInvalidSaleItemParameters(int quantity)
        {
            var faker = new Faker();
            string productExternalId = faker.Commerce.Ean13();
            string productName = faker.Commerce.ProductName();
            decimal unitPrice = faker.Random.Decimal(10, 100);

            return (productExternalId, productName, quantity, unitPrice);
        }
    }
}
