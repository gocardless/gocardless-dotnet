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
    /// Outbound Payments represent payments sent from <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditors</a>.
    ///
    /// GoCardless will notify you via a <a
    /// href="https://developer.gocardless.com/api-reference/#appendix-webhooks">webhook</a>
    /// when the status of the outbound payment <a
    /// href="https://developer.gocardless.com/api-reference/#event-types-outbound-payment">changes</a>.
    ///
    /// <h4>Rate limiting</h4>
    /// Two rate limits apply to the Outbound Payments APIs:
    ///
    /// <ul>
    /// <li>All POST Outbound Payment endpoints (create, withdraw, approve,
    /// cancel and etc.) share a single rate-limit group of 300 requests per
    /// minute. As initiating a payment typically requires two API calls (one to
    /// create the payment and one to approve it), this allows you to add
    /// approximately 150 outbound payments per minute.</li>
    /// <li>All remaining Outbound Payment endpoints are limited to 500 requests
    /// per minute.</li>
    /// </ul>
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
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when the outbound payment was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency. Currently only "GBP" is supported.
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
        /// scheme.<br></br>
        /// Faster Payments <ul>
        /// <li>18 characters, including:
        /// "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789
        /// &amp;-./"</li>
        /// </ul><br></br>
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
        ///
        /// <ul>
        /// <li><c>verifying</c>: The payment has been <a
        /// href="https://developer.gocardless.com/api-reference/#outbound-payments-create-an-outbound-payment">created</a>
        /// and the verification process has begun.</li>
        /// <li><c>pending_approval</c>: The payment is awaiting <a
        /// href="https://developer.gocardless.com/api-reference/#outbound-payments-approve-an-outbound-payment">approval</a>.</li>
        /// <li><c>scheduled</c>: The payment has passed verification &amp; <a
        /// href="https://developer.gocardless.com/api-reference/#outbound-payments-approve-an-outbound-payment">approval</a>,
        /// but processing has not yet begun.</li>
        /// <li><c>executing</c>: The execution date has arrived and the payment
        /// has been placed in queue for processing.</li>
        /// <li><c>executed</c>: The payment has been accepted by the scheme and
        /// is now on its way to the recipient.</li>
        /// <li><c>cancelled</c>: The payment has been <a
        /// href="https://developer.gocardless.com/api-reference/#outbound-payments-cancel-an-outbound-payment">cancelled</a>
        /// or was not <a
        /// href="https://developer.gocardless.com/api-reference/#outbound-payments-approve-an-outbound-payment">approved</a>
        /// on time.</li>
        /// <li><c>failed</c>: The payment was not sent, usually due to an error
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
    /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO 4217</a> currency.
    /// Currently only "GBP" is supported.
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
        /// ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>
        /// that receives this outbound payment
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// ID of the outbound payment import that created this outbound
        /// payment.
        /// </summary>
        [JsonProperty("outbound_payment_import")]
        public string OutboundPaymentImport { get; set; }

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
    ///
    /// <ul>
    /// <li><c>verifying</c>: The payment has been <a
    /// href="https://developer.gocardless.com/api-reference/#outbound-payments-create-an-outbound-payment">created</a>
    /// and the verification process has begun.</li>
    /// <li><c>pending_approval</c>: The payment is awaiting <a
    /// href="https://developer.gocardless.com/api-reference/#outbound-payments-approve-an-outbound-payment">approval</a>.</li>
    /// <li><c>scheduled</c>: The payment has passed verification &amp; <a
    /// href="https://developer.gocardless.com/api-reference/#outbound-payments-approve-an-outbound-payment">approval</a>,
    /// but processing has not yet begun.</li>
    /// <li><c>executing</c>: The execution date has arrived and the payment has been placed in
    /// queue for processing.</li>
    /// <li><c>executed</c>: The payment has been accepted by the scheme and is now on its way to
    /// the recipient.</li>
    /// <li><c>cancelled</c>: The payment has been <a
    /// href="https://developer.gocardless.com/api-reference/#outbound-payments-cancel-an-outbound-payment">cancelled</a>
    /// or was not <a
    /// href="https://developer.gocardless.com/api-reference/#outbound-payments-approve-an-outbound-payment">approved</a>
    /// on time.</li>
    /// <li><c>failed</c>: The payment was not sent, usually due to an error while or after
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
        ///
        /// <ul>
        /// <li><c>full_match</c>: The verification has confirmed that the
        /// account name exactly matches the details provided.</li>
        /// <li><c>partial_match</c>: The verification has confirmed that the
        /// account name is similar but does not match to the details
        /// provided.</li>
        /// <li><c>no_match</c>: The verification concludes the provided name
        /// does not match the account details.</li>
        /// <li><c>unable_to_match</c>: The verification could not be performed
        /// due to recipient bank issues or technical issues</li>
        /// </ul>
        /// </summary>
        [JsonProperty("result")]
        public OutboundPaymentVerificationsRecipientBankAccountHolderVerificationResult? Result { get; set; }

        /// <summary>
        /// Type of the verification that has been performed
        /// eg. <a
        /// href="https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/">Confirmation
        /// of Payee</a>
        /// </summary>
        [JsonProperty("type")]
        public OutboundPaymentVerificationsRecipientBankAccountHolderVerificationType? Type { get; set; }
    }

    /// <summary>
    /// Result of the verification, could be one of
    ///
    /// <ul>
    /// <li><c>full_match</c>: The verification has confirmed that the account name exactly matches
    /// the details provided.</li>
    /// <li><c>partial_match</c>: The verification has confirmed that the account name is similar
    /// but does not match to the details provided.</li>
    /// <li><c>no_match</c>: The verification concludes the provided name does not match the account
    /// details.</li>
    /// <li><c>unable_to_match</c>: The verification could not be performed due to recipient bank
    /// issues or technical issues</li>
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
    /// eg. <a
    /// href="https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/">Confirmation
    /// of Payee</a>
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
