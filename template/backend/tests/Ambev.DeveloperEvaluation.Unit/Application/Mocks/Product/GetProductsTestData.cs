using Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Application.Mocks.Product;

/// <summary>
/// Provides test data for GetProductsHandler tests
/// </summary>
public static class GetProductsTestData
{
    /// <summary>
    /// Creates a valid GetProductsCommand with default pagination values
    /// </summary>
    /// <returns>A valid GetProductsCommand</returns>
    public static GetProductsCommand CreateValidCommand()
    {
        return new GetProductsCommand(1, 10);
    }

    /// <summary>
    /// Creates a GetProductsCommand with invalid negative page number
    /// </summary>
    /// <returns>An invalid GetProductsCommand with negative page number</returns>
    public static GetProductsCommand CreateInvalidCommandWithNegativePageNumber()
    {
        return new GetProductsCommand(-1, 10);
    }

    /// <summary>
    /// Creates a GetProductsCommand with invalid negative page size
    /// </summary>
    /// <returns>An invalid GetProductsCommand with negative page size</returns>
    public static GetProductsCommand CreateInvalidCommandWithNegativePageSize()
    {
        return new GetProductsCommand(1, -10);
    }

    /// <summary>
    /// Creates a GetProductsCommand with invalid excessive page size
    /// </summary>
    /// <returns>An invalid GetProductsCommand with excessive page size</returns>
    public static GetProductsCommand CreateInvalidCommandWithExcessivePageSize()
    {
        return new GetProductsCommand(1, 101);
    }

    /// <summary>
    /// Creates a list of test products
    /// </summary>
    /// <returns>A list of test products</returns>
    public static List<Ambev.DeveloperEvaluation.Domain.Entities.Products.Product> CreateTestProducts()
    {
        var products = new List<Ambev.DeveloperEvaluation.Domain.Entities.Products.Product>();
        
        for (int i = 1; i <= 10; i++)
        {
            var product = new Ambev.DeveloperEvaluation.Domain.Entities.Products.Product
            {
                Id = Guid.NewGuid(),
                Name = $"Test Product {i}",
                Description = $"Description for product {i}",
                Sku = $"SKU-{i:D6}",
                Price = 10.99m * i,
                StockQuantity = i * 10,
                Category = "Test Category",
                BranchExternalId = Guid.NewGuid(),
                BranchName = "Test Branch",
                Status = ProductStatus.Active,
                CreatedAt = DateTime.UtcNow.AddDays(-i)
            };
            
            products.Add(product);
        }
        
        return products;
    }
} 