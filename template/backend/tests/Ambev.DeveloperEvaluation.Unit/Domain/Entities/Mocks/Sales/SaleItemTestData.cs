using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the SaleItem entity using the Bogus library.
/// This class centralizes all test data generation to ensure consistency across test cases,
/// offering both valid and invalid data scenarios.
/// </summary>
public static class SaleItemTestData
{
    /// <summary>
    /// Configures the Faker to generate valid SaleItem entities.
    /// The generated SaleItems will have valid:
    /// - ProductExternalId (using Commerce.Ean13)
    /// - ProductName (using Commerce.ProductName)
    /// - Quantity (between 1 and 20)
    /// - UnitPrice (within a valid range)
    /// </summary>
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .CustomInstantiator(f => new SaleItem(
            f.Commerce.Ean13(),
            f.Commerce.ProductName(),
            f.Random.Int(1, 20),
            f.Random.Decimal(10, 100)
        ));

    /// <summary>
    /// Generates a valid SaleItem entity with randomized data.
    /// The generated SaleItem will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid SaleItem entity with randomly generated data.</returns>
    public static SaleItem GenerateValidSaleItem()
    {
        return SaleItemFaker.Generate();
    }

    /// <summary>
    /// Generates a valid SaleItem entity with randomized data,
    /// using the specified quantity.
    /// </summary>
    /// <param name="quantity">The desired quantity for the SaleItem (between 1 and 20).</param>
    /// <returns>A valid SaleItem entity with the specified quantity.</returns>
    public static SaleItem GenerateValidSaleItem(int quantity)
    {
        var faker = new Faker();
        return new SaleItem(
            faker.Commerce.Ean13(),
            faker.Commerce.ProductName(),
            quantity,
            faker.Random.Decimal(10, 100)
        );
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
        return (
            faker.Commerce.Ean13(),
            faker.Commerce.ProductName(),
            quantity,
            faker.Random.Decimal(10, 100)
        );
    }
}
