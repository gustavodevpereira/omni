using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Mocks.ProductMock;

/// <summary>
/// Provides methods for generating test data for Product entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated products will have valid:
    /// - Name
    /// - Description
    /// - SKU
    /// - Price
    /// - StockQuantity
    /// - Category
    /// - Status
    /// </summary>
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Sku, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 500)))
        .RuleFor(p => p.StockQuantity, f => f.Random.Number(1, 100))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.BranchExternalId, f => f.Random.Guid())
        .RuleFor(p => p.BranchName, f => f.Company.CompanyName())
        .RuleFor(p => p.Status, f => ProductStatus.Active);

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// The generated product will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates a valid product name using Faker.
    /// </summary>
    /// <returns>A valid product name.</returns>
    public static string GenerateValidName()
    {
        return new Faker().Commerce.ProductName();
    }

    /// <summary>
    /// Generates a valid product description using Faker.
    /// </summary>
    /// <returns>A valid product description.</returns>
    public static string GenerateValidDescription()
    {
        return new Faker().Commerce.ProductDescription();
    }

    /// <summary>
    /// Generates a valid SKU (Stock Keeping Unit) for a product.
    /// The generated SKU will:
    /// - Follow the format SKU-XXXXXXXX
    /// - Contain only alphanumeric characters
    /// </summary>
    /// <returns>A valid SKU.</returns>
    public static string GenerateValidSku()
    {
        var faker = new Faker();
        return faker.Random.AlphaNumeric(10).ToUpper();
    }

    /// <summary>
    /// Generates a valid price for a product.
    /// The generated price will:
    /// - Be a positive decimal number
    /// - Be in a reasonable range (10-500)
    /// </summary>
    /// <returns>A valid price.</returns>
    public static decimal GenerateValidPrice()
    {
        return decimal.Parse(new Faker().Commerce.Price(10, 500));
    }

    /// <summary>
    /// Generates a valid stock quantity for a product.
    /// The generated quantity will:
    /// - Be a positive integer
    /// - Be in a reasonable range (1-100)
    /// </summary>
    /// <returns>A valid stock quantity.</returns>
    public static int GenerateValidStockQuantity()
    {
        return new Faker().Random.Number(1, 100);
    }

    /// <summary>
    /// Generates a valid category name for a product.
    /// </summary>
    /// <returns>A valid category name.</returns>
    public static string GenerateValidCategory()
    {
        return new Faker().Commerce.Categories(1)[0];
    }

    /// <summary>
    /// Generates an invalid name (empty string) for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid product name.</returns>
    public static string GenerateInvalidName()
    {
        return string.Empty;
    }

    /// <summary>
    /// Generates an invalid SKU for testing negative scenarios.
    /// The generated SKU will:
    /// - Contain special characters not allowed in SKU format
    /// </summary>
    /// <returns>An invalid SKU.</returns>
    public static string GenerateInvalidSku()
    {
        return "SKU@#$%^&*()";
    }

    /// <summary>
    /// Generates an invalid price (negative or zero) for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid price.</returns>
    public static decimal GenerateInvalidPrice()
    {
        return new Faker().Random.Int(-100, 0);
    }

    /// <summary>
    /// Generates an invalid stock quantity (negative) for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid stock quantity.</returns>
    public static int GenerateInvalidStockQuantity()
    {
        return new Faker().Random.Int(-100, -1);
    }

    /// <summary>
    /// Generates a name that exceeds the maximum length limit.
    /// </summary>
    /// <returns>A product name that exceeds the maximum length limit.</returns>
    public static string GenerateLongName()
    {
        return new Faker().Random.String2(101);
    }

    /// <summary>
    /// Generates a description that exceeds the maximum length limit.
    /// </summary>
    /// <returns>A product description that exceeds the maximum length limit.</returns>
    public static string GenerateLongDescription()
    {
        return new Faker().Random.String2(501);
    }
}