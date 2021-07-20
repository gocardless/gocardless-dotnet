

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
    /// Billing Requests
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
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your billing_requests.
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
        /// 
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
        /// This is needed when you have mandate_request. As a scheme compliance
        /// rule we are required to
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
        /// Notifies the customer linked to the billing request, asking them to
        /// authorise it.
        /// Currently, the customer can only be notified by email.
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
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// billing_requests.
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
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
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
        /// <li>`pending`: the billing_request is pending and can be used</li>
        /// <li>`ready_to_fulfil`: the billing_request is ready to fulfil</li>
        /// <li>`fulfilled`: the billing_request has been fulfilled and a
        /// payment created</li>
        /// <li>`cancelled`: the billing_request has been cancelled and cannot
        /// be used</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
            
        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the billing_request is pending and can be used</li>
        /// <li>`ready_to_fulfil`: the billing_request is ready to fulfil</li>
        /// <li>`fulfilled`: the billing_request has been fulfilled and a
        /// payment created</li>
        /// <li>`cancelled`: the billing_request has been cancelled and cannot
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
            /// <summary>`status` with a value of "fulfilled"</summary>
            [EnumMember(Value = "fulfilled")]
            Fulfilled,
            /// <summary>`status` with a value of "cancelled"</summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,
        }
    }

        
    /// <summary>
    /// 
    /// </summary>
    public class BillingRequestCreateRequest : IHasIdempotencyKey
    {

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
            /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
            /// currency code. Currently only "GBP" is supported as we only have
            /// one scheme that is per_payment_authorised.
            /// </summary>
            [JsonProperty("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// A Direct Debit scheme. Currently "ach", "autogiro", "bacs",
            /// "becs", "becs_nz", "betalingsservice", "pad" and "sepa_core" are
            /// supported.
            /// </summary>
            [JsonProperty("scheme")]
            public string Scheme { get; set; }
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
            /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
            /// currency code. Currently only "GBP" is supported as we only have
            /// one scheme that is per_payment_authorised.
            /// </summary>
            [JsonProperty("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// A human-readable description of the payment. This will be
            /// displayed to the payer when authorising the billing request.
            /// 
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }
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
    /// Fetches a billing request
    /// </summary>
    public class BillingRequestGetRequest
    {
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
            /// [ISO
            /// 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
            /// code. Used as the language for notification emails sent by
            /// GoCardless if your organisation does not send its own (see
            /// [compliance requirements](#appendix-compliance-requirements)).
            /// Currently only "en", "fr", "de", "pt", "es", "it", "nl", "da",
            /// "nb", "sl", "sv" are supported. If this is not provided, the
            /// language will be chosen based on the `country_code` (if
            /// supplied) or default to "en".
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
    /// </summary>
    public class BillingRequestCollectBankAccountRequest
    {

        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this is
        /// the same as the name stored with the linked
        /// [creditor](#core-endpoints-creditors). This field will be
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
    /// This is needed when you have mandate_request. As a scheme compliance
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
    /// Notifies the customer linked to the billing request, asking them to
    /// authorise it.
    /// Currently, the customer can only be notified by email.
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
