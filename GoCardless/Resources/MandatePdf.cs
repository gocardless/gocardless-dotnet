using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a mandate pdf resource.
    ///
    ///  Mandate PDFs allow you to easily display [scheme-rules
    ///  compliant](#appendix-compliance-requirements) Direct Debit mandates to
    ///  your customers.
    /// </summary>
    public class MandatePdf
    {
        /// <summary>
        ///  The date and time at which the `url` will expire (10 minutes after
        ///  the original request).
        /// </summary>
        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        /// <summary>
        ///  The URL at which this mandate PDF can be viewed until it expires at
        ///  the date and time specified by `expires_at`. You should not store
        ///  this URL or rely on its structure remaining the same.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
