using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a subscription resource.
    ///
    /// Subscriptions create [payments](#core-endpoints-payments) according to a
    /// schedule.
    /// 
    /// ### Recurrence Rules
    /// 
    /// The following rules apply when specifying recurrence:
    /// 
    /// - The first payment must be charged within 1 year.
    /// - When neither `month` nor `day_of_month` are present, the subscription
    /// will recur from the `start_date` based on the `interval_unit`.
    /// - If `month` or `day_of_month` are present, the recurrence rules will be
    /// applied from the `start_date`, and the following validations apply:
    /// 
    /// | interval_unit   | month                                          |
    /// day_of_month                            |
    /// | :-------------- | :--------------------------------------------- |
    /// :-------------------------------------- |
    /// | yearly          | optional (required if `day_of_month` provided) |
    /// optional (required if `month` provided) |
    /// | monthly         | invalid                                        |
    /// required                                |
    /// | weekly          | invalid                                        |
    /// invalid                                 |
    /// 
    /// Examples:
    /// 
    /// | interval_unit   | interval   | month   | day_of_month   | valid?      
    ///                                       |
    /// | :-------------- | :--------- | :------ | :------------- |
    /// :------------------------------------------------- |
    /// | yearly          | 1          | january | -1             | valid       
    ///                                       |
    /// | yearly          | 1          | march   |                | invalid -
    /// missing `day_of_month`                   |
    /// | monthly         | 6          |         | 12             | valid       
    ///                                       |
    /// | monthly         | 6          | august  | 12             | invalid -
    /// `month` must be blank                    |
    /// | weekly          | 2          |         |                | valid       
    ///                                       |
    /// | weekly          | 2          | october | 10             | invalid -
    /// `month` and `day_of_month` must be blank |
    /// 
    /// ### Rolling dates
    /// 
    /// When a charge date falls on a non-business day, one of two things will
    /// happen:
    /// 
    /// - if the recurrence rule specified `-1` as the `day_of_month`, the
    /// charge date will be rolled __backwards__ to the previous business day
    /// (i.e., the last working day of the month).
    /// - otherwise the charge date will be rolled __forwards__ to the next
    /// business day.
    /// 
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Amount in pence (GBP), cents (EUR), or öre (SEK).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217) currency code.
        /// Currently only `GBP`, `EUR`, and `SEK` are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// As per RFC 2445. The day of the month to charge customers on.
        /// `1`-`28` or `-1` to indicate the last day of the month.
        /// </summary>
        [JsonProperty("day_of_month")]
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// Date on or after which no further payments should be created. If
        /// this field is blank and `count` is not specified, the subscription
        /// will continue forever. <p
        /// class='deprecated-notice'><strong>Deprecated</strong>: This field
        /// will be removed in a future API version. Use `count` to specify a
        /// number of payments instead. </p>
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "SB".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Number of `interval_units` between customer charge dates. Must
        /// result in at least one charge date per year. Defaults to `1`.
        /// </summary>
        [JsonProperty("interval")]
        public int? Interval { get; set; }

        /// <summary>
        /// The unit of time between customer charge dates. One of `weekly`,
        /// `monthly` or `yearly`.
        /// </summary>
        [JsonProperty("interval_unit")]
        public SubscriptionIntervalUnit? IntervalUnit { get; set; }

        /// <summary>
        /// Resources linked to this Subscription.
        /// </summary>
        [JsonProperty("links")]
        public SubscriptionLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Name of the month on which to charge a customer. Must be lowercase.
        /// </summary>
        [JsonProperty("month")]
        public SubscriptionMonth? Month { get; set; }

        /// <summary>
        /// Optional name for the subscription. This will be set as the
        /// description on each payment created. Must not exceed 255 characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// An optional payment reference. This will be set as the reference on
        /// each payment created and will appear on your customer's bank
        /// statement. See the documentation for the [create payment
        /// endpoint](#payments-create-a-payment) for more details. <p
        /// class='restricted-notice'><strong>Restricted</strong>: You need your
        /// own Service User Number to specify a payment reference for Bacs
        /// payments.</p>
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// The date on which the first payment should be charged. Must be
        /// within one year of creation and on or after the
        /// [mandate](#core-endpoints-mandates)'s `next_possible_charge_date`.
        /// When blank, this will be set as the mandate's
        /// `next_possible_charge_date`.
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending_customer_approval`: the subscription is waiting for
        /// customer approval before becoming active</li>
        /// <li>`customer_approval_denied`: the customer did not approve the
        /// subscription</li>
        /// <li>`active`: the subscription is currently active and will continue
        /// to create payments</li>
        /// <li>`finished`: all of the payments scheduled for creation under
        /// this subscription have been created</li>
        /// <li>`cancelled`: the subscription has been cancelled and will no
        /// longer create payments</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Up to 10 upcoming payments with the amount, in pence, and charge
        /// date for each.
        /// </summary>
        [JsonProperty("upcoming_payments")]
        public List<SubscriptionUpcomingPayment> UpcomingPayments { get; set; }
    }
    
    /// <summary>
    /// The unit of time between customer charge dates. One of `weekly`, `monthly` or `yearly`.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubscriptionIntervalUnit {

        /// <summary>`interval_unit` with a value of "weekly"</summary>
        [EnumMember(Value = "weekly")]
        Weekly,
        /// <summary>`interval_unit` with a value of "monthly"</summary>
        [EnumMember(Value = "monthly")]
        Monthly,
        /// <summary>`interval_unit` with a value of "yearly"</summary>
        [EnumMember(Value = "yearly")]
        Yearly,
    }

    /// <summary>
    /// Resources linked to this Subscription
    /// </summary>
    public class SubscriptionLinks
    {
        /// <summary>
        /// ID of the associated [mandate](#core-endpoints-mandates) which the
        /// subscription will create payments against.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }
    }
    
    /// <summary>
    /// Name of the month on which to charge a customer. Must be lowercase.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubscriptionMonth {

        /// <summary>`month` with a value of "january"</summary>
        [EnumMember(Value = "january")]
        January,
        /// <summary>`month` with a value of "february"</summary>
        [EnumMember(Value = "february")]
        February,
        /// <summary>`month` with a value of "march"</summary>
        [EnumMember(Value = "march")]
        March,
        /// <summary>`month` with a value of "april"</summary>
        [EnumMember(Value = "april")]
        April,
        /// <summary>`month` with a value of "may"</summary>
        [EnumMember(Value = "may")]
        May,
        /// <summary>`month` with a value of "june"</summary>
        [EnumMember(Value = "june")]
        June,
        /// <summary>`month` with a value of "july"</summary>
        [EnumMember(Value = "july")]
        July,
        /// <summary>`month` with a value of "august"</summary>
        [EnumMember(Value = "august")]
        August,
        /// <summary>`month` with a value of "september"</summary>
        [EnumMember(Value = "september")]
        September,
        /// <summary>`month` with a value of "october"</summary>
        [EnumMember(Value = "october")]
        October,
        /// <summary>`month` with a value of "november"</summary>
        [EnumMember(Value = "november")]
        November,
        /// <summary>`month` with a value of "december"</summary>
        [EnumMember(Value = "december")]
        December,
    }

    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`pending_customer_approval`: the subscription is waiting for customer approval before
    /// becoming active</li>
    /// <li>`customer_approval_denied`: the customer did not approve the subscription</li>
    /// <li>`active`: the subscription is currently active and will continue to create payments</li>
    /// <li>`finished`: all of the payments scheduled for creation under this subscription have been
    /// created</li>
    /// <li>`cancelled`: the subscription has been cancelled and will no longer create payments</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubscriptionStatus {

        /// <summary>`status` with a value of "pending_customer_approval"</summary>
        [EnumMember(Value = "pending_customer_approval")]
        PendingCustomerApproval,
        /// <summary>`status` with a value of "customer_approval_denied"</summary>
        [EnumMember(Value = "customer_approval_denied")]
        CustomerApprovalDenied,
        /// <summary>`status` with a value of "active"</summary>
        [EnumMember(Value = "active")]
        Active,
        /// <summary>`status` with a value of "finished"</summary>
        [EnumMember(Value = "finished")]
        Finished,
        /// <summary>`status` with a value of "cancelled"</summary>
        [EnumMember(Value = "cancelled")]
        Cancelled,
    }

    /// <summary>
    /// Up to 10 upcoming payments with the amount, in pence, and charge date
    /// for each.
    /// </summary>
    public class SubscriptionUpcomingPayment
    {
        /// <summary>
        /// The amount of this payment, in pence.
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The date on which this payment will be charged.
        /// </summary>
        [JsonProperty("charge_date")]
        public string ChargeDate { get; set; }
    }
    
}
