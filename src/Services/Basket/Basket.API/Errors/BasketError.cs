using ErrorOr;

namespace Basket.API.Errors
{
    public static class BasketError
    {
        public static Error NotFoundError(string userName) => Error.NotFound("Basket.NotFound", $"No basket found for user {userName}");
    }
}
