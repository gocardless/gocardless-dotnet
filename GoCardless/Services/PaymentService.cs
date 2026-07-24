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
    /// Payment objects represent payments from a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>
    /// to a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>,
    /// taken against a Direct Debit <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>.
    ///
    /// GoCardless will notify you via a <a
    /// href="https://developer.gocardless.com/api-reference/#appendix-webhooks">webhook</a>
    /// whenever the state of a payment changes.
    /// </summary>
    public class PaymentService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public PaymentService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// <a name="mandate_is_inactive"></a>Creates a new payment object.
        ///
        /// This fails with a <c>mandate_is_inactive</c> error if the linked <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
        /// is cancelled or has failed. Payments can be created against mandates
        /// with status of: <c>pending_customer_approval</c>,
        /// <c>pending_submission</c>, <c>submitted</c>, and <c>active</c>.
        /// </summary>
        /// <param name="request">An optional `PaymentCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> CreateAsync(
            PaymentCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>(
                "POST",
                "/payments",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "payments",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-cursor-pagination">cursor-paginated</a>
        /// list of your payments.
        /// </summary>
        /// <param name="request">An optional `PaymentListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of payment resources</returns>
        public Task<PaymentListResponse> ListAsync(
            PaymentListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<PaymentListResponse>(
                "GET",
                "/payments",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of payments.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Payment> All(
            PaymentListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
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
        public IEnumerable<Task<IReadOnlyList<Payment>>> AllAsync(
            PaymentListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
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
        /// <param name="identity"></param>Unique identifier, beginning with "PM".
        /// <param name="request">An optional `PaymentGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> GetAsync(
            string identity,
            PaymentGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>(
                "GET",
                "/payments/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Updates a payment object. This accepts only the metadata parameter.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "PM".
        /// <param name="request">An optional `PaymentUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> UpdateAsync(
            string identity,
            PaymentUpdateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentUpdateRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>(
                "PUT",
                "/payments/:identity",
                urlParams,
                request,
                null,
                "payments",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Cancels the payment if it has not already been submitted to the
        /// banks. Any metadata supplied to this endpoint will be stored on the
        /// payment cancellation event it causes.
        ///
        /// This will fail with a <c>cancellation_failed</c> error unless the
        /// payment's status is <c>pending_submission</c>.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "PM".
        /// <param name="request">An optional `PaymentCancelRequest` representing the body for this cancel request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> CancelAsync(
            string identity,
            PaymentCancelRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentCancelRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>(
                "POST",
                "/payments/:identity/actions/cancel",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// <a name="retry_failed"></a>Retries a failed payment if the
        /// underlying mandate is active. You will receive a
        /// <c>resubmission_requested</c> webhook, but after that retrying the
        /// payment follows the same process as its initial creation, so you
        /// will receive a <c>submitted</c> webhook, followed by a
        /// <c>confirmed</c> or <c>failed</c> event. Any metadata supplied to
        /// this endpoint will be stored against the payment submission event it
        /// causes.
        ///
        /// This will return a <c>retry_failed</c> error if the payment has not
        /// failed.
        ///
        /// Payments can be retried up to 3 times.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "PM".
        /// <param name="request">An optional `PaymentRetryRequest` representing the body for this retry request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payment resource</returns>
        public Task<PaymentResponse> RetryAsync(
            string identity,
            PaymentRetryRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentRetryRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentResponse>(
                "POST",
                "/payments/:identity/actions/retry",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// <a name="mandate_is_inactive"></a>Creates a new payment object.
    ///
    /// This fails with a <c>mandate_is_inactive</c> error if the linked <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
    /// is cancelled or has failed. Payments can be created against mandates
    /// with status of: <c>pending_customer_approval</c>,
    /// <c>pending_submission</c>, <c>submitted</c>, and <c>active</c>.
    /// </summary>
    public class PaymentCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        ///
        /// Minimum and maximum amounts vary by payment scheme. For more
        /// information, see <a
        /// href="https://support.gocardless.com/hc/en-gb/articles/115000309245-Transaction-limits">Transaction
        /// limits</a>
        ///
        /// For Variable Recurring Payments (VRP), this must not exceed the
        /// mandate's <c>max_amount_per_payment</c>
        /// constraint.
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount to be deducted from the payment as the OAuth app's fee,
        /// in the lowest denomination for the currency (e.g. pence in GBP,
        /// cents in EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// A future date on which the payment should be collected. If not
        /// specified, the payment will be collected as soon as possible. If the
        /// value is before the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>'s
        /// <c>next_possible_charge_date</c> creation will fail. If the value is
        /// not a working day it will be rolled forwards to the next available
        /// one.
        /// </summary>
        [JsonProperty("charge_date")]
        public string ChargeDate { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PaymentCurrency? Currency { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentCurrency
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
        /// A human-readable description of the payment. This will be included
        /// in the notification email GoCardless sends to your customer if your
        /// organisation does not send its own notifications (see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-compliance-requirements">compliance
        /// requirements</a>).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Set this to true or false in the request to create an ACH payment to
        /// explicitly choose whether the payment should be processed through
        /// Faster
        /// ACH or standard ACH, rather than relying on the presence or absence
        /// of the
        /// charge date to indicate that.
        /// </summary>
        [JsonProperty("faster_ach")]
        public bool? FasterAch { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public PaymentLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a Payment.
        /// </summary>
        public class PaymentLinks
        {
            /// <summary>
            /// ID of the <a
            /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
            /// against which this payment should be collected.
            /// </summary>
            [JsonProperty("mandate")]
            public string Mandate { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Indicates how a Variable Recurring Payment (VRP) is initiated, by or
        /// on behalf of the payer.
        ///
        /// <ul>
        /// <li><c>in_session</c>: The payer is actively participating in the
        /// payment creation session.</li>
        /// <li><c>off_session</c>: The payer is not present during the
        /// transaction, and the payment is initiated by the merchant based on
        /// an established consent (e.g., a recurring subscription
        /// payment).</li>
        /// </ul>
        /// </summary>
        [JsonProperty("psu_interaction_type")]
        public PaymentPsuInteractionType? PsuInteractionType { get; set; }

        /// <summary>
        /// Indicates how a Variable Recurring Payment (VRP) is initiated, by or
        /// on behalf of the payer.
        ///
        /// <ul>
        /// <li><c>in_session</c>: The payer is actively participating in the
        /// payment creation session.</li>
        /// <li><c>off_session</c>: The payer is not present during the
        /// transaction, and the payment is initiated by the merchant based on
        /// an established consent (e.g., a recurring subscription
        /// payment).</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentPsuInteractionType
        {
            /// <summary>`psu_interaction_type` with a value of "in_session"</summary>
            [EnumMember(Value = "in_session")]
            InSession,

            /// <summary>`psu_interaction_type` with a value of "off_session"</summary>
            [EnumMember(Value = "off_session")]
            OffSession,
        }

        /// <summary>
        /// An optional reference that will appear on your customer's bank
        /// statement. The character limit for this reference is dependent on
        /// the scheme.<br></br> ACH <ul>
        /// <li>10 characters</li>
        /// </ul><br></br> Autogiro <ul>
        /// <li>11 characters</li>
        /// </ul><br></br> Bacs <ul>
        /// <li>10 characters</li>
        /// </ul><br></br> BECS <ul>
        /// <li>30 characters</li>
        /// </ul><br></br> BECS NZ <ul>
        /// <li>12 characters</li>
        /// </ul><br></br> Betalingsservice <ul>
        /// <li>30 characters</li>
        /// </ul><br></br> Faster Payments <ul>
        /// <li>18 characters</li>
        /// </ul><br></br> PAD <ul>
        /// <li>scheme doesn't offer references</li>
        /// </ul><br></br> PayTo <ul>
        /// <li>18 characters</li>
        /// </ul><br></br> SEPA <ul>
        /// <li>140 characters</li>
        /// </ul><br></br> Note that this reference must be unique (for each
        /// merchant) for the BECS scheme as it is a scheme requirement. <p
        /// class="restricted-notice">Restricted: You can only specify a payment
        /// reference for Bacs payments (that is, when collecting from the UK)
        /// if you're on the <a href="https://gocardless.com/pricing">GoCardless
        /// Plus, Pro or Enterprise packages</a>.</p> <p
        /// class="restricted-notice">Restricted: You can not specify a payment
        /// reference for Faster Payments.</p>
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// On failure, automatically retry the payment using <a
        /// href="https://developer.gocardless.com/success-plus/overview">intelligent
        /// retries</a>. Default is <c>false</c>. <p class="notice">Important:
        /// To be able to use intelligent retries, Success+ needs to be enabled
        /// in <a href="https://manage.gocardless.com/success-plus">GoCardless
        /// dashboard</a>. </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// Returns a <a
    /// href="https://developer.gocardless.com/api-reference/#api-usage-cursor-pagination">cursor-paginated</a>
    /// list of your payments.
    /// </summary>
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

        [JsonProperty("charge_date")]
        public PaymentChargeDate ChargeDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class PaymentChargeDate
        {
            /// <summary>
            /// Limit to records where the payment was or will be collected from
            /// the customer's bank account after the specified date.
            /// </summary>
            [JsonProperty("gt")]
            public string Gt { get; set; }

            /// <summary>
            /// Limit to records where the payment was or will be collected from
            /// the customer's bank account on or after the specified date.
            /// </summary>
            [JsonProperty("gte")]
            public string Gte { get; set; }

            /// <summary>
            /// Limit to records where the payment was or will be collected from
            /// the customer's bank account before the specified date.
            /// </summary>
            [JsonProperty("lt")]
            public string Lt { get; set; }

            /// <summary>
            /// Limit to records where the payment was or will be collected from
            /// the customer's bank account on or before the specified date.
            /// </summary>
            [JsonProperty("lte")]
            public string Lte { get; set; }
        }

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
            /// Limit to records created on or before the specified date-time.
            /// </summary>
            [JsonProperty("lte")]
            public DateTimeOffset? LessThanOrEqual { get; set; }
        }

        /// <summary>
        /// ID of a creditor to filter payments by. If you pass this parameter,
        /// you cannot also pass <c>customer</c>.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PaymentCurrency? Currency { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentCurrency
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
        /// ID of a customer to filter payments by. If you pass this parameter,
        /// you cannot also pass <c>creditor</c>.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

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
        /// A bank payment scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "faster_payments", "pad", "pay_to",
        /// "sepa_core", "sepa_credit_transfer" and
        /// "sepa_instant_credit_transfer" are supported.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// The direction to sort in.
        /// One of:
        ///
        /// <ul>
        /// <li><c>asc</c></li>
        /// <li><c>desc</c></li>
        /// </ul>
        /// </summary>
        [JsonProperty("sort_direction")]
        public PaymentSortDirection? SortDirection { get; set; }

        /// <summary>
        /// The direction to sort in.
        /// One of:
        ///
        /// <ul>
        /// <li><c>asc</c></li>
        /// <li><c>desc</c></li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentSortDirection
        {
            /// <summary>`sort_direction` with a value of "asc"</summary>
            [EnumMember(Value = "asc")]
            Asc,

            /// <summary>`sort_direction` with a value of "desc"</summary>
            [EnumMember(Value = "desc")]
            Desc,
        }

        /// <summary>
        /// Field by which to sort records.
        /// One of:
        ///
        /// <ul>
        /// <li><c>charge_date</c></li>
        /// <li><c>amount</c></li>
        /// </ul>
        /// </summary>
        [JsonProperty("sort_field")]
        public PaymentSortField? SortField { get; set; }

        /// <summary>
        /// Field by which to sort records.
        /// One of:
        ///
        /// <ul>
        /// <li><c>charge_date</c></li>
        /// <li><c>amount</c></li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentSortField
        {
            /// <summary>`sort_field` with a value of "charge_date"</summary>
            [EnumMember(Value = "charge_date")]
            ChargeDate,

            /// <summary>`sort_field` with a value of "amount"</summary>
            [EnumMember(Value = "amount")]
            Amount,
        }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>pending_customer_approval</c>: we're waiting for the customer
        /// to approve this payment</li>
        /// <li><c>pending_submission</c>: the payment has been created, but not
        /// yet submitted to the banks</li>
        /// <li><c>submitted</c>: the payment has been submitted to the
        /// banks</li>
        /// <li><c>confirmed</c>: the payment has been confirmed as
        /// collected</li>
        /// <li><c>paid_out</c>: the payment has been included in a <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payouts">payout</a></li>
        /// <li><c>cancelled</c>: the payment has been cancelled</li>
        /// <li><c>customer_approval_denied</c>: the customer has denied
        /// approval for the payment. You should contact the customer
        /// directly</li>
        /// <li><c>failed</c>: the payment failed to be processed. Note that
        /// payments can fail after being confirmed if the failure message is
        /// sent late by the banks.</li>
        /// <li><c>charged_back</c>: the payment has been charged back</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public PaymentStatus? Status { get; set; }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>pending_customer_approval</c>: we're waiting for the customer
        /// to approve this payment</li>
        /// <li><c>pending_submission</c>: the payment has been created, but not
        /// yet submitted to the banks</li>
        /// <li><c>submitted</c>: the payment has been submitted to the
        /// banks</li>
        /// <li><c>confirmed</c>: the payment has been confirmed as
        /// collected</li>
        /// <li><c>paid_out</c>: the payment has been included in a <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payouts">payout</a></li>
        /// <li><c>cancelled</c>: the payment has been cancelled</li>
        /// <li><c>customer_approval_denied</c>: the customer has denied
        /// approval for the payment. You should contact the customer
        /// directly</li>
        /// <li><c>failed</c>: the payment failed to be processed. Note that
        /// payments can fail after being confirmed if the failure message is
        /// sent late by the banks.</li>
        /// <li><c>charged_back</c>: the payment has been charged back</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentStatus
        {
            /// <summary>`status` with a value of "pending_customer_approval"</summary>
            [EnumMember(Value = "pending_customer_approval")]
            PendingCustomerApproval,

            /// <summary>`status` with a value of "pending_submission"</summary>
            [EnumMember(Value = "pending_submission")]
            PendingSubmission,

            /// <summary>`status` with a value of "submitted"</summary>
            [EnumMember(Value = "submitted")]
            Submitted,

            /// <summary>`status` with a value of "confirmed"</summary>
            [EnumMember(Value = "confirmed")]
            Confirmed,

            /// <summary>`status` with a value of "paid_out"</summary>
            [EnumMember(Value = "paid_out")]
            PaidOut,

            /// <summary>`status` with a value of "cancelled"</summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,

            /// <summary>`status` with a value of "customer_approval_denied"</summary>
            [EnumMember(Value = "customer_approval_denied")]
            CustomerApprovalDenied,

            /// <summary>`status` with a value of "failed"</summary>
            [EnumMember(Value = "failed")]
            Failed,

            /// <summary>`status` with a value of "charged_back"</summary>
            [EnumMember(Value = "charged_back")]
            ChargedBack,
        }

        /// <summary>
        /// Unique identifier, beginning with "SB".
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

    /// <summary>
    /// Retrieves the details of a single existing payment.
    /// </summary>
    public class PaymentGetRequest { }

    /// <summary>
    /// Updates a payment object. This accepts only the metadata parameter.
    /// </summary>
    public class PaymentUpdateRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// On failure, automatically retry the payment using <a
        /// href="https://developer.gocardless.com/success-plus/overview">intelligent
        /// retries</a>. Default is <c>false</c>. <p class="notice">Important:
        /// To be able to use intelligent retries, Success+ needs to be enabled
        /// in <a href="https://manage.gocardless.com/success-plus">GoCardless
        /// dashboard</a>. </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }
    }

    /// <summary>
    /// Cancels the payment if it has not already been submitted to the banks.
    /// Any metadata supplied to this endpoint will be stored on the payment
    /// cancellation event it causes.
    ///
    /// This will fail with a <c>cancellation_failed</c> error unless the
    /// payment's status is <c>pending_submission</c>.
    /// </summary>
    public class PaymentCancelRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// <a name="retry_failed"></a>Retries a failed payment if the underlying
    /// mandate is active. You will receive a <c>resubmission_requested</c>
    /// webhook, but after that retrying the payment follows the same process as
    /// its initial creation, so you will receive a <c>submitted</c> webhook,
    /// followed by a <c>confirmed</c> or <c>failed</c> event. Any metadata
    /// supplied to this endpoint will be stored against the payment submission
    /// event it causes.
    ///
    /// This will return a <c>retry_failed</c> error if the payment has not
    /// failed.
    ///
    /// Payments can be retried up to 3 times.
    /// </summary>
    public class PaymentRetryRequest
    {
        /// <summary>
        /// A future date on which the payment should be collected. If not
        /// specified, the payment will be collected as soon as possible. If the
        /// value is before the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>'s
        /// <c>next_possible_charge_date</c> creation will fail. If the value is
        /// not a working day it will be rolled forwards to the next available
        /// one.
        /// </summary>
        [JsonProperty("charge_date")]
        public string ChargeDate { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single payment.
    /// </summary>
    public class PaymentResponse : ApiResponse
    {
        /// <summary>
        /// The payment from the response.
        /// </summary>
        [JsonProperty("payments")]
        public Payment Payment { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of payments.
    /// </summary>
    public class PaymentListResponse : ApiResponse
    {
        /// <summary>
        /// The list of payments from the response.
        /// </summary>
        [JsonProperty("payments")]
        public IReadOnlyList<Payment> Payments { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
