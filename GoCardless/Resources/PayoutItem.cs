using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a payout item resource.
    ///
    ///  When we collect a payment on your behalf, we add the money you've
    ///  collected to your
    ///  GoCardless balance, minus any fees paid. Periodically (usually every
    ///  working day),
    ///  we take any positive balance in your GoCardless account, and pay it out
    ///  to your
    ///  nominated bank account.
    ///
    ///  Other actions in your GoCardless account can also affect your balance.
    ///  For example,
    ///  if a customer charges back a payment, we'll deduct the payment's amount
    ///  from your
    ///  balance, but add any fees you paid for that payment back to your
    ///  balance.
    ///
    ///  The Payout Items API allows you to view, on a per-payout basis, the
    ///  credit and debit
    ///  items that make up that payout's amount.  Payout items can only be
    ///  retrieved for payouts
    ///  created in the last 6 months. Requests for older payouts will return an
    ///  HTTP status
    ///  <code>410 Gone</code>.
    /// </summary>
    public class PayoutItem
    {
        /// <summary>
        ///  The positive (credit) or negative (debit) value of the item, in
        ///  fractional currency;
        ///  the lowest denomination for the currency (e.g. pence in GBP, cents
        ///  in EUR), to one decimal place.
        ///  <p class="notice">For accuracy, we store some of our fees to
        ///  greater precision than we can actually pay out (for example, a
        ///  GoCardless fee we record might come to 0.5 pence, but it is not
        ///  possible to send a payout via bank transfer including a half
        ///  penny).<br><br>To calculate the final amount of the payout, we sum
        ///  all of the items and then round to the nearest currency unit.</p>
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        ///  Resources linked to this PayoutItem.
        /// </summary>
        [JsonProperty("links")]
        public PayoutItemLinks Links { get; set; }

        /// <summary>
        ///  An array of tax items <em>beta</em>
        ///
        ///  _Note_: VAT applies to transaction and surcharge fees for merchants
        ///  operating in the UK and France.
        /// </summary>
        [JsonProperty("taxes")]
        public List<PayoutItemTaxis> Taxes { get; set; }

        /// <summary>
        ///  The type of the credit (positive) or debit (negative) item in the
        ///  payout (inclusive of VAT if applicable). One of:
        ///  <ul>
        ///  <li>`payment_paid_out` (credit)</li>
        ///  <li>`payment_failed` (debit): The payment failed to be
        ///  processed.</li>
        ///  <li>`payment_charged_back` (debit): The payment has been charged
        ///  back.</li>
        ///  <li>`payment_refunded` (debit): The payment has been refunded to
        ///  the customer.</li>
        ///  <li>`refund` (debit): A refund sent to a customer, not linked to a
        ///  payment.</li>
        ///  <li>`refund_funds_returned` (credit): The refund could not be sent
        ///  to the customer, and the funds have been returned to you.</li>
        ///  <li>`gocardless_fee` (credit/debit): The fees that GoCardless
        ///  charged for a payment. In the case of a payment failure or
        ///  chargeback, these will appear as credits. Will include taxes if
        ///  applicable for merchants.</li>
        ///  <li>`app_fee` (credit/debit): The optional fees that a partner may
        ///  have taken for a payment. In the case of a payment failure or
        ///  chargeback, these will appear as credits.</li>
        ///  <li>`revenue_share` (credit/debit): A share of the fees that
        ///  GoCardless collected which some partner integrations receive when
        ///  their users take payments. Only shown in partner payouts. In the
        ///  case of a payment failure or chargeback, these will appear as
        ///  credits.</li>
        ///  <li>`surcharge_fee` (credit/debit): GoCardless deducted a surcharge
        ///  fee as the payment failed or was charged back, or refunded a
        ///  surcharge fee as the bank or customer cancelled the chargeback.
        ///  Will include taxes if applicable for merchants.</li>
        ///  </ul>
        ///
        /// </summary>
        [JsonProperty("type")]
        public PayoutItemType? Type { get; set; }
    }

    /// <summary>
    ///  Resources linked to this PayoutItem
    /// </summary>
    public class PayoutItemLinks
    {
        /// <summary>
        ///  Unique identifier, beginning with "MD". Note that this prefix may
        ///  not apply to mandates created before 2016. Present only for the
        ///  items of type `payment_refunded`, `refund` and
        ///  `refund_funds_returned`.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        ///  Unique identifier, beginning with "PM".
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }

        /// <summary>
        ///  Unique identifier, beginning with "RF". Present only for the items
        ///  of type `payment_refunded`, `refund` and `refund_funds_returned`.
        /// </summary>
        [JsonProperty("refund")]
        public string Refund { get; set; }
    }

    /// <summary>
    ///  Represents a payout item taxis resource.
    ///
    ///  An array of tax items <em>beta</em>
    ///
    ///  _Note_: VAT applies to transaction and surcharge fees for merchants
    ///  operating in the UK and France.
    /// </summary>
    public class PayoutItemTaxis
    {
        /// <summary>
        ///  The amount of tax applied to a fee in fractional currency; the
        ///  lowest denomination for the currency (e.g. pence in GBP, cents in
        ///  EUR), to one decimal place.
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        ///  [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        ///  currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        ///  "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PayoutItemTaxisCurrency? Currency { get; set; }

        /// <summary>
        ///  The amount of tax to be paid out to the tax authorities in
        ///  fractional currency; the lowest denomination for the currency (e.g.
        ///  pence in GBP, cents in EUR), to one decimal place.
        ///
        ///  When `currency` and `destination_currency` don't match this will be
        ///  `null` until the `exchange_rate` has been finalised.
        /// </summary>
        [JsonProperty("destination_amount")]
        public string DestinationAmount { get; set; }

        /// <summary>
        ///  [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) code
        ///  for the currency in which tax is paid out to the tax authorities of
        ///  your tax jurisdiction. Currently “EUR” for French merchants and
        ///  “GBP” for British merchants.
        /// </summary>
        [JsonProperty("destination_currency")]
        public string DestinationCurrency { get; set; }

        /// <summary>
        ///  The exchange rate for the tax from the currency into the
        ///  destination currency.
        ///
        ///  Present only if the currency and the destination currency don't
        ///  match and the exchange rate has been finalised.
        ///
        ///  You can listen for the payout's [`tax_exchange_rates_confirmed`
        ///  webhook](https://developer.gocardless.com/api-reference/#event-types-payout)
        ///  to know when the exchange rate has been finalised for all fees in
        ///  the payout.
        /// </summary>
        [JsonProperty("exchange_rate")]
        public string ExchangeRate { get; set; }

        /// <summary>
        ///  The unique identifier created by the jurisdiction, tax type and
        ///  version
        /// </summary>
        [JsonProperty("tax_rate_id")]
        public string TaxRateId { get; set; }
    }

    /// <summary>
    ///  [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency code. Currently
    ///  "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD" are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayoutItemTaxisCurrency
    {
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
    ///  The type of the credit (positive) or debit (negative) item in the payout (inclusive of VAT
    ///  if applicable). One of:
    ///  <ul>
    ///  <li>`payment_paid_out` (credit)</li>
    ///  <li>`payment_failed` (debit): The payment failed to be processed.</li>
    ///  <li>`payment_charged_back` (debit): The payment has been charged back.</li>
    ///  <li>`payment_refunded` (debit): The payment has been refunded to the customer.</li>
    ///  <li>`refund` (debit): A refund sent to a customer, not linked to a payment.</li>
    ///  <li>`refund_funds_returned` (credit): The refund could not be sent to the customer, and the
    ///  funds have been returned to you.</li>
    ///  <li>`gocardless_fee` (credit/debit): The fees that GoCardless charged for a payment. In the
    ///  case of a payment failure or chargeback, these will appear as credits. Will include taxes
    ///  if applicable for merchants.</li>
    ///  <li>`app_fee` (credit/debit): The optional fees that a partner may have taken for a
    ///  payment. In the case of a payment failure or chargeback, these will appear as credits.</li>
    ///  <li>`revenue_share` (credit/debit): A share of the fees that GoCardless collected which
    ///  some partner integrations receive when their users take payments. Only shown in partner
    ///  payouts. In the case of a payment failure or chargeback, these will appear as credits.</li>
    ///  <li>`surcharge_fee` (credit/debit): GoCardless deducted a surcharge fee as the payment
    ///  failed or was charged back, or refunded a surcharge fee as the bank or customer cancelled
    ///  the chargeback. Will include taxes if applicable for merchants.</li>
    ///  </ul>
    ///
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayoutItemType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

        /// <summary>`type` with a value of "surcharge_fee"</summary>
        [EnumMember(Value = "surcharge_fee")]
        SurchargeFee,

        /// <summary>`type` with a value of "refund_funds_returned"</summary>
        [EnumMember(Value = "refund_funds_returned")]
        RefundFundsReturned,
    }
}
