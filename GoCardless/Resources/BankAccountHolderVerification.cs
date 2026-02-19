using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a bank account holder verification resource.
    ///
    ///  Create a bank account holder verification for a bank account.
    /// </summary>
    public class BankAccountHolderVerification
    {
        /// <summary>
        ///  The actual account name returned by the recipient's bank, populated
        ///  only in the case of a partial match.
        /// </summary>
        [JsonProperty("actual_account_name")]
        public string ActualAccountName { get; set; }

        /// <summary>
        ///  The unique identifier for the bank account holder verification
        ///  resource, e.g. "BAHV123".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  Result of the verification, could be one of
        ///  <ul>
        ///    <li>`full_match`: The verification has confirmed that the account
        ///  name exactly matches the details provided.</li>
        ///    <li>`partial_match`:  The verification has confirmed that the
        ///  account name is similar but does not match to the details provided.
        ///  </li>
        ///    <li>`no_match`: The verification concludes the provided name does
        ///  not match the account details.</li>
        ///    <li>`unable_to_match`: The verification could not be performed
        ///  due to recipient bank issues or technical issues </li>
        ///  </ul>
        /// </summary>
        [JsonProperty("result")]
        public BankAccountHolderVerificationResult? Result { get; set; }

        /// <summary>
        ///  The status of the bank account holder verification.
        ///  <ul>
        ///    <li>`pending`: We have triggered the verification, but the result
        ///  has not come back yet.</li>
        ///    <li>`completed`: The verification is complete and is ready to be
        ///  used.</li>
        ///  </ul>
        ///
        /// </summary>
        [JsonProperty("status")]
        public BankAccountHolderVerificationStatus? Status { get; set; }

        /// <summary>
        ///  Type of the verification that has been performed
        ///  eg. [Confirmation of
        ///  Payee](https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/)
        /// </summary>
        [JsonProperty("type")]
        public BankAccountHolderVerificationType? Type { get; set; }
    }

    /// <summary>
    ///  Result of the verification, could be one of
    ///  <ul>
    ///    <li>`full_match`: The verification has confirmed that the account name exactly matches
    ///  the details provided.</li>
    ///    <li>`partial_match`:  The verification has confirmed that the account name is similar but
    ///  does not match to the details provided. </li>
    ///    <li>`no_match`: The verification concludes the provided name does not match the account
    ///  details.</li>
    ///    <li>`unable_to_match`: The verification could not be performed due to recipient bank
    ///  issues or technical issues </li>
    ///  </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BankAccountHolderVerificationResult
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
    ///  The status of the bank account holder verification.
    ///  <ul>
    ///    <li>`pending`: We have triggered the verification, but the result has not come back
    ///  yet.</li>
    ///    <li>`completed`: The verification is complete and is ready to be used.</li>
    ///  </ul>
    ///
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BankAccountHolderVerificationStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>`status` with a value of "completed"</summary>
        [EnumMember(Value = "completed")]
        Completed,
    }

    /// <summary>
    ///  Type of the verification that has been performed
    ///  eg. [Confirmation of
    ///  Payee](https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/)
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BankAccountHolderVerificationType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`type` with a value of "confirmation_of_payee"</summary>
        [EnumMember(Value = "confirmation_of_payee")]
        ConfirmationOfPayee,
    }
}
