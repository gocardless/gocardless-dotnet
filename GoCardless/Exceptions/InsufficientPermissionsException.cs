using System.Collections.Generic;
using System.Linq;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    public class InsufficientPermissionsException : InvalidApiUsageException
    {
        /// <summary>
        ///Specific Exception thrown when the requestor has a lack of permission for the desired action
        /// </summary>
        internal InsufficientPermissionsException(ApiErrorResponse apiErrorResponse) : base(apiErrorResponse)
        {
        }

        public new IReadOnlyList<Error.IError> Errors => base.Errors.Cast<Error.IError>().ToList().AsReadOnly();
    }
}

