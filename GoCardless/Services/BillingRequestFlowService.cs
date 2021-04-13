

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
    }

        
    /// <summary>
    /// Creates a new billing request flow.
    /// </summary>
    public class BillingRequestFlowCreateRequest
    {

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
        /// If true, the payer will not be able to edit their existing details
        /// (e.g. customer and bank account) within the billing request flow.
        /// </summary>
        [JsonProperty("lock_existing_details")]
        public bool? LockExistingDetails { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after completing the request
        /// flow.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
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
