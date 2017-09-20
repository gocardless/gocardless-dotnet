

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
    /// Service class for working with payment resources.
    ///
    /// Payment objects represent payments from a
    /// [customer](#core-endpoints-customers) to a
    /// [creditor](#core-endpoints-creditors), taken against a Direct Debit
    /// [mandate](#core-endpoints-mandates).
    /// 
    /// GoCardless will notify you via a [webhook](#appendix-webhooks) whenever
    /// the state of a payment changes.
    /// </summary>

    public class PaymentService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public PaymentService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// <a name="mandate_is_inactive"></a>Creates a new payment object.
        /// 
        /// This fails with a `mandate_is_inactive` error if the linked
        /// [mandate](#core-endpoints-mandates) is cancelled or has failed.
        /// Payments can be created against mandates with status of:
        /// `pending_customer_approval`, `pending_submission`, `submitted`, and
        /// `active`.
        /// </summary>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> CreateAsync(PaymentCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<PaymentResponse>("POST", "/payments", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "payments", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your payments.
        /// </summary>
        /// <returns>A set of payment resources</returns>
        public Task<PaymentListResponse> ListAsync(PaymentListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<PaymentListResponse>("GET", "/payments", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of payments.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Payment> All(PaymentListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Payments)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of payments.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Payment>>> AllAsync(PaymentListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentListRequest();

            return new TaskEnumerable<IReadOnlyList<Payment>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Payments, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of a single existing payment.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "PM".</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> GetAsync(string identity, PaymentGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>("GET", "/payments/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Updates a payment object. This accepts only the metadata parameter.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "PM".</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> UpdateAsync(string identity, PaymentUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>("PUT", "/payments/:identity", urlParams, request, null, "payments", customiseRequestMessage);
        }

        /// <summary>
        /// Cancels the payment if it has not already been submitted to the
        /// banks. Any metadata supplied to this endpoint will be stored on the
        /// payment cancellation event it causes.
        /// 
        /// This will fail with a `cancellation_failed` error unless the
        /// payment's status is `pending_submission`.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "PM".</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> CancelAsync(string identity, PaymentCancelRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentCancelRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>("POST", "/payments/:identity/actions/cancel", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// <a name="retry_failed"></a>Retries a failed payment if the
        /// underlying mandate is active. You will receive a
        /// `resubmission_requested` webhook, but after that retrying the
        /// payment follows the same process as its initial creation, so you
        /// will receive a `submitted` webhook, followed by a `confirmed` or
        /// `failed` event. Any metadata supplied to this endpoint will be
        /// stored against the payment submission event it causes.
        /// 
        /// This will return a `retry_failed` error if the payment has not
        /// failed.
        /// 
        /// Payments can be retried up to 3 times.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "PM".</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> RetryAsync(string identity, PaymentRetryRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PaymentRetryRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>("POST", "/payments/:identity/actions/retry", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    public class PaymentCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Amount in pence (GBP), cents (EUR), or öre (SEK).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount to be deducted from the payment as the OAuth app's fee,
        /// in pence or cents.
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// A future date on which the payment should be collected. If not
        /// specified, the payment will be collected as soon as possible. This
        /// must be on or after the [mandate](#core-endpoints-mandates)'s
        /// `next_possible_charge_date`, and will be rolled-forwards by
        /// GoCardless if it is not a working day.
        /// </summary>
        [JsonProperty("charge_date")]
        public string ChargeDate { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently only "GBP", "EUR", and "SEK" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PaymentCurrency? Currency { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentCurrency
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
        /// A human-readable description of the payment. This will be included
        /// in the notification email GoCardless sends to your customer if your
        /// organisation does not send its own notifications (see [compliance
        /// requirements](#appendix-compliance-requirements)).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("links")]
        public PaymentLinks Links { get; set; }
        public class PaymentLinks
        {

            /// <summary>
            /// ID of the [mandate](#core-endpoints-mandates) against which this
            /// payment should be collected.
            /// </summary>
            [JsonProperty("mandate")]
            public string Mandate { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// An optional payment reference that will appear on your customer's
        /// bank statement. For Bacs payments this can be up to 10 characters,
        /// for SEPA payments the limit is 140 characters, and for Autogiro
        /// payments the limit is 11 characters. <p
        /// class='restricted-notice'><strong>Restricted</strong>: You can only
        /// specify a payment reference for Bacs payments (that is, when
        /// collecting from the UK) if you're on the <a
        /// href='https://gocardless.com/pricing'>GoCardless Plus or Pro
        /// packages</a>.</p>
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    public class PaymentListRequest
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
        /// ID of a creditor to filter payments by. If you pass this parameter,
        /// you cannot also pass `customer`.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently only "GBP", "EUR", and "SEK" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PaymentCurrency? Currency { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentCurrency
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
        /// ID of a customer to filter payments by. If you pass this parameter,
        /// you cannot also pass `creditor`.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "MD".
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending_customer_approval`: we're waiting for the customer to
        /// approve this payment</li>
        /// <li>`pending_submission`: the payment has been created, but not yet
        /// submitted to the banks</li>
        /// <li>`submitted`: the payment has been submitted to the banks</li>
        /// <li>`confirmed`: the payment has been confirmed as collected</li>
        /// <li>`paid_out`:  the payment has been included in a
        /// [payout](#core-endpoints-payouts)</li>
        /// <li>`cancelled`: the payment has been cancelled</li>
        /// <li>`customer_approval_denied`: the customer has denied approval for
        /// the payment. You should contact the customer directly</li>
        /// <li>`failed`: the payment failed to be processed. Note that payments
        /// can fail after being confirmed if the failure message is sent late
        /// by the banks.</li>
        /// <li>`charged_back`: the payment has been charged back</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public PaymentStatus? Status { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentStatus
        {
            /// <summary>
            /// One of:
            /// <ul>
            /// <li>`pending_customer_approval`: we're waiting for the customer
            /// to approve this payment</li>
            /// <li>`pending_submission`: the payment has been created, but not
            /// yet submitted to the banks</li>
            /// <li>`submitted`: the payment has been submitted to the
            /// banks</li>
            /// <li>`confirmed`: the payment has been confirmed as
            /// collected</li>
            /// <li>`paid_out`:  the payment has been included in a
            /// [payout](#core-endpoints-payouts)</li>
            /// <li>`cancelled`: the payment has been cancelled</li>
            /// <li>`customer_approval_denied`: the customer has denied approval
            /// for the payment. You should contact the customer directly</li>
            /// <li>`failed`: the payment failed to be processed. Note that
            /// payments can fail after being confirmed if the failure message
            /// is sent late by the banks.</li>
            /// <li>`charged_back`: the payment has been charged back</li>
            /// </ul>
            /// </summary>
    
            [EnumMember(Value = "pending_customer_approval")]
            PendingCustomerApproval,
            [EnumMember(Value = "pending_submission")]
            PendingSubmission,
            [EnumMember(Value = "submitted")]
            Submitted,
            [EnumMember(Value = "confirmed")]
            Confirmed,
            [EnumMember(Value = "paid_out")]
            PaidOut,
            [EnumMember(Value = "cancelled")]
            Cancelled,
            [EnumMember(Value = "customer_approval_denied")]
            CustomerApprovalDenied,
            [EnumMember(Value = "failed")]
            Failed,
            [EnumMember(Value = "charged_back")]
            ChargedBack,
        }

        /// <summary>
        /// Unique identifier, beginning with "SB".
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

        
    public class PaymentGetRequest
    {
    }

        
    public class PaymentUpdateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    public class PaymentCancelRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    public class PaymentRetryRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

    public class PaymentResponse : ApiResponse
    {
        [JsonProperty("payments")]
        public Payment Payment { get; private set; }
    }

    public class PaymentListResponse : ApiResponse
    {
        public IReadOnlyList<Payment> Payments { get; private set; }
        public Meta Meta { get; private set; }
    }
}
