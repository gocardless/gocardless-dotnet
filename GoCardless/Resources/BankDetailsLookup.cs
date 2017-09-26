using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a bank details lookup resource.
    ///
    /// Look up the name and reachability of a bank.
    /// </summary>
    public class BankDetailsLookup
    {
        /// <summary>
        /// Array of [schemes](#mandates_scheme) supported for this bank
        /// account. This will be an empty array if the bank account is not
        /// reachable by any schemes.
        /// </summary>
        [JsonProperty("available_debit_schemes")]
        public List<BankDetailsLookupAvailableDebitScheme?> AvailableDebitSchemes { get; set; }

        /// <summary>
        /// The name of the bank with which the account is held (if available).
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// ISO 9362 SWIFT BIC of the bank with which the account is held.
        /// </summary>
        [JsonProperty("bic")]
        public string Bic { get; set; }
    }
    
    /// <summary>
    /// A Direct Debit scheme for this bank account.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BankDetailsLookupAvailableDebitScheme {

        /// <summary>`availableDebitScheme` with a value of "autogiro"</summary>
        [EnumMember(Value = "autogiro")]
        Autogiro,
        /// <summary>`availableDebitScheme` with a value of "bacs"</summary>
        [EnumMember(Value = "bacs")]
        Bacs,
        /// <summary>`availableDebitScheme` with a value of "sepa_core"</summary>
        [EnumMember(Value = "sepa_core")]
        SepaCore,
    }
}
