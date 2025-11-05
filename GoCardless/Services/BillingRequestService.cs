

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
    ///  Billing Requests help create resources that require input or action
    /// from a customer. An example of required input might be additional
    /// customer billing details, while an action would be asking a customer to
    /// authorise a payment using their mobile banking app.
    /// 
    /// See [Billing Requests:
    /// Overview](https://developer.gocardless.com/getting-started/billing-requests/overview/)
    /// for how-to's, explanations and tutorials. <p
    /// class="notice"><strong>Important</strong>: All properties associated
    /// with `subscription_request` and `instalment_schedule_request` are only
    /// supported for ACH and PAD schemes.</p>
    /// </summary>

    public class BillingRequestService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BillingRequestService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// <p class="notice"><strong>Important</strong>: All properties
        /// associated with `subscription_request` and
        /// `instalment_schedule_request` are only supported for ACH and PAD
        /// schemes.</p>
        /// </summary>
        /// <param name="request">An optional `BillingRequestCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CreateAsync(BillingRequestCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "billing_requests", customiseRequestMessage);
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
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestCollectCustomerDetailsRequest` representing the body for this collect_customer_details request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CollectCustomerDetailsAsync(string identity, BillingRequestCollectCustomerDetailsRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestCollectCustomerDetailsRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/collect_customer_details", urlParams, request, null, "data", customiseRequestMessage);
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
        /// _ACH scheme_ For compliance reasons, an extra validation step is
        /// done using
        /// a third-party provider to make sure the customer's bank account can
        /// accept
        /// Direct Debit. If a bank account is discovered to be closed or
        /// invalid, the
        /// customer is requested to adjust the account number/routing number
        /// and
        /// succeed in this check to continue with the flow.
        /// 
        /// _BACS scheme_ [Payer Name
        /// Verification](https://hub.gocardless.com/s/article/Introduction-to-Payer-Name-Verification?language=en_GB)
        /// is enabled by default for UK based bank accounts, meaning we verify
        /// the account holder name and bank account
        /// number match the details held by the relevant bank.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestCollectBankAccountRequest` representing the body for this collect_bank_account request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CollectBankAccountAsync(string identity, BillingRequestCollectBankAccountRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestCollectBankAccountRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/collect_bank_account", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// This is needed when you have a mandate request. As a scheme
        /// compliance rule we are required to
        /// allow the payer to crosscheck the details entered by them and
        /// confirm it.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestConfirmPayerDetailsRequest` representing the body for this confirm_payer_details request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> ConfirmPayerDetailsAsync(string identity, BillingRequestConfirmPayerDetailsRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestConfirmPayerDetailsRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/confirm_payer_details", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// If a billing request is ready to be fulfilled, call this endpoint to
        /// cause
        /// it to fulfil, executing the payment.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestFulfilRequest` representing the body for this fulfil request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> FulfilAsync(string identity, BillingRequestFulfilRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestFulfilRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/fulfil", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// Immediately cancels a billing request, causing all billing request
        /// flows
        /// to expire.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestCancelRequest` representing the body for this cancel request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> CancelAsync(string identity, BillingRequestCancelRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestCancelRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/cancel", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your billing requests.
        /// </summary>
        /// <param name="request">An optional `BillingRequestListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of billing request resources</returns>
        public Task<BillingRequestListResponse> ListAsync(BillingRequestListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BillingRequestListResponse>("GET", "/billing_requests", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of billing requests.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<BillingRequest> All(BillingRequestListRequest request = null, RequestSettings customiseRequestMessage = null)
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
        public IEnumerable<Task<IReadOnlyList<BillingRequest>>> AllAsync(BillingRequestListRequest request = null, RequestSettings customiseRequestMessage = null)
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
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> GetAsync(string identity, BillingRequestGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("GET", "/billing_requests/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Notifies the customer linked to the billing request, asking them to
        /// authorise it.
        /// Currently, the customer can only be notified by email.
        /// 
        /// This endpoint is currently supported only for Instant Bank Pay
        /// Billing Requests.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestNotifyRequest` representing the body for this notify request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> NotifyAsync(string identity, BillingRequestNotifyRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestNotifyRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/notify", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// Triggers a fallback from the open-banking flow to direct debit.
        /// Note, the billing request must have fallback enabled.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestFallbackRequest` representing the body for this fallback request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> FallbackAsync(string identity, BillingRequestFallbackRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestFallbackRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/fallback", urlParams, request, null, "data", customiseRequestMessage);
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
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestChooseCurrencyRequest` representing the body for this choose_currency request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> ChooseCurrencyAsync(string identity, BillingRequestChooseCurrencyRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestChooseCurrencyRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/choose_currency", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// Creates an Institution object and attaches it to the Billing Request
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestSelectInstitutionRequest` representing the body for this select_institution request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request resource</returns>
        public Task<BillingRequestResponse> SelectInstitutionAsync(string identity, BillingRequestSelectInstitutionRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestSelectInstitutionRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestResponse>("POST", "/billing_requests/:identity/actions/select_institution", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// <p class="notice"><strong>Important</strong>: All properties associated
    /// with `subscription_request` and `instalment_schedule_request` are only
    /// supported for ACH and PAD schemes.</p>
    /// </summary>
    public class BillingRequestCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// (Optional) If true, this billing request can fallback from instant
        /// payment to direct debit.
        /// Should not be set if GoCardless payment intelligence feature is
        /// used.
        /// 
        /// See [Billing Requests: Retain customers with
        /// Fallbacks](https://developer.gocardless.com/billing-requests/retain-customers-with-fallbacks/)
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
                            /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
            /// currency code. Currently "USD" and "CAD" are supported.
                /// </summary>
                [JsonProperty("currency")]
                public string Currency { get; set; }
                
                /// <summary>
                            /// An explicit array of instalment payments, each specifying at
            /// least an `amount` and `charge_date`. See [create (with
            /// dates)](#instalment-schedules-create-with-dates)
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
                /// </summary>
                [JsonProperty("amount")]
                public int? Amount { get; set; }
                
                /// <summary>
                            /// A future date on which the payment should be collected. If the
            /// date
            /// is before the next_possible_charge_date on the
            /// [mandate](#core-endpoints-mandates), it will be automatically
            /// rolled
            /// forwards to that date.
                /// </summary>
                [JsonProperty("charge_date")]
                public string ChargeDate { get; set; }
                
                /// <summary>
                            /// A human-readable description of the payment. This will be
            /// included in the notification email GoCardless sends to your
            /// customer if your organisation does not send its own
            /// notifications (see [compliance
            /// requirements](#appendix-compliance-requirements)).
                /// </summary>
                [JsonProperty("description")]
                public string Description { get; set; }
        }
                
                /// <summary>
                            /// Frequency of the payments you want to create, together with an
            /// array of payment
            /// amounts to be collected, with a specified start date for the
            /// first payment.
            /// See [create (with
            /// schedule)](#instalment-schedules-create-with-schedule)
            /// 
                /// </summary>
                [JsonProperty("instalments_with_schedule")]
                public BillingRequestInstalmentsWithSchedule InstalmentsWithSchedule { get; set; }
        /// <summary>
        /// Frequency of the payments you want to create, together with an array
        /// of payment
        /// amounts to be collected, with a specified start date for the first
        /// payment.
        /// See [create (with
        /// schedule)](#instalment-schedules-create-with-schedule)
        /// 
        /// </summary>
        public class BillingRequestInstalmentsWithSchedule
        {
                
                /// <summary>
                            /// List of amounts of each instalment, in the lowest denomination
            /// for the
            /// currency (e.g. cents in USD).
            /// 
                /// </summary>
                [JsonProperty("amounts")]
                public int?[] Amounts { get; set; }
                
                /// <summary>
                            /// Number of `interval_units` between charge dates. Must be greater
            /// than or
            /// equal to `1`.
            /// 
                /// </summary>
                [JsonProperty("interval")]
                public int? Interval { get; set; }
                
                /// <summary>
                            /// The unit of time between customer charge dates. One of `weekly`,
            /// `monthly` or `yearly`.
                /// </summary>
                [JsonProperty("interval_unit")]
                public BillingRequestIntervalUnit? IntervalUnit { get; set; }
        /// <summary>
        /// The unit of time between customer charge dates. One of `weekly`,
        /// `monthly` or `yearly`.
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
            /// on or after the [mandate](#core-endpoints-mandates)'s
            /// `next_possible_charge_date`. When left blank and `month` or
            /// `day_of_month` are provided, this will be set to the date of the
            /// first payment. If created without `month` or `day_of_month` this
            /// will be set as the mandate's `next_possible_charge_date`
                /// </summary>
                [JsonProperty("start_date")]
                public string StartDate { get; set; }
        }
                
                /// <summary>
                            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
                /// </summary>
                [JsonProperty("metadata")]
                public IDictionary<String, String> Metadata { get; set; }
                
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
            /// the [create payment endpoint](#payments-create-a-payment) for
            /// more details.
            /// <br />
                /// </summary>
                [JsonProperty("payment_reference")]
                public string PaymentReference { get; set; }
                
                /// <summary>
                            /// On failure, automatically retry payments using [intelligent
            /// retries](/success-plus/overview). Default is `false`. <p
            /// class="notice"><strong>Important</strong>: To be able to use
            /// intelligent retries, Success+ needs to be enabled in [GoCardless
            /// dashboard](https://manage.gocardless.com/success-plus). </p>
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
                            /// ID of the associated [creditor](#core-endpoints-creditors). Only
            /// required if your account manages multiple creditors.
                /// </summary>
                [JsonProperty("creditor")]
                public string Creditor { get; set; }
                
                /// <summary>
                            /// ID of the [customer](#core-endpoints-customers) against which
            /// this request should be made.
                /// </summary>
                [JsonProperty("customer")]
                public string Customer { get; set; }
                
                /// <summary>
                            /// (Optional) ID of the
            /// [customer_bank_account](#core-endpoints-customer-bank-accounts)
            /// against which this request should be made.
            /// 
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
                            /// This field is ACH specific, sometimes referred to as [SEC
            /// code](https://www.moderntreasury.com/learn/sec-codes).
            /// 
            /// This is the way that the payer gives authorisation to the
            /// merchant.
            ///   web: Authorisation is Internet Initiated or via Mobile Entry
            /// (maps to SEC code: WEB)
            ///   telephone: Authorisation is provided orally over telephone
            /// (maps to SEC code: TEL)
            ///   paper: Authorisation is provided in writing and signed, or
            /// similarly authenticated (maps to SEC code: PPD)
            /// 
                /// </summary>
                [JsonProperty("authorisation_source")]
                public BillingRequestAuthorisationSource? AuthorisationSource { get; set; }
        /// <summary>
        /// This field is ACH specific, sometimes referred to as [SEC
        /// code](https://www.moderntreasury.com/learn/sec-codes).
        /// 
        /// This is the way that the payer gives authorisation to the merchant.
        ///   web: Authorisation is Internet Initiated or via Mobile Entry (maps
        /// to SEC code: WEB)
        ///   telephone: Authorisation is provided orally over telephone (maps
        /// to SEC code: TEL)
        ///   paper: Authorisation is provided in writing and signed, or
        /// similarly authenticated (maps to SEC code: PPD)
        /// 
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
            /// payer and merchant. It can be set to `one_off`,
            /// `recurring` or `standing` for ACH scheme. And `single`,
            /// `recurring` and `sporadic` for PAD scheme. _Note:_ This is only
            /// supported for ACH and PAD schemes.
            /// 
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
            /// 
                /// </summary>
                [JsonProperty("end_date")]
                public string EndDate { get; set; }
                
                /// <summary>
                            /// The maximum amount that can be charged for a single payment.
            /// Required for PayTo and VRP.
                /// </summary>
                [JsonProperty("max_amount_per_payment")]
                public int? MaxAmountPerPayment { get; set; }
                
                /// <summary>
                            /// A constraint where you can specify info (free text string) about
            /// how payments are calculated. _Note:_ This is only supported for
            /// ACH and PAD schemes.
            /// 
                /// </summary>
                [JsonProperty("payment_method")]
                public string PaymentMethod { get; set; }
                
                /// <summary>
                            /// List of periodic limits and constraints which apply to them
                /// </summary>
                [JsonProperty("periodic_limits")]
                public BillingRequestPeriodicLimits[] PeriodicLimits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public class BillingRequestPeriodicLimits
        {
                
                /// <summary>
                            /// The alignment of the period.
            /// 
            /// `calendar` - this will finish on the end of the current period.
            /// For example this will expire on the Monday for the current week
            /// or the January for the next year.
            /// 
            /// `creation_date` - this will finish on the next instance of the
            /// current period. For example Monthly it will expire on the same
            /// day of the next month, or yearly the same day of the next year.
            /// 
                /// </summary>
                [JsonProperty("alignment")]
                public BillingRequestAlignment? Alignment { get; set; }
        /// <summary>
        /// The alignment of the period.
        /// 
        /// `calendar` - this will finish on the end of the current period. For
        /// example this will expire on the Monday for the current week or the
        /// January for the next year.
        /// 
        /// `creation_date` - this will finish on the next instance of the
        /// current period. For example Monthly it will expire on the same day
        /// of the next month, or yearly the same day of the next year.
        /// 
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
                            /// (Optional) The maximum number of payments that can be collected
            /// in this periodic limit.
                /// </summary>
                [JsonProperty("max_payments")]
                public int? MaxPayments { get; set; }
                
                /// <summary>
                            /// The maximum total amount that can be charged for all payments in
            /// this periodic limit.
            /// Required for VRP.
            /// 
                /// </summary>
                [JsonProperty("max_total_amount")]
                public int? MaxTotalAmount { get; set; }
                
                /// <summary>
                            /// The repeating period for this mandate. Defaults to flexible for
            /// PayTo if not specified.
                /// </summary>
                [JsonProperty("period")]
                public BillingRequestPeriod? Period { get; set; }
        /// <summary>
        /// The repeating period for this mandate. Defaults to flexible for
        /// PayTo if not specified.
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
            /// 
                /// </summary>
                [JsonProperty("start_date")]
                public string StartDate { get; set; }
        }
                
                /// <summary>
                            /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
            /// currency code.
                /// </summary>
                [JsonProperty("currency")]
                public string Currency { get; set; }
                
                /// <summary>
                            /// A human-readable description of the payment and/or mandate. This
            /// will be displayed to the payer when authorising the billing
            /// request.
            /// 
                /// </summary>
                [JsonProperty("description")]
                public string Description { get; set; }
                
                /// <summary>
                            /// This field will decide how GoCardless handles settlement of
            /// funds from the customer.
            /// 
            /// - `managed` will be moved through GoCardless' account, batched,
            /// and payed out.
            /// - `direct` will be a direct transfer from the payer's account to
            /// the merchant where
            ///   invoicing will be handled separately.
            /// 
                /// </summary>
                [JsonProperty("funds_settlement")]
                public BillingRequestFundsSettlement? FundsSettlement { get; set; }
        /// <summary>
        /// This field will decide how GoCardless handles settlement of funds
        /// from the customer.
        /// 
        /// - `managed` will be moved through GoCardless' account, batched, and
        /// payed out.
        /// - `direct` will be a direct transfer from the payer's account to the
        /// merchant where
        ///   invoicing will be handled separately.
        /// 
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
                public IDictionary<String, String> Metadata { get; set; }
                
                /// <summary>
                            /// Unique reference. Different schemes have different length and
            /// [character set](#appendix-character-sets) requirements.
            /// GoCardless will generate a unique reference satisfying the
            /// different scheme requirements if this field is left blank.
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
            /// <ul>
            ///   <li>`minimum`: only verify if absolutely required, such as
            /// when part of scheme rules</li>
            ///   <li>`recommended`: in addition to `minimum`, use the
            /// GoCardless payment intelligence solution to decide if a payer
            /// should be verified</li>
            ///   <li>`when_available`: if verification mechanisms are
            /// available, use them</li>
            ///   <li>`always`: as `when_available`, but fail to create the
            /// Billing Request if a mechanism isn't available</li>
            /// </ul>
            /// 
            /// By default, all Billing Requests use the `recommended`
            /// verification preference. It uses GoCardless payment intelligence
            /// solution to determine if a payer is fraudulent or not. The
            /// verification mechanism is based on the response and the payer
            /// may be asked to verify themselves. If the feature is not
            /// available, `recommended` behaves like `minimum`.
            /// 
            /// If you never wish to take advantage of our reduced risk products
            /// and Verified Mandates as they are released in new schemes,
            /// please use the `minimum` verification preference.
            /// 
            /// See [Billing Requests: Creating Verified
            /// Mandates](https://developer.gocardless.com/getting-started/billing-requests/verified-mandates/)
            /// for more information.
                /// </summary>
                [JsonProperty("verify")]
                public BillingRequestVerify? Verify { get; set; }
        /// <summary>
        /// Verification preference for the mandate. One of:
        /// <ul>
        ///   <li>`minimum`: only verify if absolutely required, such as when
        /// part of scheme rules</li>
        ///   <li>`recommended`: in addition to `minimum`, use the GoCardless
        /// payment intelligence solution to decide if a payer should be
        /// verified</li>
        ///   <li>`when_available`: if verification mechanisms are available,
        /// use them</li>
        ///   <li>`always`: as `when_available`, but fail to create the Billing
        /// Request if a mechanism isn't available</li>
        /// </ul>
        /// 
        /// By default, all Billing Requests use the `recommended` verification
        /// preference. It uses GoCardless payment intelligence solution to
        /// determine if a payer is fraudulent or not. The verification
        /// mechanism is based on the response and the payer may be asked to
        /// verify themselves. If the feature is not available, `recommended`
        /// behaves like `minimum`.
        /// 
        /// If you never wish to take advantage of our reduced risk products and
        /// Verified Mandates as they are released in new schemes, please use
        /// the `minimum` verification preference.
        /// 
        /// See [Billing Requests: Creating Verified
        /// Mandates](https://developer.gocardless.com/getting-started/billing-requests/verified-mandates/)
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
        public IDictionary<String, String> Metadata { get; set; }

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
                            /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
            /// currency code. `GBP` and `EUR` supported; `GBP` with your
            /// customers in the UK and for `EUR` with your customers in
            /// supported Eurozone countries only.
                /// </summary>
                [JsonProperty("currency")]
                public string Currency { get; set; }
                
                /// <summary>
                            /// A human-readable description of the payment and/or mandate. This
            /// will be displayed to the payer when authorising the billing
            /// request.
            /// 
                /// </summary>
                [JsonProperty("description")]
                public string Description { get; set; }
                
                /// <summary>
                            /// This field will decide how GoCardless handles settlement of
            /// funds from the customer.
            /// 
            /// - `managed` will be moved through GoCardless' account, batched,
            /// and payed out.
            /// - `direct` will be a direct transfer from the payer's account to
            /// the merchant where
            ///   invoicing will be handled separately.
            /// 
                /// </summary>
                [JsonProperty("funds_settlement")]
                public BillingRequestFundsSettlement? FundsSettlement { get; set; }
        /// <summary>
        /// This field will decide how GoCardless handles settlement of funds
        /// from the customer.
        /// 
        /// - `managed` will be moved through GoCardless' account, batched, and
        /// payed out.
        /// - `direct` will be a direct transfer from the payer's account to the
        /// merchant where
        ///   invoicing will be handled separately.
        /// 
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
                public IDictionary<String, String> Metadata { get; set; }
                
                /// <summary>
                            /// A custom payment reference defined by the merchant. It is only
            /// available for payments on the PayTo scheme or payments using the
            /// Direct Funds settlement model on the Faster Payments scheme.
            /// 
                /// </summary>
                [JsonProperty("reference")]
                public string Reference { get; set; }
                
                /// <summary>
                            /// On failure, automatically retry payments using [intelligent
            /// retries](/success-plus/overview). Default is `false`. <p
            /// class="notice"><strong>Important</strong>: To be able to use
            /// intelligent retries, Success+ needs to be enabled in [GoCardless
            /// dashboard](https://manage.gocardless.com/success-plus). </p> <p
            /// class="notice"><strong>Important</strong>: This is not
            /// applicable to IBP and VRP payments. </p>
                /// </summary>
                [JsonProperty("retry_if_possible")]
                public bool? RetryIfPossible { get; set; }
                
                /// <summary>
                            /// (Optional) A scheme used for Open Banking payments. Currently
            /// `faster_payments` is supported in the UK (GBP) and
            /// `sepa_credit_transfer` and `sepa_instant_credit_transfer` are
            /// supported in supported Eurozone countries (EUR). For Eurozone
            /// countries, `sepa_credit_transfer` is used as the default. Please
            /// be aware that `sepa_instant_credit_transfer` may incur an
            /// additional fee for your customer.
                /// </summary>
                [JsonProperty("scheme")]
                public string Scheme { get; set; }
        }

        /// <summary>
        /// Specifies the high-level purpose of a mandate and/or payment using a
        /// set of pre-defined categories. Required for the PayTo scheme,
        /// optional for all others. Currently `mortgage`, `utility`, `loan`,
        /// `dependant_support`, `gambling`, `retail`, `salary`, `personal`,
        /// `government`, `pension`, `tax` and `other` are supported.
        /// </summary>
        [JsonProperty("purpose_code")]
        public BillingRequestPurposeCode? PurposeCode { get; set; }
            
        /// <summary>
        /// Specifies the high-level purpose of a mandate and/or payment using a
        /// set of pre-defined categories. Required for the PayTo scheme,
        /// optional for all others. Currently `mortgage`, `utility`, `loan`,
        /// `dependant_support`, `gambling`, `retail`, `salary`, `personal`,
        /// `government`, `pension`, `tax` and `other` are supported.
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
            /// <summary>`purpose_code` with a value of "Epayment"</summary>
            [EnumMember(Value = "Epayment")]
            Epayment,
            /// <summary>`purpose_code` with a value of "Commercial"</summary>
            [EnumMember(Value = "Commercial")]
            Commercial,
            /// <summary>`purpose_code` with a value of "OtherPayment"</summary>
            [EnumMember(Value = "OtherPayment")]
            OtherPayment,
            /// <summary>`purpose_code` with a value of "Trade"</summary>
            [EnumMember(Value = "Trade")]
            Trade,
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
                            /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
            /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
            /// "NZD", "SEK" and "USD" are supported.
                /// </summary>
                [JsonProperty("currency")]
                public string Currency { get; set; }
                
                /// <summary>
                            /// As per RFC 2445. The day of the month to charge customers on.
            /// `1`-`28` or `-1` to indicate the last day of the month.
                /// </summary>
                [JsonProperty("day_of_month")]
                public int? DayOfMonth { get; set; }
                
                /// <summary>
                            /// Number of `interval_units` between customer charge dates. Must
            /// be greater than or equal to `1`. Must result in at least one
            /// charge date per year. Defaults to `1`.
                /// </summary>
                [JsonProperty("interval")]
                public int? Interval { get; set; }
                
                /// <summary>
                            /// The unit of time between customer charge dates. One of `weekly`,
            /// `monthly` or `yearly`.
                /// </summary>
                [JsonProperty("interval_unit")]
                public BillingRequestIntervalUnit? IntervalUnit { get; set; }
        /// <summary>
        /// The unit of time between customer charge dates. One of `weekly`,
        /// `monthly` or `yearly`.
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
                public IDictionary<String, String> Metadata { get; set; }
                
                /// <summary>
                            /// Name of the month on which to charge a customer. Must be
            /// lowercase. Only applies
            /// when the interval_unit is `yearly`.
            /// 
                /// </summary>
                [JsonProperty("month")]
                public BillingRequestMonth? Month { get; set; }
        /// <summary>
        /// Name of the month on which to charge a customer. Must be lowercase.
        /// Only applies
        /// when the interval_unit is `yearly`.
        /// 
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
            /// the [create payment endpoint](#payments-create-a-payment) for
            /// more details.
            /// <br />
                /// </summary>
                [JsonProperty("payment_reference")]
                public string PaymentReference { get; set; }
                
                /// <summary>
                            /// On failure, automatically retry payments using [intelligent
            /// retries](/success-plus/overview). Default is `false`. <p
            /// class="notice"><strong>Important</strong>: To be able to use
            /// intelligent retries, Success+ needs to be enabled in [GoCardless
            /// dashboard](https://manage.gocardless.com/success-plus). </p>
                /// </summary>
                [JsonProperty("retry_if_possible")]
                public bool? RetryIfPossible { get; set; }
                
                /// <summary>
                            /// The date on which the first payment should be charged. If
            /// fulfilled after this date, this will be set as the mandate's
            /// `next_possible_charge_date`.
            /// When left blank and `month` or `day_of_month` are provided, this
            /// will be set to the date of the first payment.
            /// If created without `month` or `day_of_month` this will be set as
            /// the mandate's `next_possible_charge_date`.
            /// 
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
                            /// Customer's company name. Required unless a `given_name` and
            /// `family_name` are provided. For Canadian customers, the use of a
            /// `company_name` value will mean that any mandate created from
            /// this customer will be considered to be a "Business PAD"
            /// (otherwise, any mandate will be considered to be a "Personal
            /// PAD").
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
                            /// Customer's surname. Required unless a `company_name` is
            /// provided.
                /// </summary>
                [JsonProperty("family_name")]
                public string FamilyName { get; set; }
                
                /// <summary>
                            /// Customer's first name. Required unless a `company_name` is
            /// provided.
                /// </summary>
                [JsonProperty("given_name")]
                public string GivenName { get; set; }
                
                /// <summary>
                            ///  [ISO
            /// 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
            /// code. Used as the language for notification emails sent by
            /// GoCardless if your organisation does not send its own (see
            /// [compliance requirements](#appendix-compliance-requirements)).
            /// Currently only "en", "fr", "de", "pt", "es", "it", "nl", "da",
            /// "nb", "sl", "sv" are supported. If this is not provided and a
            /// customer was linked during billing request creation, the linked
            /// customer language will be used. Otherwise, the language is
            /// default to "en".
                /// </summary>
                [JsonProperty("language")]
                public string Language { get; set; }
                
                /// <summary>
                            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
                /// </summary>
                [JsonProperty("metadata")]
                public IDictionary<String, String> Metadata { get; set; }
                
                /// <summary>
                            /// [ITU E.123](https://en.wikipedia.org/wiki/E.123) formatted phone
            /// number, including country code.
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
                            /// [ISO 3166-1 alpha-2
            /// code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
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
            /// `authorisation_source` is set to telephone or paper.
            /// 
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
            /// customers a 2 letter
            /// [ISO3166-2:US](https://en.wikipedia.org/wiki/ISO_3166-2:US)
            /// state code is required (e.g. `CA` for California).
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
    /// _ACH scheme_ For compliance reasons, an extra validation step is done
    /// using
    /// a third-party provider to make sure the customer's bank account can
    /// accept
    /// Direct Debit. If a bank account is discovered to be closed or invalid,
    /// the
    /// customer is requested to adjust the account number/routing number and
    /// succeed in this check to continue with the flow.
    /// 
    /// _BACS scheme_ [Payer Name
    /// Verification](https://hub.gocardless.com/s/article/Introduction-to-Payer-Name-Verification?language=en_GB)
    /// is enabled by default for UK based bank accounts, meaning we verify the
    /// account holder name and bank account
    /// number match the details held by the relevant bank.
    /// </summary>
    public class BillingRequestCollectBankAccountRequest
    {

        /// <summary>
        /// Name of the account holder, as known by the bank. This field will be
        /// transliterated, upcased and truncated to 18 characters. This field
        /// is required unless the request includes a [customer bank account
        /// token](#javascript-flow-customer-bank-account-tokens).
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Bank account number - see [local
        /// details](#appendix-local-bank-details) for more information.
        /// Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Account number suffix (only for bank accounts denominated in NZD) -
        /// see [local details](#local-bank-details-new-zealand) for more
        /// information.
        /// </summary>
        [JsonProperty("account_number_suffix")]
        public string AccountNumberSuffix { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public BillingRequestAccountType? AccountType { get; set; }
            
        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
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
        /// Bank code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Branch code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements).
        /// Defaults to the country code of the `iban` if supplied, otherwise is
        /// required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide
        /// [local details](#appendix-local-bank-details). IBANs are not
        /// accepted for Swedish bank accounts denominated in SEK - you must
        /// supply [local details](#local-bank-details-sweden).
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

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
        public IDictionary<String, String> Metadata { get; set; }

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
        public IDictionary<String, String> Metadata { get; set; }
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
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// billing requests.
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
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
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
        /// ID of a [customer](#core-endpoints-customers). If specified, this
        /// endpoint will return all requests for the given customer.
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
        /// <ul>
        /// <li>`pending`: the billing request is pending and can be used</li>
        /// <li>`ready_to_fulfil`: the billing request is ready to fulfil</li>
        /// <li>`fulfilling`: the billing request is currently undergoing
        /// fulfilment</li>
        /// <li>`fulfilled`: the billing request has been fulfilled and a
        /// payment created</li>
        /// <li>`cancelled`: the billing request has been cancelled and cannot
        /// be used</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
            
        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the billing request is pending and can be used</li>
        /// <li>`ready_to_fulfil`: the billing request is ready to fulfil</li>
        /// <li>`fulfilling`: the billing request is currently undergoing
        /// fulfilment</li>
        /// <li>`fulfilled`: the billing request has been fulfilled and a
        /// payment created</li>
        /// <li>`cancelled`: the billing request has been cancelled and cannot
        /// be used</li>
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
    public class BillingRequestGetRequest
    {
    }

        
    /// <summary>
    /// Notifies the customer linked to the billing request, asking them to
    /// authorise it.
    /// Currently, the customer can only be notified by email.
    /// 
    /// This endpoint is currently supported only for Instant Bank Pay Billing
    /// Requests.
    /// </summary>
    public class BillingRequestNotifyRequest
    {

        /// <summary>
        /// Currently, can only be `email`.
        /// </summary>
        [JsonProperty("notification_type")]
        public string NotificationType { get; set; }
            
        /// <summary>
        /// Currently, can only be `email`.
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
    public class BillingRequestFallbackRequest
    {
    }

        
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
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    /// <summary>
    /// Creates an Institution object and attaches it to the Billing Request
    /// </summary>
    public class BillingRequestSelectInstitutionRequest
    {

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. The country code of the institution. If nothing is
        /// provided, institutions with the country code 'GB' are returned by
        /// default.
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
        public Meta Meta { get; private set; }}
}
