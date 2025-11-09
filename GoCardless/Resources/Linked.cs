using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoCardless.Resources
{
    public class Linked
    {
        [JsonProperty("billing_requests")]
        public List<BillingRequest> BillingRequests { get; private set; }

        [JsonProperty("creditors")]
        public List<Creditor> Creditors { get; private set; }

        [JsonProperty("customers")]
        public List<Customer> Customers { get; private set; }

        [JsonProperty("instalment_schedules")]
        public List<InstalmentSchedule> InstalmentSchedules { get; private set; }

        [JsonProperty("mandates")]
        public List<Mandate> Mandates { get; private set; }

        [JsonProperty("outbound_payments")]
        public List<OutboundPayment> OutboundPayments { get; private set; }

        [JsonProperty("payer_authorisations")]
        public List<PayerAuthorisation> PayerAuthorisations { get; private set; }

        [JsonProperty("payments")]
        public List<Payment> Payments { get; private set; }

        [JsonProperty("payouts")]
        public List<Payout> Payouts { get; private set; }

        [JsonProperty("refunds")]
        public List<Refund> Refunds { get; private set; }

        [JsonProperty("scheme_identifiers")]
        public List<SchemeIdentifier> SchemeIdentifiers { get; private set; }

        [JsonProperty("subscriptions")]
        public List<Subscription> Subscriptions { get; private set; }
    }
}
