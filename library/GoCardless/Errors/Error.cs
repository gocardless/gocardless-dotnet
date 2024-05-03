using Newtonsoft.Json;

namespace GoCardless.Errors
{
    /// <summary>
    /// An individual error object from an error response.
    /// See https://developer.gocardless.com/api-reference/#api-usage-errors
    /// </summary>
    public class Error : Error.IError, Error.IValidationError
    {
        /// <summary>
        /// Returns a key defining the cause of the error.
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Returns a short message describing the error.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Returns a dictionary of resources linked to the error.
        /// </summary>
        [JsonProperty("links")]
        public ErrorLinks Links { get; set; }

        /// <summary>
        /// For validation errors, returns the invalid field name.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// A JSON pointer for a validation error.
        /// </summary>
        [JsonProperty("request_pointer")]
        public string RequestPointer { get; set; }

        /// <summary>
        /// An API error
        /// </summary>
        public interface IError
        {
            /// <summary>
            /// Returns a key defining the cause of the error.
            /// </summary>
            string Reason { get; }

            /// <summary>
            /// Returns a short message describing the error.
            /// </summary>
            string Message { get; }

            /// <summary>
            /// Returns a dictionary of resources linked to the error.
            /// </summary>
            ErrorLinks Links { get; }
        }

        /// <summary>
        /// A validation error
        /// </summary>
        public interface IValidationError
        {
            /// <summary>
            /// Returns the invalid field name.
            /// </summary>
            string Field { get; }

            /// <summary>
            /// Returns a short message describing the error.
            /// </summary>
            string Message { get; }

            /// <summary>
            /// A JSON pointer for a validation error.
            /// </summary>
            string RequestPointer { get; }
        }
    }
}
