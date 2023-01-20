using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a creditor resource.
    ///
    /// Each [payment](#core-endpoints-payments) taken through the API is linked
    /// to a "creditor", to whom the payment is then paid out. In most cases
    /// your organisation will have a single "creditor", but the API also
    /// supports collecting payments on behalf of others.
    /// 
    /// Currently, for Anti Money Laundering reasons, any creditors you add must
    /// be directly related to your organisation.
    /// </summary>
    public class Creditor
    {
        /// <summary>
        /// Boolean value indicating whether the creditor is activated in the
        /// product.
        /// </summary>
        [JsonProperty("activated")]
        public bool? Activated { get; set; }

        /// <summary>
        /// The first line of the creditor's address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the creditor's address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The third line of the creditor's address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Boolean indicating whether the creditor is permitted to create
        /// refunds
        /// </summary>
        [JsonProperty("can_create_refunds")]
        public bool? CanCreateRefunds { get; set; }

        /// <summary>
        /// The city of the creditor's address.
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
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The type of business of the creditor. Currently, `individual`,
        /// `company`, `charity`, `partnership`, and `trust` are supported.
        /// </summary>
        [JsonProperty("creditor_type")]
        public CreditorCreditorType? CreditorType { get; set; }

        /// <summary>
        /// Boolean value indicating whether creditor has the [Custom Payment
        /// Pages](https://hub.gocardless.com/s/article/Custom-payment-pages)
        /// functionality enabled.
        /// </summary>
        [JsonProperty("custom_payment_pages_enabled")]
        public bool? CustomPaymentPagesEnabled { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) code
        /// for the currency in which amounts will be paid out (after foreign
        /// exchange). Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK"
        /// and "USD" are supported. Present only if payouts will be (or were)
        /// made via foreign exchange.
        /// </summary>
        [JsonProperty("fx_payout_currency")]
        public CreditorFxPayoutCurrency? FxPayoutCurrency { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "CR".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this Creditor.
        /// </summary>
        [JsonProperty("links")]
        public CreditorLinks Links { get; set; }

        /// <summary>
        /// URL for the creditor's logo, which may be shown on their payment
        /// pages.
        /// </summary>
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// Boolean value indicating whether creditor has the [Mandate
        /// Imports](#core-endpoints-mandate-imports) functionality enabled.
        /// </summary>
        [JsonProperty("mandate_imports_enabled")]
        public bool? MandateImportsEnabled { get; set; }

        /// <summary>
        /// Boolean value indicating whether the organisation is responsible for
        /// sending all customer notifications (note this is separate from the
        /// functionality described
        /// [here](/getting-started/api/handling-customer-notifications/)). If
        /// you are a partner app, and this value is true, you should not send
        /// notifications on behalf of this organisation.
        /// </summary>
        [JsonProperty("merchant_responsible_for_notifications")]
        public bool? MerchantResponsibleForNotifications { get; set; }

        /// <summary>
        /// The creditor's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The creditor's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The creditor's address region, county or department.
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// An array of the scheme identifiers this creditor can create mandates
        /// against.
        /// 
        /// The support address, `phone_number` and `email` fields are for
        /// customers to contact the merchant for support purposes. They must be
        /// displayed on the payment page, please see our [compliance
        /// requirements](#appendix-compliance-requirements) for more details.
        /// </summary>
        [JsonProperty("scheme_identifiers")]
        public List<CreditorSchemeIdentifier> SchemeIdentifiers { get; set; }

        /// <summary>
        /// The creditor's verification status, indicating whether they can yet
        /// receive payouts. For more details on handling verification as a
        /// partner, see our ["Helping your users get verified"
        /// guide](/getting-started/partners/helping-your-users-get-verified/).
        /// One of:
        /// <ul>
        /// <li>`successful`: The creditor's account is fully verified, and they
        /// can receive payouts. Once a creditor has been successfully verified,
        /// they may in the future require further verification - for example,
        /// if they change their payout bank account, we will have to check that
        /// they own the new bank account before they can receive payouts
        /// again.</li>
        /// <li>`in_review`: The creditor has provided all of the information
        /// currently requested, and it is awaiting review by GoCardless before
        /// they can be verified and receive payouts.</li>
        /// <li>`action_required`: The creditor needs to provide further
        /// information to verify their account so they can receive payouts, and
        /// should visit the verification flow.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("verification_status")]
        public CreditorVerificationStatus? VerificationStatus { get; set; }
    }
    
    /// <summary>
    /// The type of business of the creditor. Currently, `individual`, `company`, `charity`,
    /// `partnership`, and `trust` are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CreditorCreditorType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`creditor_type` with a value of "company"</summary>
        [EnumMember(Value = "company")]
        Company,
        /// <summary>`creditor_type` with a value of "individual"</summary>
        [EnumMember(Value = "individual")]
        Individual,
        /// <summary>`creditor_type` with a value of "charity"</summary>
        [EnumMember(Value = "charity")]
        Charity,
        /// <summary>`creditor_type` with a value of "partnership"</summary>
        [EnumMember(Value = "partnership")]
        Partnership,
        /// <summary>`creditor_type` with a value of "trust"</summary>
        [EnumMember(Value = "trust")]
        Trust,
    }

    /// <summary>
    /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes) code for the currency in
    /// which amounts will be paid out (after foreign exchange). Currently "AUD", "CAD", "DKK",
    /// "EUR", "GBP", "NZD", "SEK" and "USD" are supported. Present only if payouts will be (or
    /// were) made via foreign exchange.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CreditorFxPayoutCurrency {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`fx_payout_currency` with a value of "AUD"</summary>
        [EnumMember(Value = "AUD")]
        AUD,
        /// <summary>`fx_payout_currency` with a value of "CAD"</summary>
        [EnumMember(Value = "CAD")]
        CAD,
        /// <summary>`fx_payout_currency` with a value of "DKK"</summary>
        [EnumMember(Value = "DKK")]
        DKK,
        /// <summary>`fx_payout_currency` with a value of "EUR"</summary>
        [EnumMember(Value = "EUR")]
        EUR,
        /// <summary>`fx_payout_currency` with a value of "GBP"</summary>
        [EnumMember(Value = "GBP")]
        GBP,
        /// <summary>`fx_payout_currency` with a value of "NZD"</summary>
        [EnumMember(Value = "NZD")]
        NZD,
        /// <summary>`fx_payout_currency` with a value of "SEK"</summary>
        [EnumMember(Value = "SEK")]
        SEK,
        /// <summary>`fx_payout_currency` with a value of "USD"</summary>
        [EnumMember(Value = "USD")]
        USD,
    }

    /// <summary>
    /// Resources linked to this Creditor
    /// </summary>
    public class CreditorLinks
    {
        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in AUD.
        /// </summary>
        [JsonProperty("default_aud_payout_account")]
        public string DefaultAudPayoutAccount { get; set; }

        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in CAD.
        /// </summary>
        [JsonProperty("default_cad_payout_account")]
        public string DefaultCadPayoutAccount { get; set; }

        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in DKK.
        /// </summary>
        [JsonProperty("default_dkk_payout_account")]
        public string DefaultDkkPayoutAccount { get; set; }

        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in EUR.
        /// </summary>
        [JsonProperty("default_eur_payout_account")]
        public string DefaultEurPayoutAccount { get; set; }

        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in GBP.
        /// </summary>
        [JsonProperty("default_gbp_payout_account")]
        public string DefaultGbpPayoutAccount { get; set; }

        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in NZD.
        /// </summary>
        [JsonProperty("default_nzd_payout_account")]
        public string DefaultNzdPayoutAccount { get; set; }

        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in SEK.
        /// </summary>
        [JsonProperty("default_sek_payout_account")]
        public string DefaultSekPayoutAccount { get; set; }

        /// <summary>
        /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
        /// which is set up to receive payouts in USD.
        /// </summary>
        [JsonProperty("default_usd_payout_account")]
        public string DefaultUsdPayoutAccount { get; set; }
    }
    
    /// <summary>
    /// An array of the scheme identifiers this creditor can create mandates
    /// against.
    /// 
    /// The support address, `phone_number` and `email` fields are for customers
    /// to contact the merchant for support purposes. They must be displayed on
    /// the payment page, please see our [compliance
    /// requirements](#appendix-compliance-requirements) for more details.
    /// </summary>
    public class CreditorSchemeIdentifier
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
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The currency of the scheme identifier.
        /// </summary>
        [JsonProperty("currency")]
        public CreditorSchemeIdentifierCurrency? Currency { get; set; }

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
        /// The name which appears on customers' bank statements.
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
        public CreditorSchemeIdentifierScheme? Scheme { get; set; }

        /// <summary>
        /// The status of the scheme identifier. Only `active` scheme
        /// identifiers will be applied to a creditor and used against payments.
        /// </summary>
        [JsonProperty("status")]
        public CreditorSchemeIdentifierStatus? Status { get; set; }
    }
    
    /// <summary>
    /// The currency of the scheme identifier.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CreditorSchemeIdentifierCurrency {
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
    public enum CreditorSchemeIdentifierScheme {
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
    public enum CreditorSchemeIdentifierStatus {
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

    /// <summary>
    /// The creditor's verification status, indicating whether they can yet receive payouts. For
    /// more details on handling verification as a partner, see our ["Helping your users get
    /// verified" guide](/getting-started/partners/helping-your-users-get-verified/). One of:
    /// <ul>
    /// <li>`successful`: The creditor's account is fully verified, and they can receive payouts.
    /// Once a creditor has been successfully verified, they may in the future require further
    /// verification - for example, if they change their payout bank account, we will have to check
    /// that they own the new bank account before they can receive payouts again.</li>
    /// <li>`in_review`: The creditor has provided all of the information currently requested, and
    /// it is awaiting review by GoCardless before they can be verified and receive payouts.</li>
    /// <li>`action_required`: The creditor needs to provide further information to verify their
    /// account so they can receive payouts, and should visit the verification flow.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum CreditorVerificationStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`verification_status` with a value of "successful"</summary>
        [EnumMember(Value = "successful")]
        Successful,
        /// <summary>`verification_status` with a value of "in_review"</summary>
        [EnumMember(Value = "in_review")]
        InReview,
        /// <summary>`verification_status` with a value of "action_required"</summary>
        [EnumMember(Value = "action_required")]
        ActionRequired,
    }

}
