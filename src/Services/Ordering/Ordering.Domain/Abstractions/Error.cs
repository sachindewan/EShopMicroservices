using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions
{
    public static class OrderError
    {
        public static readonly Error QuantityLessThanZero =  Error.Validation("Order.QuantityLessThanZero", "Order quantity can not be less than zero.");
        public static readonly Error PriceLessThanZero =  Error.Validation("Order.PriceLessThanZero", "Order quantity can not be less than zero.");
    }
    public static class ProductError
    {
        public static readonly Error NameIsNullOrEmpty = Error.Validation("Product.NameIsNullOrEmpty", "Product name is missing.");
        public static readonly Error PriceLessThanZero = Error.Validation("Product.PriceLessThanZero", "Order quantity can not be less than zero.");
    }
}
