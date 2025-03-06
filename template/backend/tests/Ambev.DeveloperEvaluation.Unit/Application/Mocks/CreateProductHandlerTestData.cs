using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks;

/// <summary>
/// Provides methods for generating test data for the CreateProductHandler using the Bogus library.
/// </summary>
public static class CreateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateProductCommand instances.
    /// </summary>
    private static readonly Faker<CreateProductCommand> CreateProductCommandFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Sku, f => $"SKU-{f.Random.AlphaNumeric(8)}")
        .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 500)))
        .RuleFor(p => p.StockQuantity, f => f.Random.Number(1, 100))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.CustomerExternalId, f => f.Random.Guid().ToString())
        .RuleFor(p => p.CustomerName, f => f.Name.FullName());

    /// <summary>
    /// Generates a valid CreateProductCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateProductCommand with randomly generated data.</returns>
    public static CreateProductCommand GenerateValidCommand()
    {
        return CreateProductCommandFaker.Generate();
    }
} 