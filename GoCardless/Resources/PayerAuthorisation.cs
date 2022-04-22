using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a payer authorisation resource.
    ///
    /// <p class="restricted-notice">
    ///   Don't use Payer Authorisations for new integrations.
    ///   It is deprecated in favour of 
    ///   <a
    /// href="https://developer.gocardless.com/getting-started/billing-requests/overview/">
    ///   Billing Requests</a>. Use Billing Requests to build any future
    /// integrations.
    /// </p>
    /// 
    /// Payer Authorisation resource acts as a wrapper for creating customer,
    /// bank account and mandate details in a single request.
    /// PayerAuthorisation API enables the integrators to build their own custom
    /// payment pages.
    /// 
    /// The process to use the Payer Authorisation API is as follows:
    /// 
    ///   1. Create a Payer Authorisation, either empty or with already
    /// available information
    ///   2. Update the authorisation with additional information or fix any
    /// mistakes
    ///   3. Submit the authorisation, after the payer has reviewed their
    /// information
    ///   4. [coming soon] Redirect the payer to the verification mechanisms
    /// from the response of the Submit request (this will be introduced as a
    /// non-breaking change)
    ///   5. Confirm the authorisation to indicate that the resources can be
    /// created
    /// 
    /// After the Payer Authorisation is confirmed, resources will eventually be
    /// created as it's an asynchronous process.
    /// 
    /// To retrieve the status and ID of the linked resources you can do one of
    /// the following:
    /// <ol>
    ///   <li> Listen to <code>  payer_authorisation_completed </code>  <a
    /// href="#appendix-webhooks"> webhook</a> (recommended)</li>
    ///   <li> Poll the GET <a
    /// href="#payer-authorisations-get-a-single-payer-authorisation">
    /// endpoint</a></li>
    ///   <li> Poll the GET events API
    /// <code>https://api.gocardless.com/events?payer_authorisation={id}&action=completed</code>
    /// </li>
    /// </ol>
    /// 
    /// <p class="notice">
    ///   Note that the `create` and `update` endpoints behave differently than
    ///   other existing `create` and `update` endpoints. The Payer
    /// Authorisation is still saved if incomplete data is provided.
    ///   We return the list of incomplete data in the `incomplete_fields` along
    /// with the resources in the body of the response.
    ///   The bank account details(account_number, bank_code & branch_code) must
    /// be sent together rather than splitting across different request for both
    /// `create` and `update` endpoints.
    ///   <br><br>
    ///   The API is designed to be flexible and allows you to collect
    /// information in multiple steps without storing any sensitive data in the
    /// browser or in your servers.
    /// </p>
    /// </summary>
    public class PayerAuthorisation
    {
        /// <summary>
        /// All details required for the creation of a
        /// [Customer Bank Account](#core-endpoints-customer-bank-accounts).
        /// </summary>
        [JsonProperty("bank_account")]
        public PayerAuthorisationBankAccount BankAccount { get; set; }

        /// <summary>
        /// [Timestamp](#api-usage-time-zones--dates), recording when this Payer
        /// Authorisation was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// All details required for the creation of a
        /// [Customer](#core-endpoints-customers).
        /// </summary>
        [JsonProperty("customer")]
        public PayerAuthorisationCustomer Customer { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PA".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// An array of fields which are missing and is required to set up the
        /// mandate.
        /// </summary>
        [JsonProperty("incomplete_fields")]
        public List<PayerAuthorisationIncompleteField> IncompleteFields { get; set; }

        /// <summary>
        /// Resources linked to this PayerAuthorisation.
        /// </summary>
        [JsonProperty("links")]
        public PayerAuthorisationLinks Links { get; set; }

        /// <summary>
        /// All details required for the creation of a
        /// [Mandate](#core-endpoints-mandates).
        /// </summary>
        [JsonProperty("mandate")]
        public PayerAuthorisationMandate Mandate { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`created`: The PayerAuthorisation has been created, and not been
        /// confirmed yet</li>
        /// <li>`submitted`: The payer information has been submitted</li>
        /// <li>`confirmed`: PayerAuthorisation is confirmed and resources are
        /// ready to be created</li>
        /// <li>`completed`: The PayerAuthorisation has been completed and
        /// customer, bank_account and mandate has been created</li>
        /// <li>`failed`: The PayerAuthorisation has failed and customer,
        /// bank_account and mandate is not created</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public PayerAuthorisationStatus? Status { get; set; }
    }
    
    /// <summary>
    /// Represents a payer authorisation bank account resource.
    ///
    /// All details required for the creation of a
    /// [Customer Bank Account](#core-endpoints-customer-bank-accounts).
    /// </summary>
    public class PayerAuthorisationBankAccount
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this is
        /// the same as the name stored with the linked
        /// [creditor](#core-endpoints-creditors). This field will be
        /// transliterated, upcased and truncated to 18 characters. This field
        /// is required unless the request includes a [customer bank account
        /// token](#javascript-flow-customer-bank-account-tokens).
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Bank account number - see [local
        /// details](#appendix-local-bank-details) for more information.
        /// Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// The last few digits of the account number. Currently 4 digits for
        /// NZD bank accounts and 2 digits for other currencies.
        /// </summary>
        [JsonProperty("account_number_ending")]
        public string AccountNumberEnding { get; set; }

        /// <summary>
        /// Account number suffix (only for bank accounts denominated in NZD) -
        /// see [local details](#local-bank-details-new-zealand) for more
        /// information.
        /// </summary>
        [JsonProperty("account_number_suffix")]
        public string AccountNumberSuffix { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public PayerAuthorisationBankAccountAccountType? AccountType { get; set; }

        /// <summary>
        /// Bank code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Branch code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements).
        /// Defaults to the country code of the `iban` if supplied, otherwise is
        /// required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide
        /// [local details](#appendix-local-bank-details). IBANs are not
        /// accepted for Swedish bank accounts denominated in SEK - you must
        /// supply [local details](#local-bank-details-sweden).
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }
    
    /// <summary>
    /// Bank account type. Required for USD-denominated bank accounts. Must not be provided for bank
    /// accounts in other currencies. See [local details](#local-bank-details-united-states) for
    /// more information.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayerAuthorisationBankAccountAccountType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`account_type` with a value of "savings"</summary>
        [EnumMember(Value = "savings")]
        Savings,
        /// <summary>`account_type` with a value of "checking"</summary>
        [EnumMember(Value = "checking")]
        Checking,
    }

    /// <summary>
    /// Represents a payer authorisation customer resource.
    ///
    /// All details required for the creation of a
    /// [Customer](#core-endpoints-customers).
    /// </summary>
    public class PayerAuthorisationCustomer
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
        /// [ISO 3166-1 alpha-2
        /// code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

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
        /// An [IETF Language Tag](https://tools.ietf.org/html/rfc5646), used
        /// for both language
        /// and regional variations of our product.
        /// 
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// The customer's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The customer's address region, county or department. For US
        /// customers a 2 letter
        /// [ISO3166-2:US](https://en.wikipedia.org/wiki/ISO_3166-2:US) state
        /// code is required (e.g. `CA` for California).
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
    
    /// <summary>
    /// An array of fields which are missing and is required to set up the
    /// mandate.
    /// </summary>
    public class PayerAuthorisationIncompleteField
    {
        /// <summary>
        /// The root resource.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// A localised error message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// The path to the field e.g. "/payer_authorisations/customer/city"
        /// </summary>
        [JsonProperty("request_pointer")]
        public string RequestPointer { get; set; }
    }
    
    /// <summary>
    /// Represents a payer authorisation link resource.
    ///
    /// IDs of the created resources. Available after the Payer Authorisation is
    /// completed.
    /// </summary>
    public class PayerAuthorisationLinks
    {
        /// <summary>
        /// Unique identifier, beginning with "BA".
        /// </summary>
        [JsonProperty("bank_account")]
        public string BankAccount { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "MD". Note that this prefix may
        /// not apply to mandates created before 2016.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }
    }
    
    /// <summary>
    /// Represents a payer authorisation mandate resource.
    ///
    /// All details required for the creation of a
    /// [Mandate](#core-endpoints-mandates).
    /// </summary>
    public class PayerAuthorisationMandate
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// For ACH customers only. Required for ACH customers. A string
        /// containing the IP address of the payer to whom the mandate belongs
        /// (i.e. as a result of their completion of a mandate setup flow in
        /// their browser).
        /// </summary>
        [JsonProperty("payer_ip_address")]
        public string PayerIpAddress { get; set; }

        /// <summary>
        /// Unique reference. Different schemes have different length and
        /// [character set](#appendix-character-sets) requirements. GoCardless
        /// will generate a unique reference satisfying the different scheme
        /// requirements if this field is left blank.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// A Direct Debit scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "pad" and "sepa_core" are supported.
        /// </summary>
        [JsonProperty("scheme")]
        public PayerAuthorisationMandateScheme? Scheme { get; set; }
    }
    
    /// <summary>
    /// A Direct Debit scheme. Currently "ach", "autogiro", "bacs", "becs", "becs_nz",
    /// "betalingsservice", "pad" and "sepa_core" are supported.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayerAuthorisationMandateScheme {
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
        /// <summary>`scheme` with a value of "pad"</summary>
        [EnumMember(Value = "pad")]
        Pad,
        /// <summary>`scheme` with a value of "sepa_core"</summary>
        [EnumMember(Value = "sepa_core")]
        SepaCore,
    }

    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`created`: The PayerAuthorisation has been created, and not been confirmed yet</li>
    /// <li>`submitted`: The payer information has been submitted</li>
    /// <li>`confirmed`: PayerAuthorisation is confirmed and resources are ready to be created</li>
    /// <li>`completed`: The PayerAuthorisation has been completed and customer, bank_account and
    /// mandate has been created</li>
    /// <li>`failed`: The PayerAuthorisation has failed and customer, bank_account and mandate is
    /// not created</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum PayerAuthorisationStatus {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "created"</summary>
        [EnumMember(Value = "created")]
        Created,
        /// <summary>`status` with a value of "submitted"</summary>
        [EnumMember(Value = "submitted")]
        Submitted,
        /// <summary>`status` with a value of "confirmed"</summary>
        [EnumMember(Value = "confirmed")]
        Confirmed,
        /// <summary>`status` with a value of "completed"</summary>
        [EnumMember(Value = "completed")]
        Completed,
        /// <summary>`status` with a value of "failed"</summary>
        [EnumMember(Value = "failed")]
        Failed,
    }

}
