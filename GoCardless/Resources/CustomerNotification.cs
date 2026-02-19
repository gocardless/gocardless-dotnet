using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a customer notification resource.
    ///
    ///  Customer Notifications represent the notification which is due to be
    ///  sent to a customer
    ///  after an event has happened. The event, the resource and the customer
    ///  to be notified
    ///  are all identified in the `links` property.
    ///
    ///  Note that these are ephemeral records - once the notification has been
    ///  actioned in some
    ///  way, it is no longer visible using this API.
    ///
    ///  <p class="restricted-notice"><strong>Restricted</strong>: This API is
    ///  currently only available for approved integrators - please <a
    ///  href="mailto:help@gocardless.com">get in touch</a> if you would like to
    ///  use this API.</p>
    /// </summary>
    public class CustomerNotification
    {
        /// <summary>
        ///  The action that was taken on the notification. Currently this can
        ///  only be `handled`,
        ///  which means the integrator sent the notification themselves.
        ///
        /// </summary>
        [JsonProperty("action_taken")]
        public string ActionTaken { get; set; }

        /// <summary>
        ///  Fixed [timestamp](#api-usage-dates-and-times), recording when this
        ///  action was taken.
        /// </summary>
        [JsonProperty("action_taken_at")]
        public string ActionTakenAt { get; set; }

        /// <summary>
        ///  A string identifying the integrator who was able to handle this
        ///  notification.
        /// </summary>
        [JsonProperty("action_taken_by")]
        public string ActionTakenBy { get; set; }

        /// <summary>
        ///  The id of the notification.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  Resources linked to this CustomerNotification.
        /// </summary>
        [JsonProperty("links")]
        public CustomerNotificationLinks Links { get; set; }

        /// <summary>
        ///  The type of notification the customer shall receive.
        ///  One of:
        ///  <ul>
        ///  <li>`payment_created`</li>
        ///  <li>`payment_cancelled`</li>
        ///  <li>`mandate_created`</li>
        ///  <li>`mandate_blocked`</li>
        ///  <li>`subscription_created`</li>
        ///  <li>`subscription_cancelled`</li>
        ///  <li>`instalment_schedule_created`</li>
        ///  <li>`instalment_schedule_cancelled`</li>
        ///  </ul>
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    /// <summary>
    ///  The action that was taken on the notification. Currently this can only be `handled`,
    ///  which means the integrator sent the notification themselves.
    ///
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CustomerNotificationActionTaken
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`action_taken` with a value of "handled"</summary>
        [EnumMember(Value = "handled")]
        Handled,
    }

    /// <summary>
    ///  Resources linked to this CustomerNotification
    /// </summary>
    public class CustomerNotificationLinks
    {
        /// <summary>
        ///  The customer who should be contacted with this notification.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        ///  The event that triggered the notification to be scheduled.
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; }

        /// <summary>
        ///  The identifier of the related mandate.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        ///  The identifier of the related payment.
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }

        /// <summary>
        ///  The identifier of the related refund.
        /// </summary>
        [JsonProperty("refund")]
        public string Refund { get; set; }

        /// <summary>
        ///  The identifier of the related subscription.
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

    /// <summary>
    ///  The type of notification the customer shall receive.
    ///  One of:
    ///  <ul>
    ///  <li>`payment_created`</li>
    ///  <li>`payment_cancelled`</li>
    ///  <li>`mandate_created`</li>
    ///  <li>`mandate_blocked`</li>
    ///  <li>`subscription_created`</li>
    ///  <li>`subscription_cancelled`</li>
    ///  <li>`instalment_schedule_created`</li>
    ///  <li>`instalment_schedule_cancelled`</li>
    ///  </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CustomerNotificationType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`type` with a value of "payment_created"</summary>
        [EnumMember(Value = "payment_created")]
        PaymentCreated,

        /// <summary>`type` with a value of "payment_cancelled"</summary>
        [EnumMember(Value = "payment_cancelled")]
        PaymentCancelled,

        /// <summary>`type` with a value of "mandate_created"</summary>
        [EnumMember(Value = "mandate_created")]
        MandateCreated,

        /// <summary>`type` with a value of "mandate_blocked"</summary>
        [EnumMember(Value = "mandate_blocked")]
        MandateBlocked,

        /// <summary>`type` with a value of "subscription_created"</summary>
        [EnumMember(Value = "subscription_created")]
        SubscriptionCreated,

        /// <summary>`type` with a value of "subscription_cancelled"</summary>
        [EnumMember(Value = "subscription_cancelled")]
        SubscriptionCancelled,

        /// <summary>`type` with a value of "instalment_schedule_created"</summary>
        [EnumMember(Value = "instalment_schedule_created")]
        InstalmentScheduleCreated,

        /// <summary>`type` with a value of "instalment_schedule_cancelled"</summary>
        [EnumMember(Value = "instalment_schedule_cancelled")]
        InstalmentScheduleCancelled,
    }
}
