using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        /// Amount in pence (GBP), cents (EUR), öre (SEK), or øre (DKK).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Amount [refunded](#core-endpoints-refunds) in pence/cents/öre/øre.
        /// </summary>
        [JsonProperty("amount_refunded")]
        public int? AmountRefunded { get; set; }

        /// <summary>
        /// A future date on which the payment should be collected. If not
        /// specified, the payment will be collected as soon as possible. This
        /// must be on or after the [mandate](#core-endpoints-mandates)'s
        /// `next_possible_charge_date`, and will be rolled-forwards by
        /// GoCardless if it is not a working day.
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
        /// currency code. Currently "GBP", "EUR", "SEK" and "DKK" are
        /// supported.
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
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// An optional payment reference that will appear on your customer's
        /// bank statement. For Bacs payments this can be up to 10 characters,
        /// for SEPA payments the limit is 140 characters, for Betalingsservice
        /// payments the limit is 30 characters and for Autogiro payments the
        /// limit is 11 characters. <p
        /// class='restricted-notice'><strong>Restricted</strong>: You can only
        /// specify a payment reference for Bacs payments (that is, when
        /// collecting from the UK) if you're on the <a
        /// href='https://gocardless.com/pricing'>GoCardless Plus or Pro
        /// packages</a>.</p>
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

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
    /// "GBP", "EUR", "SEK" and "DKK" are supported.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentCurrency {

        /// <summary>`currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,
        /// <summary>`currency` with a value of "EUR"</summary>
        [EnumMember(Value = "EUR")]
        EUR,
        /// <summary>`currency` with a value of "SEK"</summary>
        [EnumMember(Value = "SEK")]
        SEK,
        /// <summary>`currency` with a value of "DKK"</summary>
        [EnumMember(Value = "DKK")]
        DKK,
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
        /// ID of the [mandate](#core-endpoints-mandates) against which this
        /// payment should be collected.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// ID of [payout](#core-endpoints-payouts) which contains the funds
        /// from this payment.<br/>**Note**: this property will not be present
        /// until the payment has been successfully collected.
        /// </summary>
        [JsonProperty("payout")]
        public string Payout { get; set; }

        /// <summary>
        /// ID of [subscription](#core-endpoints-subscriptions) from which this
        /// payment was created.<br/>**Note**: this property will only be
        /// present if this payment is part of a subscription.
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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentStatus {

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
