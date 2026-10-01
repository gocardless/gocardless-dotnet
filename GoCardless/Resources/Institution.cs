using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a institution resource.
    ///
    /// Institutions that are supported when creating <a
    /// href="https://developer.gocardless.com/api-reference/#billing-requests-bank-authorisations">Bank
    /// Authorisations</a> for a particular country or purpose.
    ///
    /// Not all institutions support both Payment Initiation (PIS) and Account
    /// Information (AIS) services.
    /// </summary>
    public class Institution
    {
        /// <summary>
        /// <a
        /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
        /// 3166-1</a> alpha-2 code. The country code of the institution. If
        /// nothing is provided, institutions with the country code 'GB' are
        /// returned by default.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

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
        /// Defines individual limits for business and personal accounts.
        /// </summary>
        [JsonProperty("limits")]
        public InstitutionLimits Limits { get; set; }

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

        /// <summary>
        /// The roles assigned to this institution, representing the open
        /// banking features it supports.
        /// </summary>
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }

    /// <summary>
    /// Represents a institution limit resource.
    ///
    /// Defines individual limits for business and personal accounts.
    /// </summary>
    public class InstitutionLimits
    {
        /// <summary>
        /// Daily limit details for this institution, in the lowest denomination
        /// for the currency (e.g. pence in GBP, cents in EUR). The 'limits'
        /// property is only available via an authenticated request with a
        /// generated access token
        /// </summary>
        [JsonProperty("daily")]
        public IDictionary<string, string> Daily { get; set; }

        /// <summary>
        /// Single transaction limit details for this institution, in the lowest
        /// denomination for the currency (e.g. pence in GBP, cents in EUR). The
        /// 'limits' property is only available via an authenticated request
        /// with a generated access token
        /// </summary>
        [JsonProperty("single")]
        public IDictionary<string, string> Single { get; set; }
    }
}
