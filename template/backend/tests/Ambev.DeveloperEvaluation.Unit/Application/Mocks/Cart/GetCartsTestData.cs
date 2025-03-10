using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks.Cart;

/// <summary>
/// Provides methods for generating test data for GetCarts use cases.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class GetCartsTestData
{
    /// <summary>
    /// Configures the Faker to generate valid GetCartsCommand entities.
    /// The generated commands will have valid:
    /// - PageNumber (positive integer)
    /// - PageSize (positive integer within limits)
    /// - Optional CustomerId
    /// </summary>
    private static readonly Faker<GetCartsCommand> GetCartsCommandFaker = new Faker<GetCartsCommand>()
        .CustomInstantiator(f => new GetCartsCommand(
            pageNumber: f.Random.Int(1, 10),
            pageSize: f.Random.Int(1, 50)
        ));
    
    /// <summary>
    /// Configures the Faker to generate valid GetCartsCommand entities with a customer ID.
    /// </summary>
    private static readonly Faker<GetCartsCommand> GetCartsWithCustomerCommandFaker = new Faker<GetCartsCommand>()
        .CustomInstantiator(f => new GetCartsCommand(
            pageNumber: f.Random.Int(1, 10),
            pageSize: f.Random.Int(1, 50),
            customerId: f.Random.Guid()
        ));

    /// <summary>
    /// Generates a valid GetCartsCommand without a customer ID filter.
    /// </summary>
    /// <returns>A valid GetCartsCommand with randomly generated pagination values.</returns>
    public static GetCartsCommand GenerateValidCommand()
    {
        return GetCartsCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetCartsCommand with a customer ID filter.
    /// </summary>
    /// <returns>A valid GetCartsCommand with a customer ID and randomly generated pagination values.</returns>
    public static GetCartsCommand GenerateValidCommandWithCustomer()
    {
        return GetCartsWithCustomerCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid GetCartsCommand with an invalid page number.
    /// </summary>
    /// <returns>An invalid GetCartsCommand with negative page number.</returns>
    public static GetCartsCommand GenerateInvalidCommandWithNegativePageNumber()
    {
        return new GetCartsCommand(pageNumber: -1, pageSize: 10, customerId: Guid.NewGuid());
    }

    /// <summary>
    /// Generates an invalid GetCartsCommand with an invalid page size.
    /// </summary>
    /// <returns>An invalid GetCartsCommand with negative page size.</returns>
    public static GetCartsCommand GenerateInvalidCommandWithNegativePageSize()
    {
        return new GetCartsCommand(pageNumber: 1, pageSize: -1, customerId: Guid.NewGuid());
    }

    /// <summary>
    /// Generates an invalid GetCartsCommand with page size exceeding the maximum.
    /// </summary>
    /// <returns>An invalid GetCartsCommand with excessive page size.</returns>
    public static GetCartsCommand GenerateInvalidCommandWithExcessivePageSize()
    {
        return new GetCartsCommand(pageNumber: 1, pageSize: 101, customerId: Guid.NewGuid());
    }
} 