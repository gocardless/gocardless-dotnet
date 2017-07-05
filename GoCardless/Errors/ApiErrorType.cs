using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Errors
{
    /// <summary>
    /// Types of error that can be returned by the API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ApiErrorType {
        /// <summary>
        /// An internal error occurred while processing your request. This should be
        /// reported to our support team with the id, so we can resolve the issue.
        /// </summary>
        [EnumMember(Value = "gocardless")]
        GOCARDLESS,
        /// <summary>
        /// This is an error with the request you made. It could be an invalid URL, the
        /// authentication header could be missing, invalid, or grant insufficient
        /// permissions, you may have reached your rate limit, or the syntax of your
        /// request could be incorrect. The errors will give more detail of the specific
        /// issue.
        /// </summary>
        [EnumMember(Value = "invalid_api_usage")]
        INVALID_API_USAGE,
        /// <summary>
        /// The action you are trying to perform is invalid due to the state of the
        /// resource you are requesting it on. For example, a payment you are trying to
        /// cancel might already have been submitted. The errors will give more details.
        /// </summary>
        [EnumMember(Value = "invalid_state")]
        INVALID_STATE,
        /// <summary>
        /// The parameters submitted with your request were invalid. Details of which
        /// fields were invalid and why are included in the response.
        /// </summary>
        [EnumMember(Value = "validation_failed")]
        VALIDATION_FAILED
    }
}
