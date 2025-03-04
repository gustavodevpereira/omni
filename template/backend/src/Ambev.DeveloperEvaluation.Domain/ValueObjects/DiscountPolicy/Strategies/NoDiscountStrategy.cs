using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy.Strategies
{
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(int quantity) => 0m;
    }
}
