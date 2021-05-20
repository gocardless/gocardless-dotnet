using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a institution resource.
    ///
    /// Institutions that are supported when creating [Bank
    /// Authorisations](#billing-requests-bank-authorisations).
    /// </summary>
    public class Institution
    {
        /// <summary>
        /// A URL pointing to the icon for this institution
        /// </summary>
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        /// <summary>
        /// The unique identifier for this institution
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// A URL pointing to the logo for this institution
        /// </summary>
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// A human readable name for this institution
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    
}
