using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a institution resource.
    ///
    ///  Institutions that are supported when creating [Bank
    ///  Authorisations](#billing-requests-bank-authorisations) for a particular
    ///  country or purpose.
    ///
    ///  Not all institutions support both Payment Initiation (PIS) and Account
    ///  Information (AIS) services.
    /// </summary>
    public class Institution
    {
        /// <summary>
        ///  Flag to show if selecting this institution in the
        ///  select_institution action can auto-complete the
        ///  collect_bank_account action. The bank can return the payer's bank
        ///  account details to GoCardless.
        /// </summary>
        [JsonProperty("autocompletes_collect_bank_account")]
        public bool? AutocompletesCollectBankAccount { get; set; }

        /// <summary>
        ///  [ISO
        ///  3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        ///  alpha-2 code. The country code of the institution. If nothing is
        ///  provided, institutions with the country code 'GB' are returned by
        ///  default.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        ///  A URL pointing to the icon for this institution
        /// </summary>
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        /// <summary>
        ///  The unique identifier for this institution
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  Defines individual limits for business and personal accounts.
        /// </summary>
        [JsonProperty("limits")]
        public InstitutionLimits Limits { get; set; }

        /// <summary>
        ///  A URL pointing to the logo for this institution
        /// </summary>
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }

        /// <summary>
        ///  A human readable name for this institution
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  The status of the institution
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    /// <summary>
    ///  Represents a institution limit resource.
    ///
    ///  Defines individual limits for business and personal accounts.
    /// </summary>
    public class InstitutionLimits
    {
        /// <summary>
        ///  Daily limit details for this institution, in the lowest
        ///  denomination for the currency (e.g. pence in GBP, cents in EUR).
        ///  The 'limits' property is only available via an authenticated
        ///  request with a generated access token
        /// </summary>
        [JsonProperty("daily")]
        public IDictionary<string, string> Daily { get; set; }

        /// <summary>
        ///  Single transaction limit details for this institution, in the
        ///  lowest denomination for the currency (e.g. pence in GBP, cents in
        ///  EUR). The 'limits' property is only available via an authenticated
        ///  request with a generated access token
        /// </summary>
        [JsonProperty("single")]
        public IDictionary<string, string> Single { get; set; }
    }

    /// <summary>
    ///  The status of the institution
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum InstitutionStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "enabled"</summary>
        [EnumMember(Value = "enabled")]
        Enabled,

        /// <summary>`status` with a value of "disabled"</summary>
        [EnumMember(Value = "disabled")]
        Disabled,

        /// <summary>`status` with a value of "temporarily_disabled"</summary>
        [EnumMember(Value = "temporarily_disabled")]
        TemporarilyDisabled,
    }
}
