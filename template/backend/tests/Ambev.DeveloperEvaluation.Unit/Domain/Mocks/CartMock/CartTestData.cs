namespace Ambev.DeveloperEvaluation.Unit.Domain.Mocks.CartMock
{
    /// <summary>
    /// Provides test data for Cart entity tests.
    /// </summary>
    public static class CartTestData
    {
        /// <summary>
        /// Generates a valid cart with default values.
        /// </summary>
        /// <param name="customerId">Optional customer ID to use. If not provided, a random GUID will be used.</param>
        /// <returns>A valid cart instance.</returns>
        public static DeveloperEvaluation.Domain.Entities.Carts.Cart GenerateValidCart(Guid? customerId = null)
        {
            return new DeveloperEvaluation.Domain.Entities.Carts.Cart(
                DateTime.UtcNow,
                customerId ?? Guid.NewGuid(),
                "Test Customer",
                Guid.NewGuid(),
                "Test Branch"
            );
        }

        /// <summary>
        /// Generates a valid cart with products.
        /// </summary>
        /// <param name="numberOfProducts">Number of products to add to the cart.</param>
        /// <param name="customerId">Optional customer ID to use. If not provided, a random GUID will be used.</param>
        /// <returns>A valid cart instance with products.</returns>
        public static DeveloperEvaluation.Domain.Entities.Carts.Cart GenerateCartWithProducts(int numberOfProducts = 3, Guid? customerId = null)
        {
            var cart = GenerateValidCart(customerId);

            for (int i = 0; i < numberOfProducts; i++)
            {
                // Add a product with a quantity that avoids discount (1-3 items)
                cart.AddProduct(
                    Guid.NewGuid(),
                    $"Test Product {i + 1}",
                    i % 3 + 1,  // Quantities 1, 2, 3
                    10.0m * (i + 1)  // Different prices
                );
            }

            return cart;
        }

        /// <summary>
        /// Generates a cancelled cart.
        /// </summary>
        /// <param name="customerId">Optional customer ID to use. If not provided, a random GUID will be used.</param>
        /// <returns>A cancelled cart instance.</returns>
        public static DeveloperEvaluation.Domain.Entities.Carts.Cart GenerateCancelledCart(Guid? customerId = null)
        {
            var cart = GenerateValidCart(customerId);
            cart.CancelCart();
            return cart;
        }
    }
}