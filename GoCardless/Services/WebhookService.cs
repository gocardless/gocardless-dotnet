using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GoCardless.Internals;
using GoCardless.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Services
{
    /// <summary>
    /// Service class for working with webhook resources.
    ///
    ///  Basic description of a webhook
    /// </summary>
    public class WebhookService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public WebhookService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        ///  Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        ///  your webhooks.
        /// </summary>
        /// <param name="request">An optional `WebhookListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of webhook resources</returns>
        public Task<WebhookListResponse> ListAsync(
            WebhookListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new WebhookListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<WebhookListResponse>(
                "GET",
                "/webhooks",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of webhooks.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Webhook> All(
            WebhookListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new WebhookListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Webhooks)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of webhooks.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Webhook>>> AllAsync(
            WebhookListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new WebhookListRequest();

            return new TaskEnumerable<IReadOnlyList<Webhook>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Webhooks, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        ///  Retrieves the details of an existing webhook.
        /// </summary>
        ///  <param name="identity">Unique identifier, beginning with "WB".</param>
        /// <param name="request">An optional `WebhookGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single webhook resource</returns>
        public Task<WebhookResponse> GetAsync(
            string identity,
            WebhookGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new WebhookGetRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<WebhookResponse>(
                "GET",
                "/webhooks/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        ///  Requests for a previous webhook to be sent again
        /// </summary>
        ///  <param name="identity">Unique identifier, beginning with "WB".</param>
        /// <param name="request">An optional `WebhookRetryRequest` representing the body for this retry request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single webhook resource</returns>
        public Task<WebhookResponse> RetryAsync(
            string identity,
            WebhookRetryRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new WebhookRetryRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<WebhookResponse>(
                "POST",
                "/webhooks/:identity/actions/retry",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    ///  Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    ///  webhooks.
    /// </summary>
    public class WebhookListRequest
    {
        /// <summary>
        ///  Cursor pointing to the start of the desired set.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        ///  Cursor pointing to the end of the desired set.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }

        /// <summary>
        ///  Limit to records created within certain times.
        /// </summary>
        [JsonProperty("created_at")]
        public CreatedAtParam CreatedAt { get; set; }

        /// <summary>
        /// Specify filters to limit records by creation time.
        /// </summary>
        public class CreatedAtParam
        {
            /// <summary>
            /// Limit to records created after the specified date-time.
            /// </summary>
            [JsonProperty("gt")]
            public DateTimeOffset? GreaterThan { get; set; }

            /// <summary>
            /// Limit to records created on or after the specified date-time.
            /// </summary>
            [JsonProperty("gte")]
            public DateTimeOffset? GreaterThanOrEqual { get; set; }

            /// <summary>
            /// Limit to records created before the specified date-time.
            /// </summary>
            [JsonProperty("lt")]
            public DateTimeOffset? LessThan { get; set; }

            /// <summary>
            /// Limit to records created on or before the specified date-time.
            /// </summary>
            [JsonProperty("lte")]
            public DateTimeOffset? LessThanOrEqual { get; set; }
        }

        /// <summary>
        ///  Show only test/non test webhooks
        /// </summary>
        [JsonProperty("is_test")]
        public bool? IsTest { get; set; }

        /// <summary>
        ///  Show only test/non test webhooks
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum WebhookIsTest
        {
            /// <summary>`is_test` with a value of "true"</summary>
            [EnumMember(Value = "true")]
            True,

            /// <summary>`is_test` with a value of "false"</summary>
            [EnumMember(Value = "false")]
            False,
        }

        /// <summary>
        ///  Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        ///  Show only successful/failed webhooks
        /// </summary>
        [JsonProperty("successful")]
        public bool? Successful { get; set; }

        /// <summary>
        ///  Show only successful/failed webhooks
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum WebhookSuccessful
        {
            /// <summary>`successful` with a value of "true"</summary>
            [EnumMember(Value = "true")]
            True,

            /// <summary>`successful` with a value of "false"</summary>
            [EnumMember(Value = "false")]
            False,
        }
    }

    /// <summary>
    ///  Retrieves the details of an existing webhook.
    /// </summary>
    public class WebhookGetRequest { }

    /// <summary>
    ///  Requests for a previous webhook to be sent again
    /// </summary>
    public class WebhookRetryRequest { }

    /// <summary>
    /// An API response for a request returning a single webhook.
    /// </summary>
    public class WebhookResponse : ApiResponse
    {
        /// <summary>
        /// The webhook from the response.
        /// </summary>
        [JsonProperty("webhooks")]
        public Webhook Webhook { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of webhooks.
    /// </summary>
    public class WebhookListResponse : ApiResponse
    {
        /// <summary>
        /// The list of webhooks from the response.
        /// </summary>
        [JsonProperty("webhooks")]
        public IReadOnlyList<Webhook> Webhooks { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
