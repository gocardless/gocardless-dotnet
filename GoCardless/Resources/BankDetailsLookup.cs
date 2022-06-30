using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a bank details lookup resource.
    ///
    /// Look up the name and reachability of a bank account.
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
        /// 
        /// <p class="notice">Even if no BIC is returned for an account,
        /// GoCardless may still be able to collect payments from it - you
        /// should refer to the `available_debit_schemes` attribute to determine
        /// reachability.</p>
        /// </summary>
        [JsonProperty("bic")]
        public string Bic { get; set; }
    }
    
    /// <summary>
    /// A Direct Debit scheme for this bank account.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BankDetailsLookupAvailableDebitScheme {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`available_debit_scheme` with a value of "ach"</summary>
        [EnumMember(Value = "ach")]
        Ach,
        /// <summary>`available_debit_scheme` with a value of "autogiro"</summary>
        [EnumMember(Value = "autogiro")]
        Autogiro,
        /// <summary>`available_debit_scheme` with a value of "bacs"</summary>
        [EnumMember(Value = "bacs")]
        Bacs,
        /// <summary>`available_debit_scheme` with a value of "becs"</summary>
        [EnumMember(Value = "becs")]
        Becs,
        /// <summary>`available_debit_scheme` with a value of "becs_nz"</summary>
        [EnumMember(Value = "becs_nz")]
        BecsNz,
        /// <summary>`available_debit_scheme` with a value of "betalingsservice"</summary>
        [EnumMember(Value = "betalingsservice")]
        Betalingsservice,
        /// <summary>`available_debit_scheme` with a value of "pad"</summary>
        [EnumMember(Value = "pad")]
        Pad,
        /// <summary>`available_debit_scheme` with a value of "sepa_core"</summary>
        [EnumMember(Value = "sepa_core")]
        SepaCore,
        /// <summary>`available_debit_scheme` with a value of "pay_to"</summary>
        [EnumMember(Value = "pay_to")]
        PayTo,
    }
}
