using Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;

/// <summary>
/// Provides methods for generating test data for CalculateCartDiscount use cases.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CalculateCartDiscountTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CalculateCartDiscountCommand entities.
    /// </summary>
    private static readonly Faker<CalculateCartDiscountCommand> CalculateCartDiscountCommandFaker = new Faker<CalculateCartDiscountCommand>()
        .RuleFor(x => x.CustomerId, f => f.Random.Guid())
        .RuleFor(x => x.CustomerName, f => f.Person.FullName)
        .RuleFor(x => x.CustomerEmail, f => f.Internet.Email())
        .RuleFor(x => x.BranchExternalId, f => f.Random.Guid().ToString())
        .RuleFor(x => x.BranchName, f => f.Company.CompanyName())
        .RuleFor(x => x.Date, f => f.Date.Recent())
        .RuleFor(x => x.Products, f => GenerateCartProducts(f.Random.Int(1, 5)).ToList());

    /// <summary>
    /// Configures the Faker to generate valid CalculateCartDiscountProductCommand entities.
    /// </summary>
    private static readonly Faker<CalculateCartDiscountProductCommand> CalculateCartDiscountProductCommandFaker = new Faker<CalculateCartDiscountProductCommand>()
        .RuleFor(x => x.ProductId, f => f.Random.Guid())
        .RuleFor(x => x.Name, f => f.Commerce.ProductName())
        .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(x => x.Price, f => f.Random.Decimal(1, 100));

    /// <summary>
    /// Generates a collection of valid cart product commands.
    /// </summary>
    /// <param name="count">Number of product commands to generate.</param>
    /// <returns>A collection of valid cart product commands.</returns>
    private static IEnumerable<CalculateCartDiscountProductCommand> GenerateCartProducts(int count)
    {
        return CalculateCartDiscountProductCommandFaker.Generate(count);
    }

    /// <summary>
    /// Generates a valid CalculateCartDiscountCommand with all required properties.
    /// </summary>
    /// <returns>A valid CalculateCartDiscountCommand with randomly generated data.</returns>
    public static CalculateCartDiscountCommand GenerateValidCommand()
    {
        return CalculateCartDiscountCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid command with multiple products with different quantities
    /// to test various discount scenarios.
    /// </summary>
    /// <returns>A command with products eligible for different discount tiers.</returns>
    public static CalculateCartDiscountCommand GenerateCommandWithVariedDiscounts()
    {
        var command = CalculateCartDiscountCommandFaker.Generate();
        command.Products = new List<CalculateCartDiscountProductCommand>
        {
            // No discount (quantity < 4)
            new CalculateCartDiscountProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Product with no discount",
                Quantity = 2,
                Price = 10.0m
            },
            // 10% discount (4 <= quantity < 10)
            new CalculateCartDiscountProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Product with 10% discount",
                Quantity = 6,
                Price = 20.0m
            },
            // 20% discount (quantity >= 10)
            new CalculateCartDiscountProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Product with 20% discount",
                Quantity = 15,
                Price = 30.0m
            }
        };
        return command;
    }

    /// <summary>
    /// Generates an invalid CalculateCartDiscountCommand with missing customer data.
    /// </summary>
    /// <returns>An invalid CalculateCartDiscountCommand with missing customer data.</returns>
    public static CalculateCartDiscountCommand GenerateInvalidCommandWithMissingCustomerData()
    {
        var command = CalculateCartDiscountCommandFaker.Generate();
        command.CustomerId = Guid.Empty;
        command.CustomerName = string.Empty;
        return command;
    }

    /// <summary>
    /// Generates an invalid CalculateCartDiscountCommand with missing branch data.
    /// </summary>
    /// <returns>An invalid CalculateCartDiscountCommand with missing branch data.</returns>
    public static CalculateCartDiscountCommand GenerateInvalidCommandWithMissingBranchData()
    {
        var command = CalculateCartDiscountCommandFaker.Generate();
        command.BranchExternalId = string.Empty;
        command.BranchName = string.Empty;
        return command;
    }

    /// <summary>
    /// Generates an invalid CalculateCartDiscountCommand with invalid product data.
    /// </summary>
    /// <returns>An invalid CalculateCartDiscountCommand with invalid product data.</returns>
    public static CalculateCartDiscountCommand GenerateInvalidCommandWithInvalidProductData()
    {
        var command = CalculateCartDiscountCommandFaker.Generate();
        var invalidProduct = new CalculateCartDiscountProductCommand
        {
            ProductId = Guid.Empty,
            Name = string.Empty,
            Quantity = 0,
            Price = 0
        };
        command.Products = [invalidProduct];
        return command;
    }
} 