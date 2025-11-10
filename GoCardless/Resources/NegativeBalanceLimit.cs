using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a negative balance limit resource.
    ///
    /// The negative balance limit is a threshold for the creditor balance
    /// beyond which refunds are not permitted. The default limit is zero —
    /// refunds are not permitted if the creditor has a negative balance. The
    /// limit can be changed on a per-creditor basis.
    ///
    /// </summary>
    public class NegativeBalanceLimit
    {
        /// <summary>
        /// The limit amount in pence (e.g. 10000 for a -100 GBP limit).
        /// </summary>
        [JsonProperty("balance_limit")]
        public int? BalanceLimit { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// limit was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public NegativeBalanceLimitCurrency? Currency { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "NBL".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this NegativeBalanceLimit.
        /// </summary>
        [JsonProperty("links")]
        public NegativeBalanceLimitLinks Links { get; set; }
    }

    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) currency code. Currently
    /// "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD" are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum NegativeBalanceLimitCurrency
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
    /// Resources linked to this NegativeBalanceLimit
    /// </summary>
    public class NegativeBalanceLimitLinks
    {
        /// <summary>
        /// ID of the creator_user who created this limit
        /// </summary>
        [JsonProperty("creator_user")]
        public string CreatorUser { get; set; }

        /// <summary>
        /// ID of [creditor](#core-endpoints-creditors) which this limit relates
        /// to
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }
    }
}
