using System;
using System.Collections.Generic;
using System.Net.Http;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    /// <summary>
    ///Base class for exceptions that are thrown as a result of error responses from
    ///the API.
    /// </summary>
    public class ApiException : Exception
    {
        public ApiErrorResponse ApiErrorResponse { get; }
        public HttpResponseMessage ResponseMessage { get; set; }

        public ApiException(ApiErrorResponse apiErrorResponse) : base(apiErrorResponse.Error.Message)
        {
            this.ApiErrorResponse = apiErrorResponse;
            this.ResponseMessage = apiErrorResponse.ResponseMessage;
        }

        /// <summary>
        ///Returns the type of the error.
        /// </summary>
        public ApiErrorType Type => ApiErrorResponse.Error.Type;

        /// <summary>
        ///Returns the URL to the documentation describing the error.
        /// </summary>
        public string DocumentationUrl => ApiErrorResponse.Error.DocumentationUrl;

        /// <summary>
        ///Returns the ID of the request.  This can be used to help the support
        ///team find your error quickly.
        /// </summary>
        public string RequestId => ApiErrorResponse.Error.RequestId;

        /// <summary>
        ///Returns the HTTP status code.
        /// </summary>
        public int Code => ApiErrorResponse.Error.Code;

        /// <summary>
        ///Returns a list of errors.
        /// </summary>
        public IReadOnlyList<Error> Errors => ApiErrorResponse?.Error.Errors;

    }
}
