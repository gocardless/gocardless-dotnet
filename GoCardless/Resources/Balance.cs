using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a balance resource.
    ///
    /// Returns the balances for a creditor. These balances are the same as
    /// what’s shown in the dashboard with one exception (mentioned below under
    /// balance_type).
    /// 
    /// These balances will typically be 3-5 minutes old. The balance amounts
    /// likely won’t match what’s shown in the dashboard as the dashboard
    /// balances are updated much less frequently (once per day).
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// The total amount in the balance, defined as the sum of all debits
        /// subtracted from the sum of all credits,
        /// in the lowest denomination for the currency (e.g. pence in GBP,
        /// cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Type of the balance. Could be one of
        /// <ul>
        /// <li>pending_payments_submitted: Payments we have submitted to the
        /// scheme but not yet confirmed. This does not exactly correspond to
        /// <i>Pending payments</i> in the dashboard, because this balance does
        /// not include payments that are pending submission.</li>
        /// <li>confirmed_funds: Payments that have been confirmed minus fees
        /// and unclaimed debits for refunds, failures and chargebacks. These
        /// funds have not yet been moved into a payout.</li>
        /// <li>pending_payouts: Confirmed payments that have been moved into a
        /// payout. This is the total due to be paid into your bank account in
        /// the next payout run (payouts happen once every business day).
        /// pending_payouts will only be non-zero while we are generating and
        /// submitting the payouts to our partner bank.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("balance_type")]
        public BalanceBalanceType? BalanceType { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public BalanceCurrency? Currency { get; set; }

        /// <summary>
        /// Dynamic [timestamp](#api-usage-time-zones--dates) recording when
        /// this resource was last updated.
        /// </summary>
        [JsonProperty("last_updated_at")]
        public string LastUpdatedAt { get; set; }

        /// <summary>
        /// Resources linked to this Balance.
        /// </summary>
        [JsonProperty("links")]
        public BalanceLinks Links { get; set; }
    }
    
    /// <summary>
    /// Type of the balance. Could be one of
    /// <ul>
    /// <li>pending_payments_submitted: Payments we have submitted to the scheme but not yet
    /// confirmed. This does not exactly correspond to <i>Pending payments</i> in the dashboard,
    /// because this balance does not include payments that are pending submission.</li>
    /// <li>confirmed_funds: Payments that have been confirmed minus fees and unclaimed debits for
    /// refunds, failures and chargebacks. These funds have not yet been moved into a payout.</li>
    /// <li>pending_payouts: Confirmed payments that have been moved into a payout. This is the
    /// total due to be paid into your bank account in the next payout run (payouts happen once
    /// every business day).
    /// pending_payouts will only be non-zero while we are generating and submitting the payouts to
    /// our partner bank.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BalanceBalanceType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`balance_type` with a value of "confirmed_funds"</summary>
        [EnumMember(Value = "confirmed_funds")]
        ConfirmedFunds,
        /// <summary>`balance_type` with a value of "pending_payouts"</summary>
        [EnumMember(Value = "pending_payouts")]
        PendingPayouts,
        /// <summary>`balance_type` with a value of "pending_payments_submitted"</summary>
        [EnumMember(Value = "pending_payments_submitted")]
        PendingPaymentsSubmitted,
    }

    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency code. Currently
    /// "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD" are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BalanceCurrency {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`currency` with a value of "AUD"</summary>
        [EnumMember(Value = "AUD")]
        AUD,
        /// <summary>`currency` with a value of "CAD"</summary>
        [EnumMember(Value = "CAD")]
        CAD,
        /// <summary>`currency` with a value of "DKK"</summary>
        [EnumMember(Value = "DKK")]
        DKK,
        /// <summary>`currency` with a value of "EUR"</summary>
        [EnumMember(Value = "EUR")]
        EUR,
        /// <summary>`currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,
        /// <summary>`currency` with a value of "NZD"</summary>
        [EnumMember(Value = "NZD")]
        NZD,
        /// <summary>`currency` with a value of "SEK"</summary>
        [EnumMember(Value = "SEK")]
        SEK,
        /// <summary>`currency` with a value of "USD"</summary>
        [EnumMember(Value = "USD")]
        USD,
    }

    /// <summary>
    /// Resources linked to this Balance
    /// </summary>
    public class BalanceLinks
    {
        /// <summary>
        /// ID of the associated [creditor](#core-endpoints-creditors).
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }
    }
    
}
