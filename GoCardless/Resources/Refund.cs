using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a refund resource.
    ///
    /// Refund objects represent (partial) refunds of a
    /// [payment](#core-endpoints-payments) back to the
    /// [customer](#core-endpoints-customers).
    /// 
    /// GoCardless will notify you via a [webhook](#appendix-webhooks) whenever
    /// a refund is created, and will update the `amount_refunded` property of
    /// the payment.
    /// </summary>
    public class Refund
    {
        /// <summary>
        /// Amount in pence/cents/öre.
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
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. This is set to the currency of the refund's
        /// [payment](#core-endpoints-payments).
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "RF".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this Refund.
        /// </summary>
        [JsonProperty("links")]
        public RefundLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// An optional refund reference, displayed on your customer's bank
        /// statement. This can be up to 18 characters long for Bacs or BECS
        /// payments, 140 characters for SEPA payments, or 25 characters for
        /// Autogiro payments.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this Refund
    /// </summary>
    public class RefundLinks
    {
        /// <summary>
        /// ID of the [mandate](#core-endpoints-mandates) against which the
        /// refund is being made.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// ID of the [payment](#core-endpoints-payments) against which the
        /// refund is being made.
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }
    }
    
}
