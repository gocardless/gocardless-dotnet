

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
    /// Service class for working with payout resources.
    ///
    /// Payouts represent transfers from GoCardless to a
    /// [creditor](#core-endpoints-creditors). Each payout contains the funds
    /// collected from one or many [payments](#core-endpoints-payments). Payouts
    /// are created automatically after a payment has been successfully
    /// collected.
    /// </summary>

    public class PayoutService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public PayoutService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your payouts.
        /// </summary>
        /// <returns>A set of payout resources</returns>
        public Task<PayoutListResponse> ListAsync(PayoutListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PayoutListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<PayoutListResponse>("GET", "/payouts", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of payouts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Payout> All(PayoutListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PayoutListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Payouts)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of payouts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Payout>>> AllAsync(PayoutListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PayoutListRequest();

            return new TaskEnumerable<IReadOnlyList<Payout>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Payouts, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of a single payout. For an example of how to
        /// reconcile the transactions in a payout, see [this
        /// guide](#events-reconciling-payouts-with-events).
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "PO".</param>
        /// <returns>A single payout resource</returns>
        public Task<PayoutResponse> GetAsync(string identity, PayoutGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PayoutGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PayoutResponse>("GET", "/payouts/:identity", urlParams, request, null, null, customiseRequestMessage);
        }
    }

        
    public class PayoutListRequest
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
        /// Unique identifier, beginning with "CR".
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BA".
        /// </summary>
        [JsonProperty("creditor_bank_account")]
        public string CreditorBankAccount { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently only "GBP", "EUR", and "SEK" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PayoutCurrency? Currency { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayoutCurrency
        {
            /// <summary>
            /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
            /// currency code. Currently only "GBP", "EUR", and "SEK" are
            /// supported.
            /// </summary>
    
            [EnumMember(Value = "GBP")]
            GBP,
            [EnumMember(Value = "EUR")]
            EUR,
            [EnumMember(Value = "SEK")]
            SEK,
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Whether a payout contains merchant revenue or partner fees.
        /// </summary>
        [JsonProperty("payout_type")]
        public PayoutPayoutType? PayoutType { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayoutPayoutType
        {
            /// <summary>
            /// Whether a payout contains merchant revenue or partner fees.
            /// </summary>
    
            [EnumMember(Value = "merchant")]
            Merchant,
            [EnumMember(Value = "partner")]
            Partner,
        }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the payout has
        /// been created, but not yet sent to the banks</li>
        ///
        /// <li>`paid`: the payout has been sent to the banks</li>
        ///
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public PayoutStatus? Status { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayoutStatus
        {
            /// <summary>
            /// One of:
            /// <ul>
            /// <li>`pending`: the
            /// payout has been created, but not yet sent to the banks</li>
    
            ///        /// <li>`paid`: the payout has been sent to the
            /// banks</li>
            /// </ul>
            /// </summary>
    
            [EnumMember(Value = "pending")]
            Pending,
            [EnumMember(Value = "paid")]
            Paid,
        }
    }

        
    public class PayoutGetRequest
    {
    }

    public class PayoutResponse : ApiResponse
    {
        [JsonProperty("payouts")]
        public Payout Payout { get; private set; }
    }

    public class PayoutListResponse : ApiResponse
    {
        public IReadOnlyList<Payout> Payouts { get; private set; }
        public Meta Meta { get; private set; }
    }
}
