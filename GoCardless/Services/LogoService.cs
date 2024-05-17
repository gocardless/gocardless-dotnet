

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
