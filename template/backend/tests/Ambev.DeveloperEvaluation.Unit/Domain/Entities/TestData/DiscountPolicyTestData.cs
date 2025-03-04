using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides helper methods for generating test data for DiscountPolicy.
/// Centralizes test data generation to ensure consistency across test scenarios.
/// </summary>
public static class DiscountPolicyTestData
{
    private static readonly Faker Faker = new Faker();

    /// <summary>
    /// Generates a quantity that should result in 0% discount (from 1 to 3).
    /// </summary>
    public static int GenerateQuantityNoDiscount() => Faker.Random.Int(1, 3);

    /// <summary>
    /// Generates a quantity that should result in 10% discount (from 4 to 9).
    /// </summary>
    public static int GenerateQuantityTenPercent() => Faker.Random.Int(4, 9);

    /// <summary>
    /// Generates a quantity that should result in 20% discount (from 10 to 20).
    /// </summary>
    public static int GenerateQuantityTwentyPercent() => Faker.Random.Int(10, 20);

    /// <summary>
    /// Generates an invalid quantity (greater than 20) that should trigger an exception.
    /// </summary>
    public static int GenerateInvalidQuantity() => Faker.Random.Int(21, 100);
}
