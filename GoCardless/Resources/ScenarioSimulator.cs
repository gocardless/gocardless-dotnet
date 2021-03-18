using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a scenario simulator resource.
    ///
    /// Scenario Simulators allow you to manually trigger and test certain paths
    /// that your
    /// integration will encounter in the real world. These endpoints are only
    /// active in the
    /// sandbox environment.
    /// </summary>
    public class ScenarioSimulator
    {
        /// <summary>
        /// The unique identifier of the simulator, used to initiate
        /// simulations. One of:
        /// <ul>
        /// <li>`creditor_verification_status_action_required`</li>
        /// <li>`creditor_verification_status_in_review`</li>
        /// <li>`creditor_verification_status_successful`</li>
        /// <li>`payment_paid_out`</li>
        /// <li>`payment_failed`</li>
        /// <li>`payment_charged_back`</li>
        /// <li>`payment_late_failure`</li>
        /// <li>`payment_late_failure_settled`</li>
        /// <li>`payment_submitted`</li>
        /// <li>`mandate_activated`</li>
        /// <li>`mandate_failed`</li>
        /// <li>`mandate_expired`</li>
        /// <li>`mandate_transferred`</li>
        /// <li>`refund_paid`</li>
        /// <li>`payout_bounced`</li>
        /// </ul>
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    
}
