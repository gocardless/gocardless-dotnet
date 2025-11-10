using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a schemeentifier resource.
    ///
    /// This represents a scheme identifier (e.g. a SUN in Bacs or a CID in
    /// SEPA). Scheme identifiers are used to specify the beneficiary name that
    /// appears on customers' bank statements.
    ///
    /// </summary>
    public class SchemeIdentifier
    {
        /// <summary>
        /// The first line of the scheme identifier's support address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the scheme identifier's support address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The third line of the scheme identifier's support address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Whether a custom reference can be submitted for mandates using this
        /// scheme identifier.
        /// </summary>
        [JsonProperty("can_specify_mandate_reference")]
        public bool? CanSpecifyMandateReference { get; set; }

        /// <summary>
        /// The city of the scheme identifier's support address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The currency of the scheme identifier.
        /// </summary>
        [JsonProperty("currency")]
        public SchemeIdentifierCurrency? Currency { get; set; }

        /// <summary>
        /// Scheme identifier's support email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Unique identifier, usually beginning with "SU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The minimum interval, in working days, between the sending of a
        /// pre-notification to the customer, and the charge date of a payment
        /// using this scheme identifier.
        ///
        /// By default, GoCardless sends these notifications automatically.
        /// Please see our [compliance
        /// requirements](#appendix-compliance-requirements) for more details.
        /// </summary>
        [JsonProperty("minimum_advance_notice")]
        public int? MinimumAdvanceNotice { get; set; }

        /// <summary>
        /// The name which appears on customers' bank statements. This should
        /// usually be the merchant's trading name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Scheme identifier's support phone number.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The scheme identifier's support postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The scheme-unique identifier against which payments are submitted.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// The scheme identifier's support address region, county or
        /// department.
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// The scheme which this scheme identifier applies to.
        /// </summary>
        [JsonProperty("scheme")]
        public SchemeIdentifierScheme? Scheme { get; set; }

        /// <summary>
        /// The status of the scheme identifier. Only `active` scheme
        /// identifiers will be applied to a creditor and used against payments.
        /// </summary>
        [JsonProperty("status")]
        public SchemeIdentifierStatus? Status { get; set; }
    }

    /// <summary>
    /// The currency of the scheme identifier.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum SchemeIdentifierCurrency
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`currency` with a value of "AUD"</summary>
        [EnumMember(Value = "AUD")]
        AUD,

        /// <summary>`currency` with a value of "CAD"</summary>
        [EnumMember(Value = "CAD")]
        CAD,

        /// <summary>`currency` with a value of "DKK"</summary>
        [EnumMember(Value = "DKK")]
        DKK,

        /// <summary>`currency` with a value of "EUR"</summary>
        [EnumMember(Value = "EUR")]
        EUR,

        /// <summary>`currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,

        /// <summary>`currency` with a value of "NZD"</summary>
        [EnumMember(Value = "NZD")]
        NZD,

        /// <summary>`currency` with a value of "SEK"</summary>
        [EnumMember(Value = "SEK")]
        SEK,

        /// <summary>`currency` with a value of "USD"</summary>
        [EnumMember(Value = "USD")]
        USD,
    }

    /// <summary>
    /// The scheme which this scheme identifier applies to.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum SchemeIdentifierScheme
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`scheme` with a value of "ach"</summary>
        [EnumMember(Value = "ach")]
        Ach,

        /// <summary>`scheme` with a value of "autogiro"</summary>
        [EnumMember(Value = "autogiro")]
        Autogiro,

        /// <summary>`scheme` with a value of "bacs"</summary>
        [EnumMember(Value = "bacs")]
        Bacs,

        /// <summary>`scheme` with a value of "becs"</summary>
        [EnumMember(Value = "becs")]
        Becs,

        /// <summary>`scheme` with a value of "becs_nz"</summary>
        [EnumMember(Value = "becs_nz")]
        BecsNz,

        /// <summary>`scheme` with a value of "betalingsservice"</summary>
        [EnumMember(Value = "betalingsservice")]
        Betalingsservice,

        /// <summary>`scheme` with a value of "faster_payments"</summary>
        [EnumMember(Value = "faster_payments")]
        FasterPayments,

        /// <summary>`scheme` with a value of "pad"</summary>
        [EnumMember(Value = "pad")]
        Pad,

        /// <summary>`scheme` with a value of "pay_to"</summary>
        [EnumMember(Value = "pay_to")]
        PayTo,

        /// <summary>`scheme` with a value of "sepa"</summary>
        [EnumMember(Value = "sepa")]
        Sepa,

        /// <summary>`scheme` with a value of "sepa_credit_transfer"</summary>
        [EnumMember(Value = "sepa_credit_transfer")]
        SepaCreditTransfer,

        /// <summary>`scheme` with a value of "sepa_instant_credit_transfer"</summary>
        [EnumMember(Value = "sepa_instant_credit_transfer")]
        SepaInstantCreditTransfer,
    }

    /// <summary>
    /// The status of the scheme identifier. Only `active` scheme identifiers will be applied to a
    /// creditor and used against payments.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum SchemeIdentifierStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>`status` with a value of "active"</summary>
        [EnumMember(Value = "active")]
        Active,
    }
}
