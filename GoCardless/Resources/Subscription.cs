using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a subscription resource.
    ///
    ///  Subscriptions create [payments](#core-endpoints-payments) according to
    ///  a schedule.
    ///
    ///  ### Recurrence Rules
    ///
    ///  The following rules apply when specifying recurrence:
    ///
    ///  - If `day_of_month` and `start_date` are not provided `start_date` will
    ///  be the [mandate](#core-endpoints-mandates)'s
    ///  `next_possible_charge_date` and the subscription will then recur based
    ///  on the `interval` & `interval_unit`
    ///  - If `month` or `day_of_month` are present the following validations
    ///  apply:
    ///
    ///  | __interval_unit__ | __month__                                      |
    ///  __day_of_month__                           |
    ///  | :---------------- | :--------------------------------------------- |
    ///  :----------------------------------------- |
    ///  | yearly            | optional (required if `day_of_month` provided) |
    ///  optional (invalid if `month` not provided) |
    ///  | monthly           | invalid                                        |
    ///  optional                                   |
    ///  | weekly            | invalid                                        |
    ///  invalid                                    |
    ///
    ///  Examples:
    ///
    ///  | __interval_unit__ | __interval__ | __month__ | __day_of_month__ |
    ///  valid?                                             |
    ///  | :---------------- | :----------- | :-------- | :--------------- |
    ///  :------------------------------------------------- |
    ///  | yearly            | 1            | january   | -1               |
    ///  valid                                              |
    ///  | monthly           | 6            |           |                  |
    ///  valid                                              |
    ///  | monthly           | 6            |           | 12               |
    ///  valid                                              |
    ///  | weekly            | 2            |           |                  |
    ///  valid                                              |
    ///  | yearly            | 1            | march     |                  |
    ///  invalid - missing `day_of_month`                   |
    ///  | yearly            | 1            |           | 2                |
    ///  invalid - missing `month`                          |
    ///  | monthly           | 6            | august    | 12               |
    ///  invalid - `month` must be blank                    |
    ///  | weekly            | 2            | october   | 10               |
    ///  invalid - `month` and `day_of_month` must be blank |
    ///
    ///  ### Rolling dates
    ///
    ///  When a charge date falls on a non-business day, one of two things will
    ///  happen:
    ///
    ///  - if the recurrence rule specified `-1` as the `day_of_month`, the
    ///  charge date will be rolled __backwards__ to the previous business day
    ///  (i.e., the last working day of the month).
    ///  - otherwise the charge date will be rolled __forwards__ to the next
    ///  business day.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        ///  Amount in the lowest denomination for the currency (e.g. pence in
        ///  GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        ///  The amount to be deducted from each payment as an app fee, to be
        ///  paid to the partner integration which created the subscription, in
        ///  the lowest denomination for the currency (e.g. pence in GBP, cents
        ///  in EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        ///  The total number of payments that should be taken by this
        ///  subscription.
        /// </summary>
        [JsonProperty("count")]
        public int? Count { get; set; }

        /// <summary>
        ///  Fixed [timestamp](#api-usage-dates-and-times), recording when this
        ///  resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        ///  [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        ///  currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        ///  "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        ///  As per RFC 2445. The day of the month to charge customers on.
        ///  `1`-`28` or `-1` to indicate the last day of the month.
        /// </summary>
        [JsonProperty("day_of_month")]
        public int? DayOfMonth { get; set; }

        /// <summary>
        ///  The earliest date that will be used as a `charge_date` on payments
        ///  created for this subscription if it is resumed. Only present for
        ///  `paused` subscriptions.
        ///  This value will change over time.
        /// </summary>
        [JsonProperty("earliest_charge_date_after_resume")]
        public string EarliestChargeDateAfterResume { get; set; }

        /// <summary>
        ///  Date on or after which no further payments should be created.
        ///  <br />
        ///  If this field is blank and `count` is not specified, the
        ///  subscription will continue forever.
        ///  <br />
        ///  <p class="deprecated-notice"><strong>Deprecated</strong>: This
        ///  field will be removed in a future API version. Use `count` to
        ///  specify a number of payments instead.</p>
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        /// <summary>
        ///  Unique identifier, beginning with "SB".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  Number of `interval_units` between customer charge dates. Must be
        ///  greater than or equal to `1`. Must result in at least one charge
        ///  date per year. Defaults to `1`.
        /// </summary>
        [JsonProperty("interval")]
        public int? Interval { get; set; }

        /// <summary>
        ///  The unit of time between customer charge dates. One of `weekly`,
        ///  `monthly` or `yearly`.
        /// </summary>
        [JsonProperty("interval_unit")]
        public SubscriptionIntervalUnit? IntervalUnit { get; set; }

        /// <summary>
        ///  Resources linked to this Subscription.
        /// </summary>
        [JsonProperty("links")]
        public SubscriptionLinks Links { get; set; }

        /// <summary>
        ///  Key-value store of custom data. Up to 3 keys are permitted, with
        ///  key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        ///  Name of the month on which to charge a customer. Must be lowercase.
        ///  Only applies
        ///  when the interval_unit is `yearly`.
        ///
        /// </summary>
        [JsonProperty("month")]
        public SubscriptionMonth? Month { get; set; }

        /// <summary>
        ///  Optional name for the subscription. This will be set as the
        ///  description on each payment created. Must not exceed 255
        ///  characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  Whether the parent plan of this subscription is paused.
        /// </summary>
        [JsonProperty("parent_plan_paused")]
        public bool? ParentPlanPaused { get; set; }

        /// <summary>
        ///  An optional payment reference. This will be set as the reference on
        ///  each payment
        ///  created and will appear on your customer's bank statement. See the
        ///  documentation for
        ///  the [create payment endpoint](#payments-create-a-payment) for more
        ///  details.
        ///  <br />
        ///  <p class="restricted-notice"><strong>Restricted</strong>: You need
        ///  your own Service User Number to specify a payment reference for
        ///  Bacs payments.</p>
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        ///  On failure, automatically retry payments using [intelligent
        ///  retries](/success-plus/overview). Default is `false`. <p
        ///  class="notice"><strong>Important</strong>: To be able to use
        ///  intelligent retries, Success+ needs to be enabled in [GoCardless
        ///  dashboard](https://manage.gocardless.com/success-plus). </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        ///  The date on which the first payment should be charged. Must be on
        ///  or after the [mandate](#core-endpoints-mandates)'s
        ///  `next_possible_charge_date`. When left blank and `month` or
        ///  `day_of_month` are provided, this will be set to the date of the
        ///  first payment. If created without `month` or `day_of_month` this
        ///  will be set as the mandate's `next_possible_charge_date`
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        ///  One of:
        ///  <ul>
        ///  <li>`pending_customer_approval`: the subscription is waiting for
        ///  customer approval before becoming active</li>
        ///  <li>`customer_approval_denied`: the customer did not approve the
        ///  subscription</li>
        ///  <li>`active`: the subscription is currently active and will
        ///  continue to create payments</li>
        ///  <li>`finished`: all of the payments scheduled for creation under
        ///  this subscription have been created</li>
        ///  <li>`cancelled`: the subscription has been cancelled and will no
        ///  longer create payments</li>
        ///  <li>`paused`: the subscription has been paused and will not create
        ///  payments</li>
        ///  </ul>
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        ///  Up to 10 upcoming payments with their amounts and charge dates.
        /// </summary>
        [JsonProperty("upcoming_payments")]
        public List<SubscriptionUpcomingPayment> UpcomingPayments { get; set; }
    }

    /// <summary>
    ///  The unit of time between customer charge dates. One of `weekly`, `monthly` or `yearly`.
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
    ///  Resources linked to this Subscription
    /// </summary>
    public class SubscriptionLinks
    {
        /// <summary>
        ///  ID of the associated [mandate](#core-endpoints-mandates) which the
        ///  subscription will create payments against.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }
    }

    /// <summary>
    ///  Name of the month on which to charge a customer. Must be lowercase. Only applies
    ///  when the interval_unit is `yearly`.
    ///
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
    ///  One of:
    ///  <ul>
    ///  <li>`pending_customer_approval`: the subscription is waiting for customer approval before
    ///  becoming active</li>
    ///  <li>`customer_approval_denied`: the customer did not approve the subscription</li>
    ///  <li>`active`: the subscription is currently active and will continue to create
    ///  payments</li>
    ///  <li>`finished`: all of the payments scheduled for creation under this subscription have
    ///  been created</li>
    ///  <li>`cancelled`: the subscription has been cancelled and will no longer create
    ///  payments</li>
    ///  <li>`paused`: the subscription has been paused and will not create payments</li>
    ///  </ul>
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
    ///  Represents a subscription upcoming payment resource.
    ///
    ///  Up to 10 upcoming payments with their amounts and charge dates.
    /// </summary>
    public class SubscriptionUpcomingPayment
    {
        /// <summary>
        ///  The amount of this payment, in minor unit (e.g. pence in GBP, cents
        ///  in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        ///  The date on which this payment will be charged.
        /// </summary>
        [JsonProperty("charge_date")]
        public string ChargeDate { get; set; }
    }
}
