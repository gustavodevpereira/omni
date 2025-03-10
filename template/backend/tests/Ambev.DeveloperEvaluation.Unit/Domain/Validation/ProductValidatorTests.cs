using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Mocks.ProductMock;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Tests for the ProductValidator class.
/// </summary>
public class ProductValidatorTests
{
    /// <summary>
    /// Tests that validation passes for a valid product.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid product")]
    public void Given_ValidProduct_When_Validated_Then_ShouldPass()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var validator = new ProductValidator();

        // Act
        var result = validator.Validate(product);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when product name is empty.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when name is empty")]
    public void Given_ProductWithEmptyName_When_Validated_Then_ShouldFail()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Name = string.Empty;
        var validator = new ProductValidator();

        // Act
        var result = validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        // Just test that validation fails
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when SKU format is invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when SKU format is invalid")]
    public void Given_ProductWithInvalidSku_When_Validated_Then_ShouldFail()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Sku = "invalid-sku";
        var validator = new ProductValidator();

        // Act
        var result = validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        // Just test that validation fails
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when price is zero or negative.
    /// </summary>
    [Theory(DisplayName = "Validation should fail when price is zero or negative")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_ProductWithInvalidPrice_When_Validated_Then_ShouldFail(decimal price)
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Price = price;
        var validator = new ProductValidator();

        // Act
        var result = validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        // Just test that validation fails
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that the Product entity's Validate method works correctly.
    /// </summary>
    [Fact(DisplayName = "Product's Validate method should work correctly")]
    public void Given_Product_When_CallingValidateMethod_Then_ShouldReturnCorrectResult()
    {
        // Arrange
        var validProduct = ProductTestData.GenerateValidProduct();
        var invalidProduct = ProductTestData.GenerateValidProduct();
        invalidProduct.Price = -1;

        // Act
        var validResult = validProduct.Validate();
        var invalidResult = invalidProduct.Validate();

        // Assert
        Assert.True(validResult.IsValid);
        Assert.False(invalidResult.IsValid);
        // Just test that validation fails
        Assert.NotEmpty(invalidResult.Errors);
    }
} 