using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        /// Array of <a
        /// href="https://developer.gocardless.com/api-reference/#mandates_scheme">schemes</a>
        /// supported for this bank account. This will be an empty array if the
        /// bank account is not reachable by any schemes.
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
        /// should refer to the <code>available_debit_schemes</code> attribute
        /// to determine reachability.</p>
        /// </summary>
        [JsonProperty("bic")]
        public string Bic { get; set; }

        /// <summary>
        /// The result of the payer name verification check performed during the
        /// lookup. <c>null</c> if no check was performed.
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
        /// <li><c>null</c>: Verification was not triggered. Either PNV is not
        /// supported for the scheme, or PNV feature is disabled for your
        /// organisation.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("payer_name_verification_result")]
        public BankDetailsLookupPayerNameVerificationResult? PayerNameVerificationResult { get; set; }
    }

    /// <summary>
    /// A bank payment scheme for this bank account.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BankDetailsLookupAvailableDebitScheme
    {
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

        /// <summary>`available_debit_scheme` with a value of "faster_payments"</summary>
        [EnumMember(Value = "faster_payments")]
        FasterPayments,

        /// <summary>`available_debit_scheme` with a value of "pad"</summary>
        [EnumMember(Value = "pad")]
        Pad,

        /// <summary>`available_debit_scheme` with a value of "pay_to"</summary>
        [EnumMember(Value = "pay_to")]
        PayTo,

        /// <summary>`available_debit_scheme` with a value of "sepa_core"</summary>
        [EnumMember(Value = "sepa_core")]
        SepaCore,
    }

    /// <summary>
    /// The result of the payer name verification check performed during the lookup. <c>null</c> if
    /// no check was performed.
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
    /// <li><c>null</c>: Verification was not triggered. Either PNV is not supported for the scheme,
    /// or PNV feature is disabled for your organisation.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BankDetailsLookupPayerNameVerificationResult
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
