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
    /// corresponding event getting included in API responses. See <a
    /// href="https://developer.gocardless.com/api-reference/#event-types">here</a>
    /// for a complete list of event types.
    ///
    /// <p class="notice">Important: Events older than 18 months will be
    /// archived and no longer accessible via the API or exports. Archival will
    /// begin no sooner than 1 August 2026 in sandbox environments, and no
    /// sooner than 1 October 2026 in live environments. Events within the
    /// 18-month window are unaffected. If you need archived data, contact
    /// GoCardless support.</p>
    /// </summary>
    public class Event
    {
        /// <summary>
        /// What has happened to the resource. See <a
        /// href="https://developer.gocardless.com/api-reference/#event-types">Event
        /// Types</a> for the possible actions.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when this resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Present only in webhooks when an integrator is authorised to send
        /// their own
        /// notifications. See <a
        /// href="https://developer.gocardless.com/getting-started/api/handling-customer-notifications/">here</a>
        /// for further information.
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
        /// This field will only be populated if the <c>details[origin]</c>
        /// field is <c>api</c> otherwise it will be an empty object.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// The metadata of the resource that the event is for. For example,
        /// this field will have the same
        /// value of the <c>mandate[metadata]</c> field on the response you
        /// would receive from performing a GET request on a mandate.
        /// </summary>
        [JsonProperty("resource_metadata")]
        public IDictionary<string, string> ResourceMetadata { get; set; }

        /// <summary>
        /// The resource type for this event. One of:
        ///
        /// <ul>
        /// <li><c>billing_requests</c></li>
        /// <li><c>creditors</c></li>
        /// <li><c>exports</c></li>
        /// <li><c>instalment_schedules</c></li>
        /// <li><c>mandates</c></li>
        /// <li><c>payer_authorisations</c></li>
        /// <li><c>payments</c></li>
        /// <li><c>payouts</c></li>
        /// <li><c>refunds</c></li>
        /// <li><c>scheme_identifiers</c></li>
        /// <li><c>subscriptions</c></li>
        /// <li><c>outbound_payments</c></li>
        /// <li><c>payment_account_transactions</c></li>
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
    /// notifications. See <a
    /// href="https://developer.gocardless.com/getting-started/api/handling-customer-notifications/">here</a>
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
        /// See <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customer-notifications">here</a>
        /// for a complete list of customer notification types.
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
        /// When we send a creditor <c>new_payout_currency_added</c> webhook, we
        /// also send the bank account id of the new account
        /// </summary>
        [JsonProperty("bank_account_id")]
        public string BankAccountId { get; set; }

        /// <summary>
        /// What triggered the event. Note: <c>cause</c> is our simplified and
        /// predictable key indicating what triggered the event.
        /// </summary>
        [JsonProperty("cause")]
        public string Cause { get; set; }

        /// <summary>
        /// When we send a creditor <c>new_payout_currency_added</c> webhook, we
        /// also send the currency of the new account
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Human readable description of the cause. Note: Changes to event
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
        ///
        /// <ul>
        /// <li><c>failure_filter_applied</c>: The payment won't be
        /// intelligently retried as there is a high likelihood of failure on
        /// retry.</li>
        /// <li><c>other</c>: The payment won't be intelligently retried due to
        /// any other reason.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("not_retried_reason")]
        public string NotRetriedReason { get; set; }

        /// <summary>
        /// Who initiated the event. One of:
        ///
        /// <ul>
        /// <li><c>bank</c>: this event was triggered by a report from the
        /// banks</li>
        /// <li><c>gocardless</c>: this event was performed by GoCardless
        /// automatically</li>
        /// <li><c>api</c>: this event was triggered by an API endpoint</li>
        /// <li><c>customer</c>: this event was triggered by a Customer</li>
        /// <li><c>payer</c>: this event was triggered by a Payer</li>
        /// </ul>
        /// </summary>
        [JsonProperty("origin")]
        public EventDetailsOrigin? Origin { get; set; }

        /// <summary>
        /// When we send a creditor <c>creditor_updated</c> webhook, this tells
        /// you which property on the creditor has been updated
        /// </summary>
        [JsonProperty("property")]
        public string Property { get; set; }

        /// <summary>
        /// Set when a <c>bank</c> is the origin of the event. This is the
        /// reason code received in the report from the customer's bank. See the
        /// <a
        /// href="https://gocardless.com/direct-debit/receiving-messages">GoCardless
        /// Direct Debit guide</a> for information on the meanings of different
        /// reason codes. Note: <c>reason_code</c> is payment scheme-specific
        /// and can be inconsistent between banks.
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
    ///
    /// <ul>
    /// <li><c>bank</c>: this event was triggered by a report from the banks</li>
    /// <li><c>gocardless</c>: this event was performed by GoCardless automatically</li>
    /// <li><c>api</c>: this event was triggered by an API endpoint</li>
    /// <li><c>customer</c>: this event was triggered by a Customer</li>
    /// <li><c>payer</c>: this event was triggered by a Payer</li>
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
        /// ID of a <a
        /// href="https://developer.gocardless.com/api-reference/#billing-requests-bank-authorisations">bank
        /// authorisation</a>.
        /// </summary>
        [JsonProperty("bank_authorisation")]
        public string BankAuthorisation { get; set; }

        /// <summary>
        /// ID of a <a
        /// href="https://developer.gocardless.com/api-reference/#billing-requests-billing-requests">billing
        /// request</a>.
        /// </summary>
        [JsonProperty("billing_request")]
        public string BillingRequest { get; set; }

        /// <summary>
        /// ID of a <a
        /// href="https://developer.gocardless.com/api-reference/#billing-requests-billing-request-flows">billing
        /// request flow</a>.
        /// </summary>
        [JsonProperty("billing_request_flow")]
        public string BillingRequestFlow { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>creditor</c>, this is the ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>
        /// which has been updated.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of a <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// ID of a <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customer-bank-accounts">customer
        /// bank account</a>.
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>instalment_schedule</c>, this is the
        /// ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-instalment-schedules">instalment
        /// schedule</a> which has been updated.
        /// </summary>
        [JsonProperty("instalment_schedule")]
        public string InstalmentSchedule { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>mandates</c>, this is the ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
        /// which has been updated.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// This is the id of the mandate request associated to this event
        /// </summary>
        [JsonProperty("mandate_request")]
        public string MandateRequest { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>billing_requests</c>, this is the ID
        /// of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
        /// which has been created.
        /// </summary>
        [JsonProperty("mandate_request_mandate")]
        public string MandateRequestMandate { get; set; }

        /// <summary>
        /// This is only included for mandate transfer events, when it is the ID
        /// of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customer-bank-accounts">customer
        /// bank account</a> which the mandate is being transferred to.
        /// </summary>
        [JsonProperty("new_customer_bank_account")]
        public string NewCustomerBankAccount { get; set; }

        /// <summary>
        /// This is only included for mandate replaced events, when it is the ID
        /// of the new <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
        /// that replaces the existing mandate.
        /// </summary>
        [JsonProperty("new_mandate")]
        public string NewMandate { get; set; }

        /// <summary>
        /// If the event is included in a <a
        /// href="https://developer.gocardless.com/api-reference/#webhooks-overview">webhook</a>
        /// to an <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-oauth">OAuth
        /// app</a>, this is the ID of the account to which it belongs.
        /// </summary>
        [JsonProperty("organisation")]
        public string Organisation { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>outbound_payments</c>, this is the ID
        /// of the outbound_payment which has been updated.
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
        /// ID of a <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payer-authorisations">payer
        /// authorisation</a>.
        /// </summary>
        [JsonProperty("payer_authorisation")]
        public string PayerAuthorisation { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>payments</c>, this is the ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payments">payment</a>
        /// which has been updated.
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>payment_account_transaction</c>, this
        /// is the ID of a transaction which has been recorded on the payment
        /// account.
        /// </summary>
        [JsonProperty("payment_account_transaction")]
        public string PaymentAccountTransaction { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>billing_requests</c>, this is the ID
        /// of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payments">payment</a>
        /// which has been created for Pay by Bank.
        /// </summary>
        [JsonProperty("payment_request_payment")]
        public string PaymentRequestPayment { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>payouts</c>, this is the ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payouts">payout</a>
        /// which has been updated.
        /// </summary>
        [JsonProperty("payout")]
        public string Payout { get; set; }

        /// <summary>
        /// This is only included for mandate transfer events, when it is the ID
        /// of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customer-bank-accounts">customer
        /// bank account</a> which the mandate is being transferred from.
        /// </summary>
        [JsonProperty("previous_customer_bank_account")]
        public string PreviousCustomerBankAccount { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>refunds</c>, this is the ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-refunds">refund</a>
        /// which has been updated.
        /// </summary>
        [JsonProperty("refund")]
        public string Refund { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>scheme_identifiers</c>, this is the ID
        /// of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-scheme-identifiers">scheme_identifier</a>
        /// which has been updated.
        /// </summary>
        [JsonProperty("scheme_identifier")]
        public string SchemeIdentifier { get; set; }

        /// <summary>
        /// If <c>resource_type</c> is <c>subscription</c>, this is the ID of
        /// the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-subscriptions">subscription</a>
        /// which has been updated.
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

    /// <summary>
    /// The resource type for this event. One of:
    ///
    /// <ul>
    /// <li><c>billing_requests</c></li>
    /// <li><c>creditors</c></li>
    /// <li><c>exports</c></li>
    /// <li><c>instalment_schedules</c></li>
    /// <li><c>mandates</c></li>
    /// <li><c>payer_authorisations</c></li>
    /// <li><c>payments</c></li>
    /// <li><c>payouts</c></li>
    /// <li><c>refunds</c></li>
    /// <li><c>scheme_identifiers</c></li>
    /// <li><c>subscriptions</c></li>
    /// <li><c>outbound_payments</c></li>
    /// <li><c>payment_account_transactions</c></li>
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

        /// <summary>`resource_type` with a value of "payment_account_transactions"</summary>
        [EnumMember(Value = "payment_account_transactions")]
        PaymentAccountTransactions,

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
