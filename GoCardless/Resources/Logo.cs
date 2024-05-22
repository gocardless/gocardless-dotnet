using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a logo resource.
    ///
    /// Logos are image uploads that, when associated with a creditor, are shown
    /// on the [billing request flow](#billing-requests-billing-request-flows)
    /// payment pages.
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
