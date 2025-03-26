

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
    /// Service class for working with refund resources.
    ///
    /// Refund objects represent (partial) refunds of a
    /// [payment](#core-endpoints-payments) back to the
    /// [customer](#core-endpoints-customers).
    /// 
    /// GoCardless will notify you via a [webhook](#appendix-webhooks) whenever
    /// a refund is created, and will update the `amount_refunded` property of
    /// the payment.
    /// </summary>

    public class RefundService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public RefundService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new refund object.
        /// 
        /// This fails with:<a name="total_amount_confirmation_invalid"></a><a
        /// name="number_of_refunds_exceeded"></a><a
        /// name="available_refund_amount_insufficient"></a>
        /// 
        /// - `total_amount_confirmation_invalid` if the confirmation amount
        /// doesn't match the total amount refunded for the payment. This
        /// safeguard is there to prevent two processes from creating refunds
        /// without awareness of each other.
        /// 
        /// - `number_of_refunds_exceeded` if twenty five or more refunds have
        /// already been created against the payment.
        /// 
        /// - `available_refund_amount_insufficient` if the creditor does not
        /// have sufficient balance for refunds available to cover the cost of
        /// the requested refund.
        /// 
        /// </summary>
        /// <param name="request">An optional `RefundCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single refund resource</returns>
        public Task<RefundResponse> CreateAsync(RefundCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RefundCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<RefundResponse>("POST", "/refunds", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "refunds", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your refunds.
        /// </summary>
        /// <param name="request">An optional `RefundListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of refund resources</returns>
        public Task<RefundListResponse> ListAsync(RefundListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RefundListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<RefundListResponse>("GET", "/refunds", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of refunds.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Refund> All(RefundListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RefundListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Refunds)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of refunds.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Refund>>> AllAsync(RefundListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RefundListRequest();

            return new TaskEnumerable<IReadOnlyList<Refund>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Refunds, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves all details for a single refund
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "RF".</param> 
        /// <param name="request">An optional `RefundGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single refund resource</returns>
        public Task<RefundResponse> GetAsync(string identity, RefundGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RefundGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<RefundResponse>("GET", "/refunds/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Updates a refund object.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "RF".</param> 
        /// <param name="request">An optional `RefundUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single refund resource</returns>
        public Task<RefundResponse> UpdateAsync(string identity, RefundUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RefundUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<RefundResponse>("PUT", "/refunds/:identity", urlParams, request, null, "refunds", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new refund object.
    /// 
    /// This fails with:<a name="total_amount_confirmation_invalid"></a><a
    /// name="number_of_refunds_exceeded"></a><a
    /// name="available_refund_amount_insufficient"></a>
    /// 
    /// - `total_amount_confirmation_invalid` if the confirmation amount doesn't
    /// match the total amount refunded for the payment. This safeguard is there
    /// to prevent two processes from creating refunds without awareness of each
    /// other.
    /// 
    /// - `number_of_refunds_exceeded` if twenty five or more refunds have
    /// already been created against the payment.
    /// 
    /// - `available_refund_amount_insufficient` if the creditor does not have
    /// sufficient balance for refunds available to cover the cost of the
    /// requested refund.
    /// 
    /// </summary>
    public class RefundCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Amount in minor unit (e.g. pence in GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public RefundLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a Refund.
        /// </summary>
        public class RefundLinks
        {
                
                /// <summary>
                            ///  ID of the [mandate](#core-endpoints-mandates) against which the
            /// refund is being made. <br /> <p
            /// class="restricted-notice"><strong>Restricted</strong>: You must
            /// request access to Mandate Refunds by contacting <a
            /// href="mailto:support@gocardless.com">our support team</a>.</p>
                /// </summary>
                [JsonProperty("mandate")]
                public string Mandate { get; set; }
                
                /// <summary>
                            /// ID of the [payment](#core-endpoints-payments) against which the
            /// refund is being made.
                /// </summary>
                [JsonProperty("payment")]
                public string Payment { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// An optional reference that will appear on your customer's bank
        /// statement. The character limit for this reference is dependent on
        /// the scheme.<br /> <strong>ACH</strong> - 10 characters<br />
        /// <strong>Autogiro</strong> - 11 characters<br />
        /// <strong>Bacs</strong> - 10 characters<br /> <strong>BECS</strong> -
        /// 30 characters<br /> <strong>BECS NZ</strong> - 12 characters<br />
        /// <strong>Betalingsservice</strong> - 30 characters<br />
        /// <strong>Faster Payments</strong> - 18 characters<br />
        /// <strong>PAD</strong> - scheme doesn't offer references<br />
        /// <strong>PayTo</strong> - 18 characters<br /> <strong>SEPA</strong> -
        /// 140 characters<br /> Note that this reference must be unique (for
        /// each merchant) for the BECS scheme as it is a scheme requirement. <p
        /// class='restricted-notice'><strong>Restricted</strong>: You can only
        /// specify a payment reference for Bacs payments (that is, when
        /// collecting from the UK) if you're on the <a
        /// href='https://gocardless.com/pricing'>GoCardless Plus, Pro or
        /// Enterprise packages</a>.</p> <p
        /// class='restricted-notice'><strong>Restricted</strong>: You can not
        /// specify a payment reference for Faster Payments.</p>
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Total expected refunded amount in minor unit (e.g. pence/cents/öre).
        /// If there are
        /// other partial refunds against this payment, this value should be the
        /// sum of the
        /// existing refunds plus the amount of the refund being created.
        /// <br />
        /// Must be supplied if `links[payment]` is present.
        /// <p class="notice">It is possible to opt out of requiring
        /// `total_amount_confirmation`, please contact <a
        /// href="mailto:support@gocardless.com">our support team</a> for more
        /// information.</p>
        /// </summary>
        [JsonProperty("total_amount_confirmation")]
        public int? TotalAmountConfirmation { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// refunds.
    /// </summary>
    public class RefundListRequest
    {

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
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "MD". Note that this prefix may
        /// not apply to mandates created before 2016.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PM".
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }

        /// <summary>
        /// Whether a refund was issued against a mandate or a payment. One of:
        /// <ul>
        ///   <li>`payment`: <em>default</em> returns refunds created against
        /// payments only</li>
        ///   <li>`mandate`: returns refunds created against mandates only</li>
        /// </ul>
        /// </summary>
        [JsonProperty("refund_type")]
        public RefundRefundType? RefundType { get; set; }
            
        /// <summary>
        /// Whether a refund was issued against a mandate or a payment. One of:
        /// <ul>
        ///   <li>`payment`: <em>default</em> returns refunds created against
        /// payments only</li>
        ///   <li>`mandate`: returns refunds created against mandates only</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum RefundRefundType
        {
    
            /// <summary>`refund_type` with a value of "mandate"</summary>
            [EnumMember(Value = "mandate")]
            Mandate,
            /// <summary>`refund_type` with a value of "payment"</summary>
            [EnumMember(Value = "payment")]
            Payment,
        }
    }

        
    /// <summary>
    /// Retrieves all details for a single refund
    /// </summary>
    public class RefundGetRequest
    {
    }

        
    /// <summary>
    /// Updates a refund object.
    /// </summary>
    public class RefundUpdateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single refund.
    /// </summary>
    public class RefundResponse : ApiResponse
    {
        /// <summary>
        /// The refund from the response.
        /// </summary>
        [JsonProperty("refunds")]
        public Refund Refund { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of refunds.
    /// </summary>
    public class RefundListResponse : ApiResponse
    {
        /// <summary>
        /// The list of refunds from the response.
        /// </summary>
        [JsonProperty("refunds")]
        public IReadOnlyList<Refund> Refunds { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
