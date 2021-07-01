

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
    /// Service class for working with bank authorisation resources.
    ///
    /// Bank Authorisations can be used to authorise Billing Requests.
    /// Authorisations
    /// are created against a specific bank, usually the bank that provides the
    /// payer's
    /// account.
    /// 
    /// Creation of Bank Authorisations is only permitted from GoCardless hosted
    /// UIs
    /// (see Billing Request Flows) to ensure we meet regulatory requirements
    /// for
    /// checkout flows.
    /// </summary>

    public class BankAuthorisationService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BankAuthorisationService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Fetches a bank authorisation
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BAU".</param> 
        /// <param name="request">An optional `BankAuthorisationGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank authorisation resource</returns>
        public Task<BankAuthorisationResponse> GetAsync(string identity, BankAuthorisationGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BankAuthorisationGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BankAuthorisationResponse>("GET", "/bank_authorisations/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Create a Bank Authorisation.
        /// </summary>
        /// <param name="request">An optional `BankAuthorisationCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank authorisation resource</returns>
        public Task<BankAuthorisationResponse> CreateAsync(BankAuthorisationCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BankAuthorisationCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BankAuthorisationResponse>("POST", "/bank_authorisations", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "bank_authorisations", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Fetches a bank authorisation
    /// </summary>
    public class BankAuthorisationGetRequest
    {
    }

        
    /// <summary>
    /// Create a Bank Authorisation.
    /// </summary>
    public class BankAuthorisationCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Type of authorisation, can be either 'mandate' or 'payment'.
        /// </summary>
        [JsonProperty("authorisation_type")]
        public string AuthorisationType { get; set; }
            
        /// <summary>
        /// Type of authorisation, can be either 'mandate' or 'payment'.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BankAuthorisationAuthorisationType
        {
    
            /// <summary>`authorisation_type` with a value of "mandate"</summary>
            [EnumMember(Value = "mandate")]
            Mandate,
            /// <summary>`authorisation_type` with a value of "payment"</summary>
            [EnumMember(Value = "payment")]
            Payment,
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public BankAuthorisationLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a BankAuthorisation.
        /// </summary>
        public class BankAuthorisationLinks
        {

            /// <summary>
            /// ID of the [billing request](#billing-requests-billing-requests)
            /// against which this authorisation was created.
            /// </summary>
            [JsonProperty("billing_request")]
            public string BillingRequest { get; set; }

            /// <summary>
            /// ID of the [institution](#billing-requests-institutions) against
            /// which this authorisation was created.
            /// </summary>
            [JsonProperty("institution")]
            public string Institution { get; set; }

            /// <summary>
            /// ID of the payment request against which this authorisation was
            /// created.
            /// </summary>
            [JsonProperty("payment_request")]
            public string PaymentRequest { get; set; }
        }

        /// <summary>
        /// URL that the payer can be redirected to after authorising the
        /// payment.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single bank authorisation.
    /// </summary>
    public class BankAuthorisationResponse : ApiResponse
    {
        /// <summary>
        /// The bank authorisation from the response.
        /// </summary>
        [JsonProperty("bank_authorisations")]
        public BankAuthorisation BankAuthorisation { get; private set; }
    }
}
