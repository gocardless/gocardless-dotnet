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
    /// Service class for working with billing request resources.
    ///
    /// Billing Requests help create resources that require input or action from
    /// a customer. An example of required input might be additional customer
    /// billing details, while an action would be asking a customer to authorise
    /// a payment using their mobile banking app.
    ///
    /// See <a
    /// href="https://developer.gocardless.com/getting-started/billing-requests/overview/">Billing
    /// Requests: Overview</a> for how-to's, explanations and tutorials. <p
    /// class="notice">Important: All properties associated with
    /// <c>subscription_request</c> and <c>instalment_schedule_request</c> are
    /// only supported for ACH and PAD schemes.</p>
    /// </summary>
    public class BillingRequestService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public BillingRequestService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// <p class="notice">Important: All properties associated with
        /// <code>subscription_request</code> and
        /// <code>instalment_schedule_request</code> are only supported for ACH
        /// and PAD schemes.</p>
        /// </summary>
        /// <param name="request">An optional `BillingRequestCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CreateAsync(
            BillingRequestCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "billing_requests",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// If the billing request has a pending
        /// <code>collect_customer_details</code>
        /// action, this endpoint can be used to collect the details in order to
        /// complete it.
        ///
        /// The endpoint takes the same payload as Customers, but checks that
        /// the
        /// customer fields are populated correctly for the billing request
        /// scheme.
        ///
        /// Whatever is provided to this endpoint is used to update the
        /// referenced
        /// customer, and will take effect immediately after the request is
        /// successful.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestCollectCustomerDetailsRequest` representing the body for this collect_customer_details request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CollectCustomerDetailsAsync(
            string identity,
            BillingRequestCollectCustomerDetailsRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestCollectCustomerDetailsRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/collect_customer_details",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// If the billing request has a pending
        /// <code>collect_bank_account</code> action, this endpoint can be
        /// used to collect the details in order to complete it.
        ///
        /// The endpoint takes the same payload as Customer Bank Accounts, but
        /// check
        /// the bank account is valid for the billing request scheme before
        /// creating
        /// and attaching it.
        ///
        /// If the scheme is PayTo and the pay_id is available, this can be
        /// included in the payload along with the
        /// country_code.
        ///
        /// ACH scheme For compliance reasons, an extra validation step is done
        /// using
        /// a third-party provider to make sure the customer's bank account can
        /// accept
        /// Direct Debit. If a bank account is discovered to be closed or
        /// invalid, the
        /// customer is requested to adjust the account number/routing number
        /// and
        /// succeed in this check to continue with the flow.
        ///
        /// BACS and SEPA schemes <a
        /// href="https://hub.gocardless.com/s/article/Introduction-to-Payer-Name-Verification?language=en_GB">Payer
        /// Name Verification</a>
        /// is enabled by default for UK and Eurozone based bank accounts,
        /// meaning we verify the account holder name and bank account
        /// number/IBAN match
        /// the details held by the relevant bank. If there is no match, the
        /// endpoint will return a 422 - validation error on
        /// account_holder_name:
        /// "Account holder name does not match bank account details provided".
        /// Testing instructions are <a
        /// href="https://developer.gocardless.com/developer-tools/scenario-simulators/#payer_name_verification">here</a>
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestCollectBankAccountRequest` representing the body for this collect_bank_account request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CollectBankAccountAsync(
            string identity,
            BillingRequestCollectBankAccountRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestCollectBankAccountRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/collect_bank_account",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// This is needed when you have a mandate request. As a scheme
        /// compliance rule we are required to
        /// allow the payer to crosscheck the details entered by them and
        /// confirm it.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestConfirmPayerDetailsRequest` representing the body for this confirm_payer_details request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> ConfirmPayerDetailsAsync(
            string identity,
            BillingRequestConfirmPayerDetailsRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestConfirmPayerDetailsRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/confirm_payer_details",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// If a billing request is ready to be fulfilled, call this endpoint to
        /// cause
        /// it to fulfil, executing the payment.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestFulfilRequest` representing the body for this fulfil request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> FulfilAsync(
            string identity,
            BillingRequestFulfilRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestFulfilRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/fulfil",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Immediately cancels a billing request, causing all billing request
        /// flows
        /// to expire.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestCancelRequest` representing the body for this cancel request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CancelAsync(
            string identity,
            BillingRequestCancelRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestCancelRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/cancel",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-cursor-pagination">cursor-paginated</a>
        /// list of your billing requests.
        /// </summary>
        /// <param name="request">An optional `BillingRequestListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of billing request resources</returns>
        public Task<BillingRequestListResponse> ListAsync(
            BillingRequestListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BillingRequestListResponse>(
                "GET",
                "/billing_requests",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of billing requests.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<BillingRequest> All(
            BillingRequestListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.BillingRequests)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of billing requests.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<BillingRequest>>> AllAsync(
            BillingRequestListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestListRequest();

            return new TaskEnumerable<IReadOnlyList<BillingRequest>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.BillingRequests, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Fetches a billing request
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> GetAsync(
            string identity,
            BillingRequestGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "GET",
                "/billing_requests/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Notifies the customer linked to the billing request, asking them to
        /// authorise it.
        /// Currently, the customer can only be notified by email.
        ///
        /// This endpoint is currently supported only for Pay by Bank Billing
        /// Requests.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestNotifyRequest` representing the body for this notify request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> NotifyAsync(
            string identity,
            BillingRequestNotifyRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestNotifyRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/notify",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Triggers a fallback from the open-banking flow to direct debit.
        /// Note, the billing request must have fallback enabled.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestFallbackRequest` representing the body for this fallback request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> FallbackAsync(
            string identity,
            BillingRequestFallbackRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestFallbackRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/fallback",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// This will allow for the updating of the currency and subsequently
        /// the scheme if
        /// needed for a Billing Request. This will only be available for
        /// mandate only flows
        /// which do not have the lock_currency flag set to true on the Billing
        /// Request Flow. It
        /// will also not support any request which has a payments request.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestChooseCurrencyRequest` representing the body for this choose_currency request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> ChooseCurrencyAsync(
            string identity,
            BillingRequestChooseCurrencyRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestChooseCurrencyRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/choose_currency",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Creates an Institution object and attaches it to the Billing Request
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BRQ".
        /// <param name="request">An optional `BillingRequestSelectInstitutionRequest` representing the body for this select_institution request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> SelectInstitutionAsync(
            string identity,
            BillingRequestSelectInstitutionRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BillingRequestSelectInstitutionRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>(
                "POST",
                "/billing_requests/:identity/actions/select_institution",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// <p class="notice">Important: All properties associated with
    /// <code>subscription_request</code> and
    /// <code>instalment_schedule_request</code> are only supported for ACH and
    /// PAD schemes.</p>
    /// </summary>
    public class BillingRequestCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        /// (Optional) If true, this billing request can fallback from instant
        /// payment to direct debit.
        /// Should not be set if GoCardless payment intelligence feature is
        /// used.
        ///
        /// See <a
        /// href="https://developer.gocardless.com/billing-requests/retain-customers-with-fallbacks/">Billing
        /// Requests: Retain customers with Fallbacks</a>
        /// for more information.
        /// </summary>
        [JsonProperty("fallback_enabled")]
        public bool? FallbackEnabled { get; set; }

        [JsonProperty("instalment_schedule_request")]
        public BillingRequestInstalmentScheduleRequest InstalmentScheduleRequest { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class BillingRequestInstalmentScheduleRequest
        {
            /// <summary>
            /// The amount to be deducted from each payment as an app fee, to be
            /// paid to the partner integration which created the subscription,
            /// in the lowest denomination for the currency (e.g. pence in GBP,
            /// cents in EUR).
            /// </summary>
            [JsonProperty("app_fee")]
            public int? AppFee { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
            /// 4217</a> currency code. Currently "USD" and "CAD" are supported.
            /// </summary>
            [JsonProperty("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// An explicit array of instalment payments, each specifying at
            /// least an <c>amount</c> and <c>charge_date</c>. See <a
            /// href="https://developer.gocardless.com/api-reference/#instalment-schedules-create-with-dates">create
            /// (with dates)</a>
            /// </summary>
            [JsonProperty("instalments_with_dates")]
            public BillingRequestInstalmentsWithDates[] InstalmentsWithDates { get; set; }

            /// <summary>
            ///
            /// </summary>
            public class BillingRequestInstalmentsWithDates
            {
                /// <summary>
                /// Amount, in the lowest denomination for the currency (e.g. pence
                /// in GBP, cents in EUR).
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
                /// A future date on which the payment should be collected. If the
                /// date
                /// is before the next_possible_charge_date on the
                /// <a
                /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>,
                /// it will be automatically rolled
                /// forwards to that date.
                /// </summary>
                [JsonProperty("charge_date")]
                public string ChargeDate { get; set; }

                /// <summary>
                /// A human-readable description of the payment. This will be
                /// included in the notification email GoCardless sends to your
                /// customer if your organisation does not send its own
                /// notifications (see <a
                /// href="https://developer.gocardless.com/api-reference/#appendix-compliance-requirements">compliance
                /// requirements</a>).
                /// </summary>
                [JsonProperty("description")]
                public string Description { get; set; }
            }

            /// <summary>
            /// Frequency of the payments you want to create, together with an
            /// array of payment
            /// amounts to be collected, with a specified start date for the
            /// first payment.
            /// See <a
            /// href="https://developer.gocardless.com/api-reference/#instalment-schedules-create-with-schedule">create
            /// (with schedule)</a>
            /// </summary>
            [JsonProperty("instalments_with_schedule")]
            public BillingRequestInstalmentsWithSchedule InstalmentsWithSchedule { get; set; }

            /// <summary>
            /// Frequency of the payments you want to create, together with an array
            /// of payment
            /// amounts to be collected, with a specified start date for the first
            /// payment.
            /// See <a
            /// href="https://developer.gocardless.com/api-reference/#instalment-schedules-create-with-schedule">create
            /// (with schedule)</a>
            /// </summary>
            public class BillingRequestInstalmentsWithSchedule
            {
                /// <summary>
                /// List of amounts of each instalment, in the lowest denomination
                /// for the
                /// currency (e.g. cents in USD).
                /// </summary>
                [JsonProperty("amounts")]
                public int?[] Amounts { get; set; }

                /// <summary>
                /// Number of <c>interval_units</c> between charge dates. Must be
                /// greater than or
                /// equal to <c>1</c>.
                /// </summary>
                [JsonProperty("interval")]
                public int? Interval { get; set; }

                /// <summary>
                /// The unit of time between customer charge dates. One of
                /// <c>weekly</c>, <c>monthly</c> or <c>yearly</c>.
                /// </summary>
                [JsonProperty("interval_unit")]
                public BillingRequestIntervalUnit? IntervalUnit { get; set; }

                /// <summary>
                /// The unit of time between customer charge dates. One of
                /// <c>weekly</c>, <c>monthly</c> or <c>yearly</c>.
                /// </summary>
                [JsonConverter(typeof(StringEnumConverter))]
                public enum BillingRequestIntervalUnit
                {
                    /// <summary>`interval_unit` with a value of "weekly"</summary>
                    [EnumMember(Value = "weekly")]
                    Weekly,

                    /// <summary>`interval_unit` with a value of "monthly"</summary>
                    [EnumMember(Value = "monthly")]
                    Monthly,

                    /// <summary>`interval_unit` with a value of "yearly"</summary>
                    [EnumMember(Value = "yearly")]
                    Yearly,
                }

                /// <summary>
                /// The date on which the first payment should be charged. Must be
                /// on or after the <a
                /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>'s
                /// <c>next_possible_charge_date</c>. When left blank and
                /// <c>month</c> or <c>day_of_month</c> are provided, this will be
                /// set to the date of the first payment. If created without
                /// <c>month</c> or <c>day_of_month</c> this will be set as the
                /// mandate's <c>next_possible_charge_date</c>
                /// </summary>
                [JsonProperty("start_date")]
                public string StartDate { get; set; }
            }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// Name of the instalment schedule, up to 100 chars. This name will
            /// also be
            /// copied to the payments of the instalment schedule if you use
            /// schedule-based creation.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// An optional payment reference. This will be set as the reference
            /// on each payment
            /// created and will appear on your customer's bank statement. See
            /// the documentation for
            /// the <a
            /// href="https://developer.gocardless.com/api-reference/#payments-create-a-payment">create
            /// payment endpoint</a> for more details.
            /// <br></br>
            /// </summary>
            [JsonProperty("payment_reference")]
            public string PaymentReference { get; set; }

            /// <summary>
            /// On failure, automatically retry payments using <a
            /// href="https://developer.gocardless.com/success-plus/overview">intelligent
            /// retries</a>. Default is <c>false</c>. <p
            /// class="notice">Important: To be able to use intelligent retries,
            /// Success+ needs to be enabled in <a
            /// href="https://manage.gocardless.com/success-plus">GoCardless
            /// dashboard</a>. </p>
            /// </summary>
            [JsonProperty("retry_if_possible")]
            public bool? RetryIfPossible { get; set; }

            /// <summary>
            /// The total amount of the instalment schedule, defined as the sum
            /// of all individual
            /// payments, in the lowest denomination for the currency (e.g.
            /// pence in GBP, cents in
            /// EUR). If the requested payment amounts do not sum up correctly,
            /// a validation error
            /// will be returned.
            /// </summary>
            [JsonProperty("total_amount")]
            public int? TotalAmount { get; set; }
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a BillingRequest.
        /// </summary>
        public class BillingRequestLinks
        {
            /// <summary>
            /// ID of the associated <a
            /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>.
            /// Only required if your account manages multiple creditors.
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }

            /// <summary>
            /// ID of the <a
            /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>
            /// against which this request should be made.
            /// </summary>
            [JsonProperty("customer")]
            public string Customer { get; set; }

            /// <summary>
            /// (Optional) ID of the <a
            /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customer-bank-accounts">customer_bank_account</a>
            /// against which this request should be made.
            /// </summary>
            [JsonProperty("customer_bank_account")]
            public string CustomerBankAccount { get; set; }
        }

        [JsonProperty("mandate_request")]
        public BillingRequestMandateRequest MandateRequest { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class BillingRequestMandateRequest
        {
            /// <summary>
            /// This field is ACH specific, sometimes referred to as <a
            /// href="https://www.moderntreasury.com/learn/sec-codes">SEC
            /// code</a>.
            ///
            /// This is the way that the payer gives authorisation to the
            /// merchant.
            /// web: Authorisation is Internet Initiated or via Mobile Entry
            /// (maps to SEC code: WEB)
            /// telephone: Authorisation is provided orally over telephone (maps
            /// to SEC code: TEL)
            /// paper: Authorisation is provided in writing and signed, or
            /// similarly authenticated (maps to SEC code: PPD)
            /// </summary>
            [JsonProperty("authorisation_source")]
            public BillingRequestAuthorisationSource? AuthorisationSource { get; set; }

            /// <summary>
            /// This field is ACH specific, sometimes referred to as <a
            /// href="https://www.moderntreasury.com/learn/sec-codes">SEC code</a>.
            ///
            /// This is the way that the payer gives authorisation to the merchant.
            /// web: Authorisation is Internet Initiated or via Mobile Entry (maps
            /// to SEC code: WEB)
            /// telephone: Authorisation is provided orally over telephone (maps to
            /// SEC code: TEL)
            /// paper: Authorisation is provided in writing and signed, or similarly
            /// authenticated (maps to SEC code: PPD)
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum BillingRequestAuthorisationSource
            {
                /// <summary>`authorisation_source` with a value of "web"</summary>
                [EnumMember(Value = "web")]
                Web,

                /// <summary>`authorisation_source` with a value of "telephone"</summary>
                [EnumMember(Value = "telephone")]
                Telephone,

                /// <summary>`authorisation_source` with a value of "paper"</summary>
                [EnumMember(Value = "paper")]
                Paper,
            }

            /// <summary>
            /// This attribute represents the authorisation type between the
            /// payer and merchant. It can be set to <c>one_off</c>,
            /// <c>recurring</c> or <c>standing</c> for ACH scheme. And
            /// <c>single</c>, <c>recurring</c> and <c>sporadic</c> for PAD
            /// scheme. Note: This is only supported for ACH and PAD schemes.
            /// </summary>
            [JsonProperty("consent_type")]
            public string ConsentType { get; set; }

            /// <summary>
            /// Constraints that will apply to the mandate_request. (Optional)
            /// Specifically required for PayTo and VRP.
            /// </summary>
            [JsonProperty("constraints")]
            public BillingRequestConstraints Constraints { get; set; }

            /// <summary>
            /// Constraints that will apply to the mandate_request. (Optional)
            /// Specifically required for PayTo and VRP.
            /// </summary>
            public class BillingRequestConstraints
            {
                /// <summary>
                /// The latest date at which payments can be taken, must occur after
                /// start_date if present
                ///
                /// This is an optional field and if it is not supplied the
                /// agreement will be considered open and
                /// will not have an end date. Keep in mind the end date must take
                /// into account how long it will
                /// take the user to set up this agreement via the Billing Request.
                /// </summary>
                [JsonProperty("end_date")]
                public string EndDate { get; set; }

                /// <summary>
                /// The maximum amount that can be charged for a single payment in
                /// the lowest denomination for the currency (e.g. pence in GBP,
                /// cents in EUR). Note: Required for PayTo and VRP.
                /// </summary>
                [JsonProperty("max_amount_per_payment")]
                public int? MaxAmountPerPayment { get; set; }

                /// <summary>
                /// A constraint where you can specify info (free text string) about
                /// how payments are calculated. For use when payments vary and
                /// cannot be expressed as a fixed amount and frequency. Note: This
                /// is only supported for ACH and PAD schemes.
                /// </summary>
                [JsonProperty("payment_method")]
                public string PaymentMethod { get; set; }

                /// <summary>
                /// Caps on the total amount and/or number of payments that can be
                /// collected within a
                /// repeating period (e.g. no more than a set amount per month), as
                /// opposed to
                /// <c>max_amount_per_payment</c> which caps a single payment.
                ///
                /// Note: Required for VRP, where exactly one periodic limit must be
                /// provided. Optional for
                /// PayTo.
                /// </summary>
                [JsonProperty("periodic_limits")]
                public BillingRequestPeriodicLimits[] PeriodicLimits { get; set; }

                /// <summary>
                ///
                /// </summary>
                public class BillingRequestPeriodicLimits
                {
                    /// <summary>
                    /// The alignment of the period. Defaults to <c>creation_date</c> if
                    /// not specified.
                    ///
                    /// <c>calendar</c> <ul>
                    /// <li>the period follows fixed calendar boundaries, the same for
                    /// every mandate:</li>
                    /// </ul>
                    /// <c>week</c> runs Monday to Sunday, <c>month</c> runs from the
                    /// 1st to the last day of the calendar
                    /// month, and <c>year</c> runs from 1 January to 31 December. If
                    /// the mandate starts partway
                    /// through a period, the limit for that first period is reduced
                    /// proportionally to the days
                    /// remaining (e.g. a monthly limit starting on the 15th gives
                    /// roughly half the limit for
                    /// that first month).
                    ///
                    /// <c>creation_date</c> <ul>
                    /// <li>the period follows the mandate's own start date rather than
                    /// the
                    /// calendar. For example, if the mandate starts on the 15th, each
                    /// monthly period runs from
                    /// the 15th to the 14th of the following month. The first period is
                    /// a full period, not
                    /// reduced proportionally.</li>
                    /// </ul>
                    /// Note: Has no effect when period is <c>flexible</c>.
                    /// </summary>
                    [JsonProperty("alignment")]
                    public BillingRequestAlignment? Alignment { get; set; }

                    /// <summary>
                    /// The alignment of the period. Defaults to <c>creation_date</c> if not
                    /// specified.
                    ///
                    /// <c>calendar</c> <ul>
                    /// <li>the period follows fixed calendar boundaries, the same for every
                    /// mandate:</li>
                    /// </ul>
                    /// <c>week</c> runs Monday to Sunday, <c>month</c> runs from the 1st to
                    /// the last day of the calendar
                    /// month, and <c>year</c> runs from 1 January to 31 December. If the
                    /// mandate starts partway
                    /// through a period, the limit for that first period is reduced
                    /// proportionally to the days
                    /// remaining (e.g. a monthly limit starting on the 15th gives roughly
                    /// half the limit for
                    /// that first month).
                    ///
                    /// <c>creation_date</c> <ul>
                    /// <li>the period follows the mandate's own start date rather than the
                    /// calendar. For example, if the mandate starts on the 15th, each
                    /// monthly period runs from
                    /// the 15th to the 14th of the following month. The first period is a
                    /// full period, not
                    /// reduced proportionally.</li>
                    /// </ul>
                    /// Note: Has no effect when period is <c>flexible</c>.
                    /// </summary>
                    [JsonConverter(typeof(StringEnumConverter))]
                    public enum BillingRequestAlignment
                    {
                        /// <summary>`alignment` with a value of "calendar"</summary>
                        [EnumMember(Value = "calendar")]
                        Calendar,

                        /// <summary>`alignment` with a value of "creation_date"</summary>
                        [EnumMember(Value = "creation_date")]
                        CreationDate,
                    }

                    /// <summary>
                    /// The maximum number of payments that can be collected in this
                    /// periodic limit.
                    ///
                    /// Note: Only supported for the PayTo scheme, where it is optional.
                    /// </summary>
                    [JsonProperty("max_payments")]
                    public int? MaxPayments { get; set; }

                    /// <summary>
                    /// The maximum total amount that can be charged for all payments in
                    /// this periodic limit,
                    /// in the lowest denomination for the currency (e.g. pence in GBP,
                    /// cents in EUR).
                    ///
                    /// Note: Required for VRP. This is not permitted for the PayTo
                    /// scheme.
                    /// </summary>
                    [JsonProperty("max_total_amount")]
                    public int? MaxTotalAmount { get; set; }

                    /// <summary>
                    /// The repeating period for this mandate. Required whenever a
                    /// periodic limit is provided
                    /// (for both VRP and PayTo). If periodic_limits is omitted entirely
                    /// for PayTo, this
                    /// defaults to flexible.
                    /// </summary>
                    [JsonProperty("period")]
                    public BillingRequestPeriod? Period { get; set; }

                    /// <summary>
                    /// The repeating period for this mandate. Required whenever a periodic
                    /// limit is provided
                    /// (for both VRP and PayTo). If periodic_limits is omitted entirely for
                    /// PayTo, this
                    /// defaults to flexible.
                    /// </summary>
                    [JsonConverter(typeof(StringEnumConverter))]
                    public enum BillingRequestPeriod
                    {
                        /// <summary>`period` with a value of "day"</summary>
                        [EnumMember(Value = "day")]
                        Day,

                        /// <summary>`period` with a value of "week"</summary>
                        [EnumMember(Value = "week")]
                        Week,

                        /// <summary>`period` with a value of "month"</summary>
                        [EnumMember(Value = "month")]
                        Month,

                        /// <summary>`period` with a value of "year"</summary>
                        [EnumMember(Value = "year")]
                        Year,

                        /// <summary>`period` with a value of "flexible"</summary>
                        [EnumMember(Value = "flexible")]
                        Flexible,
                    }
                }

                /// <summary>
                /// The date from which payments can be taken.
                ///
                /// This is an optional field and if it is not supplied the start
                /// date will be set to the day
                /// authorisation happens.
                /// </summary>
                [JsonProperty("start_date")]
                public string StartDate { get; set; }
            }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
            /// 4217</a> currency code.
            /// </summary>
            [JsonProperty("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// A human-readable description of the payment and/or mandate. This
            /// will be displayed to the payer when authorising the billing
            /// request.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// This field will decide how GoCardless handles settlement of
            /// funds from the customer.
            ///
            /// <ul>
            /// <li><c>managed</c> will be moved through GoCardless' account,
            /// batched, and payed out.</li>
            /// <li><c>direct</c> will be a direct transfer from the payer's
            /// account to the merchant where
            /// invoicing will be handled separately.</li>
            /// </ul>
            /// </summary>
            [JsonProperty("funds_settlement")]
            public BillingRequestFundsSettlement? FundsSettlement { get; set; }

            /// <summary>
            /// This field will decide how GoCardless handles settlement of funds
            /// from the customer.
            ///
            /// <ul>
            /// <li><c>managed</c> will be moved through GoCardless' account,
            /// batched, and payed out.</li>
            /// <li><c>direct</c> will be a direct transfer from the payer's account
            /// to the merchant where
            /// invoicing will be handled separately.</li>
            /// </ul>
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum BillingRequestFundsSettlement
            {
                /// <summary>`funds_settlement` with a value of "managed"</summary>
                [EnumMember(Value = "managed")]
                Managed,

                /// <summary>`funds_settlement` with a value of "direct"</summary>
                [EnumMember(Value = "direct")]
                Direct,
            }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// Unique reference. Different schemes have different length and <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-character-sets">character
            /// set</a> requirements. GoCardless will generate a unique
            /// reference satisfying the different scheme requirements if this
            /// field is left blank.
            /// </summary>
            [JsonProperty("reference")]
            public string Reference { get; set; }

            /// <summary>
            /// A bank payment scheme. Currently "ach", "autogiro", "bacs",
            /// "becs", "becs_nz", "betalingsservice", "faster_payments", "pad",
            /// "pay_to" and "sepa_core" are supported. Optional for mandate
            /// only requests - if left blank, the payer will be able to select
            /// the currency/scheme to pay with from a list of your available
            /// schemes.
            /// </summary>
            [JsonProperty("scheme")]
            public string Scheme { get; set; }

            /// <summary>
            /// If true, this billing request would be used to set up a mandate
            /// solely for moving (or sweeping) money from one account owned by
            /// the payer to another account that the payer also owns. This is
            /// required for Faster Payments
            /// </summary>
            [JsonProperty("sweeping")]
            public bool? Sweeping { get; set; }

            /// <summary>
            /// Verification preference for the mandate. One of:
            ///
            /// <ul>
            /// <li><c>minimum</c>: only verify if absolutely required, such as
            /// when part of scheme rules</li>
            /// <li><c>recommended</c>: in addition to <c>minimum</c>, use the
            /// GoCardless payment intelligence solution to decide if a payer
            /// should be verified</li>
            /// <li><c>when_available</c>: if verification mechanisms are
            /// available, use them</li>
            /// <li><c>always</c>: as <c>when_available</c>, but fail to create
            /// the Billing Request if a mechanism isn't available</li>
            /// </ul>
            /// By default, all Billing Requests use the <c>recommended</c>
            /// verification preference. It uses GoCardless payment intelligence
            /// solution to determine if a payer is fraudulent or not. The
            /// verification mechanism is based on the response and the payer
            /// may be asked to verify themselves. If the feature is not
            /// available, <c>recommended</c> behaves like <c>minimum</c>.
            ///
            /// If you never wish to take advantage of our reduced risk products
            /// and Verified Mandates as they are released in new schemes,
            /// please use the <c>minimum</c> verification preference.
            ///
            /// See <a
            /// href="https://developer.gocardless.com/getting-started/billing-requests/verified-mandates/">Billing
            /// Requests: Creating Verified Mandates</a>
            /// for more information.
            /// </summary>
            [JsonProperty("verify")]
            public BillingRequestVerify? Verify { get; set; }

            /// <summary>
            /// Verification preference for the mandate. One of:
            ///
            /// <ul>
            /// <li><c>minimum</c>: only verify if absolutely required, such as when
            /// part of scheme rules</li>
            /// <li><c>recommended</c>: in addition to <c>minimum</c>, use the
            /// GoCardless payment intelligence solution to decide if a payer should
            /// be verified</li>
            /// <li><c>when_available</c>: if verification mechanisms are available,
            /// use them</li>
            /// <li><c>always</c>: as <c>when_available</c>, but fail to create the
            /// Billing Request if a mechanism isn't available</li>
            /// </ul>
            /// By default, all Billing Requests use the <c>recommended</c>
            /// verification preference. It uses GoCardless payment intelligence
            /// solution to determine if a payer is fraudulent or not. The
            /// verification mechanism is based on the response and the payer may be
            /// asked to verify themselves. If the feature is not available,
            /// <c>recommended</c> behaves like <c>minimum</c>.
            ///
            /// If you never wish to take advantage of our reduced risk products and
            /// Verified Mandates as they are released in new schemes, please use
            /// the <c>minimum</c> verification preference.
            ///
            /// See <a
            /// href="https://developer.gocardless.com/getting-started/billing-requests/verified-mandates/">Billing
            /// Requests: Creating Verified Mandates</a>
            /// for more information.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum BillingRequestVerify
            {
                /// <summary>`verify` with a value of "minimum"</summary>
                [EnumMember(Value = "minimum")]
                Minimum,

                /// <summary>`verify` with a value of "recommended"</summary>
                [EnumMember(Value = "recommended")]
                Recommended,

                /// <summary>`verify` with a value of "when_available"</summary>
                [EnumMember(Value = "when_available")]
                WhenAvailable,

                /// <summary>`verify` with a value of "always"</summary>
                [EnumMember(Value = "always")]
                Always,
            }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Specifies the context or scenario in which the payment is being
        /// made. Defines whether the payment is for advance/arrears billing,
        /// point of sale transactions, ecommerce, or account transfers. This
        /// helps banks and payment processors understand the payment scenario
        /// and apply appropriate processing rules and risk controls.
        /// </summary>
        [JsonProperty("payment_context_code")]
        public BillingRequestPaymentContextCode? PaymentContextCode { get; set; }

        /// <summary>
        /// Specifies the context or scenario in which the payment is being
        /// made. Defines whether the payment is for advance/arrears billing,
        /// point of sale transactions, ecommerce, or account transfers. This
        /// helps banks and payment processors understand the payment scenario
        /// and apply appropriate processing rules and risk controls.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestPaymentContextCode
        {
            /// <summary>`payment_context_code` with a value of "billing_goods_and_services_in_advance"</summary>
            [EnumMember(Value = "billing_goods_and_services_in_advance")]
            BillingGoodsAndServicesInAdvance,

            /// <summary>`payment_context_code` with a value of "billing_goods_and_services_in_arrears"</summary>
            [EnumMember(Value = "billing_goods_and_services_in_arrears")]
            BillingGoodsAndServicesInArrears,

            /// <summary>`payment_context_code` with a value of "face_to_face_point_of_sale"</summary>
            [EnumMember(Value = "face_to_face_point_of_sale")]
            FaceToFacePointOfSale,

            /// <summary>`payment_context_code` with a value of "ecommerce_merchant_initiated_payment"</summary>
            [EnumMember(Value = "ecommerce_merchant_initiated_payment")]
            EcommerceMerchantInitiatedPayment,

            /// <summary>`payment_context_code` with a value of "transfer_to_self"</summary>
            [EnumMember(Value = "transfer_to_self")]
            TransferToSelf,

            /// <summary>`payment_context_code` with a value of "transfer_to_third_party"</summary>
            [EnumMember(Value = "transfer_to_third_party")]
            TransferToThirdParty,
        }

        /// <summary>
        /// Specifies the underlying purpose of the payment. Defines the
        /// specific reason or type of service/goods the payment relates to,
        /// improving straight-through processing and compliance.
        /// See <a
        /// href="https://developer.gocardless.com/vrp-commercial-payment-purpose-codes/">VRP
        /// Commercial Payment Purpose Codes</a> for the complete list of valid
        /// codes.
        /// </summary>
        [JsonProperty("payment_purpose_code")]
        public string PaymentPurposeCode { get; set; }

        [JsonProperty("payment_request")]
        public BillingRequestPaymentRequest PaymentRequest { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class BillingRequestPaymentRequest
        {
            /// <summary>
            /// Amount in minor unit (e.g. pence in GBP, cents in EUR).
            /// </summary>
            [JsonProperty("amount")]
            public int? Amount { get; set; }

            /// <summary>
            /// The amount to be deducted from the payment as an app fee, to be
            /// paid to the partner integration which created the billing
            /// request, in the lowest denomination for the currency (e.g. pence
            /// in GBP, cents in EUR).
            /// </summary>
            [JsonProperty("app_fee")]
            public int? AppFee { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
            /// 4217</a> currency code. <c>GBP</c> and <c>EUR</c> supported;
            /// <c>GBP</c> with your customers in the UK and for <c>EUR</c> with
            /// your customers in supported Eurozone countries only.
            /// </summary>
            [JsonProperty("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// A human-readable description of the payment and/or mandate. This
            /// will be displayed to the payer when authorising the billing
            /// request.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// This field will decide how GoCardless handles settlement of
            /// funds from the customer.
            ///
            /// <ul>
            /// <li><c>managed</c> will be moved through GoCardless' account,
            /// batched, and payed out.</li>
            /// <li><c>direct</c> will be a direct transfer from the payer's
            /// account to the merchant where
            /// invoicing will be handled separately.</li>
            /// </ul>
            /// </summary>
            [JsonProperty("funds_settlement")]
            public BillingRequestFundsSettlement? FundsSettlement { get; set; }

            /// <summary>
            /// This field will decide how GoCardless handles settlement of funds
            /// from the customer.
            ///
            /// <ul>
            /// <li><c>managed</c> will be moved through GoCardless' account,
            /// batched, and payed out.</li>
            /// <li><c>direct</c> will be a direct transfer from the payer's account
            /// to the merchant where
            /// invoicing will be handled separately.</li>
            /// </ul>
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum BillingRequestFundsSettlement
            {
                /// <summary>`funds_settlement` with a value of "managed"</summary>
                [EnumMember(Value = "managed")]
                Managed,

                /// <summary>`funds_settlement` with a value of "direct"</summary>
                [EnumMember(Value = "direct")]
                Direct,
            }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// A custom payment reference defined by the merchant. It is only
            /// available for payments on the PayTo scheme or payments using the
            /// Direct Funds settlement model on the Faster Payments scheme.
            /// </summary>
            [JsonProperty("reference")]
            public string Reference { get; set; }

            /// <summary>
            /// On failure, automatically retry payments using <a
            /// href="https://developer.gocardless.com/success-plus/overview">intelligent
            /// retries</a>. Default is <c>false</c>. <p
            /// class="notice">Important: To be able to use intelligent retries,
            /// Success+ needs to be enabled in <a
            /// href="https://manage.gocardless.com/success-plus">GoCardless
            /// dashboard</a>. </p> <p class="notice">Important: This is not
            /// applicable to Pay by Bank and VRP payments. </p>
            /// </summary>
            [JsonProperty("retry_if_possible")]
            public bool? RetryIfPossible { get; set; }

            /// <summary>
            /// (Optional) A scheme used for Open Banking payments. Currently
            /// <c>faster_payments</c> is supported in the UK (GBP) and
            /// <c>sepa_credit_transfer</c> and
            /// <c>sepa_instant_credit_transfer</c> are supported in supported
            /// Eurozone countries (EUR). For Eurozone countries,
            /// <c>sepa_credit_transfer</c> is used as the default. Please be
            /// aware that <c>sepa_instant_credit_transfer</c> may incur an
            /// additional fee for your customer.
            /// </summary>
            [JsonProperty("scheme")]
            public string Scheme { get; set; }
        }

        /// <summary>
        /// Specifies the high-level purpose/category of a mandate and/or
        /// payment using a set of pre-defined categories. Provides context on
        /// the nature and reason for the payment to facilitate processing and
        /// compliance.
        /// See <a
        /// href="https://developer.gocardless.com/billing-request-purpose-codes/">Billing
        /// Request Purpose Codes</a> for the complete list of valid codes.
        /// </summary>
        [JsonProperty("purpose_code")]
        public BillingRequestPurposeCode? PurposeCode { get; set; }

        /// <summary>
        /// Specifies the high-level purpose/category of a mandate and/or
        /// payment using a set of pre-defined categories. Provides context on
        /// the nature and reason for the payment to facilitate processing and
        /// compliance.
        /// See <a
        /// href="https://developer.gocardless.com/billing-request-purpose-codes/">Billing
        /// Request Purpose Codes</a> for the complete list of valid codes.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestPurposeCode
        {
            /// <summary>`purpose_code` with a value of "mortgage"</summary>
            [EnumMember(Value = "mortgage")]
            Mortgage,

            /// <summary>`purpose_code` with a value of "utility"</summary>
            [EnumMember(Value = "utility")]
            Utility,

            /// <summary>`purpose_code` with a value of "loan"</summary>
            [EnumMember(Value = "loan")]
            Loan,

            /// <summary>`purpose_code` with a value of "dependant_support"</summary>
            [EnumMember(Value = "dependant_support")]
            DependantSupport,

            /// <summary>`purpose_code` with a value of "gambling"</summary>
            [EnumMember(Value = "gambling")]
            Gambling,

            /// <summary>`purpose_code` with a value of "retail"</summary>
            [EnumMember(Value = "retail")]
            Retail,

            /// <summary>`purpose_code` with a value of "salary"</summary>
            [EnumMember(Value = "salary")]
            Salary,

            /// <summary>`purpose_code` with a value of "personal"</summary>
            [EnumMember(Value = "personal")]
            Personal,

            /// <summary>`purpose_code` with a value of "government"</summary>
            [EnumMember(Value = "government")]
            Government,

            /// <summary>`purpose_code` with a value of "pension"</summary>
            [EnumMember(Value = "pension")]
            Pension,

            /// <summary>`purpose_code` with a value of "tax"</summary>
            [EnumMember(Value = "tax")]
            Tax,

            /// <summary>`purpose_code` with a value of "other"</summary>
            [EnumMember(Value = "other")]
            Other,

            /// <summary>`purpose_code` with a value of "bonus_payment"</summary>
            [EnumMember(Value = "bonus_payment")]
            BonusPayment,

            /// <summary>`purpose_code` with a value of "cash_management_transfer"</summary>
            [EnumMember(Value = "cash_management_transfer")]
            CashManagementTransfer,

            /// <summary>`purpose_code` with a value of "card_bulk_clearing"</summary>
            [EnumMember(Value = "card_bulk_clearing")]
            CardBulkClearing,

            /// <summary>`purpose_code` with a value of "credit_card_payment"</summary>
            [EnumMember(Value = "credit_card_payment")]
            CreditCardPayment,

            /// <summary>`purpose_code` with a value of "trade_settlement_payment"</summary>
            [EnumMember(Value = "trade_settlement_payment")]
            TradeSettlementPayment,

            /// <summary>`purpose_code` with a value of "debit_card_payment"</summary>
            [EnumMember(Value = "debit_card_payment")]
            DebitCardPayment,

            /// <summary>`purpose_code` with a value of "dividend"</summary>
            [EnumMember(Value = "dividend")]
            Dividend,

            /// <summary>`purpose_code` with a value of "deliver_against_payment"</summary>
            [EnumMember(Value = "deliver_against_payment")]
            DeliverAgainstPayment,

            /// <summary>`purpose_code` with a value of "epayment"</summary>
            [EnumMember(Value = "epayment")]
            Epayment,

            /// <summary>`purpose_code` with a value of "fee_collection_and_interest"</summary>
            [EnumMember(Value = "fee_collection_and_interest")]
            FeeCollectionAndInterest,

            /// <summary>`purpose_code` with a value of "fee_collection"</summary>
            [EnumMember(Value = "fee_collection")]
            FeeCollection,

            /// <summary>`purpose_code` with a value of "person_to_person_payment"</summary>
            [EnumMember(Value = "person_to_person_payment")]
            PersonToPersonPayment,

            /// <summary>`purpose_code` with a value of "government_payment"</summary>
            [EnumMember(Value = "government_payment")]
            GovernmentPayment,

            /// <summary>`purpose_code` with a value of "hedging_transaction"</summary>
            [EnumMember(Value = "hedging_transaction")]
            HedgingTransaction,

            /// <summary>`purpose_code` with a value of "irrevocable_credit_card_payment"</summary>
            [EnumMember(Value = "irrevocable_credit_card_payment")]
            IrrevocableCreditCardPayment,

            /// <summary>`purpose_code` with a value of "irrevocable_debit_card_payment"</summary>
            [EnumMember(Value = "irrevocable_debit_card_payment")]
            IrrevocableDebitCardPayment,

            /// <summary>`purpose_code` with a value of "intra_company_payment"</summary>
            [EnumMember(Value = "intra_company_payment")]
            IntraCompanyPayment,

            /// <summary>`purpose_code` with a value of "interest"</summary>
            [EnumMember(Value = "interest")]
            Interest,

            /// <summary>`purpose_code` with a value of "lockbox_transactions"</summary>
            [EnumMember(Value = "lockbox_transactions")]
            LockboxTransactions,

            /// <summary>`purpose_code` with a value of "commercial"</summary>
            [EnumMember(Value = "commercial")]
            Commercial,

            /// <summary>`purpose_code` with a value of "consumer"</summary>
            [EnumMember(Value = "consumer")]
            Consumer,

            /// <summary>`purpose_code` with a value of "other_payment"</summary>
            [EnumMember(Value = "other_payment")]
            OtherPayment,

            /// <summary>`purpose_code` with a value of "pension_payment"</summary>
            [EnumMember(Value = "pension_payment")]
            PensionPayment,

            /// <summary>`purpose_code` with a value of "represented"</summary>
            [EnumMember(Value = "represented")]
            Represented,

            /// <summary>`purpose_code` with a value of "reimbursement_received_credit_transfer"</summary>
            [EnumMember(Value = "reimbursement_received_credit_transfer")]
            ReimbursementReceivedCreditTransfer,

            /// <summary>`purpose_code` with a value of "receive_against_payment"</summary>
            [EnumMember(Value = "receive_against_payment")]
            ReceiveAgainstPayment,

            /// <summary>`purpose_code` with a value of "salary_payment"</summary>
            [EnumMember(Value = "salary_payment")]
            SalaryPayment,

            /// <summary>`purpose_code` with a value of "securities"</summary>
            [EnumMember(Value = "securities")]
            Securities,

            /// <summary>`purpose_code` with a value of "social_security_benefit"</summary>
            [EnumMember(Value = "social_security_benefit")]
            SocialSecurityBenefit,

            /// <summary>`purpose_code` with a value of "supplier_payment"</summary>
            [EnumMember(Value = "supplier_payment")]
            SupplierPayment,

            /// <summary>`purpose_code` with a value of "tax_payment"</summary>
            [EnumMember(Value = "tax_payment")]
            TaxPayment,

            /// <summary>`purpose_code` with a value of "trade"</summary>
            [EnumMember(Value = "trade")]
            Trade,

            /// <summary>`purpose_code` with a value of "treasury_payment"</summary>
            [EnumMember(Value = "treasury_payment")]
            TreasuryPayment,

            /// <summary>`purpose_code` with a value of "value_added_tax_payment"</summary>
            [EnumMember(Value = "value_added_tax_payment")]
            ValueAddedTaxPayment,

            /// <summary>`purpose_code` with a value of "with_holding"</summary>
            [EnumMember(Value = "with_holding")]
            WithHolding,

            /// <summary>`purpose_code` with a value of "cash_management_sweep_account"</summary>
            [EnumMember(Value = "cash_management_sweep_account")]
            CashManagementSweepAccount,

            /// <summary>`purpose_code` with a value of "cash_management_top_account"</summary>
            [EnumMember(Value = "cash_management_top_account")]
            CashManagementTopAccount,

            /// <summary>`purpose_code` with a value of "cash_management_zero_balance_account"</summary>
            [EnumMember(Value = "cash_management_zero_balance_account")]
            CashManagementZeroBalanceAccount,

            /// <summary>`purpose_code` with a value of "crossborder_mi_payments"</summary>
            [EnumMember(Value = "crossborder_mi_payments")]
            CrossborderMiPayments,

            /// <summary>`purpose_code` with a value of "foreign_currency_domestic_transfer"</summary>
            [EnumMember(Value = "foreign_currency_domestic_transfer")]
            ForeignCurrencyDomesticTransfer,

            /// <summary>`purpose_code` with a value of "cash_in_pre_credit"</summary>
            [EnumMember(Value = "cash_in_pre_credit")]
            CashInPreCredit,

            /// <summary>`purpose_code` with a value of "cash_out_notes_coins"</summary>
            [EnumMember(Value = "cash_out_notes_coins")]
            CashOutNotesCoins,

            /// <summary>`purpose_code` with a value of "carrier_guarded_wholesale_valuables"</summary>
            [EnumMember(Value = "carrier_guarded_wholesale_valuables")]
            CarrierGuardedWholesaleValuables,
        }

        [JsonProperty("subscription_request")]
        public BillingRequestSubscriptionRequest SubscriptionRequest { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class BillingRequestSubscriptionRequest
        {
            /// <summary>
            /// Amount in the lowest denomination for the currency (e.g. pence
            /// in GBP, cents in EUR).
            /// </summary>
            [JsonProperty("amount")]
            public int? Amount { get; set; }

            /// <summary>
            /// The amount to be deducted from each payment as an app fee, to be
            /// paid to the partner integration which created the subscription,
            /// in the lowest denomination for the currency (e.g. pence in GBP,
            /// cents in EUR).
            /// </summary>
            [JsonProperty("app_fee")]
            public int? AppFee { get; set; }

            /// <summary>
            /// The total number of payments that should be taken by this
            /// subscription.
            /// </summary>
            [JsonProperty("count")]
            public int? Count { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
            /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR",
            /// "GBP", "NZD", "SEK" and "USD" are supported.
            /// </summary>
            [JsonProperty("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// As per RFC 2445. The day of the month to charge customers on.
            /// <c>1</c><ul>
            /// <li></li>
            /// </ul><c>28</c> or <c>-1</c> to indicate the last day of the
            /// month.
            /// </summary>
            [JsonProperty("day_of_month")]
            public int? DayOfMonth { get; set; }

            /// <summary>
            /// Number of <c>interval_units</c> between customer charge dates.
            /// Must be greater than or equal to <c>1</c>. Must result in at
            /// least one charge date per year. Defaults to <c>1</c>.
            /// </summary>
            [JsonProperty("interval")]
            public int? Interval { get; set; }

            /// <summary>
            /// The unit of time between customer charge dates. One of
            /// <c>weekly</c>, <c>monthly</c> or <c>yearly</c>.
            /// </summary>
            [JsonProperty("interval_unit")]
            public BillingRequestIntervalUnit? IntervalUnit { get; set; }

            /// <summary>
            /// The unit of time between customer charge dates. One of
            /// <c>weekly</c>, <c>monthly</c> or <c>yearly</c>.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum BillingRequestIntervalUnit
            {
                /// <summary>`interval_unit` with a value of "weekly"</summary>
                [EnumMember(Value = "weekly")]
                Weekly,

                /// <summary>`interval_unit` with a value of "monthly"</summary>
                [EnumMember(Value = "monthly")]
                Monthly,

                /// <summary>`interval_unit` with a value of "yearly"</summary>
                [EnumMember(Value = "yearly")]
                Yearly,
            }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// Name of the month on which to charge a customer. Must be
            /// lowercase. Only applies
            /// when the interval_unit is <c>yearly</c>.
            /// </summary>
            [JsonProperty("month")]
            public BillingRequestMonth? Month { get; set; }

            /// <summary>
            /// Name of the month on which to charge a customer. Must be lowercase.
            /// Only applies
            /// when the interval_unit is <c>yearly</c>.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum BillingRequestMonth
            {
                /// <summary>`month` with a value of "january"</summary>
                [EnumMember(Value = "january")]
                January,

                /// <summary>`month` with a value of "february"</summary>
                [EnumMember(Value = "february")]
                February,

                /// <summary>`month` with a value of "march"</summary>
                [EnumMember(Value = "march")]
                March,

                /// <summary>`month` with a value of "april"</summary>
                [EnumMember(Value = "april")]
                April,

                /// <summary>`month` with a value of "may"</summary>
                [EnumMember(Value = "may")]
                May,

                /// <summary>`month` with a value of "june"</summary>
                [EnumMember(Value = "june")]
                June,

                /// <summary>`month` with a value of "july"</summary>
                [EnumMember(Value = "july")]
                July,

                /// <summary>`month` with a value of "august"</summary>
                [EnumMember(Value = "august")]
                August,

                /// <summary>`month` with a value of "september"</summary>
                [EnumMember(Value = "september")]
                September,

                /// <summary>`month` with a value of "october"</summary>
                [EnumMember(Value = "october")]
                October,

                /// <summary>`month` with a value of "november"</summary>
                [EnumMember(Value = "november")]
                November,

                /// <summary>`month` with a value of "december"</summary>
                [EnumMember(Value = "december")]
                December,
            }

            /// <summary>
            /// Optional name for the subscription. This will be set as the
            /// description on each payment created. Must not exceed 255
            /// characters.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// An optional payment reference. This will be set as the reference
            /// on each payment
            /// created and will appear on your customer's bank statement. See
            /// the documentation for
            /// the <a
            /// href="https://developer.gocardless.com/api-reference/#payments-create-a-payment">create
            /// payment endpoint</a> for more details.
            /// <br></br>
            /// </summary>
            [JsonProperty("payment_reference")]
            public string PaymentReference { get; set; }

            /// <summary>
            /// On failure, automatically retry payments using <a
            /// href="https://developer.gocardless.com/success-plus/overview">intelligent
            /// retries</a>. Default is <c>false</c>. <p
            /// class="notice">Important: To be able to use intelligent retries,
            /// Success+ needs to be enabled in <a
            /// href="https://manage.gocardless.com/success-plus">GoCardless
            /// dashboard</a>. </p>
            /// </summary>
            [JsonProperty("retry_if_possible")]
            public bool? RetryIfPossible { get; set; }

            /// <summary>
            /// The date on which the first payment should be charged. If
            /// fulfilled after this date, this will be set as the mandate's
            /// <c>next_possible_charge_date</c>.
            /// When left blank and <c>month</c> or <c>day_of_month</c> are
            /// provided, this will be set to the date of the first payment.
            /// If created without <c>month</c> or <c>day_of_month</c> this will
            /// be set as the mandate's <c>next_possible_charge_date</c>.
            /// </summary>
            [JsonProperty("start_date")]
            public string StartDate { get; set; }
        }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// If the billing request has a pending
    /// <code>collect_customer_details</code>
    /// action, this endpoint can be used to collect the details in order to
    /// complete it.
    ///
    /// The endpoint takes the same payload as Customers, but checks that the
    /// customer fields are populated correctly for the billing request scheme.
    ///
    /// Whatever is provided to this endpoint is used to update the referenced
    /// customer, and will take effect immediately after the request is
    /// successful.
    /// </summary>
    public class BillingRequestCollectCustomerDetailsRequest
    {
        [JsonProperty("customer")]
        public BillingRequestCustomer Customer { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class BillingRequestCustomer
        {
            /// <summary>
            /// Customer's company name. Required unless a <c>given_name</c> and
            /// <c>family_name</c> are provided. For Canadian customers, the use
            /// of a <c>company_name</c> value will mean that any mandate
            /// created from this customer will be considered to be a "Business
            /// PAD" (otherwise, any mandate will be considered to be a
            /// "Personal PAD").
            /// </summary>
            [JsonProperty("company_name")]
            public string CompanyName { get; set; }

            /// <summary>
            /// Customer's email address. Required in most cases, as this allows
            /// GoCardless to send notifications to this customer.
            /// </summary>
            [JsonProperty("email")]
            public string Email { get; set; }

            /// <summary>
            /// Customer's surname. Required unless a <c>company_name</c> is
            /// provided.
            /// </summary>
            [JsonProperty("family_name")]
            public string FamilyName { get; set; }

            /// <summary>
            /// Customer's first name. Required unless a <c>company_name</c> is
            /// provided.
            /// </summary>
            [JsonProperty("given_name")]
            public string GivenName { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO
            /// 639-1</a> code. Used as the language for notification emails
            /// sent by GoCardless if your organisation does not send its own
            /// (see <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-compliance-requirements">compliance
            /// requirements</a>). Currently only "en", "fr", "de", "pt", "es",
            /// "it", "nl", "da", "nb", "sl", "sv" are supported. If this is not
            /// provided and a customer was linked during billing request
            /// creation, the linked customer language will be used. Otherwise,
            /// the language is default to "en".
            /// </summary>
            [JsonProperty("language")]
            public string Language { get; set; }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// <a href="https://en.wikipedia.org/wiki/E.123">ITU E.123</a>
            /// formatted phone number, including country code.
            /// </summary>
            [JsonProperty("phone_number")]
            public string PhoneNumber { get; set; }
        }

        [JsonProperty("customer_billing_detail")]
        public BillingRequestCustomerBillingDetail CustomerBillingDetail { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class BillingRequestCustomerBillingDetail
        {
            /// <summary>
            /// The first line of the customer's address.
            /// </summary>
            [JsonProperty("address_line1")]
            public string AddressLine1 { get; set; }

            /// <summary>
            /// The second line of the customer's address.
            /// </summary>
            [JsonProperty("address_line2")]
            public string AddressLine2 { get; set; }

            /// <summary>
            /// The third line of the customer's address.
            /// </summary>
            [JsonProperty("address_line3")]
            public string AddressLine3 { get; set; }

            /// <summary>
            /// The city of the customer's address.
            /// </summary>
            [JsonProperty("city")]
            public string City { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
            /// 3166-1 alpha-2 code.</a>
            /// </summary>
            [JsonProperty("country_code")]
            public string CountryCode { get; set; }

            /// <summary>
            /// For Danish customers only. The civic/company number (CPR or CVR)
            /// of the customer. Must be supplied if the customer's bank account
            /// is denominated in Danish krone (DKK).
            /// </summary>
            [JsonProperty("danish_identity_number")]
            public string DanishIdentityNumber { get; set; }

            /// <summary>
            /// For ACH customers only. Required for ACH customers. A string
            /// containing the IP address of the payer to whom the mandate
            /// belongs (i.e. as a result of their completion of a mandate setup
            /// flow in their browser).
            ///
            /// Not required for creating offline mandates where
            /// <c>authorisation_source</c> is set to telephone or paper.
            /// </summary>
            [JsonProperty("ip_address")]
            public string IpAddress { get; set; }

            /// <summary>
            /// The customer's postal code.
            /// </summary>
            [JsonProperty("postal_code")]
            public string PostalCode { get; set; }

            /// <summary>
            /// The customer's address region, county or department. For US
            /// customers a 2 letter <a
            /// href="https://en.wikipedia.org/wiki/ISO_3166-2:US">ISO3166-2:US</a>
            /// state code is required (e.g. <c>CA</c> for California).
            /// </summary>
            [JsonProperty("region")]
            public string Region { get; set; }

            /// <summary>
            /// For Swedish customers only. The civic/company number
            /// (personnummer, samordningsnummer, or organisationsnummer) of the
            /// customer. Must be supplied if the customer's bank account is
            /// denominated in Swedish krona (SEK). This field cannot be changed
            /// once it has been set.
            /// </summary>
            [JsonProperty("swedish_identity_number")]
            public string SwedishIdentityNumber { get; set; }
        }
    }

    /// <summary>
    /// If the billing request has a pending
    /// <code>collect_bank_account</code> action, this endpoint can be
    /// used to collect the details in order to complete it.
    ///
    /// The endpoint takes the same payload as Customer Bank Accounts, but check
    /// the bank account is valid for the billing request scheme before creating
    /// and attaching it.
    ///
    /// If the scheme is PayTo and the pay_id is available, this can be included
    /// in the payload along with the
    /// country_code.
    ///
    /// ACH scheme For compliance reasons, an extra validation step is done
    /// using
    /// a third-party provider to make sure the customer's bank account can
    /// accept
    /// Direct Debit. If a bank account is discovered to be closed or invalid,
    /// the
    /// customer is requested to adjust the account number/routing number and
    /// succeed in this check to continue with the flow.
    ///
    /// BACS and SEPA schemes <a
    /// href="https://hub.gocardless.com/s/article/Introduction-to-Payer-Name-Verification?language=en_GB">Payer
    /// Name Verification</a>
    /// is enabled by default for UK and Eurozone based bank accounts, meaning
    /// we verify the account holder name and bank account number/IBAN match
    /// the details held by the relevant bank. If there is no match, the
    /// endpoint will return a 422 - validation error on account_holder_name:
    /// "Account holder name does not match bank account details provided".
    /// Testing instructions are <a
    /// href="https://developer.gocardless.com/developer-tools/scenario-simulators/#payer_name_verification">here</a>
    /// </summary>
    public class BillingRequestCollectBankAccountRequest
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. The full name
        /// provided when the customer is created is stored and is available via
        /// the API, but is transliterated, upcased, and truncated to 18
        /// characters in bank submissions. This field is required unless the
        /// request includes a <a
        /// href="https://developer.gocardless.com/api-reference/#javascript-flow-customer-bank-account-tokens">customer
        /// bank account token</a>.
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Bank account number - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Account number suffix (only for bank accounts denominated in NZD) -
        /// see <a
        /// href="https://developer.gocardless.com/api-reference/#local-bank-details-new-zealand">local
        /// details</a> for more information.
        /// </summary>
        [JsonProperty("account_number_suffix")]
        public string AccountNumberSuffix { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See <a
        /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
        /// details</a> for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public BillingRequestAccountType? AccountType { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See <a
        /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
        /// details</a> for more information.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestAccountType
        {
            /// <summary>`account_type` with a value of "savings"</summary>
            [EnumMember(Value = "savings")]
            Savings,

            /// <summary>`account_type` with a value of "checking"</summary>
            [EnumMember(Value = "checking")]
            Checking,
        }

        /// <summary>
        /// Bank code - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Branch code - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// <a
        /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
        /// 3166-1 alpha-2 code</a>. Defaults to the country code of the
        /// <c>iban</c> if supplied, otherwise is required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a>. IBANs are not accepted for Swedish bank accounts
        /// denominated in SEK - you must supply <a
        /// href="https://developer.gocardless.com/api-reference/#local-bank-details-sweden">local
        /// details</a>.
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// A unique record such as an email address, mobile number or company
        /// number, that can be used to make and accept payments.
        /// </summary>
        [JsonProperty("pay_id")]
        public string PayId { get; set; }
    }

    /// <summary>
    /// This is needed when you have a mandate request. As a scheme compliance
    /// rule we are required to
    /// allow the payer to crosscheck the details entered by them and confirm
    /// it.
    /// </summary>
    public class BillingRequestConfirmPayerDetailsRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// This attribute can be set to true if the payer has indicated that
        /// multiple signatures are required for the mandate. As long as every
        /// other Billing Request actions have been completed, the payer will
        /// receive an email notification containing instructions on how to
        /// complete the additional signature. The dual signature flow can only
        /// be completed using GoCardless branded pages.
        /// </summary>
        [JsonProperty("payer_requested_dual_signature")]
        public bool? PayerRequestedDualSignature { get; set; }
    }

    /// <summary>
    /// If a billing request is ready to be fulfilled, call this endpoint to
    /// cause
    /// it to fulfil, executing the payment.
    /// </summary>
    public class BillingRequestFulfilRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// Immediately cancels a billing request, causing all billing request flows
    /// to expire.
    /// </summary>
    public class BillingRequestCancelRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// Returns a <a
    /// href="https://developer.gocardless.com/api-reference/#api-usage-cursor-pagination">cursor-paginated</a>
    /// list of your billing requests.
    /// </summary>
    public class BillingRequestListRequest
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
        /// ID of a <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>.
        /// If specified, this endpoint will return all requests for the given
        /// customer.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>pending</c>: the billing request is pending and can be
        /// used</li>
        /// <li><c>ready_to_fulfil</c>: the billing request is ready to
        /// fulfil</li>
        /// <li><c>fulfilling</c>: the billing request is currently undergoing
        /// fulfilment</li>
        /// <li><c>fulfilled</c>: the billing request has been fulfilled and a
        /// payment created</li>
        /// <li><c>cancelled</c>: the billing request has been cancelled and
        /// cannot be used</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>pending</c>: the billing request is pending and can be
        /// used</li>
        /// <li><c>ready_to_fulfil</c>: the billing request is ready to
        /// fulfil</li>
        /// <li><c>fulfilling</c>: the billing request is currently undergoing
        /// fulfilment</li>
        /// <li><c>fulfilled</c>: the billing request has been fulfilled and a
        /// payment created</li>
        /// <li><c>cancelled</c>: the billing request has been cancelled and
        /// cannot be used</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestStatus
        {
            /// <summary>`status` with a value of "pending"</summary>
            [EnumMember(Value = "pending")]
            Pending,

            /// <summary>`status` with a value of "ready_to_fulfil"</summary>
            [EnumMember(Value = "ready_to_fulfil")]
            ReadyToFulfil,

            /// <summary>`status` with a value of "fulfilling"</summary>
            [EnumMember(Value = "fulfilling")]
            Fulfilling,

            /// <summary>`status` with a value of "fulfilled"</summary>
            [EnumMember(Value = "fulfilled")]
            Fulfilled,

            /// <summary>`status` with a value of "cancelled"</summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,
        }
    }

    /// <summary>
    /// Fetches a billing request
    /// </summary>
    public class BillingRequestGetRequest { }

    /// <summary>
    /// Notifies the customer linked to the billing request, asking them to
    /// authorise it.
    /// Currently, the customer can only be notified by email.
    ///
    /// This endpoint is currently supported only for Pay by Bank Billing
    /// Requests.
    /// </summary>
    public class BillingRequestNotifyRequest
    {
        /// <summary>
        /// Currently, can only be <c>email</c>.
        /// </summary>
        [JsonProperty("notification_type")]
        public string NotificationType { get; set; }

        /// <summary>
        /// Currently, can only be <c>email</c>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestNotificationType
        {
            /// <summary>`notification_type` with a value of "email"</summary>
            [EnumMember(Value = "email")]
            Email,
        }

        /// <summary>
        /// URL that the payer can be redirected to after authorising the
        /// payment.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }

    /// <summary>
    /// Triggers a fallback from the open-banking flow to direct debit. Note,
    /// the billing request must have fallback enabled.
    /// </summary>
    public class BillingRequestFallbackRequest { }

    /// <summary>
    /// This will allow for the updating of the currency and subsequently the
    /// scheme if
    /// needed for a Billing Request. This will only be available for mandate
    /// only flows
    /// which do not have the lock_currency flag set to true on the Billing
    /// Request Flow. It
    /// will also not support any request which has a payments request.
    /// </summary>
    public class BillingRequestChooseCurrencyRequest
    {
        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// Creates an Institution object and attaches it to the Billing Request
    /// </summary>
    public class BillingRequestSelectInstitutionRequest
    {
        /// <summary>
        /// <a
        /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
        /// 3166-1</a> alpha-2 code. The country code of the institution. If
        /// nothing is provided, institutions with the country code 'GB' are
        /// returned by default.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// The unique identifier for this institution
        /// </summary>
        [JsonProperty("institution")]
        public string Institution { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single billing request.
    /// </summary>
    public class BillingRequestResponse : ApiResponse
    {
        /// <summary>
        /// The billing request from the response.
        /// </summary>
        [JsonProperty("billing_requests")]
        public BillingRequest BillingRequest { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of billing requests.
    /// </summary>
    public class BillingRequestListResponse : ApiResponse
    {
        /// <summary>
        /// The list of billing requests from the response.
        /// </summary>
        [JsonProperty("billing_requests")]
        public IReadOnlyList<BillingRequest> BillingRequests { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
