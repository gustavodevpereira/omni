using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sale
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public string ProductExternalId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public decimal TotalAmount { get; private set; }

        public SaleItem(string productExternalId, string productName, int quantity, decimal unitPrice)
        {
            if (quantity < 1 || quantity > 20)
                throw new DomainException("Quantity must be between 1 and 20.");

            Id = Guid.NewGuid();
            ProductExternalId = productExternalId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;

            DiscountPercentage = GetDiscountPercentage(quantity);

            TotalAmount = CalculateTotalAmount();
        }

        private decimal GetDiscountPercentage(int quantity)
        {
            var discountPolicy = new DiscountPolicy();
            return discountPolicy.GetDiscountPercentage(quantity);
        }

        private decimal CalculateTotalAmount()
        {
            decimal grossAmount = Quantity * UnitPrice;
            decimal discountAmount = grossAmount * DiscountPercentage;
            return grossAmount - discountAmount;
        }
    }
}
