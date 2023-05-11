

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
    /// Service class for working with negative balance limit resources.
    ///
    /// The negative balance limit is a threshold for the creditor balance
    /// beyond which refunds are not permitted. The default limit is zero —
    /// refunds are not permitted if the creditor has a negative balance. The
    /// limit can be changed on a per-creditor basis.
    /// 
    /// </summary>

    public class NegativeBalanceLimitService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public NegativeBalanceLimitService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// negative balance limits.
        /// </summary>
        /// <param name="request">An optional `NegativeBalanceLimitListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of negative balance limit resources</returns>
        public Task<NegativeBalanceLimitListResponse> ListAsync(NegativeBalanceLimitListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new NegativeBalanceLimitListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<NegativeBalanceLimitListResponse>("GET", "/negative_balance_limits", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of negative balance limits.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<NegativeBalanceLimit> All(NegativeBalanceLimitListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new NegativeBalanceLimitListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.NegativeBalanceLimits)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of negative balance limits.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<NegativeBalanceLimit>>> AllAsync(NegativeBalanceLimitListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new NegativeBalanceLimitListRequest();

            return new TaskEnumerable<IReadOnlyList<NegativeBalanceLimit>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.NegativeBalanceLimits, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Creates a new negative balance limit, which also deactivates the
        /// existing limit (if present) for that currency and creditor
        /// combination.
        /// </summary>
        /// <param name="request">An optional `NegativeBalanceLimitCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single negative balance limit resource</returns>
        public Task<NegativeBalanceLimitResponse> CreateAsync(NegativeBalanceLimitCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new NegativeBalanceLimitCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<NegativeBalanceLimitResponse>("POST", "/negative_balance_limits", urlParams, request, null, "negative_balance_limits", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
    /// negative balance limits.
    /// </summary>
    public class NegativeBalanceLimitListRequest
    {

        /// <summary>
        /// Whether or not this limit is currently active
        /// </summary>
        [JsonProperty("active")]
        public NegativeBalanceLimitActive? Active { get; set; }
            
        /// <summary>
        /// Whether or not this limit is currently active
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum NegativeBalanceLimitActive
        {
    
            /// <summary>`active` with a value of "true"</summary>
            [EnumMember(Value = "true")]
            True,
            /// <summary>`active` with a value of "false"</summary>
            [EnumMember(Value = "false")]
            False,
        }

        /// <summary>
        /// Cursor pointing to the start of the desired set.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        /// Cursor pointing to the end of the desired set.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }

        /// <summary>
        /// Limit to records created within certain times.
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
            ///Limit to records created on or before the specified date-time.
            /// </summary>
            [JsonProperty("lte")]
            public DateTimeOffset? LessThanOrEqual { get; set; }
        }

        /// <summary>
        /// Unique identifier, beginning with "CR".
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public NegativeBalanceLimitCurrency? Currency { get; set; }
            
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum NegativeBalanceLimitCurrency
        {
    
            /// <summary>`currency` with a value of "AUD"</summary>
            [EnumMember(Value = "AUD")]
            AUD,
            /// <summary>`currency` with a value of "CAD"</summary>
            [EnumMember(Value = "CAD")]
            CAD,
            /// <summary>`currency` with a value of "DKK"</summary>
            [EnumMember(Value = "DKK")]
            DKK,
            /// <summary>`currency` with a value of "EUR"</summary>
            [EnumMember(Value = "EUR")]
            EUR,
            /// <summary>`currency` with a value of "GBP"</summary>
            [EnumMember(Value = "GBP")]
            GBP,
            /// <summary>`currency` with a value of "NZD"</summary>
            [EnumMember(Value = "NZD")]
            NZD,
            /// <summary>`currency` with a value of "SEK"</summary>
            [EnumMember(Value = "SEK")]
            SEK,
            /// <summary>`currency` with a value of "USD"</summary>
            [EnumMember(Value = "USD")]
            USD,
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

        
    /// <summary>
    /// Creates a new negative balance limit, which also deactivates the
    /// existing limit (if present) for that currency and creditor combination.
    /// </summary>
    public class NegativeBalanceLimitCreateRequest
    {

        /// <summary>
        /// The limit amount in pence (e.g. 10000 for a -100 GBP limit).
        /// </summary>
        [JsonProperty("balance_limit")]
        public int? BalanceLimit { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public NegativeBalanceLimitCurrency? Currency { get; set; }
            
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum NegativeBalanceLimitCurrency
        {
    
            /// <summary>`currency` with a value of "AUD"</summary>
            [EnumMember(Value = "AUD")]
            AUD,
            /// <summary>`currency` with a value of "CAD"</summary>
            [EnumMember(Value = "CAD")]
            CAD,
            /// <summary>`currency` with a value of "DKK"</summary>
            [EnumMember(Value = "DKK")]
            DKK,
            /// <summary>`currency` with a value of "EUR"</summary>
            [EnumMember(Value = "EUR")]
            EUR,
            /// <summary>`currency` with a value of "GBP"</summary>
            [EnumMember(Value = "GBP")]
            GBP,
            /// <summary>`currency` with a value of "NZD"</summary>
            [EnumMember(Value = "NZD")]
            NZD,
            /// <summary>`currency` with a value of "SEK"</summary>
            [EnumMember(Value = "SEK")]
            SEK,
            /// <summary>`currency` with a value of "USD"</summary>
            [EnumMember(Value = "USD")]
            USD,
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public NegativeBalanceLimitLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a NegativeBalanceLimit.
        /// </summary>
        public class NegativeBalanceLimitLinks
        {
                
                /// <summary>
                            /// ID of the [creditor](#core-endpoints-creditors) this limit
            /// relates to
                /// </summary>
                [JsonProperty("creditor")]
                public string Creditor { get; set; }
        }

        /// <summary>
        /// the reason this limit was created
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single negative balance limit.
    /// </summary>
    public class NegativeBalanceLimitResponse : ApiResponse
    {
        /// <summary>
        /// The negative balance limit from the response.
        /// </summary>
        [JsonProperty("negative_balance_limits")]
        public NegativeBalanceLimit NegativeBalanceLimit { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of negative balance limits.
    /// </summary>
    public class NegativeBalanceLimitListResponse : ApiResponse
    {
        /// <summary>
        /// The list of negative balance limits from the response.
        /// </summary>
        [JsonProperty("negative_balance_limits")]
        public IReadOnlyList<NegativeBalanceLimit> NegativeBalanceLimits { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
