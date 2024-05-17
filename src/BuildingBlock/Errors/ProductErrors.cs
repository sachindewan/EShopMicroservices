using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Errors
{
    public static class ProductErrors
    {
        public static Error CreateFailed = Error.Failure("Products.CreateFailed", "products creation failed please contact administrator or analyze logs");
    }
}
