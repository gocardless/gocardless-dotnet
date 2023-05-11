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
    /// Authorisations](#billing-requests-bank-authorisations) for a particular
    /// country or purpose.
    /// 
    /// Not all institutions support both Payment Initiation (PIS) and Account
    /// Information (AIS) services.
    /// </summary>
    public class Institution
    {
        /// <summary>
        /// Flag to show if selecting this institution in the select_institution
        /// action can auto-complete the collect_bank_account action. The bank
        /// can return the payer's bank account details to GoCardless.
        /// </summary>
        [JsonProperty("autocompletes_collect_bank_account")]
        public bool? AutocompletesCollectBankAccount { get; set; }

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. The country code of the institution.
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
