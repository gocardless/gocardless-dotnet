using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a payout resource.
    ///
    /// Payouts represent transfers from GoCardless to a
    /// [creditor](#core-endpoints-creditors). Each payout contains the funds
    /// collected from one or many [payments](#core-endpoints-payments). Payouts
    /// are created automatically after a payment has been successfully
    /// collected.
    /// </summary>
    
    public class Payout
    {
        /// <summary>
        /// Amount in pence or cents.
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Date the payout is due to arrive in the creditor's bank account.
   
        ///     /// One of:
        /// <ul>
        ///   <li>`yyyy-mm-dd`: the
        /// payout has been paid and is due to arrive in the creditor's bank
   
        ///     ///   account on this day</li>
        ///   <li>`null`: the
        /// payout hasn't been paid yet</li>
        /// </ul>
        /// 
        /// </summary>
        [JsonProperty("arrival_date")]
        public string ArrivalDate { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently only "GBP", "EUR", and "SEK" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PayoutCurrency? Currency { get; set; }

        /// <summary>
        /// Fees that have already been deducted from the payout amount in pence
        /// or cents.
        /// 
        /// For each `late_failure_settled`
        /// or `chargeback_settled` action, we refund the transaction fees in a
        /// payout. This means that a payout can have a negative
        /// `deducted_fees`. This field is calculated as `GoCardless fees + app
        /// fees - refunded fees`
        /// 
        /// If the merchant is
        /// invoiced for fees separately from the payout, then `deducted_fees`
        /// will be 0.
        /// </summary>
        [JsonProperty("deducted_fees")]
        public int? DeductedFees { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PO".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("links")]
        public PayoutLinks Links { get; set; }

        /// <summary>
        /// Whether a payout contains merchant revenue or partner fees.
        /// </summary>
        [JsonProperty("payout_type")]
        public PayoutPayoutType? PayoutType { get; set; }

        /// <summary>
        /// Reference which appears on the creditor's bank statement.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the payout has
        /// been created, but not yet sent to the banks</li>
        ///
        /// <li>`paid`: the payout has been sent to the banks</li>
        ///
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public PayoutStatus? Status { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PayoutCurrency {
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency code. Currently
        /// only "GBP", "EUR", and "SEK" are supported.
        /// </summary>

        [EnumMember(Value = "GBP")]
        GBP,
        [EnumMember(Value = "EUR")]
        EUR,
        [EnumMember(Value = "SEK")]
        SEK,
    }

    public class PayoutLinks
    {
        /// <summary>
        /// ID of [creditor](#core-endpoints-creditors) who will receive this
        /// payout, i.e. the owner of the `creditor_bank_account`.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of [bank account](#core-endpoints-creditor-bank-accounts) which
        /// this will be sent to.
        /// </summary>
        [JsonProperty("creditor_bank_account")]
        public string CreditorBankAccount { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PayoutPayoutType {
        /// <summary>
        /// Whether a payout contains merchant revenue or partner fees.
        /// </summary>

        [EnumMember(Value = "merchant")]
        Merchant,
        [EnumMember(Value = "partner")]
        Partner,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PayoutStatus {
        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the payout has been created, but not
        /// yet sent to the banks</li>
        /// <li>`paid`: the payout has been sent to the
        /// banks</li>
        /// </ul>
        /// </summary>

        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "paid")]
        Paid,
    }

}
