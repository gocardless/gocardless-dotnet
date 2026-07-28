using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GoCardless.Internals;
using GoCardless.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Services
{
    /// <summary>
    /// Service class for working with mandate pdf resources.
    ///
    /// Mandate PDFs allow you to easily display <a
    /// href="https://developer.gocardless.com/api-reference/#appendix-compliance-requirements">scheme-rules
    /// compliant</a> Direct Debit mandates to your customers.
    /// </summary>
    public class MandatePdfService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public MandatePdfService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Generates a PDF mandate and returns its temporary URL.
        ///
        /// Customer and bank account details can be left blank (for a blank
        /// mandate), provided manually, or inferred from the ID of an existing
        /// <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>.
        ///
        /// By default, we'll generate PDF mandates in English.
        ///
        /// To generate a PDF mandate in another language, set the
        /// <c>Accept-Language</c> header when creating the PDF mandate to the
        /// relevant <a
        /// href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO
        /// 639-1</a> language code supported for the scheme.
        ///
        /// | Scheme           | Supported languages
        ///
        ///                         |
        /// | :--------------- |
        /// :-------------------------------------------------------------------------------------------------------------------------------------------
        /// |
        /// | ACH              | English (<c>en</c>)
        ///
        ///                              |
        /// | Autogiro         | English (<c>en</c>), Swedish (<c>sv</c>)
        ///
        ///                                   |
        /// | Bacs             | English (<c>en</c>)
        ///
        ///                              |
        /// | BECS             | English (<c>en</c>)
        ///
        ///                              |
        /// | BECS NZ          | English (<c>en</c>)
        ///
        ///                              |
        /// | Betalingsservice | Danish (<c>da</c>), English (<c>en</c>)
        ///
        ///                                   |
        /// | PAD              | English (<c>en</c>)
        ///
        ///                              |
        /// | SEPA Core        | Danish (<c>da</c>), Dutch (<c>nl</c>), English
        /// (<c>en</c>), French (<c>fr</c>), German (<c>de</c>), Italian
        /// (<c>it</c>), Portuguese (<c>pt</c>), Spanish (<c>es</c>), Swedish
        /// (<c>sv</c>) |
        /// </summary>
        /// <param name="request">An optional `MandatePdfCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single mandate pdf resource</returns>
        public Task<MandatePdfResponse> CreateAsync(
            MandatePdfCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandatePdfCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<MandatePdfResponse>(
                "POST",
                "/mandate_pdfs",
                urlParams,
                request,
                null,
                "mandate_pdfs",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Generates a PDF mandate and returns its temporary URL.
    ///
    /// Customer and bank account details can be left blank (for a blank
    /// mandate), provided manually, or inferred from the ID of an existing <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>.
    ///
    /// By default, we'll generate PDF mandates in English.
    ///
    /// To generate a PDF mandate in another language, set the
    /// <c>Accept-Language</c> header when creating the PDF mandate to the
    /// relevant <a
    /// href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO
    /// 639-1</a> language code supported for the scheme.
    ///
    /// | Scheme           | Supported languages
    ///
    ///                 |
    /// | :--------------- |
    /// :-------------------------------------------------------------------------------------------------------------------------------------------
    /// |
    /// | ACH              | English (<c>en</c>)
    ///
    ///                      |
    /// | Autogiro         | English (<c>en</c>), Swedish (<c>sv</c>)
    ///
    ///                           |
    /// | Bacs             | English (<c>en</c>)
    ///
    ///                      |
    /// | BECS             | English (<c>en</c>)
    ///
    ///                      |
    /// | BECS NZ          | English (<c>en</c>)
    ///
    ///                      |
    /// | Betalingsservice | Danish (<c>da</c>), English (<c>en</c>)
    ///
    ///                           |
    /// | PAD              | English (<c>en</c>)
    ///
    ///                      |
    /// | SEPA Core        | Danish (<c>da</c>), Dutch (<c>nl</c>), English
    /// (<c>en</c>), French (<c>fr</c>), German (<c>de</c>), Italian
    /// (<c>it</c>), Portuguese (<c>pt</c>), Spanish (<c>es</c>), Swedish
    /// (<c>sv</c>) |
    /// </summary>
    public class MandatePdfCreateRequest
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this
        /// matches the name of the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-customers">customer</a>.
        /// This field cannot exceed 18 characters.
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Bank account number - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See <a
        /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
        /// details</a> for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public MandatePdfAccountType? AccountType { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See <a
        /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
        /// details</a> for more information.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MandatePdfAccountType
        {
            /// <summary>`account_type` with a value of "savings"</summary>
            [EnumMember(Value = "savings")]
            Savings,

            /// <summary>`account_type` with a value of "checking"</summary>
            [EnumMember(Value = "checking")]
            Checking,
        }

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
        /// Bank code - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// SWIFT BIC. Will be derived automatically if a valid <c>iban</c> or
        /// <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> are provided.
        /// </summary>
        [JsonProperty("bic")]
        public string Bic { get; set; }

        /// <summary>
        /// Branch code - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// The city of the customer's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The customer's company name. Used to populate the "Customer Name or
        /// Company name" field on the PDF.
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// <a
        /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
        /// 3166-1</a> alpha-2 code. Required if providing local details.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// For Danish customers only. The civic/company number (CPR or CVR) of
        /// the customer. Should only be supplied for Betalingsservice mandates.
        /// </summary>
        [JsonProperty("danish_identity_number")]
        public string DanishIdentityNumber { get; set; }

        /// <summary>
        /// The customer's family name (i.e. last name). Used to populate the
        /// "Customer Name or Company name" field on the PDF. Ignored if
        /// <c>company_name</c> is provided.
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// The customer's given name (i.e. first name). Used to populate the
        /// "Customer Name or Company name" field on the PDF. Ignored if
        /// <c>company_name</c> is provided.
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a>. IBANs cannot be provided for Autogiro mandates.
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public MandatePdfLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a MandatePdf.
        /// </summary>
        public class MandatePdfLinks
        {
            /// <summary>
            /// ID of an existing <a
            /// href="https://developer.gocardless.com/api-reference/#core-endpoints-creditors">creditor</a>.
            /// Only required if your account manages multiple creditors.
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }

            /// <summary>
            /// ID of an existing <a
            /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
            /// to build the PDF from. The customer's bank details will be
            /// censored in the generated PDF. No other parameters may be
            /// provided alongside this.
            /// </summary>
            [JsonProperty("mandate")]
            public string Mandate { get; set; }
        }

        /// <summary>
        /// Unique 6 to 18 character reference. This may be left blank at the
        /// point of signing.
        /// </summary>
        [JsonProperty("mandate_reference")]
        public string MandateReference { get; set; }

        /// <summary>
        /// For American customers only. IP address of the computer used by the
        /// customer to set up the mandate. This is required in order to create
        /// compliant Mandate PDFs according to the ACH scheme rules.
        /// </summary>
        [JsonProperty("payer_ip_address")]
        public string PayerIpAddress { get; set; }

        /// <summary>
        /// The customer phone number. Should only be provided for BECS NZ
        /// mandates.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The customer's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The customer's address region, county or department. For US
        /// customers a 2 letter <a
        /// href="https://en.wikipedia.org/wiki/ISO_3166-2:US">ISO3166-2:US</a>
        /// state code is required (e.g. <c>CA</c> for California).
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// Direct Debit scheme. Can be supplied or automatically detected from
        /// the bank account details provided. If you do not provide a scheme,
        /// you must provide either a mandate, an <c>iban</c>, or <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> including a <c>country_code</c>.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// If provided, a form will be generated with this date and no
        /// signature field.
        /// </summary>
        [JsonProperty("signature_date")]
        public string SignatureDate { get; set; }

        /// <summary>
        /// For American customers only. Subscription amount being authorised by
        /// the mandate. In the lowest denomination for the currency (cents in
        /// USD). Is required if <c>subscription_frequency</c> has been
        /// provided.
        /// </summary>
        [JsonProperty("subscription_amount")]
        public int? SubscriptionAmount { get; set; }

        /// <summary>
        /// For American customers only. Frequency of the subscription being
        /// authorised by the mandate. One of <c>weekly</c>, <c>monthly</c> or
        /// <c>yearly</c>. Is required if <c>subscription_amount</c> has been
        /// provided.
        /// </summary>
        [JsonProperty("subscription_frequency")]
        public string SubscriptionFrequency { get; set; }

        /// <summary>
        /// For American customers only. Frequency of the subscription being
        /// authorised by the mandate. One of <c>weekly</c>, <c>monthly</c> or
        /// <c>yearly</c>. Is required if <c>subscription_amount</c> has been
        /// provided.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MandatePdfSubscriptionFrequency
        {
            /// <summary>`subscription_frequency` with a value of "weekly"</summary>
            [EnumMember(Value = "weekly")]
            Weekly,

            /// <summary>`subscription_frequency` with a value of "monthly"</summary>
            [EnumMember(Value = "monthly")]
            Monthly,

            /// <summary>`subscription_frequency` with a value of "yearly"</summary>
            [EnumMember(Value = "yearly")]
            Yearly,
        }

        /// <summary>
        /// For Swedish customers only. The civic/company number (personnummer,
        /// samordningsnummer, or organisationsnummer) of the customer. Should
        /// only be supplied for Autogiro mandates.
        /// </summary>
        [JsonProperty("swedish_identity_number")]
        public string SwedishIdentityNumber { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single mandate pdf.
    /// </summary>
    public class MandatePdfResponse : ApiResponse
    {
        /// <summary>
        /// The mandate pdf from the response.
        /// </summary>
        [JsonProperty("mandate_pdfs")]
        public MandatePdf MandatePdf { get; private set; }
    }
}
