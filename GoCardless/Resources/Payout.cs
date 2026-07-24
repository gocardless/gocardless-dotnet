using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a payout resource.
    ///
    /// Payouts represent transfers from GoCardless to a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>.
    /// Each payout contains the funds collected from one or many <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payments">payments</a>.
    /// All the payments in a payout will have been collected in the same
    /// currency. Payouts are created automatically after a payment has been
    /// successfully collected.
    /// </summary>
    public class Payout
    {
        /// <summary>
        /// Amount in minor unit (e.g. pence in GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Date the payout is due to arrive in the creditor's bank account.
        /// One of:
        ///
        /// <ul>
        /// <li><c>yyyy-mm-dd</c>: the payout has been paid and is due to arrive
        /// in the creditor's bank account on this day</li>
        /// <li><c>null</c>: the payout hasn't been paid yet</li>
        /// </ul>
        /// </summary>
        [JsonProperty("arrival_date")]
        public string ArrivalDate { get; set; }

        /// <summary>
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when this resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public PayoutCurrency? Currency { get; set; }

        /// <summary>
        /// Fees that have already been deducted from the payout amount in minor
        /// unit (e.g. pence in GBP, cents in EUR), inclusive of tax if
        /// applicable.
        /// <br></br>
        /// For each <c>late_failure_settled</c> or <c>chargeback_settled</c>
        /// action, we refund the transaction fees in a payout. This means that
        /// a payout can have a negative <c>deducted_fees</c> value.
        /// <br></br>
        /// This field is calculated as <c>(GoCardless fees + app fees +
        /// surcharge fees) - (refunded fees)</c>
        /// <br></br>
        /// If the merchant is invoiced for fees separately from the payout,
        /// then <c>deducted_fees</c> will be 0.
        /// </summary>
        [JsonProperty("deducted_fees")]
        public int? DeductedFees { get; set; }

        [JsonProperty("fx")]
        public PayoutFx Fx { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PO".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this Payout.
        /// </summary>
        [JsonProperty("links")]
        public PayoutLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters. Note:
        /// This should not be used for storing PII data.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

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
        ///
        /// <ul>
        /// <li><c>pending</c>: the payout has been created, but not yet sent to
        /// your bank or it is in the process of being exchanged through our FX
        /// provider.</li>
        /// <li><c>paid</c>: the payout has been sent to the your bank. FX
        /// payouts will become <c>paid</c> after we emit the
        /// <c>fx_rate_confirmed</c> webhook.</li>
        /// <li><c>bounced</c>: the payout bounced when sent, the payout can be
        /// retried.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public PayoutStatus? Status { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> code for the currency in which tax is paid out to the tax
        /// authorities of your tax jurisdiction. Currently “EUR”, “GBP”, for
        /// French or British merchants, this will be <c>null</c> if tax is not
        /// applicable beta
        /// </summary>
        [JsonProperty("tax_currency")]
        public string TaxCurrency { get; set; }
    }

    /// <summary>
    /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO 4217</a> currency code.
    /// Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD" are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayoutCurrency
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
    /// Represents a payout fx resource.
    /// </summary>
    public class PayoutFx
    {
        /// <summary>
        /// Estimated rate that will be used in the foreign exchange of the
        /// <c>amount</c> into the <c>fx_currency</c>.
        /// This will vary based on the prevailing market rate until the moment
        /// that it is paid out.
        /// Present only before a resource is paid out. Has up to 10 decimal
        /// places.
        /// </summary>
        [JsonProperty("estimated_exchange_rate")]
        public string EstimatedExchangeRate { get; set; }

        /// <summary>
        /// Rate used in the foreign exchange of the <c>amount</c> into the
        /// <c>fx_currency</c>.
        /// Present only after a resource is paid out. Has up to 10 decimal
        /// places.
        /// </summary>
        [JsonProperty("exchange_rate")]
        public string ExchangeRate { get; set; }

        /// <summary>
        /// Amount that was paid out in the <c>fx_currency</c> after foreign
        /// exchange.
        /// Present only after the resource has been paid out.
        /// </summary>
        [JsonProperty("fx_amount")]
        public int? FxAmount { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> code for the currency in which amounts will be paid out
        /// (after foreign exchange). Currently "AUD", "CAD", "DKK", "EUR",
        /// "GBP", "NZD", "SEK" and "USD" are supported. Present only if payouts
        /// will be (or were) made via foreign exchange.
        /// </summary>
        [JsonProperty("fx_currency")]
        public PayoutFxFxCurrency? FxCurrency { get; set; }
    }

    /// <summary>
    /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO 4217</a> code for the
    /// currency in which amounts will be paid out (after foreign exchange). Currently "AUD", "CAD",
    /// "DKK", "EUR", "GBP", "NZD", "SEK" and "USD" are supported. Present only if payouts will be
    /// (or were) made via foreign exchange.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayoutFxFxCurrency
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`fx_currency` with a value of "AUD"</summary>
        [EnumMember(Value = "AUD")]
        AUD,

        /// <summary>`fx_currency` with a value of "CAD"</summary>
        [EnumMember(Value = "CAD")]
        CAD,

        /// <summary>`fx_currency` with a value of "DKK"</summary>
        [EnumMember(Value = "DKK")]
        DKK,

        /// <summary>`fx_currency` with a value of "EUR"</summary>
        [EnumMember(Value = "EUR")]
        EUR,

        /// <summary>`fx_currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,

        /// <summary>`fx_currency` with a value of "NZD"</summary>
        [EnumMember(Value = "NZD")]
        NZD,

        /// <summary>`fx_currency` with a value of "SEK"</summary>
        [EnumMember(Value = "SEK")]
        SEK,

        /// <summary>`fx_currency` with a value of "USD"</summary>
        [EnumMember(Value = "USD")]
        USD,
    }

    /// <summary>
    /// Resources linked to this Payout
    /// </summary>
    public class PayoutLinks
    {
        /// <summary>
        /// ID of <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>
        /// who will receive this payout, i.e. the owner of the
        /// <c>creditor_bank_account</c>.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditor-bank-accounts">bank
        /// account</a> which this will be sent to.
        /// </summary>
        [JsonProperty("creditor_bank_account")]
        public string CreditorBankAccount { get; set; }
    }

    /// <summary>
    /// Whether a payout contains merchant revenue or partner fees.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayoutPayoutType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`payout_type` with a value of "merchant"</summary>
        [EnumMember(Value = "merchant")]
        Merchant,

        /// <summary>`payout_type` with a value of "partner"</summary>
        [EnumMember(Value = "partner")]
        Partner,
    }

    /// <summary>
    /// One of:
    ///
    /// <ul>
    /// <li><c>pending</c>: the payout has been created, but not yet sent to your bank or it is in
    /// the process of being exchanged through our FX provider.</li>
    /// <li><c>paid</c>: the payout has been sent to the your bank. FX payouts will become
    /// <c>paid</c> after we emit the <c>fx_rate_confirmed</c> webhook.</li>
    /// <li><c>bounced</c>: the payout bounced when sent, the payout can be retried.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayoutStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>`status` with a value of "paid"</summary>
        [EnumMember(Value = "paid")]
        Paid,

        /// <summary>`status` with a value of "bounced"</summary>
        [EnumMember(Value = "bounced")]
        Bounced,
    }
}
