

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
    /// Service class for working with transferred mandate resources.
    ///
    /// Mandates that have been transferred using Current Account Switch Service
    /// </summary>

    public class TransferredMandateService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public TransferredMandateService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns new customer bank details for a mandate that's been recently
        /// transferred
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "MD". Note that this prefix may
        /// not apply to mandates created before 2016.</param> 
        /// <param name="request">An optional `TransferredMandateTransferredMandatesRequest` representing the query parameters for this transferred_mandates request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single transferred mandate resource</returns>
        public Task<TransferredMandateResponse> TransferredMandatesAsync(string identity, TransferredMandateTransferredMandatesRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new TransferredMandateTransferredMandatesRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<TransferredMandateResponse>("GET", "/transferred_mandates/:identity", urlParams, request, null, null, customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Returns new customer bank details for a mandate that's been recently
    /// transferred
    /// </summary>
    public class TransferredMandateTransferredMandatesRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single transferred mandate.
    /// </summary>
    public class TransferredMandateResponse : ApiResponse
    {
        /// <summary>
        /// The transferred mandate from the response.
        /// </summary>
        [JsonProperty("transferred_mandates")]
        public TransferredMandate TransferredMandate { get; private set; }
    }
}
