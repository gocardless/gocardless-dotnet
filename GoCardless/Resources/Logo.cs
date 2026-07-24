using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a logo resource.
    ///
    /// Logos are image uploads that, when associated with a creditor, are shown
    /// on the <a
    /// href="https://developer.gocardless.com/api-reference/#billing-requests-billing-request-flows">billing
    /// request flow</a> payment pages.
    /// </summary>
    public class Logo
    {
        /// <summary>
        /// Unique identifier, beginning with "LO".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
