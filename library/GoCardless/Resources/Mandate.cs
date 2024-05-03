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
        /// This field is ACH specific, sometimes referred to as [SEC
        /// code](https://www.moderntreasury.com/learn/sec-codes).
        /// 
        /// This is the way that the payer gives authorisation to the merchant.
        ///   web: Authorisation is Internet Initiated or via Mobile Entry (maps
        /// to SEC code: WEB)
        ///   telephone: Authorisation is provided orally over telephone (maps
        /// to SEC code: TEL)
        ///   paper: Authorisation is provided in writing and signed, or
        /// similarly authenticated (maps to SEC code: PPD)
        /// 
        /// </summary>
        [JsonProperty("authorisation_source")]
        public MandateAuthorisationSource? AuthorisationSource { get; set; }

        /// <summary>
        /// (Optional) Payto and VRP Scheme specific information
        /// </summary>
        [JsonProperty("consent_parameters")]
        public MandateConsentParameters ConsentParameters { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// This field will decide how GoCardless handles settlement of funds
        /// from the customer.
        /// 
        /// - `managed` will be moved through GoCardless' account, batched, and
        /// payed out.
        /// - `direct` will be a direct transfer from the payer's account to the
        /// merchant where
        ///   invoicing will be handled separately.
        /// 
        /// </summary>
        [JsonProperty("funds_settlement")]
        public MandateFundsSettlement? FundsSettlement { get; set; }

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
        /// If this is an an ACH mandate, the earliest date that can be used as
        /// a
        /// `charge_date` on any newly created payment to be charged through
        /// standard
        /// ACH, rather than Faster ACH. This value will change over time.
        /// 
        /// It is only present in the API response for ACH mandates.
        /// </summary>
        [JsonProperty("next_possible_standard_ach_charge_date")]
        public string NextPossibleStandardAchChargeDate { get; set; }

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
        /// <a name="mandates_scheme"></a>Bank payment scheme to which this
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

        /// <summary>
        /// [Timestamp](#api-usage-time-zones--dates) recording when this
        /// mandate was verified.
        /// </summary>
        [JsonProperty("verified_at")]
        public string VerifiedAt { get; set; }
    }
    
    /// <summary>
    /// This field is ACH specific, sometimes referred to as [SEC
    /// code](https://www.moderntreasury.com/learn/sec-codes).
    /// 
    /// This is the way that the payer gives authorisation to the merchant.
    ///   web: Authorisation is Internet Initiated or via Mobile Entry (maps to SEC code: WEB)
    ///   telephone: Authorisation is provided orally over telephone (maps to SEC code: TEL)
    ///   paper: Authorisation is provided in writing and signed, or similarly authenticated (maps
    /// to SEC code: PPD)
    /// 
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum MandateAuthorisationSource {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`authorisation_source` with a value of "web"</summary>
        [EnumMember(Value = "web")]
        Web,
        /// <summary>`authorisation_source` with a value of "telephone"</summary>
        [EnumMember(Value = "telephone")]
        Telephone,
        /// <summary>`authorisation_source` with a value of "paper"</summary>
        [EnumMember(Value = "paper")]
        Paper,
    }

    /// <summary>
    /// Represents a mandate consent parameter resource.
    ///
    /// (Optional) Payto and VRP Scheme specific information
    /// </summary>
    public class MandateConsentParameters
    {
        /// <summary>
        /// The latest date at which payments can be taken, must occur after
        /// start_date if present
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        /// <summary>
        /// The maximum amount that can be charged for a single payment
        /// </summary>
        [JsonProperty("max_amount_per_payment")]
        public int? MaxAmountPerPayment { get; set; }

        /// <summary>
        /// Frequency configuration
        /// </summary>
        [JsonProperty("periods")]
        public List<MandateConsentParameterPeriod> Periods { get; set; }

        /// <summary>
        /// The date from which payments can be taken
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }
    }
    
    /// <summary>
    /// Frequency configuration
    /// </summary>
    public class MandateConsentParameterPeriod
    {
        /// <summary>
        /// The maximum total amount that can be charged for all payments in
        /// this period
        /// </summary>
        [JsonProperty("max_amount_per_period")]
        public int? MaxAmountPerPeriod { get; set; }

        /// <summary>
        /// The maximum number of payments that can be collected in this period
        /// </summary>
        [JsonProperty("max_payments_per_period")]
        public int? MaxPaymentsPerPeriod { get; set; }

        /// <summary>
        /// The repeating period for this mandate
        /// </summary>
        [JsonProperty("period")]
        public MandateConsentParameterPeriodPeriod? Period { get; set; }
    }
    
    /// <summary>
    /// The repeating period for this mandate
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum MandateConsentParameterPeriodPeriod {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`period` with a value of "day"</summary>
        [EnumMember(Value = "day")]
        Day,
        /// <summary>`period` with a value of "week"</summary>
        [EnumMember(Value = "week")]
        Week,
        /// <summary>`period` with a value of "month"</summary>
        [EnumMember(Value = "month")]
        Month,
        /// <summary>`period` with a value of "year"</summary>
        [EnumMember(Value = "year")]
        Year,
        /// <summary>`period` with a value of "flexible"</summary>
        [EnumMember(Value = "flexible")]
        Flexible,
    }

    /// <summary>
    /// This field will decide how GoCardless handles settlement of funds from the customer.
    /// 
    /// - `managed` will be moved through GoCardless' account, batched, and payed out.
    /// - `direct` will be a direct transfer from the payer's account to the merchant where
    ///   invoicing will be handled separately.
    /// 
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum MandateFundsSettlement {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`funds_settlement` with a value of "managed"</summary>
        [EnumMember(Value = "managed")]
        Managed,
        /// <summary>`funds_settlement` with a value of "direct"</summary>
        [EnumMember(Value = "direct")]
        Direct,
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
