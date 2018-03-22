using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a payout item resource.
    ///
    /// When we collect a payment on your behalf, we add the money you've
    /// collected to your
    /// GoCardless balance, minus any fees paid. Periodically (usually every
    /// working day),
    /// we take any positive balance in your GoCardless account, and pay it out
    /// to your
    /// nominated bank account.
    /// 
    /// Other actions in your GoCardless account can also affect your balance.
    /// For example,
    /// if a customer charges back a payment, we'll deduct the payment's amount
    /// from your
    /// balance, but add any fees you paid for that payment back to your
    /// balance.
    /// 
    /// The Payout Items API allows you to view, on a per-payout basis, the
    /// credit and debit
    /// items that make up that payout's amount.
    /// 
    /// <p class="beta-notice"><strong>Beta</strong>:	The Payout Items API is in
    /// beta, and is
    /// subject to <a href="#overview-backwards-compatibility">backwards
    /// incompatible changes</a>
    /// with 30 days' notice. Before making any breaking changes, we will
    /// contact all integrators
    /// who have used the API.</p>
    /// 
    /// </summary>
    public class PayoutItem
    {
        /// <summary>
        /// The positive (credit) or negative (debit) value of the item, in
        /// fractional currency;
        /// either pence (GBP), cents (EUR), or öre (SEK), to one decimal place.
        /// <p class="notice">For accuracy, we store some of our fees to greater
        /// precision than
        /// we can actually pay out (for example, a GoCardless fee we record
        /// might come to 0.5
        /// pence, but it is not possible to send a payout via bank transfer
        /// including a half
        /// penny).<br><br>To calculate the final amount of the payout, we sum
        /// all of the items
        /// and then round to the nearest currency unit.</p>
        /// 
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Resources linked to this PayoutItem.
        /// </summary>
        [JsonProperty("links")]
        public PayoutItemLinks Links { get; set; }

        /// <summary>
        /// The type of the credit (positive) or debit (negative) item in the
        /// payout. One of:
        /// <ul>
        /// <li>`payment_paid_out` (credit)</li>
        /// <li>`payment_failed` (debit): The payment failed to be
        /// processed.</li>
        /// <li>`payment_charged_back` (debit): The payment has been charged
        /// back.</li>
        /// <li>`payment_refunded` (debit): The payment has been refunded to the
        /// customer.</li>
        /// <li>`refund` (debit): <em>private beta</em> A refund sent to a
        /// customer, not linked to a payment.</li>
        /// <li>`gocardless_fee` (credit/debit): The fees that GoCardless
        /// charged for a payment. In the case of a payment failure or
        /// chargeback, these will appear as credits.</li>
        /// <li>`app_fee` (credit/debit): The optional fees that a partner may
        /// have taken for a payment. In the case of a payment failure or
        /// chargeback, these will appear as credits.</li>
        /// <li>`revenue_share` (credit): Only shown in partner payouts.</li>
        /// </ul>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public PayoutItemType? Type { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this PayoutItem
    /// </summary>
    public class PayoutItemLinks
    {
        /// <summary>
        /// Unique identifier, beginning with "MD".
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PM".
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }
    }
    
    /// <summary>
    /// The type of the credit (positive) or debit (negative) item in the payout. One of:
    /// <ul>
    /// <li>`payment_paid_out` (credit)</li>
    /// <li>`payment_failed` (debit): The payment failed to be processed.</li>
    /// <li>`payment_charged_back` (debit): The payment has been charged back.</li>
    /// <li>`payment_refunded` (debit): The payment has been refunded to the customer.</li>
    /// <li>`refund` (debit): <em>private beta</em> A refund sent to a customer, not linked to a
    /// payment.</li>
    /// <li>`gocardless_fee` (credit/debit): The fees that GoCardless charged for a payment. In the
    /// case of a payment failure or chargeback, these will appear as credits.</li>
    /// <li>`app_fee` (credit/debit): The optional fees that a partner may have taken for a payment.
    /// In the case of a payment failure or chargeback, these will appear as credits.</li>
    /// <li>`revenue_share` (credit): Only shown in partner payouts.</li>
    /// </ul>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PayoutItemType {

        /// <summary>`type` with a value of "payment_paid_out"</summary>
        [EnumMember(Value = "payment_paid_out")]
        PaymentPaidOut,
        /// <summary>`type` with a value of "payment_failed"</summary>
        [EnumMember(Value = "payment_failed")]
        PaymentFailed,
        /// <summary>`type` with a value of "payment_charged_back"</summary>
        [EnumMember(Value = "payment_charged_back")]
        PaymentChargedBack,
        /// <summary>`type` with a value of "payment_refunded"</summary>
        [EnumMember(Value = "payment_refunded")]
        PaymentRefunded,
        /// <summary>`type` with a value of "refund"</summary>
        [EnumMember(Value = "refund")]
        Refund,
        /// <summary>`type` with a value of "gocardless_fee"</summary>
        [EnumMember(Value = "gocardless_fee")]
        GocardlessFee,
        /// <summary>`type` with a value of "app_fee"</summary>
        [EnumMember(Value = "app_fee")]
        AppFee,
        /// <summary>`type` with a value of "revenue_share"</summary>
        [EnumMember(Value = "revenue_share")]
        RevenueShare,
    }

}
