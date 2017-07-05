

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
    /// GoCardless will
    /// notify you via a [webhook](#appendix-webhooks) whenever a refund is
    /// created, and will update the `amount_refunded` property of the payment.
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
        /// This fails
        /// with:<a name="refund_payment_invalid_state"></a><a
        /// name="total_amount_confirmation_invalid"></a><a
        /// name="number_of_refunds_exceeded"></a>
        /// 
        /// -
        /// `refund_payment_invalid_state` error if the linked
        /// [payment](#core-endpoints-payments) isn't either `confirmed` or
        /// `paid_out`.
        /// 
        /// -
        /// `total_amount_confirmation_invalid` if the confirmation amount
        /// doesn't match the total amount refunded for the payment. This
        /// safeguard is there to prevent two processes from creating refunds
        /// without awareness of each other.
        /// 
        /// -
        /// `number_of_refunds_exceeded` if five or more refunds have already
        /// been created against the payment.
        /// 
        /// </summary>
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

        
    public class RefundCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Amount in pence/cents/öre.
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        [JsonProperty("links")]
        public RefundLinks Links { get; set; }
        public class RefundLinks
        {

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
        /// An optional refund reference, displayed on your customer's bank
        /// statement. This can be up to 18 characters long for Bacs payments,
        /// 140 characters for SEPA payments, or 25 characters for Autogiro
        /// payments.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Total expected refunded amount in pence/cents/öre. If there are
        /// other partial refunds against this payment, this value should be the
        /// sum of the existing refunds plus the amount of the refund being
        /// created.
        /// </summary>
        [JsonProperty("total_amount_confirmation")]
        public int? TotalAmountConfirmation { get; set; }

        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
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

        [JsonProperty("created_at")]
        public CreatedAtParam CreatedAt { get; set; }

        public class CreatedAtParam
        {
            /// <summary>
            /// Limit to records created within certain times
            /// </summary>
            [JsonProperty("gt")]
            public DateTimeOffset? GreaterThan { get; set; }

            [JsonProperty("gte")]
            public DateTimeOffset? GreaterThanOrEqual { get; set; }

            [JsonProperty("lt")]
            public DateTimeOffset? LessThan { get; set; }

            [JsonProperty("lte")]
            public DateTimeOffset? LessThanOrEqual { get; set; }
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PM".
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }
    }

        
    public class RefundGetRequest
    {
    }

        
    public class RefundUpdateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

    public class RefundResponse : ApiResponse
    {
        [JsonProperty("refunds")]
        public Refund Refund { get; private set; }
    }

    public class RefundListResponse : ApiResponse
    {
        public IReadOnlyList<Refund> Refunds { get; private set; }
        public Meta Meta { get; private set; }
    }
}
