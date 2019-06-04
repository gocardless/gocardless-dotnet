using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a customer resource.
    ///
    /// Customer objects hold the contact details for a customer. A customer can
    /// have several [customer bank
    /// accounts](#core-endpoints-customer-bank-accounts), which in turn can
    /// have several Direct Debit [mandates](#core-endpoints-mandates).
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The first line of the customer's address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the customer's address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The third line of the customer's address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// The city of the customer's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// Customer's company name. Required unless a `given_name` and
        /// `family_name` are provided. For Canadian customers, the use of a
        /// `company_name` value will mean that any mandate created from this
        /// customer will be considered to be a "Business PAD" (otherwise, any
        /// mandate will be considered to be a "Personal PAD").
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// For Danish customers only. The civic/company number (CPR or CVR) of
        /// the customer. Must be supplied if the customer's bank account is
        /// denominated in Danish krone (DKK).
        /// </summary>
        [JsonProperty("danish_identity_number")]
        public string DanishIdentityNumber { get; set; }

        /// <summary>
        /// Customer's email address. Required in most cases, as this allows
        /// GoCardless to send notifications to this customer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Customer's surname. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Customer's first name. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// [ISO 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
        /// code. Used as the language for notification emails sent by
        /// GoCardless if your organisation does not send its own (see
        /// [compliance requirements](#appendix-compliance-requirements)).
        /// Currently only "en", "fr", "de", "pt", "es", "it", "nl", "da", "nb",
        /// "sl", "sv" are supported. If this is not provided, the language will
        /// be chosen based on the `country_code` (if supplied) or default to
        /// "en".
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// [ITU E.123](https://en.wikipedia.org/wiki/E.123) formatted phone
        /// number, including country code. Required for New Zealand customers
        /// only. Must be supplied if the customer's bank account is denominated
        /// in New Zealand Dollars (NZD).
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The customer's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The customer's address region, county or department.
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// For Swedish customers only. The civic/company number (personnummer,
        /// samordningsnummer, or organisationsnummer) of the customer. Must be
        /// supplied if the customer's bank account is denominated in Swedish
        /// krona (SEK). This field cannot be changed once it has been set.
        /// </summary>
        [JsonProperty("swedish_identity_number")]
        public string SwedishIdentityNumber { get; set; }
    }
    
}
