

using System.Collections.Generic;
using System.Linq;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    /// <summary>
    ///Exception thrown when an internal error occurred while processing your request.
    /// </summary>
    public class InternalException : ApiException {
        internal InternalException(ApiErrorResponse apiErrorResponse) : base(apiErrorResponse){
        }

        public new IReadOnlyList<Error.IError> Errors => base.Errors.Cast<Error.IError>().ToList().AsReadOnly();

    }
}
