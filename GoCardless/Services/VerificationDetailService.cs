

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
    /// Service class for working with verification detail resources.
    ///
    /// Details of a creditor that are required for verification
    /// </summary>

    public class VerificationDetailService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public VerificationDetailService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Verification details represent any information needed by GoCardless
        /// to verify a creditor.
        /// Currently, only UK-based companies are supported. 
        /// In other words, to submit verification details for a creditor, their
        /// creditor_type must be company and their country_code must be GB.
        /// </summary>
        /// <param name="request">An optional `VerificationDetailCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single verification detail resource</returns>
        public Task<VerificationDetailResponse> CreateAsync(VerificationDetailCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new VerificationDetailCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<VerificationDetailResponse>("POST", "/verification_details", urlParams, request, null, "verification_details", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Verification details represent any information needed by GoCardless to
    /// verify a creditor.
    /// Currently, only UK-based companies are supported. 
    /// In other words, to submit verification details for a creditor, their
    /// creditor_type must be company and their country_code must be GB.
    /// </summary>
    public class VerificationDetailCreateRequest
    {

        /// <summary>
        /// The first line of the company's address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the company's address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The third line of the company's address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// The city of the company's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The company's registration number.
        /// </summary>
        [JsonProperty("company_number")]
        public string CompanyNumber { get; set; }

        /// <summary>
        /// A summary describing what the company does.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The company's directors.
        /// </summary>
        [JsonProperty("directors")]
        public VerificationDetailDirectors[] Directors { get; set; }
        /// <summary>
        /// A primary director of the company represented by the creditor.
        /// </summary>
        public class VerificationDetailDirectors
        {
                
                /// <summary>
                            /// The city of the person's address.
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
                            /// The person's date of birth.
                /// </summary>
                [JsonProperty("date_of_birth")]
                public string DateOfBirth { get; set; }
                
                /// <summary>
                            /// The person's family name.
                /// </summary>
                [JsonProperty("family_name")]
                public string FamilyName { get; set; }
                
                /// <summary>
                            /// The person's given name.
                /// </summary>
                [JsonProperty("given_name")]
                public string GivenName { get; set; }
                
                /// <summary>
                            /// The person's postal code.
                /// </summary>
                [JsonProperty("postal_code")]
                public string PostalCode { get; set; }
                
                /// <summary>
                            /// The street of the person's address.
                /// </summary>
                [JsonProperty("street")]
                public string Street { get; set; }
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public VerificationDetailLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a VerificationDetail.
        /// </summary>
        public class VerificationDetailLinks
        {
                
                /// <summary>
                            /// ID of the associated [creditor](#core-endpoints-creditors).
                /// </summary>
                [JsonProperty("creditor")]
                public string Creditor { get; set; }
        }

        /// <summary>
        /// The company's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single verification detail.
    /// </summary>
    public class VerificationDetailResponse : ApiResponse
    {
        /// <summary>
        /// The verification detail from the response.
        /// </summary>
        [JsonProperty("verification_details")]
        public VerificationDetail VerificationDetail { get; private set; }
    }
}
