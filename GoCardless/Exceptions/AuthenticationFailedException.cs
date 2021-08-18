using System;
using System.Collections.Generic;
using System.Linq;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    public class AuthenticationFailedException : InvalidApiUsageException
    {
        /// <summary>
        ///Exception thrown when you have failed the authentication check
        /// </summary>
        internal AuthenticationFailedException(ApiErrorResponse apiErrorResponse) :base(apiErrorResponse)
        {
        }
        public new IReadOnlyList<Error.IError> Errors => base.Errors.Cast<Error.IError>().ToList().AsReadOnly();
    }
}
