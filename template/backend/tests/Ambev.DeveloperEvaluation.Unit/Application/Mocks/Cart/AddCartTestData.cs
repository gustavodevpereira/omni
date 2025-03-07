using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;

/// <summary>
/// Provides methods for generating test data for the AddCartHandler using the Bogus library.
/// </summary>
public static class AddCartTestData
{
    /// <summary>
    /// Configures the Faker to generate valid AddCartCommand instances.
    /// </summary>
    private static readonly Faker<AddCartCommand> AddCartCommandFaker = new Faker<AddCartCommand>()
        .RuleFor(c => c.CostumerId, f => f.Random.Guid())
        .RuleFor(c => c.CostumerName, f => f.Company.CompanyName())
        .RuleFor(c => c.BranchId, f => f.Random.Guid())
        .RuleFor(c => c.BranchName, f => f.Commerce.Department())
        .RuleFor(c => c.CreatedOn, f => f.Date.Recent())
        .RuleFor(c => c.Products, f => new List<AddCartProductCommand>());

    /// <summary>
    /// Configures the Faker to generate valid AddCartProductCommand instances.
    /// </summary>
    private static readonly Faker<AddCartProductCommand> AddCartProductCommandFaker = new Faker<AddCartProductCommand>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(1, 1000)));

    /// <summary>
    /// Generates a valid AddCartCommand with randomized data and no products.
    /// </summary>
    /// <returns>A valid AddCartCommand with randomly generated data and no products.</returns>
    public static AddCartCommand GenerateValidCommand()
    {
        return AddCartCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid AddCartCommand with one product.
    /// </summary>
    /// <returns>A valid AddCartCommand with one product.</returns>
    public static AddCartCommand GenerateValidCommandWithOneProduct()
    {
        var command = AddCartCommandFaker.Generate();
        command.Products.Add(AddCartProductCommandFaker.Generate());
        return command;
    }

    /// <summary>
    /// Generates a valid AddCartCommand with multiple products.
    /// </summary>
    /// <param name="productCount">Number of products to generate.</param>
    /// <returns>A valid AddCartCommand with randomly generated data and the specified number of products.</returns>
    public static AddCartCommand GenerateValidCommandWithProducts(int productCount = 2)
    {
        var command = AddCartCommandFaker.Generate();
        command.Products.AddRange(AddCartProductCommandFaker.Generate(productCount));
        return command;
    }

    /// <summary>
    /// Generates an invalid AddCartCommand missing required fields.
    /// </summary>
    /// <returns>An invalid AddCartCommand missing required customer data.</returns>
    public static AddCartCommand GenerateInvalidCommand()
    {
        return new AddCartCommand
        {
            // Missing CostumerId and CostumerName
            BranchId = Guid.NewGuid(),
            BranchName = "Test Branch",
            CreatedOn = DateTime.UtcNow,
            Products = new List<AddCartProductCommand>()
        };
    }
}