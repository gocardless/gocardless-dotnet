using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a event resource.
    ///
    /// Events are stored for all webhooks. An event refers to a resource which
    /// has been updated, for example a payment which has been collected, or a
    /// mandate which has been transferred. Event creation is an asynchronous
    /// process, so it can take some time between an action occurring and its
    /// corresponding event getting included in API responses. See
    /// [here](#event-types) for a complete list of event types.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// What has happened to the resource. See [Event Types](#event-types)
        /// for the possible actions.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Present only in webhooks when an integrator is authorised to send
        /// their own
        /// notifications. See
        /// [here](/getting-started/api/handling-customer-notifications/)
        /// for further information.
        ///
        /// </summary>
        [JsonProperty("customer_notifications")]
        public List<EventCustomerNotification> CustomerNotifications { get; set; }

        [JsonProperty("details")]
        public EventDetails Details { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "EV".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this Event.
        /// </summary>
        [JsonProperty("links")]
        public EventLinks Links { get; set; }

        /// <summary>
        /// The metadata that was passed when making the API request that
        /// triggered the event
        /// (for instance, cancelling a mandate).
        ///
        /// This field will only be populated if the `details[origin]` field is
        /// `api` otherwise it will be an empty object.
        ///
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// The metadata of the resource that the event is for. For example,
        /// this field will have the same
        /// value of the `mandate[metadata]` field on the response you would
        /// receive from performing a GET request on a mandate.
        ///
        /// </summary>
        [JsonProperty("resource_metadata")]
        public IDictionary<string, string> ResourceMetadata { get; set; }

        /// <summary>
        /// The resource type for this event. One of:
        /// <ul>
        /// <li>`billing_requests`</li>
        /// <li>`creditors`</li>
        /// <li>`exports`</li>
        /// <li>`instalment_schedules`</li>
        /// <li>`mandates`</li>
        /// <li>`payer_authorisations`</li>
        /// <li>`payments`</li>
        /// <li>`payouts`</li>
        /// <li>`refunds`</li>
        /// <li>`scheme_identifiers`</li>
        /// <li>`subscriptions`</li>
        /// <li>`outbound_payments`</li>
        /// </ul>
        /// </summary>
        [JsonProperty("resource_type")]
        public EventResourceType? ResourceType { get; set; }

        /// <summary>
        /// Audit information about the source of the event.
        /// </summary>
        [JsonProperty("source")]
        public EventSource Source { get; set; }
    }

    /// <summary>
    /// Represents a event customer notification resource.
    ///
    /// Present only in webhooks when an integrator is authorised to send their
    /// own
    /// notifications. See
    /// [here](/getting-started/api/handling-customer-notifications/)
    /// for further information.
    /// </summary>
    public class EventCustomerNotification
    {
        /// <summary>
        /// Time after which GoCardless will send the notification by email.
        /// </summary>
        [JsonProperty("deadline")]
        public string Deadline { get; set; }

        /// <summary>
        /// The id of the notification.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Whether or not the notification must be sent.
        /// </summary>
        [JsonProperty("mandatory")]
        public bool? Mandatory { get; set; }

        /// <summary>
        /// See [here](#core-endpoints-customer-notifications) for a complete
        /// list of customer notification types.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// Represents a event detail resource.
    /// </summary>
    public class EventDetails
    {
        /// <summary>
        /// When we send a creditor `new_payout_currency_added` webhook, we also
        /// send the bank account id of the new account
        /// </summary>
        [JsonProperty("bank_account_id")]
        public string BankAccountId { get; set; }

        /// <summary>
        /// What triggered the event. _Note:_ `cause` is our simplified and
        /// predictable key indicating what triggered the event.
        /// </summary>
        [JsonProperty("cause")]
        public string Cause { get; set; }

        /// <summary>
        /// When we send a creditor `new_payout_currency_added` webhook, we also
        /// send the currency of the new account
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Human readable description of the cause. _Note:_ Changes to event
        /// descriptions are not considered breaking.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Count of rows in the csv. This is sent for export events
        /// </summary>
        [JsonProperty("item_count")]
        public int? ItemCount { get; set; }

        /// <summary>
        /// When will_attempt_retry is set to false, this field will contain
        /// the reason the payment was not retried. This can be one of:
        /// <ul>
        /// <li>`failure_filter_applied`: The payment won't be intelligently
        /// retried as
        ///   there is a high likelihood of failure on retry.</li>
        /// <li>`other`: The payment won't be intelligently retried due to any
        /// other
        ///   reason.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("not_retried_reason")]
        public string NotRetriedReason { get; set; }

        /// <summary>
        /// Who initiated the event. One of:
        /// <ul>
        /// <li>`bank`: this event was triggered by a report from the banks</li>
        /// <li>`gocardless`: this event was performed by GoCardless
        /// automatically</li>
        /// <li>`api`: this event was triggered by an API endpoint</li>
        /// <li>`customer`: this event was triggered by a Customer</li>
        /// <li>`payer`: this event was triggered by a Payer</li>
        /// </ul>
        /// </summary>
        [JsonProperty("origin")]
        public EventDetailsOrigin? Origin { get; set; }

        /// <summary>
        /// When we send a creditor `creditor_updated` webhook, this tells you
        /// which property on the creditor has been updated
        /// </summary>
        [JsonProperty("property")]
        public string Property { get; set; }

        /// <summary>
        /// Set when a `bank` is the origin of the event. This is the reason
        /// code received in the report from the customer's bank. See the
        /// [GoCardless Direct Debit
        /// guide](https://gocardless.com/direct-debit/receiving-messages) for
        /// information on the meanings of different reason codes. _Note:_
        /// `reason_code` is payment scheme-specific and can be inconsistent
        /// between banks.
        /// </summary>
        [JsonProperty("reason_code")]
        public string ReasonCode { get; set; }

        /// <summary>
        /// A bank payment scheme. Set when a bank is the origin of the event.
        /// </summary>
        [JsonProperty("scheme")]
        public EventDetailsScheme? Scheme { get; set; }

        /// <summary>
        /// Whether the payment will be retried automatically. Set on a payment
        /// failed event.
        /// </summary>
        [JsonProperty("will_attempt_retry")]
        public bool? WillAttemptRetry { get; set; }
    }

    /// <summary>
    /// Who initiated the event. One of:
    /// <ul>
    /// <li>`bank`: this event was triggered by a report from the banks</li>
    /// <li>`gocardless`: this event was performed by GoCardless automatically</li>
    /// <li>`api`: this event was triggered by an API endpoint</li>
    /// <li>`customer`: this event was triggered by a Customer</li>
    /// <li>`payer`: this event was triggered by a Payer</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum EventDetailsOrigin
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`origin` with a value of "bank"</summary>
        [EnumMember(Value = "bank")]
        Bank,

        /// <summary>`origin` with a value of "api"</summary>
        [EnumMember(Value = "api")]
        Api,

        /// <summary>`origin` with a value of "gocardless"</summary>
        [EnumMember(Value = "gocardless")]
        Gocardless,

        /// <summary>`origin` with a value of "customer"</summary>
        [EnumMember(Value = "customer")]
        Customer,

        /// <summary>`origin` with a value of "payer"</summary>
        [EnumMember(Value = "payer")]
        Payer,
    }

    /// <summary>
    /// A bank payment scheme. Set when a bank is the origin of the event.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum EventDetailsScheme
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`scheme` with a value of "ach"</summary>
        [EnumMember(Value = "ach")]
        Ach,

        /// <summary>`scheme` with a value of "autogiro"</summary>
        [EnumMember(Value = "autogiro")]
        Autogiro,

        /// <summary>`scheme` with a value of "bacs"</summary>
        [EnumMember(Value = "bacs")]
        Bacs,

        /// <summary>`scheme` with a value of "becs"</summary>
        [EnumMember(Value = "becs")]
        Becs,

        /// <summary>`scheme` with a value of "becs_nz"</summary>
        [EnumMember(Value = "becs_nz")]
        BecsNz,

        /// <summary>`scheme` with a value of "betalingsservice"</summary>
        [EnumMember(Value = "betalingsservice")]
        Betalingsservice,

        /// <summary>`scheme` with a value of "faster_payments"</summary>
        [EnumMember(Value = "faster_payments")]
        FasterPayments,

        /// <summary>`scheme` with a value of "pad"</summary>
        [EnumMember(Value = "pad")]
        Pad,

        /// <summary>`scheme` with a value of "pay_to"</summary>
        [EnumMember(Value = "pay_to")]
        PayTo,

        /// <summary>`scheme` with a value of "sepa_core"</summary>
        [EnumMember(Value = "sepa_core")]
        SepaCore,

        /// <summary>`scheme` with a value of "sepa_cor1"</summary>
        [EnumMember(Value = "sepa_cor1")]
        SepaCor1,
    }

    /// <summary>
    /// Resources linked to this Event
    /// </summary>
    public class EventLinks
    {
        /// <summary>
        /// ID of a [bank authorisation](#billing-requests-bank-authorisations).
        /// </summary>
        [JsonProperty("bank_authorisation")]
        public string BankAuthorisation { get; set; }

        /// <summary>
        /// ID of a [billing request](#billing-requests-billing-requests).
        /// </summary>
        [JsonProperty("billing_request")]
        public string BillingRequest { get; set; }

        /// <summary>
        /// ID of a [billing request
        /// flow](#billing-requests-billing-request-flows).
        /// </summary>
        [JsonProperty("billing_request_flow")]
        public string BillingRequestFlow { get; set; }

        /// <summary>
        /// If `resource_type` is `creditor`, this is the ID of the
        /// [creditor](#core-endpoints-creditors) which has been updated.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of a [customer](#core-endpoints-customers).
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// ID of a [customer bank
        /// account](#core-endpoints-customer-bank-accounts).
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// If `resource_type` is `instalment_schedule`, this is the ID of the
        /// [instalment schedule](#core-endpoints-instalment-schedules) which
        /// has been updated.
        /// </summary>
        [JsonProperty("instalment_schedule")]
        public string InstalmentSchedule { get; set; }

        /// <summary>
        /// If `resource_type` is `mandates`, this is the ID of the
        /// [mandate](#core-endpoints-mandates) which has been updated.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// This is the id of the mandate request associated to this event
        /// </summary>
        [JsonProperty("mandate_request")]
        public string MandateRequest { get; set; }

        /// <summary>
        /// If `resource_type` is `billing_requests`, this is the ID of the
        /// [mandate](#core-endpoints-mandates) which has been created.
        /// </summary>
        [JsonProperty("mandate_request_mandate")]
        public string MandateRequestMandate { get; set; }

        /// <summary>
        /// This is only included for mandate transfer events, when it is the ID
        /// of the [customer bank
        /// account](#core-endpoints-customer-bank-accounts) which the mandate
        /// is being transferred to.
        /// </summary>
        [JsonProperty("new_customer_bank_account")]
        public string NewCustomerBankAccount { get; set; }

        /// <summary>
        /// This is only included for mandate replaced events, when it is the ID
        /// of the new [mandate](#core-endpoints-mandates) that replaces the
        /// existing mandate.
        /// </summary>
        [JsonProperty("new_mandate")]
        public string NewMandate { get; set; }

        /// <summary>
        /// If the event is included in a [webhook](#webhooks-overview) to an
        /// [OAuth app](#appendix-oauth), this is the ID of the account to which
        /// it belongs.
        /// </summary>
        [JsonProperty("organisation")]
        public string Organisation { get; set; }

        /// <summary>
        /// If `resource_type` is `outbound_payments`, this is the ID of the
        /// outbound_payment which has been updated.
        /// </summary>
        [JsonProperty("outbound_payment")]
        public string OutboundPayment { get; set; }

        /// <summary>
        /// If this event was caused by another, this is the ID of the cause.
        /// For example, if a mandate is cancelled it automatically cancels all
        /// pending payments associated with it; in this case, the payment
        /// cancellation events would have the ID of the mandate cancellation
        /// event in this field.
        /// </summary>
        [JsonProperty("parent_event")]
        public string ParentEvent { get; set; }

        /// <summary>
        /// ID of a [payer authorisation](#core-endpoints-payer-authorisations).
        /// </summary>
        [JsonProperty("payer_authorisation")]
        public string PayerAuthorisation { get; set; }

        /// <summary>
        /// If `resource_type` is `payments`, this is the ID of the
        /// [payment](#core-endpoints-payments) which has been updated.
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }

        /// <summary>
        /// If `resource_type` is `billing_requests`, this is the ID of the
        /// [payment](#core-endpoints-payments) which has been created for
        /// Instant Bank Payment.
        /// </summary>
        [JsonProperty("payment_request_payment")]
        public string PaymentRequestPayment { get; set; }

        /// <summary>
        /// If `resource_type` is `payouts`, this is the ID of the
        /// [payout](#core-endpoints-payouts) which has been updated.
        /// </summary>
        [JsonProperty("payout")]
        public string Payout { get; set; }

        /// <summary>
        /// This is only included for mandate transfer events, when it is the ID
        /// of the [customer bank
        /// account](#core-endpoints-customer-bank-accounts) which the mandate
        /// is being transferred from.
        /// </summary>
        [JsonProperty("previous_customer_bank_account")]
        public string PreviousCustomerBankAccount { get; set; }

        /// <summary>
        /// If `resource_type` is `refunds`, this is the ID of the
        /// [refund](#core-endpoints-refunds) which has been updated.
        /// </summary>
        [JsonProperty("refund")]
        public string Refund { get; set; }

        /// <summary>
        /// If `resource_type` is `scheme_identifiers`, this is the ID of the
        /// [scheme_identifier](#core-endpoints-scheme-identifiers) which has
        /// been updated.
        /// </summary>
        [JsonProperty("scheme_identifier")]
        public string SchemeIdentifier { get; set; }

        /// <summary>
        /// If `resource_type` is `subscription`, this is the ID of the
        /// [subscription](#core-endpoints-subscriptions) which has been
        /// updated.
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

    /// <summary>
    /// The resource type for this event. One of:
    /// <ul>
    /// <li>`billing_requests`</li>
    /// <li>`creditors`</li>
    /// <li>`exports`</li>
    /// <li>`instalment_schedules`</li>
    /// <li>`mandates`</li>
    /// <li>`payer_authorisations`</li>
    /// <li>`payments`</li>
    /// <li>`payouts`</li>
    /// <li>`refunds`</li>
    /// <li>`scheme_identifiers`</li>
    /// <li>`subscriptions`</li>
    /// <li>`outbound_payments`</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum EventResourceType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`resource_type` with a value of "billing_requests"</summary>
        [EnumMember(Value = "billing_requests")]
        BillingRequests,

        /// <summary>`resource_type` with a value of "creditors"</summary>
        [EnumMember(Value = "creditors")]
        Creditors,

        /// <summary>`resource_type` with a value of "customers"</summary>
        [EnumMember(Value = "customers")]
        Customers,

        /// <summary>`resource_type` with a value of "exports"</summary>
        [EnumMember(Value = "exports")]
        Exports,

        /// <summary>`resource_type` with a value of "instalment_schedules"</summary>
        [EnumMember(Value = "instalment_schedules")]
        InstalmentSchedules,

        /// <summary>`resource_type` with a value of "mandates"</summary>
        [EnumMember(Value = "mandates")]
        Mandates,

        /// <summary>`resource_type` with a value of "organisations"</summary>
        [EnumMember(Value = "organisations")]
        Organisations,

        /// <summary>`resource_type` with a value of "outbound_payments"</summary>
        [EnumMember(Value = "outbound_payments")]
        OutboundPayments,

        /// <summary>`resource_type` with a value of "payer_authorisations"</summary>
        [EnumMember(Value = "payer_authorisations")]
        PayerAuthorisations,

        /// <summary>`resource_type` with a value of "payments"</summary>
        [EnumMember(Value = "payments")]
        Payments,

        /// <summary>`resource_type` with a value of "payouts"</summary>
        [EnumMember(Value = "payouts")]
        Payouts,

        /// <summary>`resource_type` with a value of "refunds"</summary>
        [EnumMember(Value = "refunds")]
        Refunds,

        /// <summary>`resource_type` with a value of "scheme_identifiers"</summary>
        [EnumMember(Value = "scheme_identifiers")]
        SchemeIdentifiers,

        /// <summary>`resource_type` with a value of "subscriptions"</summary>
        [EnumMember(Value = "subscriptions")]
        Subscriptions,
    }

    /// <summary>
    /// Represents a event source resource.
    ///
    /// Audit information about the source of the event.
    /// </summary>
    public class EventSource
    {
        /// <summary>
        /// The name of the event's source.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of the event's source.
        /// </summary>
        [JsonProperty("type")]
        public EventSourceType? Type { get; set; }
    }

    /// <summary>
    /// The type of the event's source.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum EventSourceType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`type` with a value of "app"</summary>
        [EnumMember(Value = "app")]
        App,

        /// <summary>`type` with a value of "user"</summary>
        [EnumMember(Value = "user")]
        User,

        /// <summary>`type` with a value of "gc_team"</summary>
        [EnumMember(Value = "gc_team")]
        GcTeam,

        /// <summary>`type` with a value of "access_token"</summary>
        [EnumMember(Value = "access_token")]
        AccessToken,
    }
}
