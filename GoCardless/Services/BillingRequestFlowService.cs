

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
    /// Service class for working with billing request flow resources.
    ///
    /// Billing Request Flows can be created to enable a payer to authorise a
    /// payment created for a scheme with strong payer
    /// authorisation (such as open banking single payments).
    /// </summary>

    public class BillingRequestFlowService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BillingRequestFlowService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new billing request flow.
        /// </summary>
        /// <param name="request">An optional `BillingRequestFlowCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request flow resource</returns>
        public Task<BillingRequestFlowResponse> CreateAsync(BillingRequestFlowCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestFlowCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BillingRequestFlowResponse>("POST", "/billing_request_flows", urlParams, request, null, "billing_request_flows", customiseRequestMessage);
        }

        /// <summary>
        /// Returns the flow having generated a fresh session token which can be
        /// used to power
        /// integrations that manipulate the flow.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRQ".</param> 
        /// <param name="request">An optional `BillingRequestFlowInitialiseRequest` representing the body for this initialise request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request flow resource</returns>
        public Task<BillingRequestFlowResponse> InitialiseAsync(string identity, BillingRequestFlowInitialiseRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestFlowInitialiseRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestFlowResponse>("POST", "/billing_request_flows/:identity/actions/initialise", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new billing request flow.
    /// </summary>
    public class BillingRequestFlowCreateRequest
    {

        /// <summary>
        /// Fulfil the Billing Request on completion of the flow (true by
        /// default)
        /// </summary>
        [JsonProperty("auto_fulfil")]
        public bool? AutoFulfil { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestFlowLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a BillingRequestFlow.
        /// </summary>
        public class BillingRequestFlowLinks
        {

            /// <summary>
            /// ID of the [billing request](#billing-requests-billing-requests)
            /// against which this flow was created.
            /// </summary>
            [JsonProperty("billing_request")]
            public string BillingRequest { get; set; }
        }

        /// <summary>
        /// If true, the payer will not be able to change their bank account
        /// within the flow
        /// </summary>
        [JsonProperty("lock_bank_account")]
        public bool? LockBankAccount { get; set; }

        /// <summary>
        /// If true, the payer will not be able to edit their customer details
        /// within the flow
        /// </summary>
        [JsonProperty("lock_customer_details")]
        public bool? LockCustomerDetails { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after completing the request
        /// flow.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }

        
    /// <summary>
    /// Returns the flow having generated a fresh session token which can be
    /// used to power
    /// integrations that manipulate the flow.
    /// </summary>
    public class BillingRequestFlowInitialiseRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single billing request flow.
    /// </summary>
    public class BillingRequestFlowResponse : ApiResponse
    {
        /// <summary>
        /// The billing request flow from the response.
        /// </summary>
        [JsonProperty("billing_request_flows")]
        public BillingRequestFlow BillingRequestFlow { get; private set; }
    }
}
