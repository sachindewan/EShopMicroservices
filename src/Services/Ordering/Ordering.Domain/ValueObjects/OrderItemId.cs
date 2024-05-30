using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public  record OrderItemId
    {
        //public Guid Value { get; }
        //private OrderItemId(Guid value) => Value = value;
        //public static OrderItemId Of(Guid value)
        //{
        //    //[TODO] we can use erroror and result pattern
        //    ArgumentNullException.ThrowIfNull(value);
        //    if (value == Guid.Empty)
        //    {
        //        throw new Exception ("OrderItemId cannot be empty.");
        //    }

        //    return new OrderItemId(value);
        //}
    }
}
