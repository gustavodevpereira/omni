using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public record DiscountPolicy
    {
        public decimal GetDiscountPercentage(int quantity)
        {
            if (quantity < 4)
                return 0m;
            if (quantity >= 4 && quantity < 10)
                return 0.1m;  // 10% de desconto
            if (quantity >= 10 && quantity <= 20)
                return 0.2m;  // 20% de desconto

            throw new ArgumentException("A quantidade não pode exceder 20.");
        }
    }
}
