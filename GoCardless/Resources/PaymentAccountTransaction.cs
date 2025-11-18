using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a payment account transaction resource.
    ///
    /// Payment account transactions represent movements of funds on a given
    /// payment account. The payment account is provisioned by GoCardless and is
    /// used to fund [outbound payments](#core-endpoints-outbound-payments).
    /// </summary>
    public class PaymentAccountTransaction
    {
        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Balance after transaction, in the lowest denomination for the
        /// currency (e.g. pence in GBP, cents in EUR).
        /// </summary>
        [JsonProperty("balance_after_transaction")]
        public int? BalanceAfterTransaction { get; set; }

        /// <summary>
        /// The name of the counterparty of the transaction. The counterparty is
        /// the recipient for a credit, or the sender for a debit.
        /// </summary>
        [JsonProperty("counterparty_name")]
        public string CounterpartyName { get; set; }

        /// <summary>
        /// The currency of the transaction.
        /// </summary>
        [JsonProperty("currency")]
        public PaymentAccountTransactionCurrency? Currency { get; set; }

        /// <summary>
        /// The description of the transaction, if available
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The direction of the transaction. Debits mean money leaving the
        /// account (e.g. outbound payment), while credits signify money coming
        /// in (e.g. manual top-up).
        /// </summary>
        [JsonProperty("direction")]
        public PaymentAccountTransactionDirection? Direction { get; set; }

        /// <summary>
        /// The unique ID of the payment account transaction.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this PaymentAccountTransaction.
        /// </summary>
        [JsonProperty("links")]
        public PaymentAccountTransactionLinks Links { get; set; }

        /// <summary>
        /// The reference of the transaction. This is typically supplied by the
        /// sender.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// The date of when the transaction occurred.
        /// </summary>
        [JsonProperty("value_date")]
        public string ValueDate { get; set; }
    }

    /// <summary>
    /// The currency of the transaction.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PaymentAccountTransactionCurrency
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,
    }

    /// <summary>
    /// The direction of the transaction. Debits mean money leaving the account (e.g. outbound
    /// payment), while credits signify money coming in (e.g. manual top-up).
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PaymentAccountTransactionDirection
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`direction` with a value of "credit"</summary>
        [EnumMember(Value = "credit")]
        Credit,

        /// <summary>`direction` with a value of "debit"</summary>
        [EnumMember(Value = "debit")]
        Debit,
    }

    /// <summary>
    /// Resources linked to this PaymentAccountTransaction
    /// </summary>
    public class PaymentAccountTransactionLinks
    {
        /// <summary>
        /// ID of the [outbound_payment](#core-endpoints-outbound-payments)
        /// linked to the transaction
        /// </summary>
        [JsonProperty("outbound_payment")]
        public string OutboundPayment { get; set; }

        /// <summary>
        /// ID of the payment bank account.
        /// </summary>
        [JsonProperty("payment_bank_account")]
        public string PaymentBankAccount { get; set; }

        /// <summary>
        /// ID of the [payout](#core-endpoints-payouts) linked to the
        /// transaction.
        /// </summary>
        [JsonProperty("payout")]
        public string Payout { get; set; }
    }
}
