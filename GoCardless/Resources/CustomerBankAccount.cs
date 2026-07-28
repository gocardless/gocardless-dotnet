using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a customer bank account resource.
    ///
    /// Customer Bank Accounts hold the bank details of a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>.
    /// They always belong to a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>,
    /// and may be linked to several Direct Debit <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandates</a>.
    ///
    /// Note that customer bank accounts must be unique, and so you will
    /// encounter a <c>bank_account_exists</c> error if you try to create a
    /// duplicate bank account. You may wish to handle this by updating the
    /// existing record instead, the ID of which will be provided as
    /// <c>links[customer_bank_account]</c> in the error response.
    ///
    /// Note: To ensure the customer's bank accounts are valid, verify them
    /// first
    /// using
    /// <a
    /// href="https://developer.gocardless.com/api-reference/#bank-details-lookups-perform-a-bank-details-lookup">bank_details_lookups</a>,
    /// before proceeding with creating the accounts
    /// </summary>
    public class CustomerBankAccount
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. The full name
        /// provided when the customer is created is stored and is available via
        /// the API, but is transliterated, upcased, and truncated to 18
        /// characters in bank submissions. This field is required unless the
        /// request includes a <a
        /// href="https://developer.gocardless.com/api-reference/#javascript-flow-customer-bank-account-tokens">customer
        /// bank account token</a>.
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
        public CustomerBankAccountAccountType? AccountType { get; set; }

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
        /// Resources linked to this CustomerBankAccount.
        /// </summary>
        [JsonProperty("links")]
        public CustomerBankAccountLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// The result of the payer name verification check performed when the
        /// bank account was created. Only present if a check was performed.
        ///
        /// <ul>
        /// <li><c>full</c>: The name provided matches the name held by the
        /// bank.</li>
        /// <li><c>close</c>: The name provided is a close but not exact match
        /// to the name held by the bank.</li>
        /// <li><c>cannot_perform_verification</c>: A verification was attempted
        /// but could not be completed. This can happen for a number of reasons,
        /// including the account holder's bank not participating in the
        /// verification scheme, the account not being eligible for verification
        /// (e.g. the account holder has opted out), or the bank details not
        /// being resolvable, among others.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("payer_name_verification_result")]
        public CustomerBankAccountPayerNameVerificationResult? PayerNameVerificationResult { get; set; }

        /// <summary>
        /// Whether this customer bank account is registered as a trusted
        /// recipient for Outbound Payments. Only present when the feature is
        /// enabled for the organisation.
        /// </summary>
        [JsonProperty("trusted_recipient")]
        public bool? TrustedRecipient { get; set; }
    }

    /// <summary>
    /// Bank account type. Required for USD-denominated bank accounts. Must not be provided for bank
    /// accounts in other currencies. See <a
    /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
    /// details</a> for more information.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CustomerBankAccountAccountType
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
    /// Resources linked to this CustomerBankAccount
    /// </summary>
    public class CustomerBankAccountLinks
    {
        /// <summary>
        /// ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>
        /// that owns this bank account.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }
    }

    /// <summary>
    /// The result of the payer name verification check performed when the bank account was created.
    /// Only present if a check was performed.
    ///
    /// <ul>
    /// <li><c>full</c>: The name provided matches the name held by the bank.</li>
    /// <li><c>close</c>: The name provided is a close but not exact match to the name held by the
    /// bank.</li>
    /// <li><c>cannot_perform_verification</c>: A verification was attempted but could not be
    /// completed. This can happen for a number of reasons, including the account holder's bank not
    /// participating in the verification scheme, the account not being eligible for verification
    /// (e.g. the account holder has opted out), or the bank details not being resolvable, among
    /// others.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CustomerBankAccountPayerNameVerificationResult
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`payer_name_verification_result` with a value of "full"</summary>
        [EnumMember(Value = "full")]
        Full,

        /// <summary>`payer_name_verification_result` with a value of "close"</summary>
        [EnumMember(Value = "close")]
        Close,

        /// <summary>`payer_name_verification_result` with a value of "cannot_perform_verification"</summary>
        [EnumMember(Value = "cannot_perform_verification")]
        CannotPerformVerification,
    }
}
