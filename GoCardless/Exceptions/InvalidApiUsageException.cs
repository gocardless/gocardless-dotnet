using System.Collections.Generic;
using System.Linq;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    /// <summary>
    ///Exception thrown when there is an error with the request you made.
    /// </summary>
    public class InvalidApiUsageException : ApiException
    {
        internal InvalidApiUsageException(ApiErrorResponse apiErrorResponse)
            : base(apiErrorResponse) { }

        public new IReadOnlyList<Error.IError> Errors =>
            base.Errors.Cast<Error.IError>().ToList().AsReadOnly();
    }
}
