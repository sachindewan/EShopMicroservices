using ErrorOr;

namespace BuildingBlock.Errors
{
    public class ApplicationError
    {
        public static Error ValidationError(string error) => Error.Validation("ApplicationError.Validation", error);
    }
}
