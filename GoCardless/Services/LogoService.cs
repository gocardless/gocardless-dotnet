

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
    /// Service class for working with logo resources.
    ///
    /// Logos are image uploads that, when associated with a creditor, are shown
    /// on the [billing request flow](#billing-requests-billing-request-flows)
    /// payment pages.
    /// </summary>

    public class LogoService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public LogoService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new logo associated with a creditor. If a creditor already
        /// has a logo, this will update the existing logo linked to the
        /// creditor.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "CR".</param> 
        /// <param name="request">An optional `LogoCreateForCreditorRequest` representing the body for this create_for_creditor request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single logo resource</returns>
        public Task<LogoResponse> CreateForCreditorAsync(string identity, LogoCreateForCreditorRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new LogoCreateForCreditorRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<LogoResponse>("POST", "/creditors/:identity/branding/logos", urlParams, request, null, "logos", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new logo associated with a creditor. If a creditor already has
    /// a logo, this will update the existing logo linked to the creditor.
    /// </summary>
    public class LogoCreateForCreditorRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single logo.
    /// </summary>
    public class LogoResponse : ApiResponse
    {
        /// <summary>
        /// The logo from the response.
        /// </summary>
        [JsonProperty("logos")]
        public Logo Logo { get; private set; }
    }
}
