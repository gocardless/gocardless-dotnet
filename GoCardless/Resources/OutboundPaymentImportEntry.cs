using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a outbound payment import entry resource.
    ///
    /// Import Entries are the individual rows of an outbound payment import,
    /// representing each payment to be created.
    /// </summary>
    public class OutboundPaymentImportEntry
    {
        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "IE".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this OutboundPaymentImportEntry.
        /// </summary>
        [JsonProperty("links")]
        public OutboundPaymentImportEntryLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with
        /// key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// entry was processed.
        /// </summary>
        [JsonProperty("processed_at")]
        public string ProcessedAt { get; set; }

        /// <summary>
        /// An optional reference for the outbound payment.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Bank payment scheme to process the outbound payment. Currently only
        /// "faster_payments" (GBP) is supported.
        /// </summary>
        [JsonProperty("scheme")]
        public OutboundPaymentImportEntryScheme? Scheme { get; set; }

        /// <summary>
        /// Per-field validation errors for this entry, keyed by resource type
        /// and then by field name.
        /// </summary>
        [JsonProperty("validation_errors")]
        public OutboundPaymentImportEntryValidationErrors ValidationErrors { get; set; }

        /// <summary>
        /// The result of bank account holder verification, if performed.
        /// </summary>
        [JsonProperty("verification_result")]
        public OutboundPaymentImportEntryVerificationResult? VerificationResult { get; set; }
    }

    /// <summary>
    /// Resources linked to this OutboundPaymentImportEntry
    /// </summary>
    public class OutboundPaymentImportEntryLinks
    {
        /// <summary>
        /// ID of the associated outbound payment, once the entry has been
        /// processed.
        /// </summary>
        [JsonProperty("outbound_payment")]
        public string OutboundPayment { get; set; }

        /// <summary>
        /// ID of the associated import.
        /// </summary>
        [JsonProperty("outbound_payment_import")]
        public string OutboundPaymentImport { get; set; }

        /// <summary>
        /// ID of the recipient bank account.
        /// </summary>
        [JsonProperty("recipient_bank_account")]
        public string RecipientBankAccount { get; set; }
    }

    /// <summary>
    /// Bank payment scheme to process the outbound payment. Currently only "faster_payments" (GBP)
    /// is supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentImportEntryScheme
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`scheme` with a value of "faster_payments"</summary>
        [EnumMember(Value = "faster_payments")]
        FasterPayments,
    }

    /// <summary>
    /// Represents a outbound payment import entry validation error resource.
    ///
    /// Per-field validation errors for this entry, keyed by resource type and
    /// then by field name.
    /// </summary>
    public class OutboundPaymentImportEntryValidationErrors
    {
        /// <summary>
        /// Validation errors for the outbound payment fields.
        /// </summary>
        [JsonProperty("outbound_payment")]
        public OutboundPaymentImportEntryValidationErrorsOutboundPayment OutboundPayment { get; set; }
    }

    /// <summary>
    /// Represents a outbound payment import entry validation errors outbound
    /// payment resource.
    ///
    /// Validation errors for the outbound payment fields.
    /// </summary>
    public class OutboundPaymentImportEntryValidationErrorsOutboundPayment
    {
        /// <summary>
        /// Errors related to the amount field.
        /// </summary>
        [JsonProperty("amount")]
        public List<string> Amount { get; set; }

        /// <summary>
        /// Errors related to the recipient bank account.
        /// </summary>
        [JsonProperty("recipient_bank_account")]
        public List<string> RecipientBankAccount { get; set; }

        /// <summary>
        /// Errors related to the reference field.
        /// </summary>
        [JsonProperty("reference")]
        public List<string> Reference { get; set; }

        /// <summary>
        /// Bank payment scheme to process the outbound payment. Currently only
        /// "faster_payments" (GBP) is supported.
        /// </summary>
        [JsonProperty("scheme")]
        public OutboundPaymentImportEntryValidationErrorsOutboundPaymentScheme? Scheme { get; set; }
    }

    /// <summary>
    /// Bank payment scheme to process the outbound payment. Currently only "faster_payments" (GBP)
    /// is supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentImportEntryValidationErrorsOutboundPaymentScheme
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`scheme` with a value of "faster_payments"</summary>
        [EnumMember(Value = "faster_payments")]
        FasterPayments,
    }

    /// <summary>
    /// The result of bank account holder verification, if performed.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentImportEntryVerificationResult
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`verification_result` with a value of "full_match"</summary>
        [EnumMember(Value = "full_match")]
        FullMatch,

        /// <summary>`verification_result` with a value of "partial_match"</summary>
        [EnumMember(Value = "partial_match")]
        PartialMatch,

        /// <summary>`verification_result` with a value of "no_match"</summary>
        [EnumMember(Value = "no_match")]
        NoMatch,

        /// <summary>`verification_result` with a value of "unable_to_match"</summary>
        [EnumMember(Value = "unable_to_match")]
        UnableToMatch,
    }
}
