using System;
using GoCardless.Errors;

namespace GoCardless.Exceptions
{
    /// <summary>
    ///Provides a mapping between API error responses and exceptions.  Users of this
    ///library will not need to use this class.
    /// </summary>
    public static class ErrorMapperExtension
    {
        /// <summary>
        ///Maps an error response to an exception.
        ///@param error the error response to map
        /// </summary>
        public static ApiException ToException(this ApiErrorResponse apiErrorResponse)
        {
            switch (apiErrorResponse.Error.Type)
            {
                case ApiErrorType.GOCARDLESS:
                    return new InternalException(apiErrorResponse);
                case ApiErrorType.INVALID_API_USAGE:
                    return new InvalidApiUsageException(apiErrorResponse);
                case ApiErrorType.INVALID_STATE:
                    return new InvalidStateException(apiErrorResponse);
                case ApiErrorType.VALIDATION_FAILED:
                    return new ValidationFailedException(apiErrorResponse);
                default:
                    throw new InvalidOperationException($"Unknown ApiErrorType {apiErrorResponse}");

            }
        }
    }
}
