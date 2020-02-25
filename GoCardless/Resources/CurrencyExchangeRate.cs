using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a currency exchange rate resource.
    ///
    /// Currency exchange rates from our foreign exchange provider.
    /// </summary>
    public class CurrencyExchangeRate
    {
        /// <summary>
        /// The exchange rate from the source to target currencies provided with
        /// up to 10 decimal places.
        /// </summary>
        [JsonProperty("rate")]
        public string Rate { get; set; }

        /// <summary>
        /// Source currency
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Target currency
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }

        /// <summary>
        /// Time at which the rate was retrieved from the provider.
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }
    }
    
}
