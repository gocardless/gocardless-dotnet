

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
    /// Service class for working with bank account detail resources.
    ///
    /// Retrieve bank account details in JWE encrypted format
    /// </summary>

    public class BankAccountDetailService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BankAccountDetailService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns bank account details in the flattened JSON Web Encryption
        /// format described in RFC 7516
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BA".</param> 
        /// <param name="request">An optional `BankAccountDetailGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank account detail resource</returns>
        public Task<BankAccountDetailResponse> GetAsync(string identity, BankAccountDetailGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BankAccountDetailGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BankAccountDetailResponse>("GET", "/bank_account_details/:identity", urlParams, request, null, null, customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Returns bank account details in the flattened JSON Web Encryption format
    /// described in RFC 7516
    /// </summary>
    public class BankAccountDetailGetRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single bank account detail.
    /// </summary>
    public class BankAccountDetailResponse : ApiResponse
    {
        /// <summary>
        /// The bank account detail from the response.
        /// </summary>
        [JsonProperty("bank_account_details")]
        public BankAccountDetail BankAccountDetail { get; private set; }
    }
}
