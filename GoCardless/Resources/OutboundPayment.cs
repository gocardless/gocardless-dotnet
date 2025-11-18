using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a outbound payment resource.
    ///
    /// Outbound Payments represent payments sent from
    /// [creditors](#core-endpoints-creditors).
    ///
    /// GoCardless will notify you via a [webhook](#appendix-webhooks) when the
    /// status of the outbound payment [changes](#event-types-outbound-payment).
    ///
    /// <p class="restricted-notice"><strong>Restricted</strong>: Outbound
    /// Payments are currently in Early Access and available only to a limited
    /// list of organisations. If you are interested in using this feature,
    /// please stay tuned for our public launch announcement. We are actively
    /// testing and refining our API to ensure it meets your needs and provides
    /// the best experience.</p>
    /// </summary>
    public class OutboundPayment
    {
        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when the
        /// outbound payment was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency. Currently only "GBP" is supported.
        /// </summary>
        [JsonProperty("currency")]
        public OutboundPaymentCurrency? Currency { get; set; }

        /// <summary>
        /// A human-readable description of the outbound payment
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A future date on which the outbound payment should be sent.
        /// If not specified, the payment will be sent as soon as possible.
        /// </summary>
        [JsonProperty("execution_date")]
        public string ExecutionDate { get; set; }

        /// <summary>
        /// Unique identifier of the outbound payment.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Indicates whether the outbound payment is a withdrawal to your
        /// verified business bank account.
        /// </summary>
        [JsonProperty("is_withdrawal")]
        public bool? IsWithdrawal { get; set; }

        /// <summary>
        /// Resources linked to this OutboundPayment.
        /// </summary>
        [JsonProperty("links")]
        public OutboundPaymentLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with
        /// key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// An optional reference that will appear on your customer's bank
        /// statement.
        /// The character limit for this reference is dependent on the
        /// scheme.<br />
        /// <strong>Faster Payments</strong> - 18 characters, including:
        /// "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789
        /// &-./"<br />
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Bank payment scheme to process the outbound payment. Currently only
        /// "faster_payments" (GBP) is supported.
        /// </summary>
        [JsonProperty("scheme")]
        public OutboundPaymentScheme? Scheme { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`verifying`: The payment has been
        /// [created](#outbound-payments-create-an-outbound-payment) and the
        /// verification process has begun.</li>
        /// <li>`pending_approval`: The payment is awaiting
        /// [approval](#outbound-payments-approve-an-outbound-payment).</li>
        /// <li>`scheduled`: The payment has passed verification &
        /// [approval](#outbound-payments-approve-an-outbound-payment), but
        /// processing has not yet begun.</li>
        /// <li>`executing`: The execution date has arrived and the payment has
        /// been placed in queue for processing.</li>
        /// <li>`executed`: The payment has been accepted by the scheme and is
        /// now on its way to the recipient.</li>
        /// <li>`cancelled`: The payment has been
        /// [cancelled](#outbound-payments-cancel-an-outbound-payment) or was
        /// not [approved](#outbound-payments-approve-an-outbound-payment) on
        /// time.</li>
        /// <li>`failed`: The payment was not sent, usually due to an error
        /// while or after executing.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public OutboundPaymentStatus? Status { get; set; }

        /// <summary>
        /// Contains details of the verifications performed for the outbound
        /// payment
        /// </summary>
        [JsonProperty("verifications")]
        public OutboundPaymentVerifications Verifications { get; set; }
    }

    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency. Currently only
    /// "GBP" is supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentCurrency
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,
    }

    /// <summary>
    /// Resources linked to this OutboundPayment
    /// </summary>
    public class OutboundPaymentLinks
    {
        /// <summary>
        /// ID of the creditor who sends the outbound payment.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of the [customer](#core-endpoints-customers) that receives this
        /// outbound payment
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// ID of the customer bank account which receives the outbound payment.
        /// </summary>
        [JsonProperty("recipient_bank_account")]
        public string RecipientBankAccount { get; set; }
    }

    /// <summary>
    /// Bank payment scheme to process the outbound payment. Currently only "faster_payments" (GBP)
    /// is supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentScheme
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`scheme` with a value of "faster_payments"</summary>
        [EnumMember(Value = "faster_payments")]
        FasterPayments,
    }

    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`verifying`: The payment has been
    /// [created](#outbound-payments-create-an-outbound-payment) and the verification process has
    /// begun.</li>
    /// <li>`pending_approval`: The payment is awaiting
    /// [approval](#outbound-payments-approve-an-outbound-payment).</li>
    /// <li>`scheduled`: The payment has passed verification &
    /// [approval](#outbound-payments-approve-an-outbound-payment), but processing has not yet
    /// begun.</li>
    /// <li>`executing`: The execution date has arrived and the payment has been placed in queue for
    /// processing.</li>
    /// <li>`executed`: The payment has been accepted by the scheme and is now on its way to the
    /// recipient.</li>
    /// <li>`cancelled`: The payment has been
    /// [cancelled](#outbound-payments-cancel-an-outbound-payment) or was not
    /// [approved](#outbound-payments-approve-an-outbound-payment) on time.</li>
    /// <li>`failed`: The payment was not sent, usually due to an error while or after
    /// executing.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "verifying"</summary>
        [EnumMember(Value = "verifying")]
        Verifying,

        /// <summary>`status` with a value of "pending_approval"</summary>
        [EnumMember(Value = "pending_approval")]
        PendingApproval,

        /// <summary>`status` with a value of "scheduled"</summary>
        [EnumMember(Value = "scheduled")]
        Scheduled,

        /// <summary>`status` with a value of "executing"</summary>
        [EnumMember(Value = "executing")]
        Executing,

        /// <summary>`status` with a value of "executed"</summary>
        [EnumMember(Value = "executed")]
        Executed,

        /// <summary>`status` with a value of "cancelled"</summary>
        [EnumMember(Value = "cancelled")]
        Cancelled,

        /// <summary>`status` with a value of "failed"</summary>
        [EnumMember(Value = "failed")]
        Failed,
    }

    /// <summary>
    /// Represents a outbound payment verification resource.
    ///
    /// Contains details of the verifications performed for the outbound payment
    /// </summary>
    public class OutboundPaymentVerifications
    {
        /// <summary>
        /// Checks if the recipient owns the provided bank account
        /// </summary>
        [JsonProperty("recipient_bank_account_holder_verification")]
        public OutboundPaymentVerificationsRecipientBankAccountHolderVerification RecipientBankAccountHolderVerification { get; set; }
    }

    /// <summary>
    /// Represents a outbound payment verifications recipient bank account
    /// holder verification resource.
    ///
    /// Checks if the recipient owns the provided bank account
    /// </summary>
    public class OutboundPaymentVerificationsRecipientBankAccountHolderVerification
    {
        /// <summary>
        /// The actual account name returned by the recipient's bank, populated
        /// only in the case of a partial match.
        /// </summary>
        [JsonProperty("actual_account_name")]
        public string ActualAccountName { get; set; }

        /// <summary>
        /// Result of the verification, could be one of
        /// <ul>
        ///   <li>`full_match`: The verification has confirmed that the account
        /// name exactly matches the details provided.</li>
        ///   <li>`partial_match`:  The verification has confirmed that the
        /// account name is similar but does not match to the details provided.
        /// </li>
        ///   <li>`no_match`: The verification concludes the provided name does
        /// not match the account details.</li>
        ///   <li>`unable_to_match`: The verification could not be performed due
        /// to recipient bank issues or technical issues </li>
        /// </ul>
        /// </summary>
        [JsonProperty("result")]
        public OutboundPaymentVerificationsRecipientBankAccountHolderVerificationResult? Result { get; set; }

        /// <summary>
        /// Type of the verification that has been performed
        /// eg. [Confirmation of
        /// Payee](https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/)
        /// </summary>
        [JsonProperty("type")]
        public OutboundPaymentVerificationsRecipientBankAccountHolderVerificationType? Type { get; set; }
    }

    /// <summary>
    /// Result of the verification, could be one of
    /// <ul>
    ///   <li>`full_match`: The verification has confirmed that the account name exactly matches the
    /// details provided.</li>
    ///   <li>`partial_match`:  The verification has confirmed that the account name is similar but
    /// does not match to the details provided. </li>
    ///   <li>`no_match`: The verification concludes the provided name does not match the account
    /// details.</li>
    ///   <li>`unable_to_match`: The verification could not be performed due to recipient bank
    /// issues or technical issues </li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentVerificationsRecipientBankAccountHolderVerificationResult
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`result` with a value of "full_match"</summary>
        [EnumMember(Value = "full_match")]
        FullMatch,

        /// <summary>`result` with a value of "partial_match"</summary>
        [EnumMember(Value = "partial_match")]
        PartialMatch,

        /// <summary>`result` with a value of "no_match"</summary>
        [EnumMember(Value = "no_match")]
        NoMatch,

        /// <summary>`result` with a value of "unable_to_match"</summary>
        [EnumMember(Value = "unable_to_match")]
        UnableToMatch,
    }

    /// <summary>
    /// Type of the verification that has been performed
    /// eg. [Confirmation of
    /// Payee](https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/)
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentVerificationsRecipientBankAccountHolderVerificationType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`type` with a value of "confirmation_of_payee"</summary>
        [EnumMember(Value = "confirmation_of_payee")]
        ConfirmationOfPayee,
    }
}
