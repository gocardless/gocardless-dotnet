using System.Collections.Generic;

namespace GoCardless.Errors
{
    public class ApiError
    {
        /// <summary>
        /// Returns a short message describing the error.
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// Returns the URL to the documentation describing the error.
        /// </summary>
        public string DocumentationUrl { get; private set; }

        /// <summary>
        /// Returns the type of the error.
        /// </summary>
        public ApiErrorType Type { get; internal set; }

        /// <summary>
        /// Returns the ID of the request.  This can be used to help the support
        /// team find your error quickly.
        /// </summary>
        public string RequestId { get; private set; }

        /// <summary>
        /// Returns the HTTP status code.
        /// </summary>
        public int Code { get; internal set; }

        /// <summary>
        /// Returns a list of errors.
        /// </summary>
        public IReadOnlyList<Error> Errors { get; private set; }
    }
}
