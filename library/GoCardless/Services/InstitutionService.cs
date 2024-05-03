

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
    /// Service class for working with institution resources.
    ///
    /// Institutions that are supported when creating [Bank
    /// Authorisations](#billing-requests-bank-authorisations) for a particular
    /// country or purpose.
    /// 
    /// Not all institutions support both Payment Initiation (PIS) and Account
    /// Information (AIS) services.
    /// </summary>

    public class InstitutionService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public InstitutionService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a list of supported institutions.
        /// </summary>
        /// <param name="request">An optional `InstitutionListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of institution resources</returns>
        public Task<InstitutionListResponse> ListAsync(InstitutionListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstitutionListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<InstitutionListResponse>("GET", "/institutions", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Returns all institutions valid for a Billing Request.
        /// 
        /// This endpoint is currently supported only for FasterPayments.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `InstitutionListForBillingRequestRequest` representing the query parameters for this list_for_billing_request request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of institution resources</returns>
        public Task<InstitutionListResponse> ListForBillingRequestAsync(string identity, InstitutionListForBillingRequestRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstitutionListForBillingRequestRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<InstitutionListResponse>("GET", "/billing_requests/:identity/institutions", urlParams, request, null, null, customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Returns a list of supported institutions.
    /// </summary>
    public class InstitutionListRequest
    {

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. The country code of the institution.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }

        
    /// <summary>
    /// Returns all institutions valid for a Billing Request.
    /// 
    /// This endpoint is currently supported only for FasterPayments.
    /// </summary>
    public class InstitutionListForBillingRequestRequest
    {

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. The country code of the institution.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// ID(s) of the institution(s) to retrieve. More than one ID can be
        /// specified using a comma-separated string.
        /// </summary>
        [JsonProperty("ids")]
        public string[] Ids { get; set; }

        /// <summary>
        /// A search substring for retrieving institution(s), based on the
        /// institution's name.
        /// </summary>
        [JsonProperty("search")]
        public string Search { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single institution.
    /// </summary>
    public class InstitutionResponse : ApiResponse
    {
        /// <summary>
        /// The institution from the response.
        /// </summary>
        [JsonProperty("institutions")]
        public Institution Institution { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of institutions.
    /// </summary>
    public class InstitutionListResponse : ApiResponse
    {
        /// <summary>
        /// The list of institutions from the response.
        /// </summary>
        [JsonProperty("institutions")]
        public IReadOnlyList<Institution> Institutions { get; private set; }
    }
}
