using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a webhook resource.
    ///
    ///  Basic description of a webhook
    /// </summary>
    public class Webhook
    {
        /// <summary>
        ///  Fixed [timestamp](#api-usage-dates-and-times), recording when this
        ///  resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        ///  Unique identifier, beginning with "WB".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  Boolean value showing whether this was a demo webhook for testing
        /// </summary>
        [JsonProperty("is_test")]
        public bool? IsTest { get; set; }

        /// <summary>
        ///  The body of the request sent to the webhook URL
        /// </summary>
        [JsonProperty("request_body")]
        public string RequestBody { get; set; }

        /// <summary>
        ///  The request headers sent with the webhook
        /// </summary>
        [JsonProperty("request_headers")]
        public IDictionary<string, string> RequestHeaders { get; set; }

        /// <summary>
        ///  The body of the response from the webhook URL
        /// </summary>
        [JsonProperty("response_body")]
        public string ResponseBody { get; set; }

        /// <summary>
        ///  Boolean value indicating the webhook response body was truncated
        /// </summary>
        [JsonProperty("response_body_truncated")]
        public bool? ResponseBodyTruncated { get; set; }

        /// <summary>
        ///  The response code from the webhook request
        /// </summary>
        [JsonProperty("response_code")]
        public int? ResponseCode { get; set; }

        /// <summary>
        ///  The headers sent with the response from the webhook URL
        /// </summary>
        [JsonProperty("response_headers")]
        public IDictionary<string, string> ResponseHeaders { get; set; }

        /// <summary>
        ///  Boolean indicating the content of response headers was truncated
        /// </summary>
        [JsonProperty("response_headers_content_truncated")]
        public bool? ResponseHeadersContentTruncated { get; set; }

        /// <summary>
        ///  Boolean indicating the number of response headers was truncated
        /// </summary>
        [JsonProperty("response_headers_count_truncated")]
        public bool? ResponseHeadersCountTruncated { get; set; }

        /// <summary>
        ///  Boolean indicating whether the request was successful or failed
        /// </summary>
        [JsonProperty("successful")]
        public bool? Successful { get; set; }

        /// <summary>
        ///  URL the webhook was POST-ed to
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
