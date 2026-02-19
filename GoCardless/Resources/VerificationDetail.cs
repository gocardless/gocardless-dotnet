using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a verification detail resource.
    ///
    ///  Verification details represent any information needed by GoCardless to
    ///  verify a creditor.
    ///
    ///  <p class="restricted-notice"><strong>Restricted</strong>:
    ///    These endpoints are restricted to customers who want to collect their
    ///  merchant's
    ///    verification details and pass them to GoCardless via our API. Please
    ///  [get in
    ///    touch](mailto:help@gocardless.com) if you wish to enable this feature
    ///  on your
    ///    account.</p>
    /// </summary>
    public class VerificationDetail
    {
        /// <summary>
        ///  The first line of the company's address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        ///  The second line of the company's address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        ///  The third line of the company's address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        ///  The city of the company's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        ///  The company's registration number.
        /// </summary>
        [JsonProperty("company_number")]
        public string CompanyNumber { get; set; }

        /// <summary>
        ///  A summary describing what the company does.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///  The company's directors.
        /// </summary>
        [JsonProperty("directors")]
        public List<VerificationDetailDirector> Directors { get; set; }

        /// <summary>
        ///  Resources linked to this VerificationDetail.
        /// </summary>
        [JsonProperty("links")]
        public VerificationDetailLinks Links { get; set; }

        /// <summary>
        ///  The company's legal name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  The company's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
    }

    /// <summary>
    ///  Represents a verification detail director resource.
    ///
    ///  The company's directors.
    /// </summary>
    public class VerificationDetailDirector
    {
        /// <summary>
        ///  The city of the person's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        ///  [ISO 3166-1 alpha-2
        ///  code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        ///  The person's date of birth.
        /// </summary>
        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        /// <summary>
        ///  The person's family name.
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        ///  The person's given name.
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        ///  The person's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        ///  The street of the person's address.
        /// </summary>
        [JsonProperty("street")]
        public string Street { get; set; }
    }

    /// <summary>
    ///  Resources linked to this VerificationDetail
    /// </summary>
    public class VerificationDetailLinks
    {
        /// <summary>
        ///  ID of the [creditor](#core-endpoints-creditors)
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }
    }
}
