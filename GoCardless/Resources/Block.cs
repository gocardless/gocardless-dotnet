using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a block resource.
    ///
    /// Blocks are created to prevent certain customer details from being used
    /// when creating
    /// mandates.
    ///
    /// The details used to create blocks can be exact matches, like a bank
    /// account or an email,
    /// or a more generic match such as an email domain or bank name. Please be
    /// careful when creating
    /// blocks for more generic matches as this may block legitimate payers from
    /// using your service.
    ///
    /// New block types may be added over time.
    ///
    /// A block is in essence a simple rule that is used to match against
    /// details in a newly
    /// created mandate. If there is a successful match then the mandate is
    /// transitioned to a
    /// "blocked" state.
    ///
    /// Please note:
    ///
    ///   - Payments and subscriptions cannot be created against a mandate in
    /// blocked state.
    ///   - A mandate can never be transitioned out of the blocked state.
    ///
    /// The one exception to this is when blocking a 'bank_name'. This block
    /// will prevent bank
    /// accounts from being created for banks that match the given name. To
    /// ensure we match
    /// bank names correctly an existing bank account must be used when creating
    /// this block. Please
    /// be aware that we cannot always match a bank account to a given bank
    /// name.
    ///
    /// <p class="notice">
    ///   This API is currently only available for GoCardless Protect+
    /// integrators - please <a href="mailto:help@gocardless.com">get in
    /// touch</a> if you would like to use this API.
    /// </p>
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Shows if the block is active or disabled. Only active blocks will be
        /// used when deciding
        /// if a mandate should be blocked.
        /// </summary>
        [JsonProperty("active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Type of entity we will seek to match against when blocking the
        /// mandate. This
        /// can currently be one of 'email', 'email_domain', 'bank_account', or
        /// 'bank_name'.
        /// </summary>
        [JsonProperty("block_type")]
        public string BlockType { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BLC".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// This field is required if the reason_type is other. It should be a
        /// description of
        /// the reason for why you wish to block this payer and why it does not
        /// align with the
        /// given reason_types. This is intended to help us improve our
        /// knowledge of types of
        /// fraud.
        /// </summary>
        [JsonProperty("reason_description")]
        public string ReasonDescription { get; set; }

        /// <summary>
        /// The reason you wish to block this payer, can currently be one of
        /// 'identity_fraud',
        /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't
        /// captured by one of the
        /// above then 'other' can be selected but you must provide a reason
        /// description.
        /// </summary>
        [JsonProperty("reason_type")]
        public string ReasonType { get; set; }

        /// <summary>
        /// This field is a reference to the value you wish to block. This may
        /// be the raw value
        /// (in the case of emails or email domains) or the ID of the resource
        /// (in the case of
        /// bank accounts and bank names). This means in order to block a
        /// specific bank account
        /// (even if you wish to block generically by name) it must already have
        /// been created as
        /// a resource.
        /// </summary>
        [JsonProperty("resource_reference")]
        public string ResourceReference { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Type of entity we will seek to match against when blocking the mandate. This
    /// can currently be one of 'email', 'email_domain', 'bank_account', or 'bank_name'.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BlockBlockType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`block_type` with a value of "email"</summary>
        [EnumMember(Value = "email")]
        Email,

        /// <summary>`block_type` with a value of "email_domain"</summary>
        [EnumMember(Value = "email_domain")]
        EmailDomain,

        /// <summary>`block_type` with a value of "bank_account"</summary>
        [EnumMember(Value = "bank_account")]
        BankAccount,

        /// <summary>`block_type` with a value of "bank_name"</summary>
        [EnumMember(Value = "bank_name")]
        BankName,
    }

    /// <summary>
    /// The reason you wish to block this payer, can currently be one of 'identity_fraud',
    /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't captured by one of the
    /// above then 'other' can be selected but you must provide a reason description.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BlockReasonType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`reason_type` with a value of "identity_fraud"</summary>
        [EnumMember(Value = "identity_fraud")]
        IdentityFraud,

        /// <summary>`reason_type` with a value of "no_intent_to_pay"</summary>
        [EnumMember(Value = "no_intent_to_pay")]
        NoIntentToPay,

        /// <summary>`reason_type` with a value of "unfair_chargeback"</summary>
        [EnumMember(Value = "unfair_chargeback")]
        UnfairChargeback,

        /// <summary>`reason_type` with a value of "other"</summary>
        [EnumMember(Value = "other")]
        Other,
    }
}
