using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a instalment schedule resource.
    ///
    /// Instalment schedules are objects which represent a collection of related
    /// payments, with the
    /// intention to collect the `total_amount` specified. The API supports both
    /// schedule-based
    /// creation (similar to subscriptions) as well as explicit selection of
    /// differing payment
    /// amounts and charge dates.
    /// 
    /// Unlike subscriptions, the payments are created immediately, so the
    /// instalment schedule
    /// cannot be modified once submitted and instead can only be cancelled
    /// (which will cancel
    /// any of the payments which have not yet been submitted).
    /// 
    /// Customers will receive a single notification about the complete schedule
    /// of collection.
    /// 
    /// </summary>
    public class InstalmentSchedule
    {
        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public InstalmentScheduleCurrency? Currency { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "IS".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this InstalmentSchedule.
        /// </summary>
        [JsonProperty("links")]
        public InstalmentScheduleLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Name of the instalment schedule, up to 100 chars. This name will
        /// also be
        /// copied to the payments of the instalment schedule if you use
        /// schedule-based creation.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// If the status is `creation_failed`, this property will be populated
        /// with validation
        /// failures from the individual payments, arranged by the index of the
        /// payment that
        /// failed.
        /// 
        /// </summary>
        [JsonProperty("payment_errors")]
        public IDictionary<string, string> PaymentErrors { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: we're waiting for GC to create the payments</li>
        /// <li>`active`: the payments have been created, and the schedule is
        /// active</li>
        /// <li>`creation_failed`: payment creation failed</li>
        /// <li>`completed`: we have passed the date of the final payment and
        /// all payments have been collected</li>
        /// <li>`cancelled`: the schedule has been cancelled</li>
        /// <li>`errored`: one or more payments have failed</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public InstalmentScheduleStatus? Status { get; set; }

        /// <summary>
        /// The total amount of the instalment schedule, defined as the sum of
        /// all individual
        /// payments, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in
        /// EUR). If the requested payment amounts do not sum up correctly, a
        /// validation error
        /// will be returned.
        /// </summary>
        [JsonProperty("total_amount")]
        public int? TotalAmount { get; set; }
    }
    
    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency code. Currently
    /// "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD" are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum InstalmentScheduleCurrency {
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
    /// Represents a instalment schedule link resource.
    ///
    /// Links to associated objects
    /// </summary>
    public class InstalmentScheduleLinks
    {
        /// <summary>
        /// ID of the associated [customer](#core-endpoints-customers).
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// ID of the associated [mandate](#core-endpoints-mandates) which the
        /// instalment schedule will create payments against.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// Array of IDs of the associated [payments](#core-endpoints-payments)
        /// </summary>
        [JsonProperty("payments")]
        public List<string> Payments { get; set; }
    }
    
    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`pending`: we're waiting for GC to create the payments</li>
    /// <li>`active`: the payments have been created, and the schedule is active</li>
    /// <li>`creation_failed`: payment creation failed</li>
    /// <li>`completed`: we have passed the date of the final payment and all payments have been
    /// collected</li>
    /// <li>`cancelled`: the schedule has been cancelled</li>
    /// <li>`errored`: one or more payments have failed</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum InstalmentScheduleStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,
        /// <summary>`status` with a value of "active"</summary>
        [EnumMember(Value = "active")]
        Active,
        /// <summary>`status` with a value of "creation_failed"</summary>
        [EnumMember(Value = "creation_failed")]
        CreationFailed,
        /// <summary>`status` with a value of "completed"</summary>
        [EnumMember(Value = "completed")]
        Completed,
        /// <summary>`status` with a value of "cancelled"</summary>
        [EnumMember(Value = "cancelled")]
        Cancelled,
        /// <summary>`status` with a value of "errored"</summary>
        [EnumMember(Value = "errored")]
        Errored,
    }

}
