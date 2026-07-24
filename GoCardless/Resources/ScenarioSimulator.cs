using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
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
        ///
        /// <ul>
        /// <li><c>creditor_verification_status_action_required</c>: Sets a
        /// creditor's <c>verification status</c> to <c>action required</c>,
        /// meaning that the creditor must provide further information to
        /// GoCardless in order to verify their account to receive payouts.</li>
        /// <li><c>creditor_verification_status_in_review</c>: Sets a creditor's
        /// <c>verification status</c> to <c>in review</c>, meaning that the
        /// creditor has provided all of the information requested by GoCardless
        /// to verify their account, and is now awaiting review.</li>
        /// <li><c>creditor_verification_status_successful</c>: Sets a
        /// creditor's <c>verification status</c> to <c>successful</c>, meaning
        /// that the creditor is fully verified and can receive payouts.</li>
        /// <li><c>payment_confirmed</c>: Transitions a payment through to
        /// <c>confirmed</c>. It must start in the <c>pending_submission</c>
        /// state, and its mandate must be in the <c>activated</c> state (unless
        /// it is a payment for ACH, BECS, BECS_NZ or SEPA, in which cases the
        /// mandate may be <c>pending_submission</c>, since their mandates are
        /// submitted with their first payment).</li>
        /// <li><c>payment_paid_out</c>: Transitions a payment through to
        /// <c>paid_out</c>, having been collected successfully and paid out to
        /// you. It must start in the <c>pending_submission</c> state, and its
        /// mandate must be in the <c>activated</c> state (unless it is a
        /// payment for ACH, BECS, BECS_NZ or SEPA, in which cases the mandate
        /// may be <c>pending_submission</c>, since their mandates are submitted
        /// with their first payment).</li>
        /// <li><c>payment_failed</c>: Transitions a payment through to
        /// <c>failed</c>. It must start in the <c>pending_submission</c> state,
        /// and its mandate must be in the <c>activated</c> state (unless it is
        /// a payment for ACH, BECS, BECS_NZ or SEPA, in which cases the mandate
        /// may be <c>pending_submission</c>, since their mandates are submitted
        /// with their first payment).</li>
        /// <li><c>payment_charged_back</c>: Behaves the same as the
        /// <c>payout_paid_out</c> simulator, except that the payment is
        /// transitioned to <c>charged_back</c> after it is paid out, having
        /// been charged back by the customer.</li>
        /// <li><c>payment_chargeback_settled</c>: Behaves the same as the
        /// <c>payment_charged_back</c> simulator, except that the charged back
        /// payment is additionally included as a debit item in a payout,
        /// thereby settling the charged back payment.</li>
        /// <li><c>payment_late_failure</c>: Transitions a payment through to
        /// <c>late_failure</c>, having been apparently collected successfully
        /// beforehand. It must start in either the <c>pending_submission</c> or
        /// <c>paid_out</c> state, and its mandate must be in the
        /// <c>activated</c> state (unless it is a payment for ACH, BECS,
        /// BECS_NZ or SEPA, in which cases the mandate may be
        /// <c>pending_submission</c>, since their mandates are submitted with
        /// their first payment). Not compatible with Autogiro mandates.</li>
        /// <li><c>payment_late_failure_settled</c>: Behaves the same as the
        /// <c>payment_late_failure</c> simulator, except that the late failure
        /// is additionally included as a debit item in a payout, thereby
        /// settling the late failure.</li>
        /// <li><c>payment_submitted</c>: Transitions a payment to
        /// <c>submitted</c>, without proceeding any further. It must start in
        /// the <c>pending_submission</c> state.</li>
        /// <li><c>mandate_activated</c>: Transitions a mandate through to
        /// <c>activated</c>, having been submitted to the banks and set up
        /// successfully. It must start in the <c>pending_submission</c> state.
        /// Not compatible with ACH, BECS, BECS_NZ and SEPA mandates, which are
        /// submitted and activated with their first payment.</li>
        /// <li><c>mandate_customer_approval_granted</c>: Transitions a mandate
        /// through to <c>pending_submission</c>, as if the customer approved
        /// the mandate creation. It must start in the
        /// <c>pending_customer_approval</c> state. Compatible only with Bacs
        /// and SEPA mandates, which support customer signatures on the mandate.
        /// All payments associated with the mandate will be transitioned to
        /// <c>pending_submission</c>. All subscriptions associated with the
        /// mandate will become <c>active</c>.</li>
        /// <li><c>mandate_customer_approval_skipped</c>: Transitions a mandate
        /// through to <c>pending_submission</c>, as if the customer skipped the
        /// mandate approval during the mandate creation process. It must start
        /// in the <c>pending_customer_approval</c> state. Compatible only with
        /// Bacs and SEPA mandates, which support customer signatures on the
        /// mandate. All payments associated with the mandate will be
        /// transitioned to <c>pending_submission</c>. All subscriptions
        /// associated with the mandate will become <c>active</c>.</li>
        /// <li><c>mandate_failed</c>: Transitions a mandate through to
        /// <c>failed</c>, having been submitted to the banks but found to be
        /// invalid (for example due to invalid bank details). It must start in
        /// the <c>pending_submission</c> or <c>submitted</c> states. Not
        /// compatible with SEPA mandates, which are submitted with their first
        /// payment.</li>
        /// <li><c>mandate_expired</c>: Transitions a mandate through to
        /// <c>expired</c>, having been submitted to the banks, set up
        /// successfully and then expired because no collection attempts were
        /// made against it for longer than the scheme's dormancy period (13
        /// months for Bacs, 3 years for SEPA, 15 months for ACH,
        /// Betalingsservice, and BECS). It must start in the
        /// <c>pending_submission</c> state. Not compatible with Autogiro, BECS
        /// NZ, and PAD mandates, which do not expire.</li>
        /// <li><c>mandate_transferred</c>: Transitions a mandate through to
        /// <c>transferred</c>, having been submitted to the banks, set up
        /// successfully and then moved to a new bank account due. It must start
        /// in the <c>pending_submission</c> state. Only compatible with Bacs
        /// and SEPA mandates.</li>
        /// <li><c>mandate_transferred_with_resubmission</c>: Transitions a
        /// mandate through <c>transferred</c> and resubmits it to the banks,
        /// can be caused be the UK's Current Account Switching Service (CASS)
        /// or when a customer contacts GoCardless to change their bank details.
        /// It must start in the <c>pending_submission</c> state. Only
        /// compatible with Bacs mandates.</li>
        /// <li><c>mandate_suspended_by_payer</c>: Transitions a mandate to
        /// <c>suspended_by_payer</c>, as if payer has suspended the mandate
        /// after it has been setup successfully. It must start in the
        /// <c>activated</c> state. Only compatible with PAY_TO mandates.</li>
        /// <li><c>refund_paid</c>: Transitions a refund to <c>paid</c>. It must
        /// start in either the <c>pending_submission</c> or <c>submitted</c>
        /// state.</li>
        /// <li><c>refund_settled</c>: Transitions a refund to <c>paid</c>, if
        /// it's not already, then generates a payout that includes the refund,
        /// thereby settling the funds. It must start in one of
        /// <c>pending_submission</c>, <c>submitted</c> or <c>paid</c>
        /// states.</li>
        /// <li><c>refund_bounced</c>: Transitions a refund to <c>bounced</c>.
        /// It must start in either the <c>pending_submission</c>,
        /// <c>submitted</c>, or <c>paid</c> state.</li>
        /// <li><c>refund_returned</c>: Transitions a refund to
        /// <c>refund_returned</c>. The refund must start in
        /// <c>pending_submission</c>.</li>
        /// <li><c>payout_bounced</c>: Transitions a payout to <c>bounced</c>.
        /// It must start in the <c>paid</c> state.</li>
        /// <li><c>billing_request_fulfilled</c>: Authorises the billing
        /// request, and then fulfils it. The billing request must be in the
        /// <c>pending</c> state, with all actions completed except for
        /// <c>bank_authorisation</c>. Only billing requests with a
        /// <c>payment_request</c> are supported.</li>
        /// <li><c>billing_request_fulfilled_and_payment_failed</c>: Authorises
        /// the billing request, fulfils it, and moves the associated payment to
        /// <c>failed</c>. The billing request must be in the <c>pending</c>
        /// state, with all actions completed except for
        /// <c>bank_authorisation</c>. Only billing requests with a
        /// <c>payment_request</c> are supported.</li>
        ///
        /// <li><c>billing_request_fulfilled_and_payment_confirmed_to_failed</c>:
        /// Authorises the billing request, fulfils it, moves the associated
        /// payment to <c>confirmed</c> and then moves it to <c>failed</c>. The
        /// billing request must be in the <c>pending</c> state, with all
        /// actions completed except for <c>bank_authorisation</c>. Only billing
        /// requests with a <c>payment_request</c> are supported.</li>
        /// <li><c>billing_request_fulfilled_and_payment_paid_out</c>:
        /// Authorises the billing request, fulfils it, and moves the associated
        /// payment to <c>paid_out</c>. The billing request must be in the
        /// <c>pending</c> state, with all actions completed except for
        /// <c>bank_authorisation</c>. Only billing requests with a
        /// <c>payment_request</c> are supported.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
