using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales
{
    /// <summary>
    /// Centralizes test data generation for the Sale entity, ensuring consistency across tests.
    /// </summary>
    public static class SaleTestData
    {
        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                f.Commerce.Ean13(),
                DateTime.UtcNow,
                f.Random.AlphaNumeric(10),
                f.Name.FullName(),
                f.Random.AlphaNumeric(10),
                f.Company.CompanyName()
            ));

        /// <summary>
        /// Generates a valid Sale instance.
        /// </summary>
        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }

        /// <summary>
        /// Generates an invalid Sale instance by setting one of the required fields as empty.
        /// The invalidField parameter specifies which field will be invalid: "saleNumber", 
        /// "customerExternalId", "customerName", "branchExternalId", or "branchName".
        /// </summary>
        public static Sale GenerateInvalidSale(string invalidField)
        {
            var faker = new Faker();
            string saleNumber = invalidField == "saleNumber" ? "" : faker.Commerce.Ean13();
            DateTime saleDate = DateTime.UtcNow;
            string customerExternalId = invalidField == "customerExternalId" ? "" : faker.Random.AlphaNumeric(10);
            string customerName = invalidField == "customerName" ? "" : faker.Name.FullName();
            string branchExternalId = invalidField == "branchExternalId" ? "" : faker.Random.AlphaNumeric(10);
            string branchName = invalidField == "branchName" ? "" : faker.Company.CompanyName();

            return new Sale(saleNumber, saleDate, customerExternalId, customerName, branchExternalId, branchName);
        }

        /// <summary>
        /// Generates product data for use in tests involving the addition of items to a sale.
        /// </summary>
        /// <param name="quantity">Product quantity (affects discount application).</param>
        /// <param name="unitPrice">Unit price of the product (if not specified, a random value between 10 and 100 is generated).</param>
        /// <returns>A tuple containing productExternalId, productName, quantity, and unitPrice.</returns>
        public static (string productExternalId, string productName, int quantity, decimal unitPrice) GenerateProductData(int quantity, decimal? unitPrice = null)
        {
            var faker = new Faker();
            return (
                productExternalId: faker.Commerce.Ean13(),
                productName: faker.Commerce.ProductName(),
                quantity: quantity,
                unitPrice: unitPrice ?? faker.Random.Decimal(10, 100)
            );
        }
    }
}
