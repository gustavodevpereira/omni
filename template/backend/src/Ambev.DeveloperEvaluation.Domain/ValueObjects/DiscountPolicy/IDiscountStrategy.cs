using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy
{
    public interface IDiscountStrategy
    {
        decimal CalculateDiscount(int quantity);
    }
}
