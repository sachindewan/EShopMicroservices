using ErrorOr;

namespace BuildingBlocks.Errors
{
    public class ApplicationError
    {
        public static Error ValidationError(string error) => Error.Validation("ApplicationError.Validation", error);
    }
}
