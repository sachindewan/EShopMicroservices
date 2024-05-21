using ErrorOr;

namespace BuildingBlock.Errors
{
    public static class ProductErrors
    {
        public static Error CreateFailed = Error.Failure("Products.CreateFailed", "products creation failed please contact administrator or analyze logs");

        public static Error NotFound= Error.NotFound("Products.ProductNotFound", "requested resource does not exist please contact administrator.");
        public static Error BadRequest = Error.Failure("Products.BadRequest", "bad request please check the request and try again!!");
    }
}
