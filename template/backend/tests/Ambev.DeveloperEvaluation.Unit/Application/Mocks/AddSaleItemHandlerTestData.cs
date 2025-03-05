using Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks;

/// <summary>
/// Provides methods for generating test data for AddSaleItemCommand using the Bogus library.
/// </summary>
public static class AddSaleItemHandlerTestData
{
    private static readonly Faker<AddSaleItemCommand> faker = new Faker<AddSaleItemCommand>()
        .RuleFor(c => c.SaleId, f => Guid.NewGuid())
        .RuleFor(c => c.ProductExternalId, f => $"PROD-{f.Random.Number(100, 999)}")
        .RuleFor(c => c.ProductName, f => f.Commerce.ProductName())
        .RuleFor(c => c.Quantity, f => f.Random.Int(1, 5))
        .RuleFor(c => c.UnitPrice, f => f.Random.Decimal(1, 100));

    /// <summary>
    /// Generates a valid AddSaleItemCommand with randomized data.
    /// </summary>
    /// <returns>A valid instance of AddSaleItemCommand.</returns>
    public static AddSaleItemCommand GenerateValidCommand()
    {
        return faker.Generate();
    }
}
