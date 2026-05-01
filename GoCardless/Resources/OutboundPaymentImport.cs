using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a outbound payment import resource.
    ///
    /// Outbound Payment Imports allow you to create multiple payments via a
    /// single API call.
    ///
    /// The Workflow:
    /// 1. Create the outbound payment import.
    /// 2. Retrieve an authorisation link from the response.
    /// 3. Redirect the user to the link to authorise the import.
    /// 4. Once the user authorises the import, the individual outbound payments
    /// are automatically submitted.
    ///
    /// Import entries are not processed as actual payments until they are
    /// reviewed and authorised in GoCardless Dashboard.
    /// Upon approval, a unique outbound payment is generated for every entry in
    /// the import.
    ///
    /// <p class="notice">Outbound Payment Imports are capped at 1000 entries.
    /// If you expect to exceed this limit, please create multiple smaller
    /// imports.</p>
    /// </summary>
    public class OutboundPaymentImport
    {
        /// <summary>
        /// The sum of all import entry amounts, in the lowest denomination for
        /// the currency (e.g. pence in GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount_sum")]
        public int? AmountSum { get; set; }

        /// <summary>
        /// The link to the GoCardless dashboard to review and authorise the
        /// import
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
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency. Currently only "GBP" is supported.
        /// </summary>
        [JsonProperty("currency")]
        public OutboundPaymentImportCurrency? Currency { get; set; }

        [JsonProperty("entry_counts")]
        public OutboundPaymentImportEntryCounts EntryCounts { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "IM".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this OutboundPaymentImport.
        /// </summary>
        [JsonProperty("links")]
        public OutboundPaymentImportLinks Links { get; set; }

        /// <summary>
        /// The status of the outbound payment import.
        /// <ul>
        /// <li>`created`: The initial state of a new import.</li>
        /// <li>`validating`: Import validation in progress.</li>
        /// <li>`invalid`: Import validation failed.</li>
        /// <li>`valid`: Import validation succeeded.</li>
        /// <li>`processing`: Authorisation received; payments are being
        /// generated.</li>
        /// <li>`processed`: All entries have been successfully converted into
        /// outbound payments.</li>
        /// <li>`cancelled`: The import was cancelled by a user or automatically
        /// expired by the system.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public OutboundPaymentImportStatus? Status { get; set; }
    }

    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency. Currently only
    /// "GBP" is supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentImportCurrency
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,
    }

    /// <summary>
    /// Represents a outbound payment import entry count resource.
    /// </summary>
    public class OutboundPaymentImportEntryCounts
    {
        /// <summary>
        /// Count of entries that encountered a terminal error during the
        /// outbound payment generation process.
        /// </summary>
        [JsonProperty("failed_to_process")]
        public int? FailedToProcess { get; set; }

        /// <summary>
        /// Count of entries that failed validation checks.
        /// </summary>
        [JsonProperty("invalid")]
        public int? Invalid { get; set; }

        /// <summary>
        /// Count of entries successfully converted into outbound payments after
        /// the import was authorised.
        /// </summary>
        [JsonProperty("processed")]
        public int? Processed { get; set; }

        /// <summary>
        /// The total number of entries included in the import.
        /// </summary>
        [JsonProperty("total")]
        public int? Total { get; set; }

        /// <summary>
        /// Count of entries that passed validation checks.
        /// </summary>
        [JsonProperty("valid")]
        public int? Valid { get; set; }

        /// <summary>
        /// Total count of entries checked against bank account holder
        /// verification services (e.g., CoP).
        /// </summary>
        [JsonProperty("verified")]
        public int? Verified { get; set; }

        /// <summary>
        /// Count of entries where the account holder name was a direct match.
        /// </summary>
        [JsonProperty("verified_with_full_match")]
        public int? VerifiedWithFullMatch { get; set; }

        /// <summary>
        /// Count of entries where the account holder name did not match the
        /// records.
        /// </summary>
        [JsonProperty("verified_with_no_match")]
        public int? VerifiedWithNoMatch { get; set; }

        /// <summary>
        /// Count of entries where the account holder name was a close match.
        /// </summary>
        [JsonProperty("verified_with_partial_match")]
        public int? VerifiedWithPartialMatch { get; set; }

        /// <summary>
        /// Count of entries where the verification service could not return a
        /// definitive result.
        /// </summary>
        [JsonProperty("verified_with_unable_to_match")]
        public int? VerifiedWithUnableToMatch { get; set; }
    }

    /// <summary>
    /// Resources linked to this OutboundPaymentImport
    /// </summary>
    public class OutboundPaymentImportLinks
    {
        /// <summary>
        /// ID of the creditor who sends the outbound payments from the import.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }
    }

    /// <summary>
    /// The status of the outbound payment import.
    /// <ul>
    /// <li>`created`: The initial state of a new import.</li>
    /// <li>`validating`: Import validation in progress.</li>
    /// <li>`invalid`: Import validation failed.</li>
    /// <li>`valid`: Import validation succeeded.</li>
    /// <li>`processing`: Authorisation received; payments are being generated.</li>
    /// <li>`processed`: All entries have been successfully converted into outbound payments.</li>
    /// <li>`cancelled`: The import was cancelled by a user or automatically expired by the
    /// system.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum OutboundPaymentImportStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "created"</summary>
        [EnumMember(Value = "created")]
        Created,

        /// <summary>`status` with a value of "validating"</summary>
        [EnumMember(Value = "validating")]
        Validating,

        /// <summary>`status` with a value of "valid"</summary>
        [EnumMember(Value = "valid")]
        Valid,

        /// <summary>`status` with a value of "invalid"</summary>
        [EnumMember(Value = "invalid")]
        Invalid,

        /// <summary>`status` with a value of "processing"</summary>
        [EnumMember(Value = "processing")]
        Processing,

        /// <summary>`status` with a value of "processed"</summary>
        [EnumMember(Value = "processed")]
        Processed,

        /// <summary>`status` with a value of "cancelled"</summary>
        [EnumMember(Value = "cancelled")]
        Cancelled,
    }
}
