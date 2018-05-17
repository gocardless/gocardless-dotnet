

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
    /// Mandate PDFs allow you to easily display [scheme-rules
    /// compliant](#appendix-compliance-requirements) Direct Debit mandates to
    /// your customers.
    /// </summary>

    public class MandatePdfService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
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
        /// [mandate](#core-endpoints-mandates).
        /// 
        /// By default, we'll generate PDF mandates in English.
        /// 
        /// To generate a PDF mandate in another language, set the
        /// `Accept-Language` header when creating the PDF mandate to the
        /// relevant [ISO
        /// 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
        /// language code supported for the scheme.
        /// 
        /// | Scheme           | Supported languages                            
        ///                                                                     
        ///                         |
        /// | :--------------- |
        /// :-------------------------------------------------------------------------------------------------------------------------------------------
        /// |
        /// | Autogiro         | English (`en`), Swedish (`sv`)                 
        ///                                                                     
        ///                         |
        /// | Bacs             | English (`en`)                                 
        ///                                                                     
        ///                         |
        /// | Becs             | English (`en`)                                 
        ///                                                                     
        ///                         |
        /// | Betalingsservice | Danish (`da`), English (`en`)                  
        ///                                                                     
        ///                         |
        /// | SEPA Core        | Danish (`da`), Dutch (`nl`), English (`en`),
        /// French (`fr`), German (`de`), Italian (`it`), Portuguese (`pt`),
        /// Spanish (`es`), Swedish (`sv`) |
        /// </summary>
        /// <param name="request">An optional `MandatePdfCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single mandate pdf resource</returns>
        public Task<MandatePdfResponse> CreateAsync(MandatePdfCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandatePdfCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<MandatePdfResponse>("POST", "/mandate_pdfs", urlParams, request, null, "mandate_pdfs", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Generates a PDF mandate and returns its temporary URL.
    /// 
    /// Customer and bank account details can be left blank (for a blank
    /// mandate), provided manually, or inferred from the ID of an existing
    /// [mandate](#core-endpoints-mandates).
    /// 
    /// By default, we'll generate PDF mandates in English.
    /// 
    /// To generate a PDF mandate in another language, set the `Accept-Language`
    /// header when creating the PDF mandate to the relevant [ISO
    /// 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes) language
    /// code supported for the scheme.
    /// 
    /// | Scheme           | Supported languages                                
    ///                                                                         
    ///                 |
    /// | :--------------- |
    /// :-------------------------------------------------------------------------------------------------------------------------------------------
    /// |
    /// | Autogiro         | English (`en`), Swedish (`sv`)                     
    ///                                                                         
    ///                 |
    /// | Bacs             | English (`en`)                                     
    ///                                                                         
    ///                 |
    /// | Becs             | English (`en`)                                     
    ///                                                                         
    ///                 |
    /// | Betalingsservice | Danish (`da`), English (`en`)                      
    ///                                                                         
    ///                 |
    /// | SEPA Core        | Danish (`da`), Dutch (`nl`), English (`en`), French
    /// (`fr`), German (`de`), Italian (`it`), Portuguese (`pt`), Spanish
    /// (`es`), Swedish (`sv`) |
    /// </summary>
    public class MandatePdfCreateRequest
    {

        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this
        /// matches the name of the [customer](#core-endpoints-customers). This
        /// field cannot exceed 18 characters.
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
        /// Bank code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// SWIFT BIC. Will be derived automatically if a valid `iban` or [local
        /// details](#appendix-local-bank-details) are provided.
        /// </summary>
        [JsonProperty("bic")]
        public string Bic { get; set; }

        /// <summary>
        /// Branch code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. Required if providing local details.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// For Danish customers only. The civic/company number (CPR or CVR) of
        /// the customer. Must be supplied if the customer's bank account is
        /// denominated in Danish krone (DKK). Can only be supplied for
        /// Betalingsservice mandates.
        /// </summary>
        [JsonProperty("danish_identity_number")]
        public string DanishIdentityNumber { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide
        /// [local details](#appendix-local-bank-details). IBANs cannot be
        /// provided for Autogiro mandates.
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
            /// ID of an existing [mandate](#core-endpoints-mandates) to build
            /// the PDF from. The customer's bank details will be censored in
            /// the generated PDF. No other parameters may be provided alongside
            /// this.
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
        /// Direct Debit scheme. Can be supplied or automatically detected from
        /// the bank account details provided. If you do not provide a scheme,
        /// you must provide either a mandate, an `iban`, or [local
        /// details](#appendix-local-bank-details) including a `country_code`.
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
        /// For Swedish customers only. The civic/company number (personnummer,
        /// samordningsnummer, or organisationsnummer) of the customer. Can only
        /// be supplied for Autogiro mandates.
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
