using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;

/// <summary>
/// Provides methods for generating test data for AddCart use cases.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class AddCartTestData
{
    /// <summary>
    /// Configures the Faker to generate valid AddCartCommand entities.
    /// </summary>
    private static readonly Faker<AddCartCommand> AddCartCommandFaker = new Faker<AddCartCommand>()
        .RuleFor(x => x.CostumerId, f => f.Random.Guid())
        .RuleFor(x => x.CostumerName, f => f.Person.FullName)
        .RuleFor(x => x.BranchId, f => f.Random.Guid())
        .RuleFor(x => x.BranchName, f => f.Company.CompanyName())
        .RuleFor(x => x.CreatedOn, f => f.Date.Recent())
        .RuleFor(x => x.Products, f => GenerateCartProducts(f.Random.Int(1, 5)).ToList());

    /// <summary>
    /// Configures the Faker to generate valid AddCartProductCommand entities.
    /// </summary>
    private static readonly Faker<AddCartProductCommand> AddCartProductCommandFaker = new Faker<AddCartProductCommand>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Name, f => f.Commerce.ProductName())
        .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(x => x.UnitPrice, f => f.Random.Decimal(1, 100));

    /// <summary>
    /// Generates a collection of valid cart product commands.
    /// </summary>
    /// <param name="count">Number of product commands to generate.</param>
    /// <returns>A collection of valid cart product commands.</returns>
    private static IEnumerable<AddCartProductCommand> GenerateCartProducts(int count)
    {
        return AddCartProductCommandFaker.Generate(count);
    }

    /// <summary>
    /// Generates a valid AddCartCommand with all required properties.
    /// </summary>
    /// <returns>A valid AddCartCommand with randomly generated data.</returns>
    public static AddCartCommand GenerateValidCommand()
    {
        return AddCartCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid AddCartCommand with missing customer data.
    /// </summary>
    /// <returns>An invalid AddCartCommand with missing customer data.</returns>
    public static AddCartCommand GenerateInvalidCommandWithMissingCustomerData()
    {
        var command = AddCartCommandFaker.Generate();
        command.CostumerId = Guid.Empty;
        command.CostumerName = string.Empty;
        return command;
    }

    /// <summary>
    /// Generates an invalid AddCartCommand with missing branch data.
    /// </summary>
    /// <returns>An invalid AddCartCommand with missing branch data.</returns>
    public static AddCartCommand GenerateInvalidCommandWithMissingBranchData()
    {
        var command = AddCartCommandFaker.Generate();
        command.BranchId = Guid.Empty;
        command.BranchName = string.Empty;
        return command;
    }

    /// <summary>
    /// Generates an invalid AddCartCommand with an invalid creation date (future date).
    /// </summary>
    /// <returns>An invalid AddCartCommand with a future creation date.</returns>
    public static AddCartCommand GenerateInvalidCommandWithFutureDate()
    {
        var command = AddCartCommandFaker.Generate();
        command.CreatedOn = DateTime.UtcNow.AddDays(1);
        return command;
    }

    /// <summary>
    /// Generates an invalid AddCartCommand with empty products.
    /// </summary>
    /// <returns>An invalid AddCartCommand with no products.</returns>
    public static AddCartCommand GenerateInvalidCommandWithEmptyProducts()
    {
        var command = AddCartCommandFaker.Generate();
        command.Products.Clear();
        return command;
    }

    /// <summary>
    /// Generates an invalid AddCartCommand with invalid product data.
    /// </summary>
    /// <returns>An invalid AddCartCommand with invalid product data.</returns>
    public static AddCartCommand GenerateInvalidCommandWithInvalidProductData()
    {
        var command = AddCartCommandFaker.Generate();
        var invalidProduct = new AddCartProductCommand
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Quantity = 0,
            UnitPrice = 0
        };
        command.Products = [invalidProduct];
        return command;
    }
} 