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
    /// Service class for working with mandate import entry resources.
    ///
    /// Mandate Import Entries are added to a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandate-imports">Mandate
    /// Import</a>.
    /// Each entry corresponds to one mandate to be imported into GoCardless.
    ///
    /// To import a mandate you will need:
    ///
    /// <ol>
    ///   <li>Identifying information about the customer (name/company and
    /// address)</li>
    ///   <li>Bank account details, consisting of an account holder name and
    /// either an IBAN or <a
    /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
    /// bank details</a></li>
    ///   <li>Amendment details (SEPA only)</li>
    /// </ol>
    /// We suggest you provide a <c>record_identifier</c> (which is unique
    /// within the context of a
    /// single mandate import) to help you to identify mandates that have been
    /// created once the
    /// import has been processed by GoCardless. You can
    /// <a
    /// href="https://developer.gocardless.com/api-reference/#mandate-import-entries-list-all-mandate-import-entries">list
    /// the mandate import entries</a>,
    /// match them up in your system using the <c>record_identifier</c>, and
    /// look at the <c>links</c>
    /// fields to find the mandate, customer and customer bank account that have
    /// been imported.
    ///
    /// <p class="restricted-notice">Restricted: This API is currently only
    /// available for approved integrators - please <a
    /// href="mailto:help@gocardless.com">get in touch</a> if you would like to
    /// use this API.</p>
    /// </summary>
    public class MandateImportEntryService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public MandateImportEntryService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// For an existing <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandate-imports">mandate
        /// import</a>, this endpoint can
        /// be used to add individual mandates to be imported into GoCardless.
        ///
        /// You can add no more than 30,000 rows to a single mandate import.
        /// If you attempt to go over this limit, the API will return a
        /// <c>record_limit_exceeded</c> error.
        /// </summary>
        /// <param name="request">An optional `MandateImportEntryCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single mandate import entry resource</returns>
        public Task<MandateImportEntryResponse> CreateAsync(
            MandateImportEntryCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportEntryCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<MandateImportEntryResponse>(
                "POST",
                "/mandate_import_entries",
                urlParams,
                request,
                null,
                "mandate_import_entries",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// For an existing mandate import, this endpoint lists all of the
        /// entries attached.
        ///
        /// After a mandate import has been submitted, you can use this endpoint
        /// to associate records
        /// in your system (using the <c>record_identifier</c> that you provided
        /// when creating the
        /// mandate import).
        /// </summary>
        /// <param name="request">An optional `MandateImportEntryListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of mandate import entry resources</returns>
        public Task<MandateImportEntryListResponse> ListAsync(
            MandateImportEntryListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportEntryListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<MandateImportEntryListResponse>(
                "GET",
                "/mandate_import_entries",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of mandate import entries.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<MandateImportEntry> All(
            MandateImportEntryListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportEntryListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.MandateImportEntries)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of mandate import entries.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<MandateImportEntry>>> AllAsync(
            MandateImportEntryListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportEntryListRequest();

            return new TaskEnumerable<IReadOnlyList<MandateImportEntry>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.MandateImportEntries, list.Meta?.Cursors?.After);
            });
        }
    }

    /// <summary>
    /// For an existing <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandate-imports">mandate
    /// import</a>, this endpoint can
    /// be used to add individual mandates to be imported into GoCardless.
    ///
    /// You can add no more than 30,000 rows to a single mandate import.
    /// If you attempt to go over this limit, the API will return a
    /// <c>record_limit_exceeded</c> error.
    /// </summary>
    public class MandateImportEntryCreateRequest
    {
        [JsonProperty("amendment")]
        public MandateImportEntryAmendment Amendment { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class MandateImportEntryAmendment
        {
            /// <summary>
            /// The creditor identifier of the direct debit originator. Required
            /// if mandate
            /// import scheme is <c>sepa</c>.
            /// </summary>
            [JsonProperty("original_creditor_id")]
            public string OriginalCreditorId { get; set; }

            /// <summary>
            /// Data about the original mandate to be moved or modified.
            /// </summary>
            [JsonProperty("original_creditor_name")]
            public string OriginalCreditorName { get; set; }

            /// <summary>
            /// The unique SEPA reference for the mandate being amended.
            /// Required if mandate
            /// import scheme is <c>sepa</c>.
            /// </summary>
            [JsonProperty("original_mandate_reference")]
            public string OriginalMandateReference { get; set; }
        }

        [JsonProperty("bank_account")]
        public MandateImportEntryBankAccount BankAccount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class MandateImportEntryBankAccount
        {
            /// <summary>
            /// Name of the account holder, as known by the bank. The full name
            /// provided when the customer is created is stored and is available
            /// via the API, but is transliterated, upcased, and truncated to 18
            /// characters in bank submissions. This field is required unless
            /// the request includes a <a
            /// href="https://developer.gocardless.com/api-reference/#javascript-flow-customer-bank-account-tokens">customer
            /// bank account token</a>.
            /// </summary>
            [JsonProperty("account_holder_name")]
            public string AccountHolderName { get; set; }

            /// <summary>
            /// Bank account number - see <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
            /// details</a> for more information. Alternatively you can provide
            /// an <c>iban</c>.
            /// </summary>
            [JsonProperty("account_number")]
            public string AccountNumber { get; set; }

            /// <summary>
            /// Bank account type. Required for USD-denominated bank accounts.
            /// Must not be provided for bank accounts in other currencies. See
            /// <a
            /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
            /// details</a> for more information.
            /// </summary>
            [JsonProperty("account_type")]
            public MandateImportEntryAccountType? AccountType { get; set; }

            /// <summary>
            /// Bank account type. Required for USD-denominated bank accounts. Must
            /// not be provided for bank accounts in other currencies. See <a
            /// href="https://developer.gocardless.com/api-reference/#local-bank-details-united-states">local
            /// details</a> for more information.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum MandateImportEntryAccountType
            {
                /// <summary>`account_type` with a value of "savings"</summary>
                [EnumMember(Value = "savings")]
                Savings,

                /// <summary>`account_type` with a value of "checking"</summary>
                [EnumMember(Value = "checking")]
                Checking,
            }

            /// <summary>
            /// Bank code - see <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
            /// details</a> for more information. Alternatively you can provide
            /// an <c>iban</c>.
            /// </summary>
            [JsonProperty("bank_code")]
            public string BankCode { get; set; }

            /// <summary>
            /// Branch code - see <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
            /// details</a> for more information. Alternatively you can provide
            /// an <c>iban</c>.
            /// </summary>
            [JsonProperty("branch_code")]
            public string BranchCode { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
            /// 3166-1 alpha-2 code</a>. Defaults to the country code of the
            /// <c>iban</c> if supplied, otherwise is required.
            /// </summary>
            [JsonProperty("country_code")]
            public string CountryCode { get; set; }

            /// <summary>
            /// International Bank Account Number. Alternatively you can provide
            /// <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
            /// details</a>. IBANs are not accepted for Swedish bank accounts
            /// denominated in SEK - you must supply <a
            /// href="https://developer.gocardless.com/api-reference/#local-bank-details-sweden">local
            /// details</a>.
            /// </summary>
            [JsonProperty("iban")]
            public string Iban { get; set; }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }
        }

        [JsonProperty("customer")]
        public MandateImportEntryCustomer Customer { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class MandateImportEntryCustomer
        {
            /// <summary>
            /// The first line of the customer's address. Required if mandate
            /// import scheme is either <c>bacs</c> or <c>sepa</c>.
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
            /// Customer's company name. Required unless a <c>given_name</c> and
            /// <c>family_name</c> are provided. For Canadian customers, the use
            /// of a <c>company_name</c> value will mean that any mandate
            /// created from this customer will be considered to be a "Business
            /// PAD" (otherwise, any mandate will be considered to be a
            /// "Personal PAD").
            /// </summary>
            [JsonProperty("company_name")]
            public string CompanyName { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
            /// 3166-1 alpha-2 code.</a>
            /// </summary>
            [JsonProperty("country_code")]
            public string CountryCode { get; set; }

            /// <summary>
            /// For Danish customers only. The civic/company number (CPR or CVR)
            /// of the customer. Must be supplied if the customer's bank account
            /// is denominated in Danish krone (DKK).
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
            /// Customer's surname. Required unless a <c>company_name</c> is
            /// provided.
            /// </summary>
            [JsonProperty("family_name")]
            public string FamilyName { get; set; }

            /// <summary>
            /// Customer's first name. Required unless a <c>company_name</c> is
            /// provided.
            /// </summary>
            [JsonProperty("given_name")]
            public string GivenName { get; set; }

            /// <summary>
            /// <a
            /// href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO
            /// 639-1</a> code. Used as the language for notification emails
            /// sent by GoCardless if your organisation does not send its own
            /// (see <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-compliance-requirements">compliance
            /// requirements</a>). Currently only "en", "fr", "de", "pt", "es",
            /// "it", "nl", "da", "nb", "sl", "sv" are supported. If this is not
            /// provided, the language will be chosen based on the
            /// <c>country_code</c> (if supplied) or default to "en".
            /// </summary>
            [JsonProperty("language")]
            public string Language { get; set; }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// <a href="https://en.wikipedia.org/wiki/E.123">ITU E.123</a>
            /// formatted phone number, including country code.
            /// </summary>
            [JsonProperty("phone_number")]
            public string PhoneNumber { get; set; }

            /// <summary>
            /// The customer's postal code. Required if mandate import scheme is
            /// either <c>bacs</c> or <c>sepa</c>.
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
            /// For Swedish customers only. The civic/company number
            /// (personnummer, samordningsnummer, or organisationsnummer) of the
            /// customer. Must be supplied if the customer's bank account is
            /// denominated in Swedish krona (SEK). This field cannot be changed
            /// once it has been set.
            /// </summary>
            [JsonProperty("swedish_identity_number")]
            public string SwedishIdentityNumber { get; set; }
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public MandateImportEntryLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a MandateImportEntry.
        /// </summary>
        public class MandateImportEntryLinks
        {
            /// <summary>
            /// Unique identifier, beginning with "IM".
            /// </summary>
            [JsonProperty("mandate_import")]
            public string MandateImport { get; set; }
        }

        [JsonProperty("mandate")]
        public MandateImportEntryMandate Mandate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public class MandateImportEntryMandate
        {
            /// <summary>
            /// This field is ACH specific, sometimes referred to as <a
            /// href="https://www.moderntreasury.com/learn/sec-codes">SEC
            /// code</a>.
            ///
            /// This is the way that the payer gives authorisation to the
            /// merchant.
            /// web: Authorisation is Internet Initiated or via Mobile Entry
            /// (maps to SEC code: WEB)
            /// telephone: Authorisation is provided orally over telephone (maps
            /// to SEC code: TEL)
            /// paper: Authorisation is provided in writing and signed, or
            /// similarly authenticated (maps to SEC code: PPD)
            /// </summary>
            [JsonProperty("authorisation_source")]
            public MandateImportEntryAuthorisationSource? AuthorisationSource { get; set; }

            /// <summary>
            /// This field is ACH specific, sometimes referred to as <a
            /// href="https://www.moderntreasury.com/learn/sec-codes">SEC code</a>.
            ///
            /// This is the way that the payer gives authorisation to the merchant.
            /// web: Authorisation is Internet Initiated or via Mobile Entry (maps
            /// to SEC code: WEB)
            /// telephone: Authorisation is provided orally over telephone (maps to
            /// SEC code: TEL)
            /// paper: Authorisation is provided in writing and signed, or similarly
            /// authenticated (maps to SEC code: PPD)
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum MandateImportEntryAuthorisationSource
            {
                /// <summary>`authorisation_source` with a value of "web"</summary>
                [EnumMember(Value = "web")]
                Web,

                /// <summary>`authorisation_source` with a value of "telephone"</summary>
                [EnumMember(Value = "telephone")]
                Telephone,

                /// <summary>`authorisation_source` with a value of "paper"</summary>
                [EnumMember(Value = "paper")]
                Paper,
            }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// Unique reference. Different schemes have different length and <a
            /// href="https://developer.gocardless.com/api-reference/#appendix-character-sets">character
            /// set</a> requirements. GoCardless will generate a unique
            /// reference satisfying the different scheme requirements if this
            /// field is left blank.
            /// </summary>
            [JsonProperty("reference")]
            public string Reference { get; set; }
        }

        /// <summary>
        /// A unique identifier for this entry, which you can use (once the
        /// import has been
        /// processed by GoCardless) to identify the records that have been
        /// created. Limited
        /// to 255 characters.
        /// </summary>
        [JsonProperty("record_identifier")]
        public string RecordIdentifier { get; set; }
    }

    /// <summary>
    /// For an existing mandate import, this endpoint lists all of the entries
    /// attached.
    ///
    /// After a mandate import has been submitted, you can use this endpoint to
    /// associate records
    /// in your system (using the <c>record_identifier</c> that you provided
    /// when creating the
    /// mandate import).
    /// </summary>
    public class MandateImportEntryListRequest
    {
        /// <summary>
        /// Cursor pointing to the start of the desired set.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        /// Cursor pointing to the end of the desired set.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "IM".
        /// </summary>
        [JsonProperty("mandate_import")]
        public string MandateImport { get; set; }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>sucessfully_processed</c>: the entry has been imported and
        /// the associated records created.</li>
        /// <li><c>unsuccessfully_processed</c>: the entry could not be
        /// processed due to an error, see the 'processing_errors' value</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public MandateImportEntryStatus? Status { get; set; }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>sucessfully_processed</c>: the entry has been imported and
        /// the associated records created.</li>
        /// <li><c>unsuccessfully_processed</c>: the entry could not be
        /// processed due to an error, see the 'processing_errors' value</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MandateImportEntryStatus
        {
            /// <summary>`status` with a value of "successfully_processed"</summary>
            [EnumMember(Value = "successfully_processed")]
            SuccessfullyProcessed,

            /// <summary>`status` with a value of "unsuccessfully_processed"</summary>
            [EnumMember(Value = "unsuccessfully_processed")]
            UnsuccessfullyProcessed,
        }
    }

    /// <summary>
    /// An API response for a request returning a single mandate import entry.
    /// </summary>
    public class MandateImportEntryResponse : ApiResponse
    {
        /// <summary>
        /// The mandate import entry from the response.
        /// </summary>
        [JsonProperty("mandate_import_entries")]
        public MandateImportEntry MandateImportEntry { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of mandate import entries.
    /// </summary>
    public class MandateImportEntryListResponse : ApiResponse
    {
        /// <summary>
        /// The list of mandate import entries from the response.
        /// </summary>
        [JsonProperty("mandate_import_entries")]
        public IReadOnlyList<MandateImportEntry> MandateImportEntries { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
