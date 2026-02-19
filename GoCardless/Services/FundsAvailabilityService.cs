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
    /// Service class for working with funds availability resources.
    ///
    ///  Checks if the payer's current balance is sufficient to cover the amount
    ///  the merchant wants to charge within the consent parameters defined on
    ///  the mandate.
    /// </summary>
    public class FundsAvailabilityService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public FundsAvailabilityService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        ///  Checks if the payer's current balance is sufficient to cover the
        ///  amount
        ///  the merchant wants to charge within the consent parameters defined
        ///  on the mandate.
        /// </summary>
        ///  <param name="identity">Unique identifier, beginning with "MD". Note that this prefix
        ///  may not apply to mandates created before 2016.</param>
        /// <param name="request">An optional `FundsAvailabilityCheckRequest` representing the query parameters for this check request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single funds availability resource</returns>
        public Task<FundsAvailabilityResponse> CheckAsync(
            string identity,
            FundsAvailabilityCheckRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new FundsAvailabilityCheckRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<FundsAvailabilityResponse>(
                "GET",
                "/funds_availability/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    ///  Checks if the payer's current balance is sufficient to cover the amount
    ///  the merchant wants to charge within the consent parameters defined on
    ///  the mandate.
    /// </summary>
    public class FundsAvailabilityCheckRequest
    {
        /// <summary>
        ///  The amount of the payment
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single funds availability.
    /// </summary>
    public class FundsAvailabilityResponse : ApiResponse
    {
        /// <summary>
        /// The funds availability from the response.
        /// </summary>
        [JsonProperty("funds_availabilities")]
        public FundsAvailability FundsAvailability { get; private set; }
    }
}
