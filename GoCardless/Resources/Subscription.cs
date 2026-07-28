using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a subscription resource.
    ///
    /// Subscriptions create <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payments">payments</a>
    /// according to a schedule.
    ///
    /// <h3>Recurrence Rules</h3>
    /// The following rules apply when specifying recurrence:
    ///
    /// <ul>
    /// <li>If <c>day_of_month</c> and <c>start_date</c> are not provided
    /// <c>start_date</c> will be the <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>'s
    /// <c>next_possible_charge_date</c> and the subscription will then recur
    /// based on the <c>interval</c> &amp; <c>interval_unit</c></li>
    /// <li>If <c>month</c> or <c>day_of_month</c> are present the following
    /// validations apply:</li>
    /// </ul>
    /// | interval_unit | month                                      |
    /// day_of_month                           |
    /// | :---------------- | :--------------------------------------------- |
    /// :----------------------------------------- |
    /// | yearly            | optional (required if <c>day_of_month</c>
    /// provided) | optional (invalid if <c>month</c> not provided) |
    /// | monthly           | invalid                                        |
    /// optional                                   |
    /// | weekly            | invalid                                        |
    /// invalid                                    |
    ///
    /// Examples:
    ///
    /// | interval_unit | interval | month | day_of_month | valid?
    ///                               |
    /// | :---------------- | :----------- | :-------- | :--------------- |
    /// :------------------------------------------------- |
    /// | yearly            | 1            | january   | -1               |
    /// valid                                              |
    /// | monthly           | 6            |           |                  |
    /// valid                                              |
    /// | monthly           | 6            |           | 12               |
    /// valid                                              |
    /// | weekly            | 2            |           |                  |
    /// valid                                              |
    /// | yearly            | 1            | march     |                  |
    /// invalid - missing <c>day_of_month</c>                   |
    /// | yearly            | 1            |           | 2                |
    /// invalid - missing <c>month</c>                          |
    /// | monthly           | 6            | august    | 12               |
    /// invalid - <c>month</c> must be blank                    |
    /// | weekly            | 2            | october   | 10               |
    /// invalid - <c>month</c> and <c>day_of_month</c> must be blank |
    ///
    /// <h3>Rolling dates</h3>
    /// When a charge date falls on a non-business day, one of two things will
    /// happen:
    ///
    /// <ul>
    /// <li>if the recurrence rule specified <c>-1</c> as the
    /// <c>day_of_month</c>, the charge date will be rolled backwards to the
    /// previous business day (i.e., the last working day of the month).</li>
    /// <li>otherwise the charge date will be rolled forwards to the next
    /// business day.</li>
    /// </ul>
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Amount in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount to be deducted from each payment as an app fee, to be
        /// paid to the partner integration which created the subscription, in
        /// the lowest denomination for the currency (e.g. pence in GBP, cents
        /// in EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// The total number of payments that should be taken by this
        /// subscription.
        /// </summary>
        [JsonProperty("count")]
        public int? Count { get; set; }

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
        public string Currency { get; set; }

        /// <summary>
        /// As per RFC 2445. The day of the month to charge customers on.
        /// <c>1</c><ul>
        /// <li></li>
        /// </ul><c>28</c> or <c>-1</c> to indicate the last day of the month.
        /// </summary>
        [JsonProperty("day_of_month")]
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// The earliest date that will be used as a <c>charge_date</c> on
        /// payments
        /// created for this subscription if it is resumed. Only present for
        /// <c>paused</c> subscriptions.
        /// This value will change over time.
        /// </summary>
        [JsonProperty("earliest_charge_date_after_resume")]
        public string EarliestChargeDateAfterResume { get; set; }

        /// <summary>
        /// Date on or after which no further payments should be created.
        /// <br></br>
        /// If this field is blank and <c>count</c> is not specified, the
        /// subscription will continue forever.
        /// <br></br>
        ///
        /// <p class="deprecated-notice">Deprecated: This field will be removed
        /// in a future API version. Use <code>count</code> to specify a number
        /// of payments instead.</p>
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "SB".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Number of <c>interval_units</c> between customer charge dates. Must
        /// be greater than or equal to <c>1</c>. Must result in at least one
        /// charge date per year. Defaults to <c>1</c>.
        /// </summary>
        [JsonProperty("interval")]
        public int? Interval { get; set; }

        /// <summary>
        /// The unit of time between customer charge dates. One of
        /// <c>weekly</c>, <c>monthly</c> or <c>yearly</c>.
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
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Name of the month on which to charge a customer. Must be lowercase.
        /// Only applies
        /// when the interval_unit is <c>yearly</c>.
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
        /// Whether the parent plan of this subscription is paused.
        /// </summary>
        [JsonProperty("parent_plan_paused")]
        public bool? ParentPlanPaused { get; set; }

        /// <summary>
        /// An optional payment reference. This will be set as the reference on
        /// each payment
        /// created and will appear on your customer's bank statement. See the
        /// documentation for
        /// the <a
        /// href="https://developer.gocardless.com/api-reference/#payments-create-a-payment">create
        /// payment endpoint</a> for more details.
        /// <br></br>
        ///
        /// <p class="restricted-notice">Restricted: You need your own Service
        /// User Number to specify a payment reference for Bacs payments.</p>
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// On failure, automatically retry payments using <a
        /// href="https://developer.gocardless.com/success-plus/overview">intelligent
        /// retries</a>. Default is <c>false</c>. <p class="notice">Important:
        /// To be able to use intelligent retries, Success+ needs to be enabled
        /// in <a href="https://manage.gocardless.com/success-plus">GoCardless
        /// dashboard</a>. </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        /// The date on which the first payment should be charged. Must be on or
        /// after the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>'s
        /// <c>next_possible_charge_date</c>. When left blank and <c>month</c>
        /// or <c>day_of_month</c> are provided, this will be set to the date of
        /// the first payment. If created without <c>month</c> or
        /// <c>day_of_month</c> this will be set as the mandate's
        /// <c>next_possible_charge_date</c>
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>pending_customer_approval</c>: the subscription is waiting
        /// for customer approval before becoming active</li>
        /// <li><c>customer_approval_denied</c>: the customer did not approve
        /// the subscription</li>
        /// <li><c>active</c>: the subscription is currently active and will
        /// continue to create payments</li>
        /// <li><c>finished</c>: all of the payments scheduled for creation
        /// under this subscription have been created</li>
        /// <li><c>cancelled</c>: the subscription has been cancelled and will
        /// no longer create payments</li>
        /// <li><c>paused</c>: the subscription has been paused and will not
        /// create payments</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Up to 10 upcoming payments with their amounts and charge dates.
        /// </summary>
        [JsonProperty("upcoming_payments")]
        public List<SubscriptionUpcomingPayment> UpcomingPayments { get; set; }
    }

    /// <summary>
    /// The unit of time between customer charge dates. One of <c>weekly</c>, <c>monthly</c> or
    /// <c>yearly</c>.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum SubscriptionIntervalUnit
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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
        /// ID of the associated <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
        /// which the subscription will create payments against.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }
    }

    /// <summary>
    /// Name of the month on which to charge a customer. Must be lowercase. Only applies
    /// when the interval_unit is <c>yearly</c>.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum SubscriptionMonth
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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
    ///
    /// <ul>
    /// <li><c>pending_customer_approval</c>: the subscription is waiting for customer approval
    /// before becoming active</li>
    /// <li><c>customer_approval_denied</c>: the customer did not approve the subscription</li>
    /// <li><c>active</c>: the subscription is currently active and will continue to create
    /// payments</li>
    /// <li><c>finished</c>: all of the payments scheduled for creation under this subscription have
    /// been created</li>
    /// <li><c>cancelled</c>: the subscription has been cancelled and will no longer create
    /// payments</li>
    /// <li><c>paused</c>: the subscription has been paused and will not create payments</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum SubscriptionStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

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

        /// <summary>`status` with a value of "paused"</summary>
        [EnumMember(Value = "paused")]
        Paused,
    }

    /// <summary>
    /// Represents a subscription upcoming payment resource.
    ///
    /// Up to 10 upcoming payments with their amounts and charge dates.
    /// </summary>
    public class SubscriptionUpcomingPayment
    {
        /// <summary>
        /// The amount of this payment, in minor unit (e.g. pence in GBP, cents
        /// in EUR).
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
