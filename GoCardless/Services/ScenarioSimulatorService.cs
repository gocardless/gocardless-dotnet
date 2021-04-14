

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
        /// <li>`creditor_verification_status_action_required`: Sets a creditor's `verification
        /// status` to `action required`, meaning that the creditor must provide further information
        /// to GoCardless in order to verify their account to receive payouts.</li>
        /// <li>`creditor_verification_status_in_review`: Sets a creditor's `verification status` to
        /// `in review`, meaning that the creditor has provided all of the information requested by
        /// GoCardless to verify their account, and is now awaiting review.</li>
        /// <li>`creditor_verification_status_successful`: Sets a creditor's `verification status`
        /// to `successful`, meaning that the creditor is fully verified and can receive
        /// payouts.</li>
        /// <li>`payment_paid_out`: Transitions a payment through to `paid_out`, having been
        /// collected successfully and paid out to you. It must start in the `pending_submission`
        /// state, and its mandate must be in the `activated` state (unless it is a payment for ACH,
        /// BECS, BECS_NZ or SEPA, in which cases the mandate may be `pending_submission`, since
        /// their mandates are submitted with their first payment).</li>
        /// <li>`payment_failed`: Transitions a payment through to `failed`. It must start in the
        /// `pending_submission` state, and its mandate must be in the `activated` state (unless it
        /// is a payment for ACH, BECS, BECS_NZ or SEPA, in which cases the mandate may be
        /// `pending_submission`, since their mandates are submitted with their first payment).</li>
        /// <li>`payment_charged_back`: Behaves the same as the `payout_paid_out` simulator, except
        /// that the payment is transitioned to `charged_back` after it is paid out, having been
        /// charged back by the customer.</li>
        /// <li>`payment_chargeback_settled`: Behaves the same as the `payment_charged_back`
        /// simulator, except that the charged back payment is additionally included as a debit item
        /// in a payout, thereby settling the charged back payment.</li>
        /// <li>`payment_late_failure`: Transitions a payment through to `late_failure`, having been
        /// apparently collected successfully beforehand. It must start in the `pending_submission`
        /// state, and its mandate must be in the `activated` state (unless it is a payment for ACH,
        /// BECS, BECS_NZ or SEPA, in which cases the mandate may be `pending_submission`, since
        /// their mandates are submitted with their first payment). Not compatible with Autogiro
        /// mandates.</li>
        /// <li>`payment_late_failure_settled`: Behaves the same as the `payment_late_failure`
        /// simulator, except that the late failure is additionally included as a debit item in a
        /// payout, thereby settling the late failure.</li>
        /// <li>`payment_submitted`: Transitions a payment to `submitted`, without proceeding any
        /// further. It must start in the `pending_submission` state.</li>
        /// <li>`mandate_activated`: Transitions a mandate through to `activated`, having been
        /// submitted to the banks and set up successfully. It must start in the
        /// `pending_submission` state. Not compatible with ACH, BECS, BECS_NZ and SEPA mandates,
        /// which are submitted and activated with their first payment.</li>
        /// <li>`mandate_customer_approval_granted`: Transitions a mandate through to
        /// `pending_submission`, as if the customer approved the mandate creation. It must start in
        /// the `pending_customer_approval` state. Compatible only with Bacs and SEPA mandates,
        /// which support customer signatures on the mandate. All payments associated with the
        /// mandate will be transitioned to `pending_submission`. All subscriptions associated with
        /// the mandate will become `active`.</li>
        /// <li>`mandate_customer_approval_skipped`: Transitions a mandate through to
        /// `pending_submission`, as if the customer skipped the mandate approval during the mandate
        /// creation process. It must start in the `pending_customer_approval` state. Compatible
        /// only with Bacs and SEPA mandates, which support customer signatures on the mandate. All
        /// payments associated with the mandate will be transitioned to `pending_submission`. All
        /// subscriptions associated with the mandate will become `active`.</li>
        /// <li>`mandate_failed`: Transitions a mandate through to `failed`, having been submitted
        /// to the banks but found to be invalid (for example due to invalid bank details). It must
        /// start in the `pending_submission` or `submitted` states. Not compatible with ACH, BECS,
        /// BECS_NZ and SEPA mandates, which are submitted with their first payment.</li>
        /// <li>`mandate_expired`: Transitions a mandate through to `expired`, having been submitted
        /// to the banks, set up successfully and then expired because no collection attempts were
        /// made against it for longer than the scheme's dormancy period (13 months for Bacs, 3
        /// years for SEPA, 15 months for ACH, Betalingsservice, and BECS). It must start in the
        /// `pending_submission` state. Not compatible with Autogiro, BECS NZ, and PAD mandates,
        /// which do not expire.</li>
        /// <li>`mandate_transferred`: Transitions a mandate through to `transferred`, having been
        /// submitted to the banks, set up successfully and then moved to a new bank account due to
        /// the customer using the UK's Current Account Switching Service (CASS). It must start in
        /// the `pending_submission` state. Only compatible with Bacs mandates.</li>
        /// <li>`mandate_transferred_with_resubmission`: Transitions a mandate through `transferred`
        /// and resubmits it to the banks, can be caused be the UK's Current Account Switching
        /// Service (CASS) or when a customer contacts GoCardless to change their bank details. It
        /// must start in the `pending_submission` state. Only compatible with Bacs, SEPA and
        /// Autogiro mandates.</li>
        /// <li>`refund_paid`: Transitions a refund to `paid`. It must start in either the
        /// `pending_submission` or `submitted` state.</li>
        /// <li>`refund_settled`: Transitions a refund to `paid`, if it's not already, then
        /// generates a payout that includes the refund, thereby settling the funds. It must start
        /// in one of `pending_submission`, `submitted` or `paid` states.</li>
        /// <li>`refund_bounced`: Transitions a refund to `bounced`. It must start in either the
        /// `pending_submission`, `submitted`, or `paid` state.</li>
        /// <li>`payout_bounced`: Transitions a payout to `bounced`. It must start in the `paid`
        /// state.</li>
        /// <li>`payout_create`: Creates a payout containing payments in `confirmed`, `failed` &
        /// `charged_back` states; refunds in `submitted` & `bounced`; and all related fees. Can
        /// only be used with a positive total payout balance and when some eligible items
        /// exist.</li>
        /// <li>`billing_request_fulfilled`: Authorises the billing request, fulfils it, and moves
        /// the associated payment to `failed`. The billing request must be in the `pending` state,
        /// with all actions completed except for `bank_authorisation`. Only billing requests with a
        /// `payment_request` are supported.</li>
        /// <li>`billing_request_fulfilled_and_payment_failed`: Authorises the billing request,
        /// fulfils it, and moves the associated payment to `failed`. The billing request must be in
        /// the `pending` state, with all actions completed except for `bank_authorisation`. Only
        /// billing requests with a `payment_request` are supported.</li>
        /// <li>`billing_request_fulfilled_and_payment_paid_out`: Authorises the billing request,
        /// fulfils it, and moves the associated payment to `paid_out`. The billing request must be
        /// in the `pending` state, with all actions completed except for `bank_authorisation`. Only
        /// billing requests with a `payment_request` are supported.</li>
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
            /// ID of the resource to run the simulation against.
            /// Must be same type of resource as the simulator that is being
            /// run.
            /// eg. Payment ID for `payment_failed`, Mandate ID for
            /// `mandate_activated` etc
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
