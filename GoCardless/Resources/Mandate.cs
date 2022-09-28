using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a mandate resource.
    ///
    /// Mandates represent the Direct Debit mandate with a
    /// [customer](#core-endpoints-customers).
    /// 
    /// GoCardless will notify you via a [webhook](#appendix-webhooks) whenever
    /// the status of a mandate changes.
    /// </summary>
    public class Mandate
    {
        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "MD". Note that this prefix may
        /// not apply to mandates created before 2016.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this Mandate.
        /// </summary>
        [JsonProperty("links")]
        public MandateLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// The earliest date that can be used as a `charge_date` on any newly
        /// created payment for this mandate. This value will change over time.
        /// </summary>
        [JsonProperty("next_possible_charge_date")]
        public string NextPossibleChargeDate { get; set; }

        /// <summary>
        /// Boolean value showing whether payments and subscriptions under this
        /// mandate require approval via an automated email before being
        /// processed.
        /// </summary>
        [JsonProperty("payments_require_approval")]
        public bool? PaymentsRequireApproval { get; set; }

        /// <summary>
        /// Unique reference. Different schemes have different length and
        /// [character set](#appendix-character-sets) requirements. GoCardless
        /// will generate a unique reference satisfying the different scheme
        /// requirements if this field is left blank.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// <a name="mandates_scheme"></a>Direct Debit scheme to which this
        /// mandate and associated payments are submitted. Can be supplied or
        /// automatically detected from the customer's bank account.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending_customer_approval`: the mandate has not yet been signed
        /// by the second customer</li>
        /// <li>`pending_submission`: the mandate has not yet been submitted to
        /// the customer's bank</li>
        /// <li>`submitted`: the mandate has been submitted to the customer's
        /// bank but has not been processed yet</li>
        /// <li>`active`: the mandate has been successfully set up by the
        /// customer's bank</li>
        /// <li>`suspended_by_payer`: the mandate has been suspended by
        /// payer</li>
        /// <li>`failed`: the mandate could not be created</li>
        /// <li>`cancelled`: the mandate has been cancelled</li>
        /// <li>`expired`: the mandate has expired due to dormancy</li>
        /// <li>`consumed`: the mandate has been consumed and cannot be reused
        /// (note that this only applies to schemes that are per-payment
        /// authorised)</li>
        /// <li>`blocked`: the mandate has been blocked and payments cannot be
        /// created</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public MandateStatus? Status { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this Mandate
    /// </summary>
    public class MandateLinks
    {
        /// <summary>
        /// ID of the associated [creditor](#core-endpoints-creditors).
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of the associated [customer](#core-endpoints-customers)
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// ID of the associated [customer bank
        /// account](#core-endpoints-customer-bank-accounts) which the mandate
        /// is created and submits payments against.
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// ID of the new mandate if this mandate has been replaced.
        /// </summary>
        [JsonProperty("new_mandate")]
        public string NewMandate { get; set; }
    }
    
    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`pending_customer_approval`: the mandate has not yet been signed by the second
    /// customer</li>
    /// <li>`pending_submission`: the mandate has not yet been submitted to the customer's bank</li>
    /// <li>`submitted`: the mandate has been submitted to the customer's bank but has not been
    /// processed yet</li>
    /// <li>`active`: the mandate has been successfully set up by the customer's bank</li>
    /// <li>`suspended_by_payer`: the mandate has been suspended by payer</li>
    /// <li>`failed`: the mandate could not be created</li>
    /// <li>`cancelled`: the mandate has been cancelled</li>
    /// <li>`expired`: the mandate has expired due to dormancy</li>
    /// <li>`consumed`: the mandate has been consumed and cannot be reused (note that this only
    /// applies to schemes that are per-payment authorised)</li>
    /// <li>`blocked`: the mandate has been blocked and payments cannot be created</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum MandateStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "pending_customer_approval"</summary>
        [EnumMember(Value = "pending_customer_approval")]
        PendingCustomerApproval,
        /// <summary>`status` with a value of "pending_submission"</summary>
        [EnumMember(Value = "pending_submission")]
        PendingSubmission,
        /// <summary>`status` with a value of "submitted"</summary>
        [EnumMember(Value = "submitted")]
        Submitted,
        /// <summary>`status` with a value of "active"</summary>
        [EnumMember(Value = "active")]
        Active,
        /// <summary>`status` with a value of "failed"</summary>
        [EnumMember(Value = "failed")]
        Failed,
        /// <summary>`status` with a value of "cancelled"</summary>
        [EnumMember(Value = "cancelled")]
        Cancelled,
        /// <summary>`status` with a value of "expired"</summary>
        [EnumMember(Value = "expired")]
        Expired,
        /// <summary>`status` with a value of "consumed"</summary>
        [EnumMember(Value = "consumed")]
        Consumed,
        /// <summary>`status` with a value of "blocked"</summary>
        [EnumMember(Value = "blocked")]
        Blocked,
        /// <summary>`status` with a value of "suspended_by_payer"</summary>
        [EnumMember(Value = "suspended_by_payer")]
        SuspendedByPayer,
    }

}
