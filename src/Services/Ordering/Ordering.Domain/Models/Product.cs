using Ordering.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public class Product : Entity<Guid>
    {
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;

        public static Product Create(Guid id, string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
              //  return ProductError.NameIsNullOrEmpty;
            }
            if (price<=0)
            {
               // return ProductError.PriceLessThanZero;
            }

            var product = new Product
            {
                Id = id,
                Name = name,
                Price = price
            };

            return product;
        }
    }
}
