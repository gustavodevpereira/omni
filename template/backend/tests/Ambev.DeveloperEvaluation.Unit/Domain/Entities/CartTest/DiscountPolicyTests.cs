﻿using Xunit;
using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Unit.Domain.Mocks.CartMock;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.CartTest;

/// <summary>
/// Contains unit tests for the DiscountPolicy value object.
/// These tests validate the discount calculation logic based on various quantity scenarios.
/// </summary>
public class DiscountPolicyTests
{
    private readonly DiscountPolicy _discountPolicy = new();

    /// <summary>
    /// Tests that when the quantity is less than 4, the discount returned is 0%.
    /// </summary>
    [Theory(DisplayName = "Should return 0 discount for quantities less than 4")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetDiscountPercentage_WhenQuantityLessThan4_ReturnsZero(int quantity)
    {
        // Act
        var discount = _discountPolicy.GetDiscountPercentage(quantity);

        // Assert
        Assert.Equal(0m, discount);
    }

    /// <summary>
    /// Tests that when the quantity is between 4 and 9, the discount returned is 10%.
    /// </summary>
    [Theory(DisplayName = "Should return 10% discount for quantities between 4 and 9")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(8)]
    [InlineData(9)]
    public void GetDiscountPercentage_WhenQuantityBetween4And9_ReturnsTenPercent(int quantity)
    {
        // Act
        var discount = _discountPolicy.GetDiscountPercentage(quantity);

        // Assert
        Assert.Equal(0.1m, discount);
    }

    /// <summary>
    /// Tests that when the quantity is between 10 and 20, the discount returned is 20%.
    /// </summary>
    [Theory(DisplayName = "Should return 20% discount for quantities between 10 and 20")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void GetDiscountPercentage_WhenQuantityBetween10And20_ReturnsTwentyPercent(int quantity)
    {
        // Act
        var discount = _discountPolicy.GetDiscountPercentage(quantity);

        // Assert
        Assert.Equal(0.2m, discount);
    }

    /// <summary>
    /// Tests that an DomainException is thrown when the quantity exceeds 20.
    /// Note: Although validating the maximum quantity is primarily the aggregate's responsibility,
    /// this test ensures that the DiscountPolicy remains consistent with the business rules.
    /// </summary>
    [Fact(DisplayName = "Should throw ArgumentException for quantities greater than 20")]
    public void GetDiscountPercentage_WhenQuantityGreaterThan20_ThrowsArgumentException()
    {
        // Arrange
        const int quantity = 21;

        // Act & Assert
        Assert.Throws<DomainException>(() => _discountPolicy.GetDiscountPercentage(quantity));
    }

    /// <summary>
    /// Tests that random quantities produce the expected discount percentages.
    /// This test uses the Bogus library to generate random test data.
    /// </summary>
    [Fact(DisplayName = "Should return correct discount for random quantities generated by Faker")]
    public void GetDiscountPercentage_WithRandomQuantities_ReturnsCorrectDiscount()
    {
        // Arrange
        var quantityNoDiscount = DiscountPolicyTestData.GenerateQuantityNoDiscount();
        var quantityTenPercent = DiscountPolicyTestData.GenerateQuantityTenPercent();
        var quantityTwentyPercent = DiscountPolicyTestData.GenerateQuantityTwentyPercent();

        // Act
        var discountNoDiscount = _discountPolicy.GetDiscountPercentage(quantityNoDiscount);
        var discountTenPercent = _discountPolicy.GetDiscountPercentage(quantityTenPercent);
        var discountTwentyPercent = _discountPolicy.GetDiscountPercentage(quantityTwentyPercent);

        // Assert
        Assert.Equal(0m, discountNoDiscount);
        Assert.Equal(0.1m, discountTenPercent);
        Assert.Equal(0.2m, discountTwentyPercent);
    }
}