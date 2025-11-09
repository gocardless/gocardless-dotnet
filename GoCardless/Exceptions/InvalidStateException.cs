using System.Collections.Generic;
using System.Linq;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    /// <summary>
    ///Exception thrown when the action you are trying to perform is invalid due to
    ///the state of the resource you are requesting it on.
    /// </summary>
    public class InvalidStateException : ApiException
    {
        internal InvalidStateException(ApiErrorResponse apiErrorResponse)
            : base(apiErrorResponse) { }

        public new IReadOnlyList<Error.IError> Errors =>
            base.Errors.Cast<Error.IError>().ToList().AsReadOnly();
    }
}
