using Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;

/// <summary>
/// Provides methods for generating test data for the UpdateCartHandler using the Bogus library.
/// </summary>
public static class UpdateCartTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateCartCommand instances.
    /// </summary>
    private static readonly Faker<UpdateCartCommand> UpdateCartCommandFaker = new Faker<UpdateCartCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.CustomerExternalId, f => f.Random.Guid())
        .RuleFor(c => c.CustomerName, f => f.Company.CompanyName())
        .RuleFor(c => c.BranchExternalId, f => f.Random.Guid())
        .RuleFor(c => c.BranchName, f => f.Commerce.Department())
        .RuleFor(c => c.Products, f => new List<UpdateCartProductCommand>());

    /// <summary>
    /// Configures the Faker to generate valid UpdateCartProductCommand instances.
    /// </summary>
    private static readonly Faker<UpdateCartProductCommand> UpdateCartProductCommandFaker = new Faker<UpdateCartProductCommand>()
        .RuleFor(p => p.ProductExternalId, f => f.Random.Guid())
        .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
        .RuleFor(p => p.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(1, 1000)));

    /// <summary>
    /// Generates a valid UpdateCartCommand with randomized data and products.
    /// </summary>
    /// <param name="productCount">Number of products to generate.</param>
    /// <returns>A valid UpdateCartCommand with randomly generated data and products.</returns>
    public static UpdateCartCommand GenerateValidCommand(int productCount = 2)
    {
        var command = UpdateCartCommandFaker.Generate();
        command.Products.AddRange(UpdateCartProductCommandFaker.Generate(productCount));
        return command;
    }

    /// <summary>
    /// Generates an invalid UpdateCartCommand missing required fields.
    /// </summary>
    /// <returns>An invalid UpdateCartCommand missing required customer data.</returns>
    public static UpdateCartCommand GenerateInvalidCommand()
    {
        return new UpdateCartCommand
        {
            Id = Guid.NewGuid(),
            // Missing CustomerExternalId and CustomerName
            BranchExternalId = Guid.NewGuid(),
            BranchName = "Test Branch",
            Products = new List<UpdateCartProductCommand>()
        };
    }
}
