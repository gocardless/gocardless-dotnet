using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a block resource.
    ///
    /// A block object is a simple rule, when matched, pushes a newly created
    /// mandate to a blocked state. These details can be an exact match, like a
    /// bank account
    /// or an email, or a more generic match such as an email domain. New block
    /// types may be added
    /// over time. Payments and subscriptions can't be created against mandates
    /// in blocked state.
    /// 
    /// <p class="notice">
    ///   Client libraries have not yet been updated for this API but will be
    /// released soon.
    ///   This API is currently only available for approved integrators - please
    /// <a href="mailto:help@gocardless.com">get in touch</a> if you would like
    /// to use this API.
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
        /// can currently be one of 'email', 'email_domain', or 'bank_account'.
        /// </summary>
        [JsonProperty("block_type")]
        public string BlockType { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
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
        /// bank accounts). This means in order to block a specific bank account
        /// it must already
        /// have been created as a resource.
        /// </summary>
        [JsonProperty("resource_reference")]
        public string ResourceReference { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// Type of entity we will seek to match against when blocking the mandate. This
    /// can currently be one of 'email', 'email_domain', or 'bank_account'.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BlockBlockType {
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
    }

    /// <summary>
    /// The reason you wish to block this payer, can currently be one of 'identity_fraud',
    /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't captured by one of the
    /// above then 'other' can be selected but you must provide a reason description.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BlockReasonType {
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
