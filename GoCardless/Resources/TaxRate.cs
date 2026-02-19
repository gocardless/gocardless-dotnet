using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a tax rate resource.
    ///
    ///  Tax rates from tax authority.
    ///
    ///  We also maintain a [static list of the tax rates for each
    ///  jurisdiction](#appendix-tax-rates).
    /// </summary>
    public class TaxRate
    {
        /// <summary>
        ///  Date at which GoCardless stopped applying the tax rate for the
        ///  jurisdiction.
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        /// <summary>
        ///  The unique identifier created by the jurisdiction, tax type and
        ///  version
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  The jurisdiction this tax rate applies to
        /// </summary>
        [JsonProperty("jurisdiction")]
        public string Jurisdiction { get; set; }

        /// <summary>
        ///  The percentage of tax that is applied onto of GoCardless fees
        /// </summary>
        [JsonProperty("percentage")]
        public string Percentage { get; set; }

        /// <summary>
        ///  Date at which GoCardless started applying the tax rate in the
        ///  jurisdiction.
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        ///  The type of tax applied by this rate
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
