using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Unit.Domain.Mocks.ProductMock;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications;

/// <summary>
/// Tests for the ActiveProductSpecification class.
/// </summary>
public class ActiveProductSpecificationTests
{
    /// <summary>
    /// Tests that the specification returns true for an active product.
    /// </summary>
    [Fact(DisplayName = "Specification should return true for active product")]
    public void Given_ActiveProduct_When_CheckingSpecification_Then_ShouldReturnTrue()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Status = ProductStatus.Active;
        var specification = new ActiveProductSpecification();

        // Act
        var result = specification.IsSatisfiedBy(product);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests that the specification returns false for a discontinued product.
    /// </summary>
    [Fact(DisplayName = "Specification should return false for discontinued product")]
    public void Given_DiscontinuedProduct_When_CheckingSpecification_Then_ShouldReturnFalse()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Status = ProductStatus.Discontinued;
        var specification = new ActiveProductSpecification();

        // Act
        var result = specification.IsSatisfiedBy(product);

        // Assert
        Assert.False(result);
    }
} 