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
    /// Customer Bank Accounts hold the bank details of a
    /// [customer](#core-endpoints-customers). They always belong to a
    /// [customer](#core-endpoints-customers), and may be linked to several
    /// Direct Debit [mandates](#core-endpoints-mandates).
    ///
    /// Note that customer bank accounts must be unique, and so you will
    /// encounter a `bank_account_exists` error if you try to create a duplicate
    /// bank account. You may wish to handle this by updating the existing
    /// record instead, the ID of which will be provided as
    /// `links[customer_bank_account]` in the error response.
    ///
    /// _Note:_ To ensure the customer's bank accounts are valid, verify them
    /// first
    /// using
    ///
    /// [bank_details_lookups](#bank-details-lookups-perform-a-bank-details-lookup),
    /// before proceeding with creating the accounts
    /// </summary>
    public class CustomerBankAccount
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. This field will be
        /// transliterated, upcased and truncated to 18 characters. This field
        /// is required unless the request includes a [customer bank account
        /// token](#javascript-flow-customer-bank-account-tokens).
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
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
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
        /// [ISO 3166-1 alpha-2
        /// code](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements).
        /// Defaults to the country code of the `iban` if supplied, otherwise is
        /// required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
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
    }

    /// <summary>
    /// Bank account type. Required for USD-denominated bank accounts. Must not be provided for bank
    /// accounts in other currencies. See [local details](#local-bank-details-united-states) for
    /// more information.
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
        /// ID of the [customer](#core-endpoints-customers) that owns this bank
        /// account.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }
    }
}
