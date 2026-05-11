using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    /// <summary>
    ///Exception thrown when the parameters submitted with your request were invalid.
    /// </summary>
    public class ValidationFailedException : ApiException
    {
        internal ValidationFailedException(ApiErrorResponse apiErrorResponse)
            : base(apiErrorResponse) { }
    }
}
