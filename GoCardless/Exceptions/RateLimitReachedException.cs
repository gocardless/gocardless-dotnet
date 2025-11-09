using System;
using System.Collections.Generic;
using System.Linq;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    public class RateLimitReachedException : InvalidApiUsageException
    {
        /// <summary>
        ///Exception thrown when you have reached the rate limit for the number of requests you can make
        ///Currently the default rate limit is sent to 1000 requests per minute per integrator
        /// </summary>
        internal RateLimitReachedException(ApiErrorResponse apiErrorResponse)
            : base(apiErrorResponse) { }

        public new IReadOnlyList<Error.IError> Errors =>
            base.Errors.Cast<Error.IError>().ToList().AsReadOnly();
    }
}
