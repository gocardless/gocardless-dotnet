using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a payer theme resource.
    ///
    /// Custom colour themes for payment pages and customer notifications.
    /// </summary>
    public class PayerTheme
    {
        /// <summary>
        /// Unique identifier, beginning with "PTH".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    
}
