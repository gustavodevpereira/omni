using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks;

/// <summary>
/// Provides methods for generating test data for the UpdateProductHandler using the Bogus library.
/// </summary>
public static class UpdateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateProductCommand instances.
    /// </summary>
    private static readonly Faker<UpdateProductCommand> UpdateProductCommandFaker = new Faker<UpdateProductCommand>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 500)))
        .RuleFor(p => p.StockQuantity, f => f.Random.Number(1, 100))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Status, f => f.PickRandom("Active", "Discontinued"))
        .RuleFor(p => p.CustomerExternalId, f => f.Random.Guid().ToString())
        .RuleFor(p => p.CustomerName, f => f.Name.FullName());

    /// <summary>
    /// Generates a valid UpdateProductCommand with randomized data.
    /// </summary>
    /// <returns>A valid UpdateProductCommand with randomly generated data.</returns>
    public static UpdateProductCommand GenerateValidCommand()
    {
        return UpdateProductCommandFaker.Generate();
    }
} 