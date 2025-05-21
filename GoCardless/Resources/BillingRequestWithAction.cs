using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a billing request with action resource.
    ///
    ///  Billing Requests help create resources that require input or action
    /// from a customer. An example of required input might be additional
    /// customer billing details, while an action would be asking a customer to
    /// authorise a payment using their mobile banking app.
    /// 
    /// See [Billing Requests:
    /// Overview](https://developer.gocardless.com/getting-started/billing-requests/overview/)
    /// for how-to's, explanations and tutorials.
    /// </summary>
    public class BillingRequestWithAction
    {
        /// <summary>
        /// Bank Authorisations can be used to authorise Billing Requests.
        /// Authorisations
        /// are created against a specific bank, usually the bank that provides
        /// the payer's
        /// account.
        /// 
        /// Creation of Bank Authorisations is only permitted from GoCardless
        /// hosted UIs
        /// (see Billing Request Flows) to ensure we meet regulatory
        /// requirements for
        /// checkout flows.
        /// </summary>
        [JsonProperty("bank_authorisations")]
        public BillingRequestWithActionBankAuthorisations BankAuthorisations { get; set; }

        /// <summary>
        ///  Billing Requests help create resources that require input or action
        /// from a customer. An example of required input might be additional
        /// customer billing details, while an action would be asking a customer
        /// to authorise a payment using their mobile banking app.
        /// 
        /// See [Billing Requests:
        /// Overview](https://developer.gocardless.com/getting-started/billing-requests/overview/)
        /// for how-to's, explanations and tutorials. <p
        /// class="notice"><strong>Important</strong>: All properties associated
        /// with `subscription_request` and `instalment_schedule_request` are
        /// only supported for ACH and PAD schemes.</p>
        /// </summary>
        [JsonProperty("billing_requests")]
        public BillingRequestWithActionBillingRequests BillingRequests { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action bank authorisation resource.
    ///
    /// Bank Authorisations can be used to authorise Billing Requests.
    /// Authorisations
    /// are created against a specific bank, usually the bank that provides the
    /// payer's
    /// account.
    /// 
    /// Creation of Bank Authorisations is only permitted from GoCardless hosted
    /// UIs
    /// (see Billing Request Flows) to ensure we meet regulatory requirements
    /// for
    /// checkout flows.
    /// </summary>
    public class BillingRequestWithActionBankAuthorisations
    {
        /// <summary>
        /// Type of authorisation, can be either 'mandate' or 'payment'.
        /// </summary>
        [JsonProperty("authorisation_type")]
        public string AuthorisationType { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when the
        /// user has been authorised.
        /// </summary>
        [JsonProperty("authorised_at")]
        public string AuthorisedAt { get; set; }

        /// <summary>
        /// Timestamp when the flow was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when the url will expire. Each authorisation url currently
        /// lasts for 15 minutes, but this can vary by bank.
        /// </summary>
        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BAU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when the
        /// authorisation URL has been visited.
        /// </summary>
        [JsonProperty("last_visited_at")]
        public string LastVisitedAt { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestWithActionBankAuthorisations.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestWithActionBankAuthorisationsLinks Links { get; set; }

        /// <summary>
        /// URL to a QR code PNG image of the bank authorisation url.
        /// This QR code can be used as an alternative to providing the `url` to
        /// the payer to allow them to authorise with their mobile devices.
        /// </summary>
        [JsonProperty("qr_code_url")]
        public string QrCodeUrl { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after authorising the
        /// payment.
        /// 
        /// On completion of bank authorisation, the query parameter of either
        /// `outcome=success` or `outcome=failure` will be
        /// appended to the `redirect_uri` to indicate the result of the bank
        /// authorisation. If the bank authorisation is
        /// expired, the query parameter `outcome=timeout` will be appended to
        /// the `redirect_uri`, in which case you should
        /// prompt the user to try the bank authorisation step again.
        /// 
        /// Please note: bank authorisations can still fail despite an
        /// `outcome=success` on the `redirect_uri`. It is therefore recommended
        /// to wait for the relevant bank authorisation event, such as
        /// [`BANK_AUTHORISATION_AUTHORISED`](#billing-request-bankauthorisationauthorised),
        /// [`BANK_AUTHORISATION_DENIED`](#billing-request-bankauthorisationdenied),
        /// or
        /// [`BANK_AUTHORISATION_FAILED`](#billing-request-bankauthorisationfailed)
        /// in order to show the correct outcome to the user.
        /// 
        /// The BillingRequestFlow ID will also be appended to the
        /// `redirect_uri` as query parameter `id=BRF123`.
        /// 
        /// Defaults to `https://pay.gocardless.com/billing/static/thankyou`.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// URL for an oauth flow that will allow the user to authorise the
        /// payment
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    
    /// <summary>
    /// Type of authorisation, can be either 'mandate' or 'payment'.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBankAuthorisationsAuthorisationType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`authorisation_type` with a value of "mandate"</summary>
        [EnumMember(Value = "mandate")]
        Mandate,
        /// <summary>`authorisation_type` with a value of "payment"</summary>
        [EnumMember(Value = "payment")]
        Payment,
    }

    /// <summary>
    /// Resources linked to this BillingRequestWithActionBankAuthorisations
    /// </summary>
    public class BillingRequestWithActionBankAuthorisationsLinks
    {
        /// <summary>
        /// ID of the [billing request](#billing-requests-billing-requests)
        /// against which this authorisation was created.
        /// </summary>
        [JsonProperty("billing_request")]
        public string BillingRequest { get; set; }

        /// <summary>
        /// ID of the [institution](#billing-requests-institutions) against
        /// which this authorisation was created.
        /// </summary>
        [JsonProperty("institution")]
        public string Institution { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing request resource.
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
    public class BillingRequestWithActionBillingRequests
    {
        /// <summary>
        /// List of actions that can be performed before this billing request
        /// can be fulfilled.
        /// </summary>
        [JsonProperty("actions")]
        public List<BillingRequestWithActionBillingRequestAction> Actions { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

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

        /// <summary>
        /// True if the billing request was completed with direct debit.
        /// </summary>
        [JsonProperty("fallback_occurred")]
        public bool? FallbackOccurred { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BRQ".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Request for an instalment schedule. Has to contain either
        /// `instalments_with_schedule` object or an array of
        /// `instalments_with_dates` objects
        /// </summary>
        [JsonProperty("instalment_schedule_request")]
        public BillingRequestWithActionBillingRequestsInstalmentScheduleRequest InstalmentScheduleRequest { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestWithActionBillingRequests.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestWithActionBillingRequestsLinks Links { get; set; }

        /// <summary>
        /// Request for a mandate
        /// </summary>
        [JsonProperty("mandate_request")]
        public BillingRequestWithActionBillingRequestsMandateRequest MandateRequest { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Request for a one-off strongly authorised payment
        /// </summary>
        [JsonProperty("payment_request")]
        public BillingRequestWithActionBillingRequestsPaymentRequest PaymentRequest { get; set; }

        /// <summary>
        /// Specifies the high-level purpose of a mandate and/or payment using a
        /// set of pre-defined categories. Required for the PayTo scheme,
        /// optional for all others. Currently `mortgage`, `utility`, `loan`,
        /// `dependant_support`, `gambling`, `retail`, `salary`, `personal`,
        /// `government`, `pension`, `tax` and `other` are supported.
        /// </summary>
        [JsonProperty("purpose_code")]
        public BillingRequestWithActionBillingRequestsPurposeCode? PurposeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("resources")]
        public BillingRequestWithActionBillingRequestsResources Resources { get; set; }

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
        /// Request for a subscription
        /// </summary>
        [JsonProperty("subscription_request")]
        public BillingRequestWithActionBillingRequestsSubscriptionRequest SubscriptionRequest { get; set; }
    }
    
    /// <summary>
    /// List of actions that can be performed before this billing request can be
    /// fulfilled.
    /// </summary>
    public class BillingRequestWithActionBillingRequestAction
    {
        /// <summary>
        /// List of currencies the current mandate supports
        /// </summary>
        [JsonProperty("available_currencies")]
        public List<string> AvailableCurrencies { get; set; }

        /// <summary>
        /// Describes the behaviour of bank authorisations, for the
        /// bank_authorisation action
        /// </summary>
        [JsonProperty("bank_authorisation")]
        public BillingRequestWithActionBillingRequestActionBankAuthorisation BankAuthorisation { get; set; }

        /// <summary>
        /// Additional parameters to help complete the collect_customer_details
        /// action
        /// </summary>
        [JsonProperty("collect_customer_details")]
        public BillingRequestWithActionBillingRequestActionCollectCustomerDetails CollectCustomerDetails { get; set; }

        /// <summary>
        /// Which other action types this action can complete.
        /// </summary>
        [JsonProperty("completes_actions")]
        public List<string> CompletesActions { get; set; }

        /// <summary>
        /// Describes whether we inferred the institution from the provided bank
        /// account details. One of:
        /// - `not_needed`: we won't attempt to infer the institution as it is
        /// not needed. Either because it was manually selected or the billing
        /// request does not support this feature
        /// - `pending`: we are waiting on the bank details in order to infer
        /// the institution
        /// - `failed`: we weren't able to infer the institution
        /// - `success`: we inferred the institution and added it to the
        /// resources of a Billing Request
        /// 
        /// </summary>
        [JsonProperty("institution_guess_status")]
        public string InstitutionGuessStatus { get; set; }

        /// <summary>
        /// Informs you whether the action is required to fulfil the billing
        /// request or not.
        /// </summary>
        [JsonProperty("required")]
        public bool? Required { get; set; }

        /// <summary>
        /// Requires completing these actions before this action can be
        /// completed.
        /// </summary>
        [JsonProperty("requires_actions")]
        public List<string> RequiresActions { get; set; }

        /// <summary>
        /// Status of the action
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Unique identifier for the action.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing request action available currency resource.
    ///
    /// List of currencies the current mandate supports
    /// </summary>
    public class BillingRequestWithActionBillingRequestActionAvailableCurrencies
    {
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing request action bank authorisation resource.
    ///
    /// Describes the behaviour of bank authorisations, for the
    /// bank_authorisation action
    /// </summary>
    public class BillingRequestWithActionBillingRequestActionBankAuthorisation
    {
        /// <summary>
        /// Which authorisation adapter will be used to power these
        /// authorisations (GoCardless internal use only)
        /// </summary>
        [JsonProperty("adapter")]
        public string Adapter { get; set; }

        /// <summary>
        /// What type of bank authorisations are supported on this billing
        /// request
        /// </summary>
        [JsonProperty("authorisation_type")]
        public string AuthorisationType { get; set; }
    }
    
    /// <summary>
    /// Which authorisation adapter will be used to power these authorisations (GoCardless internal
    /// use only)
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestActionBankAuthorisationAdapter {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`adapter` with a value of "open_banking_gateway_pis"</summary>
        [EnumMember(Value = "open_banking_gateway_pis")]
        OpenBankingGatewayPis,
        /// <summary>`adapter` with a value of "plaid_ais"</summary>
        [EnumMember(Value = "plaid_ais")]
        PlaidAis,
        /// <summary>`adapter` with a value of "open_banking_gateway_ais"</summary>
        [EnumMember(Value = "open_banking_gateway_ais")]
        OpenBankingGatewayAis,
        /// <summary>`adapter` with a value of "bankid_ais"</summary>
        [EnumMember(Value = "bankid_ais")]
        BankidAis,
        /// <summary>`adapter` with a value of "bank_pay_recurring"</summary>
        [EnumMember(Value = "bank_pay_recurring")]
        BankPayRecurring,
    }

    /// <summary>
    /// What type of bank authorisations are supported on this billing request
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestActionBankAuthorisationAuthorisationType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`authorisation_type` with a value of "payment"</summary>
        [EnumMember(Value = "payment")]
        Payment,
        /// <summary>`authorisation_type` with a value of "mandate"</summary>
        [EnumMember(Value = "mandate")]
        Mandate,
    }

    /// <summary>
    /// Represents a billing request with action billing request action collect customer detail resource.
    ///
    /// Additional parameters to help complete the collect_customer_details
    /// action
    /// </summary>
    public class BillingRequestWithActionBillingRequestActionCollectCustomerDetails
    {
        /// <summary>
        /// Default customer country code, as determined by scheme and payer
        /// location
        /// </summary>
        [JsonProperty("default_country_code")]
        public string DefaultCountryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("incomplete_fields")]
        public BillingRequestWithActionBillingRequestActionCollectCustomerDetailsIncompleteFields IncompleteFields { get; set; }
    }
    
    public class BillingRequestWithActionBillingRequestActionCollectCustomerDetailsIncompleteFields
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("customer")]
        public List<string> Customer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("customer_billing_detail")]
        public List<string> CustomerBillingDetail { get; set; }
    }
    
    /// <summary>
    /// Describes whether we inferred the institution from the provided bank account details. One
    /// of:
    /// - `not_needed`: we won't attempt to infer the institution as it is not needed. Either
    /// because it was manually selected or the billing request does not support this feature
    /// - `pending`: we are waiting on the bank details in order to infer the institution
    /// - `failed`: we weren't able to infer the institution
    /// - `success`: we inferred the institution and added it to the resources of a Billing Request
    /// 
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestActionInstitutionGuessStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`institution_guess_status` with a value of "not_needed"</summary>
        [EnumMember(Value = "not_needed")]
        NotNeeded,
        /// <summary>`institution_guess_status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,
        /// <summary>`institution_guess_status` with a value of "failed"</summary>
        [EnumMember(Value = "failed")]
        Failed,
        /// <summary>`institution_guess_status` with a value of "success"</summary>
        [EnumMember(Value = "success")]
        Success,
    }

    /// <summary>
    /// Status of the action
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestActionStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,
        /// <summary>`status` with a value of "completed"</summary>
        [EnumMember(Value = "completed")]
        Completed,
    }

    /// <summary>
    /// Unique identifier for the action.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestActionType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`type` with a value of "choose_currency"</summary>
        [EnumMember(Value = "choose_currency")]
        ChooseCurrency,
        /// <summary>`type` with a value of "collect_amount"</summary>
        [EnumMember(Value = "collect_amount")]
        CollectAmount,
        /// <summary>`type` with a value of "collect_customer_details"</summary>
        [EnumMember(Value = "collect_customer_details")]
        CollectCustomerDetails,
        /// <summary>`type` with a value of "collect_bank_account"</summary>
        [EnumMember(Value = "collect_bank_account")]
        CollectBankAccount,
        /// <summary>`type` with a value of "bank_authorisation"</summary>
        [EnumMember(Value = "bank_authorisation")]
        BankAuthorisation,
        /// <summary>`type` with a value of "confirm_payer_details"</summary>
        [EnumMember(Value = "confirm_payer_details")]
        ConfirmPayerDetails,
        /// <summary>`type` with a value of "select_institution"</summary>
        [EnumMember(Value = "select_institution")]
        SelectInstitution,
    }

    /// <summary>
    /// Represents a billing request with action billing requests instalment schedule request resource.
    ///
    /// Request for an instalment schedule. Has to contain either
    /// `instalments_with_schedule` object or an array of
    /// `instalments_with_dates` objects
    /// </summary>
    public class BillingRequestWithActionBillingRequestsInstalmentScheduleRequest
    {
        /// <summary>
        /// The amount to be deducted from each payment as an app fee, to be
        /// paid to the partner integration which created the subscription, in
        /// the lowest denomination for the currency (e.g. pence in GBP, cents
        /// in EUR).
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
        /// An explicit array of instalment payments, each specifying at least
        /// an `amount` and `charge_date`. See [create (with
        /// dates)](#instalment-schedules-create-with-dates)
        /// </summary>
        [JsonProperty("instalments_with_dates")]
        public List<BillingRequestWithActionBillingRequestsInstalmentScheduleRequestInstalmentsWithDate> InstalmentsWithDates { get; set; }

        /// <summary>
        /// Frequency of the payments you want to create, together with an array
        /// of payment
        /// amounts to be collected, with a specified start date for the first
        /// payment.
        /// See [create (with
        /// schedule)](#instalment-schedules-create-with-schedule)
        /// 
        /// </summary>
        [JsonProperty("instalments_with_schedule")]
        public BillingRequestWithActionBillingRequestsInstalmentScheduleRequestInstalmentsWithSchedule InstalmentsWithSchedule { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestWithActionBillingRequestsInstalmentScheduleRequest.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestWithActionBillingRequestsInstalmentScheduleRequestLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
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
        /// An optional payment reference. This will be set as the reference on
        /// each payment
        /// created and will appear on your customer's bank statement. See the
        /// documentation for
        /// the [create payment endpoint](#payments-create-a-payment) for more
        /// details.
        /// <br />
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// On failure, automatically retry payments using [intelligent
        /// retries](#success-intelligent-retries). Default is `false`. <p
        /// class="notice"><strong>Important</strong>: To be able to use
        /// intelligent retries, Success+ needs to be enabled in [GoCardless
        /// dashboard](https://manage.gocardless.com/success-plus). </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        /// The total amount of the instalment schedule, defined as the sum of
        /// all individual
        /// payments, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in
        /// EUR). If the requested payment amounts do not sum up correctly, a
        /// validation error
        /// will be returned.
        /// </summary>
        [JsonProperty("total_amount")]
        public int? TotalAmount { get; set; }
    }
    
    /// <summary>
    /// An explicit array of instalment payments, each specifying at least an
    /// `amount` and `charge_date`. See [create (with
    /// dates)](#instalment-schedules-create-with-dates)
    /// </summary>
    public class BillingRequestWithActionBillingRequestsInstalmentScheduleRequestInstalmentsWithDate
    {
        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// A future date on which the payment should be collected. If the date
        /// is before the next_possible_charge_date on the
        /// [mandate](#core-endpoints-mandates), it will be automatically rolled
        /// forwards to that date.
        /// </summary>
        [JsonProperty("charge_date")]
        public string ChargeDate { get; set; }

        /// <summary>
        /// A human-readable description of the payment. This will be included
        /// in the notification email GoCardless sends to your customer if your
        /// organisation does not send its own notifications (see [compliance
        /// requirements](#appendix-compliance-requirements)).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing requests instalment schedule request instalments with schedule resource.
    ///
    /// Frequency of the payments you want to create, together with an array of
    /// payment
    /// amounts to be collected, with a specified start date for the first
    /// payment.
    /// See [create (with schedule)](#instalment-schedules-create-with-schedule)
    /// 
    /// </summary>
    public class BillingRequestWithActionBillingRequestsInstalmentScheduleRequestInstalmentsWithSchedule
    {
        /// <summary>
        /// List of amounts of each instalment, in the lowest denomination for
        /// the
        /// currency (e.g. cents in USD).
        /// 
        /// </summary>
        [JsonProperty("amounts")]
        public List<int?> Amounts { get; set; }

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
        public BillingRequestWithActionBillingRequestsInstalmentScheduleRequestInstalmentsWithScheduleIntervalUnit? IntervalUnit { get; set; }

        /// <summary>
        /// The date on which the first payment should be charged. Must be on or
        /// after the [mandate](#core-endpoints-mandates)'s
        /// `next_possible_charge_date`. When left blank and `month` or
        /// `day_of_month` are provided, this will be set to the date of the
        /// first payment. If created without `month` or `day_of_month` this
        /// will be set as the mandate's `next_possible_charge_date`
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }
    }
    
    /// <summary>
    /// The unit of time between customer charge dates. One of `weekly`, `monthly` or `yearly`.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsInstalmentScheduleRequestInstalmentsWithScheduleIntervalUnit {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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
    /// Resources linked to this BillingRequestWithActionBillingRequestsInstalmentScheduleRequest
    /// </summary>
    public class BillingRequestWithActionBillingRequestsInstalmentScheduleRequestLinks
    {
        /// <summary>
        /// (Optional) ID of the
        /// [instalment_schedule](#core-endpoints-instalment-schedules) that was
        /// created from this instalment schedule request.
        /// 
        /// </summary>
        [JsonProperty("instalment_schedule")]
        public string InstalmentSchedule { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this BillingRequestWithActionBillingRequests
    /// </summary>
    public class BillingRequestWithActionBillingRequestsLinks
    {
        /// <summary>
        /// (Optional) ID of the [bank
        /// authorisation](#billing-requests-bank-authorisations) that was used
        /// to verify this request.
        /// </summary>
        [JsonProperty("bank_authorisation")]
        public string BankAuthorisation { get; set; }

        /// <summary>
        /// ID of the associated [creditor](#core-endpoints-creditors).
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of the [customer](#core-endpoints-customers) that will be used
        /// for this request
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// (Optional) ID of the
        /// [customer_bank_account](#core-endpoints-customer-bank-accounts) that
        /// will be used for this request
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// ID of the customer billing detail that will be used for this request
        /// </summary>
        [JsonProperty("customer_billing_detail")]
        public string CustomerBillingDetail { get; set; }

        /// <summary>
        /// (Optional) ID of the associated instalment schedule request
        /// </summary>
        [JsonProperty("instalment_schedule_request")]
        public string InstalmentScheduleRequest { get; set; }

        /// <summary>
        /// (Optional) ID of the
        /// [instalment_schedule](#core-endpoints-instalment-schedules) that was
        /// created from this instalment schedule request.
        /// </summary>
        [JsonProperty("instalment_schedule_request_instalment_schedule")]
        public string InstalmentScheduleRequestInstalmentSchedule { get; set; }

        /// <summary>
        /// (Optional) ID of the associated mandate request
        /// </summary>
        [JsonProperty("mandate_request")]
        public string MandateRequest { get; set; }

        /// <summary>
        /// (Optional) ID of the [mandate](#core-endpoints-mandates) that was
        /// created from this mandate request. this mandate request.
        /// </summary>
        [JsonProperty("mandate_request_mandate")]
        public string MandateRequestMandate { get; set; }

        /// <summary>
        /// ID of the associated organisation.
        /// </summary>
        [JsonProperty("organisation")]
        public string Organisation { get; set; }

        /// <summary>
        /// (Optional) ID of the associated payment provider
        /// </summary>
        [JsonProperty("payment_provider")]
        public string PaymentProvider { get; set; }

        /// <summary>
        /// (Optional) ID of the associated payment request
        /// </summary>
        [JsonProperty("payment_request")]
        public string PaymentRequest { get; set; }

        /// <summary>
        /// (Optional) ID of the [payment](#core-endpoints-payments) that was
        /// created from this payment request.
        /// </summary>
        [JsonProperty("payment_request_payment")]
        public string PaymentRequestPayment { get; set; }

        /// <summary>
        /// (Optional) ID of the associated subscription request
        /// </summary>
        [JsonProperty("subscription_request")]
        public string SubscriptionRequest { get; set; }

        /// <summary>
        /// (Optional) ID of the [subscription](#core-endpoints-subscriptions)
        /// that was created from this subscription request.
        /// </summary>
        [JsonProperty("subscription_request_subscription")]
        public string SubscriptionRequestSubscription { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing requests mandate request resource.
    ///
    /// Request for a mandate
    /// </summary>
    public class BillingRequestWithActionBillingRequestsMandateRequest
    {
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
        [JsonProperty("authorisation_source")]
        public BillingRequestWithActionBillingRequestsMandateRequestAuthorisationSource? AuthorisationSource { get; set; }

        /// <summary>
        /// This attribute represents the authorisation type between the payer
        /// and merchant. It can be set to `one_off`,
        /// `recurring` or `standing` for ACH scheme. And `single`, `recurring`
        /// and `sporadic` for PAD scheme. _Note:_ This is only supported for
        /// ACH and PAD schemes.
        /// 
        /// </summary>
        [JsonProperty("consent_type")]
        public string ConsentType { get; set; }

        /// <summary>
        /// Constraints that will apply to the mandate_request. (Optional)
        /// Specifically for PayTo and VRP.
        /// </summary>
        [JsonProperty("constraints")]
        public BillingRequestWithActionBillingRequestsMandateRequestConstraints Constraints { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// A human-readable description of the payment and/or mandate. This
        /// will be displayed to the payer when authorising the billing request.
        /// 
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestWithActionBillingRequestsMandateRequest.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestWithActionBillingRequestsMandateRequestLinks Links { get; set; }

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

        /// <summary>
        /// A bank payment scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "faster_payments", "pad", "pay_to"
        /// and "sepa_core" are supported. Optional for mandate only requests -
        /// if left blank, the payer will be able to select the currency/scheme
        /// to pay with from a list of your available schemes.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// If true, this billing request would be used to set up a mandate
        /// solely for moving (or sweeping) money from one account owned by the
        /// payer to another account that the payer also owns. This is required
        /// for Faster Payments
        /// </summary>
        [JsonProperty("sweeping")]
        public bool? Sweeping { get; set; }

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
        [JsonProperty("verify")]
        public BillingRequestWithActionBillingRequestsMandateRequestVerify? Verify { get; set; }
    }
    
    /// <summary>
    /// This field is ACH specific, sometimes referred to as [SEC
    /// code](https://www.moderntreasury.com/learn/sec-codes).
    /// 
    /// This is the way that the payer gives authorisation to the merchant.
    ///   web: Authorisation is Internet Initiated or via Mobile Entry (maps to SEC code: WEB)
    ///   telephone: Authorisation is provided orally over telephone (maps to SEC code: TEL)
    ///   paper: Authorisation is provided in writing and signed, or similarly authenticated (maps
    /// to SEC code: PPD)
    /// 
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsMandateRequestAuthorisationSource {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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
    /// Represents a billing request with action billing requests mandate request constraint resource.
    ///
    /// Constraints that will apply to the mandate_request. (Optional)
    /// Specifically for PayTo and VRP.
    /// </summary>
    public class BillingRequestWithActionBillingRequestsMandateRequestConstraints
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
        /// Required for VRP.
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
        public List<BillingRequestWithActionBillingRequestsMandateRequestConstraintPeriodicLimit> PeriodicLimits { get; set; }

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
    public class BillingRequestWithActionBillingRequestsMandateRequestConstraintPeriodicLimit
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
        public BillingRequestWithActionBillingRequestsMandateRequestConstraintPeriodicLimitAlignment? Alignment { get; set; }

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
        /// The repeating period for this mandate
        /// </summary>
        [JsonProperty("period")]
        public BillingRequestWithActionBillingRequestsMandateRequestConstraintPeriodicLimitPeriod? Period { get; set; }
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
    public enum BillingRequestWithActionBillingRequestsMandateRequestConstraintPeriodicLimitAlignment {
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
    /// The repeating period for this mandate
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsMandateRequestConstraintPeriodicLimitPeriod {
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
    /// Resources linked to this BillingRequestWithActionBillingRequestsMandateRequest
    /// </summary>
    public class BillingRequestWithActionBillingRequestsMandateRequestLinks
    {
        /// <summary>
        /// (Optional) ID of the [mandate](#core-endpoints-mandates) that was
        /// created from this mandate request. this mandate request.
        /// 
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }
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
    public enum BillingRequestWithActionBillingRequestsMandateRequestVerify {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

    /// <summary>
    /// Represents a billing request with action billing requests payment request resource.
    ///
    /// Request for a one-off strongly authorised payment
    /// </summary>
    public class BillingRequestWithActionBillingRequestsPaymentRequest
    {
        /// <summary>
        /// Amount in minor unit (e.g. pence in GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount to be deducted from the payment as an app fee, to be paid
        /// to the partner integration which created the billing request, in the
        /// lowest denomination for the currency (e.g. pence in GBP, cents in
        /// EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. `GBP` and `EUR` supported; `GBP` with your customers
        /// in the UK and for `EUR` with your customers in supported Eurozone
        /// countries only.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// A human-readable description of the payment and/or mandate. This
        /// will be displayed to the payer when authorising the billing request.
        /// 
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

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
        [JsonProperty("funds_settlement")]
        public BillingRequestWithActionBillingRequestsPaymentRequestFundsSettlement? FundsSettlement { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestWithActionBillingRequestsPaymentRequest.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestWithActionBillingRequestsPaymentRequestLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// A custom payment reference defined by the merchant. It is only
        /// available for payments using the Direct Funds settlement model on
        /// the Faster Payments scheme.
        /// 
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// (Optional) A scheme used for Open Banking payments. Currently
        /// `faster_payments` is supported in the UK (GBP) and
        /// `sepa_credit_transfer` and `sepa_instant_credit_transfer` are
        /// supported in supported Eurozone countries (EUR). For Eurozone
        /// countries, `sepa_credit_transfer` is used as the default. Please be
        /// aware that `sepa_instant_credit_transfer` may incur an additional
        /// fee for your customer.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }
    }
    
    /// <summary>
    /// This field will decide how GoCardless handles settlement of funds from the customer.
    /// 
    /// - `managed` will be moved through GoCardless' account, batched, and payed out.
    /// - `direct` will be a direct transfer from the payer's account to the merchant where
    ///   invoicing will be handled separately.
    /// 
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsPaymentRequestFundsSettlement {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`funds_settlement` with a value of "managed"</summary>
        [EnumMember(Value = "managed")]
        Managed,
        /// <summary>`funds_settlement` with a value of "direct"</summary>
        [EnumMember(Value = "direct")]
        Direct,
    }

    /// <summary>
    /// Resources linked to this BillingRequestWithActionBillingRequestsPaymentRequest
    /// </summary>
    public class BillingRequestWithActionBillingRequestsPaymentRequestLinks
    {
        /// <summary>
        /// (Optional) ID of the [payment](#core-endpoints-payments) that was
        /// created from this payment request.
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }
    }
    
    /// <summary>
    /// Specifies the high-level purpose of a mandate and/or payment using a set of pre-defined
    /// categories. Required for the PayTo scheme, optional for all others. Currently `mortgage`,
    /// `utility`, `loan`, `dependant_support`, `gambling`, `retail`, `salary`, `personal`,
    /// `government`, `pension`, `tax` and `other` are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsPurposeCode {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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
    }

    public class BillingRequestWithActionBillingRequestsResources
    {
        /// <summary>
        /// Embedded customer
        /// </summary>
        [JsonProperty("customer")]
        public BillingRequestWithActionBillingRequestsResourcesCustomer Customer { get; set; }

        /// <summary>
        /// Embedded customer bank account, only if a bank account is linked
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public BillingRequestWithActionBillingRequestsResourcesCustomerBankAccount CustomerBankAccount { get; set; }

        /// <summary>
        /// Embedded customer billing detail
        /// </summary>
        [JsonProperty("customer_billing_detail")]
        public BillingRequestWithActionBillingRequestsResourcesCustomerBillingDetail CustomerBillingDetail { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing requests resources customer resource.
    ///
    /// Embedded customer
    /// </summary>
    public class BillingRequestWithActionBillingRequestsResourcesCustomer
    {
        /// <summary>
        /// Customer's company name. Required unless a `given_name` and
        /// `family_name` are provided. For Canadian customers, the use of a
        /// `company_name` value will mean that any mandate created from this
        /// customer will be considered to be a "Business PAD" (otherwise, any
        /// mandate will be considered to be a "Personal PAD").
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Customer's email address. Required in most cases, as this allows
        /// GoCardless to send notifications to this customer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Customer's surname. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Customer's first name. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// [ISO 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
        /// code. Used as the language for notification emails sent by
        /// GoCardless if your organisation does not send its own (see
        /// [compliance requirements](#appendix-compliance-requirements)).
        /// Currently only "en", "fr", "de", "pt", "es", "it", "nl", "da", "nb",
        /// "sl", "sv" are supported. If this is not provided, the language will
        /// be chosen based on the `country_code` (if supplied) or default to
        /// "en".
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// [ITU E.123](https://en.wikipedia.org/wiki/E.123) formatted phone
        /// number, including country code.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing requests resources customer bank account resource.
    ///
    /// Embedded customer bank account, only if a bank account is linked
    /// </summary>
    public class BillingRequestWithActionBillingRequestsResourcesCustomerBankAccount
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
        /// The last few digits of the account number. Currently 4 digits for
        /// NZD bank accounts and 2 digits for other currencies.
        /// </summary>
        [JsonProperty("account_number_ending")]
        public string AccountNumberEnding { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public BillingRequestWithActionBillingRequestsResourcesCustomerBankAccountAccountType? AccountType { get; set; }

        /// <summary>
        /// A token to uniquely refer to a set of bank account details. This
        /// feature is still in early access and is only available for certain
        /// organisations.
        /// </summary>
        [JsonProperty("bank_account_token")]
        public string BankAccountToken { get; set; }

        /// <summary>
        /// Name of bank, taken from the bank details.
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements).
        /// Defaults to the country code of the `iban` if supplied, otherwise is
        /// required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Boolean value showing whether the bank account is enabled or
        /// disabled.
        /// </summary>
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BA".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestWithActionBillingRequestsResourcesCustomerBankAccount.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestWithActionBillingRequestsResourcesCustomerBankAccountLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }
    
    /// <summary>
    /// Bank account type. Required for USD-denominated bank accounts. Must not be provided for bank
    /// accounts in other currencies. See [local details](#local-bank-details-united-states) for
    /// more information.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsResourcesCustomerBankAccountAccountType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`account_type` with a value of "savings"</summary>
        [EnumMember(Value = "savings")]
        Savings,
        /// <summary>`account_type` with a value of "checking"</summary>
        [EnumMember(Value = "checking")]
        Checking,
    }

    /// <summary>
    /// Resources linked to this BillingRequestWithActionBillingRequestsResourcesCustomerBankAccount
    /// </summary>
    public class BillingRequestWithActionBillingRequestsResourcesCustomerBankAccountLinks
    {
        /// <summary>
        /// ID of the [customer](#core-endpoints-customers) that owns this bank
        /// account.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request with action billing requests resources customer billing detail resource.
    ///
    /// Embedded customer billing detail
    /// </summary>
    public class BillingRequestWithActionBillingRequestsResourcesCustomerBillingDetail
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
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// For Danish customers only. The civic/company number (CPR or CVR) of
        /// the customer. Must be supplied if the customer's bank account is
        /// denominated in Danish krone (DKK).
        /// </summary>
        [JsonProperty("danish_identity_number")]
        public string DanishIdentityNumber { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// For ACH customers only. Required for ACH customers. A string
        /// containing the IP address of the payer to whom the mandate belongs
        /// (i.e. as a result of their completion of a mandate setup flow in
        /// their browser).
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
        /// [ISO3166-2:US](https://en.wikipedia.org/wiki/ISO_3166-2:US) state
        /// code is required (e.g. `CA` for California).
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// The schemes associated with this customer billing detail
        /// </summary>
        [JsonProperty("schemes")]
        public List<string> Schemes { get; set; }

        /// <summary>
        /// For Swedish customers only. The civic/company number (personnummer,
        /// samordningsnummer, or organisationsnummer) of the customer. Must be
        /// supplied if the customer's bank account is denominated in Swedish
        /// krona (SEK). This field cannot be changed once it has been set.
        /// </summary>
        [JsonProperty("swedish_identity_number")]
        public string SwedishIdentityNumber { get; set; }
    }
    
    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`pending`: the billing request is pending and can be used</li>
    /// <li>`ready_to_fulfil`: the billing request is ready to fulfil</li>
    /// <li>`fulfilling`: the billing request is currently undergoing fulfilment</li>
    /// <li>`fulfilled`: the billing request has been fulfilled and a payment created</li>
    /// <li>`cancelled`: the billing request has been cancelled and cannot be used</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

    /// <summary>
    /// Represents a billing request with action billing requests subscription request resource.
    ///
    /// Request for a subscription
    /// </summary>
    public class BillingRequestWithActionBillingRequestsSubscriptionRequest
    {
        /// <summary>
        /// Amount in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount to be deducted from each payment as an app fee, to be
        /// paid to the partner integration which created the subscription, in
        /// the lowest denomination for the currency (e.g. pence in GBP, cents
        /// in EUR).
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
        /// currency code. Currently "USD" and "CAD" are supported.
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
        /// Number of `interval_units` between customer charge dates. Must be
        /// greater than or equal to `1`. Must result in at least one charge
        /// date per year. Defaults to `1`.
        /// </summary>
        [JsonProperty("interval")]
        public int? Interval { get; set; }

        /// <summary>
        /// The unit of time between customer charge dates. One of `weekly`,
        /// `monthly` or `yearly`.
        /// </summary>
        [JsonProperty("interval_unit")]
        public BillingRequestWithActionBillingRequestsSubscriptionRequestIntervalUnit? IntervalUnit { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestWithActionBillingRequestsSubscriptionRequest.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestWithActionBillingRequestsSubscriptionRequestLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Name of the month on which to charge a customer. Must be lowercase.
        /// Only applies
        /// when the interval_unit is `yearly`.
        /// 
        /// </summary>
        [JsonProperty("month")]
        public BillingRequestWithActionBillingRequestsSubscriptionRequestMonth? Month { get; set; }

        /// <summary>
        /// Optional name for the subscription. This will be set as the
        /// description on each payment created. Must not exceed 255 characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// An optional payment reference. This will be set as the reference on
        /// each payment
        /// created and will appear on your customer's bank statement. See the
        /// documentation for
        /// the [create payment endpoint](#payments-create-a-payment) for more
        /// details.
        /// <br />
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// On failure, automatically retry payments using [intelligent
        /// retries](#success-intelligent-retries). Default is `false`. <p
        /// class="notice"><strong>Important</strong>: To be able to use
        /// intelligent retries, Success+ needs to be enabled in [GoCardless
        /// dashboard](https://manage.gocardless.com/success-plus). </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        /// The date on which the first payment should be charged. If fulfilled
        /// after this date, this will be set as the mandate's
        /// `next_possible_charge_date`.
        /// When left blank and `month` or `day_of_month` are provided, this
        /// will be set to the date of the first payment.
        /// If created without `month` or `day_of_month` this will be set as the
        /// mandate's `next_possible_charge_date`.
        /// 
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }
    }
    
    /// <summary>
    /// The unit of time between customer charge dates. One of `weekly`, `monthly` or `yearly`.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsSubscriptionRequestIntervalUnit {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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
    /// Resources linked to this BillingRequestWithActionBillingRequestsSubscriptionRequest
    /// </summary>
    public class BillingRequestWithActionBillingRequestsSubscriptionRequestLinks
    {
        /// <summary>
        /// (Optional) ID of the [subscription](#core-endpoints-subscriptions)
        /// that was created from this subscription request.
        /// 
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }
    
    /// <summary>
    /// Name of the month on which to charge a customer. Must be lowercase. Only applies
    /// when the interval_unit is `yearly`.
    /// 
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestWithActionBillingRequestsSubscriptionRequestMonth {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

}
