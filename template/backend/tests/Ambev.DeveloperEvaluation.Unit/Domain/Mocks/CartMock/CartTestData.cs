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
        /// <returns>A valid cart instance.</returns>
        public static DeveloperEvaluation.Domain.Entities.Carts.Cart GenerateValidCart()
        {
            return new DeveloperEvaluation.Domain.Entities.Carts.Cart(
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Test Customer",
                Guid.NewGuid(),
                "Test Branch"
            );
        }

        /// <summary>
        /// Generates a valid cart with products.
        /// </summary>
        /// <param name="numberOfProducts">Number of products to add to the cart.</param>
        /// <returns>A valid cart instance with products.</returns>
        public static DeveloperEvaluation.Domain.Entities.Carts.Cart GenerateCartWithProducts(int numberOfProducts = 3)
        {
            var cart = GenerateValidCart();

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
        /// <returns>A cancelled cart instance.</returns>
        public static DeveloperEvaluation.Domain.Entities.Carts.Cart GenerateCancelledCart()
        {
            var cart = GenerateValidCart();
            cart.CancelCart();
            return cart;
        }
    }
}