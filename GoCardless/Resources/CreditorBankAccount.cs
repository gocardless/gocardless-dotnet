using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a creditor bank account resource.
    ///
    /// Creditor Bank Accounts hold the bank details of a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>.
    /// These are the bank accounts which your <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payouts">payouts</a>
    /// will be sent to.
    ///
    /// Note that creditor bank accounts must be unique, and so you will
    /// encounter a <c>bank_account_exists</c> error if you try to create a
    /// duplicate bank account. You may wish to handle this by updating the
    /// existing record instead, the ID of which will be provided as
    /// <c>links[creditor_bank_account]</c> in the error response.
    ///
    /// <p class="restricted-notice">Restricted: This API is not available for
    /// partner integrations.</p>
    /// </summary>
    public class CreditorBankAccount
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this is
        /// the same as the name stored with the linked <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>.
        /// This field will be transliterated, upcased and truncated to 18
        /// characters.
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
        /// not be provided for bank accounts in other currencies. See <a
        /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
        /// details</a> for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public CreditorBankAccountAccountType? AccountType { get; set; }

        /// <summary>
        /// Name of bank, taken from the bank details.
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// <a
        /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
        /// 3166-1 alpha-2 code</a>. Defaults to the country code of the
        /// <c>iban</c> if supplied, otherwise is required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when this resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
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
        /// Resources linked to this CreditorBankAccount.
        /// </summary>
        [JsonProperty("links")]
        public CreditorBankAccountLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Verification status of the Bank Account. Can be one of
        /// <c>pending</c>, <c>in_review</c> or <c>successful</c>
        /// </summary>
        [JsonProperty("verification_status")]
        public CreditorBankAccountVerificationStatus? VerificationStatus { get; set; }
    }

    /// <summary>
    /// Bank account type. Required for USD-denominated bank accounts. Must not be provided for bank
    /// accounts in other currencies. See <a
    /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
    /// details</a> for more information.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CreditorBankAccountAccountType
    {
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
    /// Resources linked to this CreditorBankAccount
    /// </summary>
    public class CreditorBankAccountLinks
    {
        /// <summary>
        /// ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>
        /// that owns this bank account.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }
    }

    /// <summary>
    /// Verification status of the Bank Account. Can be one of <c>pending</c>, <c>in_review</c> or
    /// <c>successful</c>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CreditorBankAccountVerificationStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`verification_status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>`verification_status` with a value of "in_review"</summary>
        [EnumMember(Value = "in_review")]
        InReview,

        /// <summary>`verification_status` with a value of "successful"</summary>
        [EnumMember(Value = "successful")]
        Successful,

        /// <summary>`verification_status` with a value of "could_not_verify"</summary>
        [EnumMember(Value = "could_not_verify")]
        CouldNotVerify,
    }
}
