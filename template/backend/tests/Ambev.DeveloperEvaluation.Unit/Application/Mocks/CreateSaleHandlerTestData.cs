using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;
namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks
{
    /// <summary>
    /// Provides methods for generating test data for CreateSaleCommand.
    /// </summary>
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> faker = new Faker<CreateSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => $"SALE-{f.Random.Number(1000, 9999)}")
            .RuleFor(s => s.SaleDate, f => f.Date.Recent().ToUniversalTime())
            .RuleFor(s => s.CustomerExternalId, f => $"CUST-{f.Random.Number(100, 999)}")
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchExternalId, f => $"BR-{f.Random.Number(10, 99)}")
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => new List<CreateSaleItemCommand>
            {
                new Faker<CreateSaleItemCommand>()
                    .RuleFor(i => i.ProductExternalId, p => $"PROD-{p.Random.Number(1, 100)}")
                    .RuleFor(i => i.ProductName, p => p.Commerce.ProductName())
                    .RuleFor(i => i.Quantity, p => p.Random.Int(1, 5))
                    .RuleFor(i => i.UnitPrice, p => p.Random.Decimal(1, 100))
                    .Generate()
            });

        /// <summary>
        /// Generates a valid CreateSaleCommand with randomized data.
        /// </summary>
        /// <returns>A valid CreateSaleCommand instance.</returns>
        public static CreateSaleCommand GenerateValidCommand()
        {
            return faker.Generate();
        }
    }
}
