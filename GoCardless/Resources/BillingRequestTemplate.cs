using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a billing request template resource.
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
    public class BillingRequestTemplate
    {
        /// <summary>
        /// Permanent URL that customers can visit to allow them to complete a
        /// flow based on this template, before being returned to the
        /// `redirect_uri`.
        /// </summary>
        [JsonProperty("authorisation_url")]
        public string AuthorisationUrl { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BRT".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Constraints that will apply to the mandate_request. (Optional)
        /// Specifically required for PayTo and VRP.
        /// </summary>
        [JsonProperty("mandate_request_constraints")]
        public BillingRequestTemplateMandateRequestConstraints MandateRequestConstraints { get; set; }

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
        public IDictionary<string, string> MandateRequestMetadata { get; set; }

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
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

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
        public IDictionary<string, string> PaymentRequestMetadata { get; set; }

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
        /// Dynamic [timestamp](#api-usage-dates-and-times) recording when this
        /// resource was last updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Represents a billing request template mandate request constraint resource.
    ///
    /// Constraints that will apply to the mandate_request. (Optional)
    /// Specifically required for PayTo and VRP.
    /// </summary>
    public class BillingRequestTemplateMandateRequestConstraints
    {
        /// <summary>
        /// The latest date at which payments can be taken, must occur after
        /// start_date if present
        ///
        /// This is an optional field and if it is not supplied the agreement
        /// will be considered open and
        /// will not have an end date. Keep in mind the end date must take into
        /// account how long it will
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
        /// A constraint where you can specify info (free text string) about how
        /// payments are calculated. _Note:_ This is only supported for ACH and
        /// PAD schemes.
        ///
        /// </summary>
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// List of periodic limits and constraints which apply to them
        /// </summary>
        [JsonProperty("periodic_limits")]
        public List<BillingRequestTemplateMandateRequestConstraintPeriodicLimit> PeriodicLimits { get; set; }

        /// <summary>
        /// The date from which payments can be taken.
        ///
        /// This is an optional field and if it is not supplied the start date
        /// will be set to the day
        /// authorisation happens.
        ///
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }
    }

    /// <summary>
    /// List of periodic limits and constraints which apply to them
    /// </summary>
    public class BillingRequestTemplateMandateRequestConstraintPeriodicLimit
    {
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
        [JsonProperty("alignment")]
        public BillingRequestTemplateMandateRequestConstraintPeriodicLimitAlignment? Alignment { get; set; }

        /// <summary>
        /// (Optional) The maximum number of payments that can be collected in
        /// this periodic limit.
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
        public BillingRequestTemplateMandateRequestConstraintPeriodicLimitPeriod? Period { get; set; }
    }

    /// <summary>
    /// The alignment of the period.
    ///
    /// `calendar` - this will finish on the end of the current period. For example this will expire
    /// on the Monday for the current week or the January for the next year.
    ///
    /// `creation_date` - this will finish on the next instance of the current period. For example
    /// Monthly it will expire on the same day of the next month, or yearly the same day of the next
    /// year.
    ///
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestTemplateMandateRequestConstraintPeriodicLimitAlignment
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`alignment` with a value of "calendar"</summary>
        [EnumMember(Value = "calendar")]
        Calendar,

        /// <summary>`alignment` with a value of "creation_date"</summary>
        [EnumMember(Value = "creation_date")]
        CreationDate,
    }

    /// <summary>
    /// The repeating period for this mandate. Defaults to flexible for PayTo if not specified.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestTemplateMandateRequestConstraintPeriodicLimitPeriod
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

    /// <summary>
    /// Verification preference for the mandate. One of:
    /// <ul>
    ///   <li>`minimum`: only verify if absolutely required, such as when part of scheme rules</li>
    ///   <li>`recommended`: in addition to `minimum`, use the GoCardless payment intelligence
    /// solution to decide if a payer should be verified</li>
    ///   <li>`when_available`: if verification mechanisms are available, use them</li>
    ///   <li>`always`: as `when_available`, but fail to create the Billing Request if a mechanism
    /// isn't available</li>
    /// </ul>
    ///
    /// By default, all Billing Requests use the `recommended` verification preference. It uses
    /// GoCardless payment intelligence solution to determine if a payer is fraudulent or not. The
    /// verification mechanism is based on the response and the payer may be asked to verify
    /// themselves. If the feature is not available, `recommended` behaves like `minimum`.
    ///
    /// If you never wish to take advantage of our reduced risk products and Verified Mandates as
    /// they are released in new schemes, please use the `minimum` verification preference.
    ///
    /// See [Billing Requests: Creating Verified
    /// Mandates](https://developer.gocardless.com/getting-started/billing-requests/verified-mandates/)
    /// for more information.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestTemplateMandateRequestVerify
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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
}
