using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a funds availability resource.
    ///
    /// Checks if the payer's current balance is sufficient to cover the amount
    /// the merchant wants to charge within the consent parameters defined on
    /// the mandate.
    /// </summary>
    public class FundsAvailability
    {
        /// <summary>
        /// Indicates if the amount is available
        /// </summary>
        [JsonProperty("available")]
        public bool? Available { get; set; }
    }
}
