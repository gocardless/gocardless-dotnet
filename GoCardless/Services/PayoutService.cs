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
    /// collected from one or many [payments](#core-endpoints-payments). All the
    /// payments in a payout will have been collected in the same currency.
    /// Payouts are created automatically after a payment has been successfully
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
        /// <param name="request">An optional `PayoutListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of payout resources</returns>
        public Task<PayoutListResponse> ListAsync(
            PayoutListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PayoutListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<PayoutListResponse>(
                "GET",
                "/payouts",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of payouts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Payout> All(
            PayoutListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
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
        public IEnumerable<Task<IReadOnlyList<Payout>>> AllAsync(
            PayoutListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
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
        /// <param name="request">An optional `PayoutGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payout resource</returns>
        public Task<PayoutResponse> GetAsync(
            string identity,
            PayoutGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PayoutGetRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PayoutResponse>(
                "GET",
                "/payouts/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Updates a payout object. This accepts only the metadata parameter.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "PO".</param>
        /// <param name="request">An optional `PayoutUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payout resource</returns>
        public Task<PayoutResponse> UpdateAsync(
            string identity,
            PayoutUpdateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PayoutUpdateRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PayoutResponse>(
                "PUT",
                "/payouts/:identity",
                urlParams,
                request,
                null,
                "payouts",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// payouts.
    /// </summary>
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
        /// Unique identifier, beginning with "BA".
        /// </summary>
        [JsonProperty("creditor_bank_account")]
        public string CreditorBankAccount { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PayoutCurrency? Currency { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayoutCurrency
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

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters. _Note:_
        /// This should not be used for storing PII data.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Whether a payout contains merchant revenue or partner fees.
        /// </summary>
        [JsonProperty("payout_type")]
        public PayoutPayoutType? PayoutType { get; set; }

        /// <summary>
        /// Whether a payout contains merchant revenue or partner fees.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayoutPayoutType
        {
            /// <summary>`payout_type` with a value of "merchant"</summary>
            [EnumMember(Value = "merchant")]
            Merchant,

            /// <summary>`payout_type` with a value of "partner"</summary>
            [EnumMember(Value = "partner")]
            Partner,
        }

        /// <summary>
        /// Reference which appears on the creditor's bank statement.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the payout has been created, but not yet sent to your
        /// bank or it is in the process of being exchanged through our FX
        /// provider.</li>
        /// <li>`paid`: the payout has been sent to the your bank. FX payouts
        /// will become `paid` after we emit the `fx_rate_confirmed`
        /// webhook.</li>
        /// <li>`bounced`: the payout bounced when sent, the payout can be
        /// retried.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public PayoutStatus? Status { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the payout has been created, but not yet sent to your
        /// bank or it is in the process of being exchanged through our FX
        /// provider.</li>
        /// <li>`paid`: the payout has been sent to the your bank. FX payouts
        /// will become `paid` after we emit the `fx_rate_confirmed`
        /// webhook.</li>
        /// <li>`bounced`: the payout bounced when sent, the payout can be
        /// retried.</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayoutStatus
        {
            /// <summary>`status` with a value of "pending"</summary>
            [EnumMember(Value = "pending")]
            Pending,

            /// <summary>`status` with a value of "paid"</summary>
            [EnumMember(Value = "paid")]
            Paid,

            /// <summary>`status` with a value of "bounced"</summary>
            [EnumMember(Value = "bounced")]
            Bounced,
        }
    }

    /// <summary>
    /// Retrieves the details of a single payout. For an example of how to
    /// reconcile the transactions in a payout, see [this
    /// guide](#events-reconciling-payouts-with-events).
    /// </summary>
    public class PayoutGetRequest { }

    /// <summary>
    /// Updates a payout object. This accepts only the metadata parameter.
    /// </summary>
    public class PayoutUpdateRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single payout.
    /// </summary>
    public class PayoutResponse : ApiResponse
    {
        /// <summary>
        /// The payout from the response.
        /// </summary>
        [JsonProperty("payouts")]
        public Payout Payout { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of payouts.
    /// </summary>
    public class PayoutListResponse : ApiResponse
    {
        /// <summary>
        /// The list of payouts from the response.
        /// </summary>
        [JsonProperty("payouts")]
        public IReadOnlyList<Payout> Payouts { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
