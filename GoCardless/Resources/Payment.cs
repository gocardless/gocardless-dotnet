using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a payment resource.
    ///
    /// Payment objects represent payments from a
    /// [customer](#core-endpoints-customers) to a
    /// [creditor](#core-endpoints-creditors), taken against a Direct Debit
    /// [mandate](#core-endpoints-mandates).
    /// 
    /// GoCardless will notify you via a [webhook](#appendix-webhooks) whenever
    /// the state of a payment changes.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Amount [refunded](#core-endpoints-refunds), in the lowest
        /// denomination for the currency (e.g. pence in GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount_refunded")]
        public int? AmountRefunded { get; set; }

        /// <summary>
        /// A future date on which the payment should be collected. If not
        /// specified, the payment will be collected as soon as possible. If the
        /// value is before the [mandate](#core-endpoints-mandates)'s
        /// `next_possible_charge_date` creation will fail. If the value is not
        /// a working day it will be rolled forwards to the next available one.
        /// </summary>
        [JsonProperty("charge_date")]
        public string ChargeDate { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
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
        public PaymentCurrency? Currency { get; set; }

        /// <summary>
        /// A human-readable description of the payment. This will be included
        /// in the notification email GoCardless sends to your customer if your
        /// organisation does not send its own notifications (see [compliance
        /// requirements](#appendix-compliance-requirements)).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("fx")]
        public PaymentFx Fx { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PM".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this Payment.
        /// </summary>
        [JsonProperty("links")]
        public PaymentLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// An optional reference that will appear on your customer's bank
        /// statement. The character limit for this reference is dependent on
        /// the scheme.<br /> <strong>ACH</strong> - 10 characters<br />
        /// <strong>Autogiro</strong> - 11 characters<br />
        /// <strong>Bacs</strong> - 10 characters<br /> <strong>BECS</strong> -
        /// 30 characters<br /> <strong>BECS NZ</strong> - 12 characters<br />
        /// <strong>Betalingsservice</strong> - 30 characters<br />
        /// <strong>PAD</strong> - scheme doesn't offer references<br />
        /// <strong>PayTo</strong> - 18 characters<br /> <strong>SEPA</strong> -
        /// 140 characters<br /> Note that this reference must be unique (for
        /// each merchant) for the BECS scheme as it is a scheme requirement. <p
        /// class='restricted-notice'><strong>Restricted</strong>: You can only
        /// specify a payment reference for Bacs payments (that is, when
        /// collecting from the UK) if you're on the <a
        /// href='https://gocardless.com/pricing'>GoCardless Plus, Pro or
        /// Enterprise packages</a>.</p>
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// On failure, automatically retry the payment using [intelligent
        /// retries](#success-intelligent-retries). Default is `false`.
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

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
    }
    
    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency code. Currently
    /// "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD" are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PaymentCurrency {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

    public class PaymentFx
    {
        /// <summary>
        /// Estimated rate that will be used in the foreign exchange of the
        /// `amount` into the `fx_currency`.
        /// This will vary based on the prevailing market rate until the moment
        /// that it is paid out.
        /// Present only before a resource is paid out. Has up to 10 decimal
        /// places.
        /// </summary>
        [JsonProperty("estimated_exchange_rate")]
        public string EstimatedExchangeRate { get; set; }

        /// <summary>
        /// Rate used in the foreign exchange of the `amount` into the
        /// `fx_currency`.
        /// Present only after a resource is paid out. Has up to 10 decimal
        /// places.
        /// </summary>
        [JsonProperty("exchange_rate")]
        public string ExchangeRate { get; set; }

        /// <summary>
        /// Amount that was paid out in the `fx_currency` after foreign
        /// exchange.
        /// Present only after the resource has been paid out.
        /// </summary>
        [JsonProperty("fx_amount")]
        public int? FxAmount { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) code
        /// for the currency in which amounts will be paid out (after foreign
        /// exchange). Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK"
        /// and "USD" are supported. Present only if payouts will be (or were)
        /// made via foreign exchange.
        /// </summary>
        [JsonProperty("fx_currency")]
        public PaymentFxFxCurrency? FxCurrency { get; set; }
    }
    
    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) code for the currency in
    /// which amounts will be paid out (after foreign exchange). Currently "AUD", "CAD", "DKK",
    /// "EUR", "GBP", "NZD", "SEK" and "USD" are supported. Present only if payouts will be (or
    /// were) made via foreign exchange.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PaymentFxFxCurrency {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`fx_currency` with a value of "AUD"</summary>
        [EnumMember(Value = "AUD")]
        AUD,
        /// <summary>`fx_currency` with a value of "CAD"</summary>
        [EnumMember(Value = "CAD")]
        CAD,
        /// <summary>`fx_currency` with a value of "DKK"</summary>
        [EnumMember(Value = "DKK")]
        DKK,
        /// <summary>`fx_currency` with a value of "EUR"</summary>
        [EnumMember(Value = "EUR")]
        EUR,
        /// <summary>`fx_currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,
        /// <summary>`fx_currency` with a value of "NZD"</summary>
        [EnumMember(Value = "NZD")]
        NZD,
        /// <summary>`fx_currency` with a value of "SEK"</summary>
        [EnumMember(Value = "SEK")]
        SEK,
        /// <summary>`fx_currency` with a value of "USD"</summary>
        [EnumMember(Value = "USD")]
        USD,
    }

    /// <summary>
    /// Resources linked to this Payment
    /// </summary>
    public class PaymentLinks
    {
        /// <summary>
        /// ID of [creditor](#core-endpoints-creditors) to which the collected
        /// payment will be sent.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of [instalment_schedule](#core-endpoints-instalment-schedules)
        /// from which this payment was created.<br/>**Note**: this property
        /// will only be present if this payment is part of an instalment
        /// schedule.
        /// </summary>
        [JsonProperty("instalment_schedule")]
        public string InstalmentSchedule { get; set; }

        /// <summary>
        /// ID of the [mandate](#core-endpoints-mandates) against which this
        /// payment should be collected.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// ID of [payout](#core-endpoints-payouts) which contains the funds
        /// from this payment.<br/>_Note_: this property will not be present
        /// until the payment has been successfully collected.
        /// </summary>
        [JsonProperty("payout")]
        public string Payout { get; set; }

        /// <summary>
        /// ID of [subscription](#core-endpoints-subscriptions) from which this
        /// payment was created.<br/>_Note_: this property will only be present
        /// if this payment is part of a subscription.
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }
    
    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`pending_customer_approval`: we're waiting for the customer to approve this payment</li>
    /// <li>`pending_submission`: the payment has been created, but not yet submitted to the
    /// banks</li>
    /// <li>`submitted`: the payment has been submitted to the banks</li>
    /// <li>`confirmed`: the payment has been confirmed as collected</li>
    /// <li>`paid_out`:  the payment has been included in a [payout](#core-endpoints-payouts)</li>
    /// <li>`cancelled`: the payment has been cancelled</li>
    /// <li>`customer_approval_denied`: the customer has denied approval for the payment. You should
    /// contact the customer directly</li>
    /// <li>`failed`: the payment failed to be processed. Note that payments can fail after being
    /// confirmed if the failure message is sent late by the banks.</li>
    /// <li>`charged_back`: the payment has been charged back</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PaymentStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

}
