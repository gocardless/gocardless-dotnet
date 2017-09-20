using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
    /// </summary>
    
    public class CustomerBankAccount
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this
        /// matches the name of the linked
        /// [customer](#core-endpoints-customers). This field will be
        /// transliterated, upcased and truncated to 18 characters.
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Last two digits of account number.
        /// </summary>
        [JsonProperty("account_number_ending")]
        public string AccountNumberEnding { get; set; }

        /// <summary>
        /// Name of bank, taken from the bank details.
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. Defaults to the country code of the `iban` if
        /// supplied, otherwise is required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code, defaults to national currency of `country_code`.
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
        /// 
        /// </summary>
        [JsonProperty("links")]
        public CustomerBankAccountLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }
    
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
