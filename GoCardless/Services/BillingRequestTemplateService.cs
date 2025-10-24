

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
    /// Service class for working with billing request template resources.
    ///
    /// Billing Request Templates are reusable templates that result in
    /// numerous Billing Requests with similar attributes. They provide
    /// a no-code solution for generating various types of multi-user payment
    /// links.
    /// 
    /// Each template includes a reusable URL that can be embedded in a website
    /// or shared with customers via email. Every time the URL is opened,
    /// it generates a new Billing Request.
    /// 
    /// Billing Request Templates overcome the key limitation of the Billing
    /// Request:
    /// a Billing Request cannot be shared among multiple users because it is
    /// intended
    /// for single-use and is designed to cater to the unique needs of
    /// individual customers.
    /// </summary>

    public class BillingRequestTemplateService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BillingRequestTemplateService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your Billing Request Templates.
        /// </summary>
        /// <param name="request">An optional `BillingRequestTemplateListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of billing request template resources</returns>
        public Task<BillingRequestTemplateListResponse> ListAsync(BillingRequestTemplateListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestTemplateListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BillingRequestTemplateListResponse>("GET", "/billing_request_templates", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of billing request templates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<BillingRequestTemplate> All(BillingRequestTemplateListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestTemplateListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.BillingRequestTemplates)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of billing request templates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<BillingRequestTemplate>>> AllAsync(BillingRequestTemplateListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestTemplateListRequest();

            return new TaskEnumerable<IReadOnlyList<BillingRequestTemplate>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.BillingRequestTemplates, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Fetches a Billing Request Template
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRT".</param> 
        /// <param name="request">An optional `BillingRequestTemplateGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request template resource</returns>
        public Task<BillingRequestTemplateResponse> GetAsync(string identity, BillingRequestTemplateGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestTemplateGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestTemplateResponse>("GET", "/billing_request_templates/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">An optional `BillingRequestTemplateCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request template resource</returns>
        public Task<BillingRequestTemplateResponse> CreateAsync(BillingRequestTemplateCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestTemplateCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BillingRequestTemplateResponse>("POST", "/billing_request_templates", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "billing_request_templates", customiseRequestMessage);
        }

        /// <summary>
        /// Updates a Billing Request Template, which will affect all future
        /// Billing Requests created by this template.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRT".</param> 
        /// <param name="request">An optional `BillingRequestTemplateUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request template resource</returns>
        public Task<BillingRequestTemplateResponse> UpdateAsync(string identity, BillingRequestTemplateUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestTemplateUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestTemplateResponse>("PUT", "/billing_request_templates/:identity", urlParams, request, null, "billing_request_templates", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// Billing Request Templates.
    /// </summary>
    public class BillingRequestTemplateListRequest
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
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// (Optional) A scheme used for Open Banking payments. Currently
        /// `faster_payments` is supported in the UK (GBP) and
        /// `sepa_credit_transfer` and `sepa_instant_credit_transfer` are
        /// supported in supported Eurozone countries (EUR). For Eurozone
        /// countries, `sepa_credit_transfer` is used as the default. Please be
        /// aware that `sepa_instant_credit_transfer` may incur an additional
        /// fee for your customer.
        /// </summary>
        [JsonProperty("payment_request_scheme")]
        public string PaymentRequestScheme { get; set; }
    }

        
    /// <summary>
    /// Fetches a Billing Request Template
    /// </summary>
    public class BillingRequestTemplateGetRequest
    {
    }

        
    /// <summary>
    /// 
    /// </summary>
    public class BillingRequestTemplateCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestTemplateLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a BillingRequestTemplate.
        /// </summary>
        public class BillingRequestTemplateLinks
        {
                
                /// <summary>
                            /// ID of the associated [creditor](#core-endpoints-creditors). Only
            /// required if your account manages multiple creditors.
                /// </summary>
                [JsonProperty("creditor")]
                public string Creditor { get; set; }
        }

        /// <summary>
        /// Constraints that will apply to the mandate_request. (Optional)
        /// Specifically required for PayTo and VRP.
        /// </summary>
        [JsonProperty("mandate_request_constraints")]
        public BillingRequestTemplateMandateRequestConstraints MandateRequestConstraints { get; set; }
        /// <summary>
        /// Constraints that will apply to the mandate_request. (Optional)
        /// Specifically required for PayTo and VRP.
        /// </summary>
        public class BillingRequestTemplateMandateRequestConstraints
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
                public BillingRequestTemplatePeriodicLimits[] PeriodicLimits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public class BillingRequestTemplatePeriodicLimits
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
                public BillingRequestTemplateAlignment? Alignment { get; set; }
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
        public enum BillingRequestTemplateAlignment
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
                public BillingRequestTemplatePeriod? Period { get; set; }
        /// <summary>
        /// The repeating period for this mandate. Defaults to flexible for
        /// PayTo if not specified.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestTemplatePeriod
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
        [JsonProperty("mandate_request_currency")]
        public string MandateRequestCurrency { get; set; }

        /// <summary>
        /// A human-readable description of the payment and/or mandate. This
        /// will be displayed to the payer when authorising the billing request.
        /// 
        /// </summary>
        [JsonProperty("mandate_request_description")]
        public string MandateRequestDescription { get; set; }

        /// <summary>
        /// Key-value store of custom data that will be applied to the mandate
        /// created when this request is fulfilled. Up to 3 keys are permitted,
        /// with key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("mandate_request_metadata")]
        public IDictionary<String, String> MandateRequestMetadata { get; set; }

        /// <summary>
        /// A bank payment scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "faster_payments", "pad", "pay_to"
        /// and "sepa_core" are supported. Optional for mandate only requests -
        /// if left blank, the payer will be able to select the currency/scheme
        /// to pay with from a list of your available schemes.
        /// </summary>
        [JsonProperty("mandate_request_scheme")]
        public string MandateRequestScheme { get; set; }

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
        [JsonProperty("mandate_request_verify")]
        public BillingRequestTemplateMandateRequestVerify? MandateRequestVerify { get; set; }
            
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
        public enum BillingRequestTemplateMandateRequestVerify
        {
    
            /// <summary>`mandate_request_verify` with a value of "minimum"</summary>
            [EnumMember(Value = "minimum")]
            Minimum,
            /// <summary>`mandate_request_verify` with a value of "recommended"</summary>
            [EnumMember(Value = "recommended")]
            Recommended,
            /// <summary>`mandate_request_verify` with a value of "when_available"</summary>
            [EnumMember(Value = "when_available")]
            WhenAvailable,
            /// <summary>`mandate_request_verify` with a value of "always"</summary>
            [EnumMember(Value = "always")]
            Always,
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Name for the template. Provides a friendly human name for the
        /// template, as it is shown in the dashboard. Must not exceed 255
        /// characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Amount in full.
        /// </summary>
        [JsonProperty("payment_request_amount")]
        public string PaymentRequestAmount { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. `GBP` and `EUR` supported; `GBP` with your customers
        /// in the UK and for `EUR` with your customers in supported Eurozone
        /// countries only.
        /// </summary>
        [JsonProperty("payment_request_currency")]
        public string PaymentRequestCurrency { get; set; }

        /// <summary>
        /// A human-readable description of the payment and/or mandate. This
        /// will be displayed to the payer when authorising the billing request.
        /// 
        /// </summary>
        [JsonProperty("payment_request_description")]
        public string PaymentRequestDescription { get; set; }

        /// <summary>
        /// Key-value store of custom data that will be applied to the payment
        /// created when this request is fulfilled. Up to 3 keys are permitted,
        /// with key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("payment_request_metadata")]
        public IDictionary<String, String> PaymentRequestMetadata { get; set; }

        /// <summary>
        /// (Optional) A scheme used for Open Banking payments. Currently
        /// `faster_payments` is supported in the UK (GBP) and
        /// `sepa_credit_transfer` and `sepa_instant_credit_transfer` are
        /// supported in supported Eurozone countries (EUR). For Eurozone
        /// countries, `sepa_credit_transfer` is used as the default. Please be
        /// aware that `sepa_instant_credit_transfer` may incur an additional
        /// fee for your customer.
        /// </summary>
        [JsonProperty("payment_request_scheme")]
        public string PaymentRequestScheme { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after completing the request
        /// flow.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    /// <summary>
    /// Updates a Billing Request Template, which will affect all future Billing
    /// Requests created by this template.
    /// </summary>
    public class BillingRequestTemplateUpdateRequest
    {

        /// <summary>
        /// Constraints that will apply to the mandate_request. (Optional)
        /// Specifically required for PayTo and VRP.
        /// </summary>
        [JsonProperty("mandate_request_constraints")]
        public BillingRequestTemplateMandateRequestConstraints MandateRequestConstraints { get; set; }
        /// <summary>
        /// Constraints that will apply to the mandate_request. (Optional)
        /// Specifically required for PayTo and VRP.
        /// </summary>
        public class BillingRequestTemplateMandateRequestConstraints
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
                public BillingRequestTemplatePeriodicLimits[] PeriodicLimits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public class BillingRequestTemplatePeriodicLimits
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
                public BillingRequestTemplateAlignment? Alignment { get; set; }
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
        public enum BillingRequestTemplateAlignment
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
                public BillingRequestTemplatePeriod? Period { get; set; }
        /// <summary>
        /// The repeating period for this mandate. Defaults to flexible for
        /// PayTo if not specified.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestTemplatePeriod
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
        [JsonProperty("mandate_request_currency")]
        public string MandateRequestCurrency { get; set; }

        /// <summary>
        /// A human-readable description of the payment and/or mandate. This
        /// will be displayed to the payer when authorising the billing request.
        /// 
        /// </summary>
        [JsonProperty("mandate_request_description")]
        public string MandateRequestDescription { get; set; }

        /// <summary>
        /// Key-value store of custom data that will be applied to the mandate
        /// created when this request is fulfilled. Up to 3 keys are permitted,
        /// with key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("mandate_request_metadata")]
        public IDictionary<String, String> MandateRequestMetadata { get; set; }

        /// <summary>
        /// A bank payment scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "faster_payments", "pad", "pay_to"
        /// and "sepa_core" are supported. Optional for mandate only requests -
        /// if left blank, the payer will be able to select the currency/scheme
        /// to pay with from a list of your available schemes.
        /// </summary>
        [JsonProperty("mandate_request_scheme")]
        public string MandateRequestScheme { get; set; }

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
        [JsonProperty("mandate_request_verify")]
        public BillingRequestTemplateMandateRequestVerify? MandateRequestVerify { get; set; }
            
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
        public enum BillingRequestTemplateMandateRequestVerify
        {
    
            /// <summary>`mandate_request_verify` with a value of "minimum"</summary>
            [EnumMember(Value = "minimum")]
            Minimum,
            /// <summary>`mandate_request_verify` with a value of "recommended"</summary>
            [EnumMember(Value = "recommended")]
            Recommended,
            /// <summary>`mandate_request_verify` with a value of "when_available"</summary>
            [EnumMember(Value = "when_available")]
            WhenAvailable,
            /// <summary>`mandate_request_verify` with a value of "always"</summary>
            [EnumMember(Value = "always")]
            Always,
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Name for the template. Provides a friendly human name for the
        /// template, as it is shown in the dashboard. Must not exceed 255
        /// characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Amount in full.
        /// </summary>
        [JsonProperty("payment_request_amount")]
        public string PaymentRequestAmount { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. `GBP` and `EUR` supported; `GBP` with your customers
        /// in the UK and for `EUR` with your customers in supported Eurozone
        /// countries only.
        /// </summary>
        [JsonProperty("payment_request_currency")]
        public string PaymentRequestCurrency { get; set; }

        /// <summary>
        /// A human-readable description of the payment and/or mandate. This
        /// will be displayed to the payer when authorising the billing request.
        /// 
        /// </summary>
        [JsonProperty("payment_request_description")]
        public string PaymentRequestDescription { get; set; }

        /// <summary>
        /// Key-value store of custom data that will be applied to the payment
        /// created when this request is fulfilled. Up to 3 keys are permitted,
        /// with key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("payment_request_metadata")]
        public IDictionary<String, String> PaymentRequestMetadata { get; set; }

        /// <summary>
        /// (Optional) A scheme used for Open Banking payments. Currently
        /// `faster_payments` is supported in the UK (GBP) and
        /// `sepa_credit_transfer` and `sepa_instant_credit_transfer` are
        /// supported in supported Eurozone countries (EUR). For Eurozone
        /// countries, `sepa_credit_transfer` is used as the default. Please be
        /// aware that `sepa_instant_credit_transfer` may incur an additional
        /// fee for your customer.
        /// </summary>
        [JsonProperty("payment_request_scheme")]
        public string PaymentRequestScheme { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after completing the request
        /// flow.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single billing request template.
    /// </summary>
    public class BillingRequestTemplateResponse : ApiResponse
    {
        /// <summary>
        /// The billing request template from the response.
        /// </summary>
        [JsonProperty("billing_request_templates")]
        public BillingRequestTemplate BillingRequestTemplate { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of billing request templates.
    /// </summary>
    public class BillingRequestTemplateListResponse : ApiResponse
    {
        /// <summary>
        /// The list of billing request templates from the response.
        /// </summary>
        [JsonProperty("billing_request_templates")]
        public IReadOnlyList<BillingRequestTemplate> BillingRequestTemplates { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }}
}
