

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
    /// Service class for working with scenario simulator resources.
    ///
    /// Scenario Simulators allow you to manually trigger and test certain paths
    /// that your
    /// integration will encounter in the real world. These endpoints are only
    /// active in the
    /// sandbox environment.
    /// </summary>

    public class ScenarioSimulatorService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public ScenarioSimulatorService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Runs the specific scenario simulator against the specific resource
        /// </summary>  
        /// <param name="identity">The unique identifier of the simulator, used to initiate
        /// simulations. One of:
        /// <ul>
        /// <li>`creditor_verification_status_action_required`</li>
        /// <li>`creditor_verification_status_in_review`</li>
        /// <li>`creditor_verification_status_successful`</li>
        /// <li>`payment_paid_out`</li>
        /// <li>`payment_failed`</li>
        /// <li>`payment_charged_back`</li>
        /// <li>`payment_chargeback_settled`</li>
        /// <li>`payment_late_failure`</li>
        /// <li>`payment_late_failure_settled`</li>
        /// <li>`payment_submitted`</li>
        /// <li>`mandate_activated`</li>
        /// <li>`mandate_failed`</li>
        /// <li>`mandate_expired`</li>
        /// <li>`mandate_transferred`</li>
        /// <li>`refund_paid`</li>
        /// <li>`payout_bounced`</li>
        /// </ul></param> 
        /// <param name="request">An optional `ScenarioSimulatorRunRequest` representing the body for this run request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single scenario simulator resource</returns>
        public Task<ScenarioSimulatorResponse> RunAsync(string identity, ScenarioSimulatorRunRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new ScenarioSimulatorRunRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<ScenarioSimulatorResponse>("POST", "/scenario_simulators/:identity/actions/run", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Runs the specific scenario simulator against the specific resource
    /// </summary>
    public class ScenarioSimulatorRunRequest
    {

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code.
        /// Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD"
        /// are supported.
        /// Only required when simulating `payout_create`
        /// </summary>
        [JsonProperty("currency")]
        public ScenarioSimulatorCurrency? Currency { get; set; }
            
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code.
        /// Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD", "SEK" and "USD"
        /// are supported.
        /// Only required when simulating `payout_create`
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ScenarioSimulatorCurrency
        {
    
            /// <summary>`currency` with a value of "AUD"</summary>
            [EnumMember(Value = "AUD")]
            AUD,
            /// <summary>`currency` with a value of "CAD"</summary>
            [EnumMember(Value = "CAD")]
            CAD,
            /// <summary>`currency` with a value of "DKK"</summary>
            [EnumMember(Value = "DKK")]
            DKK,
            /// <summary>`currency` with a value of "EUR"</summary>
            [EnumMember(Value = "EUR")]
            EUR,
            /// <summary>`currency` with a value of "GBP"</summary>
            [EnumMember(Value = "GBP")]
            GBP,
            /// <summary>`currency` with a value of "NZD"</summary>
            [EnumMember(Value = "NZD")]
            NZD,
            /// <summary>`currency` with a value of "SEK"</summary>
            [EnumMember(Value = "SEK")]
            SEK,
            /// <summary>`currency` with a value of "USD"</summary>
            [EnumMember(Value = "USD")]
            USD,
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public ScenarioSimulatorLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a ScenarioSimulator.
        /// </summary>
        public class ScenarioSimulatorLinks
        {

            /// <summary>
            /// ID of the resource to run the simulation against. This should be
            /// of the type returned for this simulator in the `GET
            /// /scenario_simulators` API.
            /// </summary>
            [JsonProperty("resource")]
            public string Resource { get; set; }
        }
    }

    /// <summary>
    /// An API response for a request returning a single scenario simulator.
    /// </summary>
    public class ScenarioSimulatorResponse : ApiResponse
    {
        /// <summary>
        /// The scenario simulator from the response.
        /// </summary>
        [JsonProperty("scenario_simulators")]
        public ScenarioSimulator ScenarioSimulator { get; private set; }
    }
}
