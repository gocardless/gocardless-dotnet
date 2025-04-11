using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a export resource.
    ///
    /// File-based exports of data
    /// </summary>
    public class Export
    {
        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The currency of the export (if applicable)
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Download url for the export file. Subject to expiry.
        /// </summary>
        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// The type of the export
        /// </summary>
        [JsonProperty("export_type")]
        public ExportExportType? ExportType { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "EX".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    
    /// <summary>
    /// The type of the export
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum ExportExportType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`export_type` with a value of "payments_index"</summary>
        [EnumMember(Value = "payments_index")]
        PaymentsIndex,
        /// <summary>`export_type` with a value of "events_index"</summary>
        [EnumMember(Value = "events_index")]
        EventsIndex,
        /// <summary>`export_type` with a value of "refunds_index"</summary>
        [EnumMember(Value = "refunds_index")]
        RefundsIndex,
        /// <summary>`export_type` with a value of "payouts_index"</summary>
        [EnumMember(Value = "payouts_index")]
        PayoutsIndex,
        /// <summary>`export_type` with a value of "customers_index"</summary>
        [EnumMember(Value = "customers_index")]
        CustomersIndex,
        /// <summary>`export_type` with a value of "subscriptions_index"</summary>
        [EnumMember(Value = "subscriptions_index")]
        SubscriptionsIndex,
        /// <summary>`export_type` with a value of "payment_events"</summary>
        [EnumMember(Value = "payment_events")]
        PaymentEvents,
        /// <summary>`export_type` with a value of "subscription_events"</summary>
        [EnumMember(Value = "subscription_events")]
        SubscriptionEvents,
        /// <summary>`export_type` with a value of "payout_events"</summary>
        [EnumMember(Value = "payout_events")]
        PayoutEvents,
        /// <summary>`export_type` with a value of "refund_events"</summary>
        [EnumMember(Value = "refund_events")]
        RefundEvents,
        /// <summary>`export_type` with a value of "mandate_events"</summary>
        [EnumMember(Value = "mandate_events")]
        MandateEvents,
        /// <summary>`export_type` with a value of "payout_events_breakdown"</summary>
        [EnumMember(Value = "payout_events_breakdown")]
        PayoutEventsBreakdown,
        /// <summary>`export_type` with a value of "payout_events_reconciliation"</summary>
        [EnumMember(Value = "payout_events_reconciliation")]
        PayoutEventsReconciliation,
        /// <summary>`export_type` with a value of "payout_transactions_breakdown"</summary>
        [EnumMember(Value = "payout_transactions_breakdown")]
        PayoutTransactionsBreakdown,
        /// <summary>`export_type` with a value of "payout_transactions_reconciliation"</summary>
        [EnumMember(Value = "payout_transactions_reconciliation")]
        PayoutTransactionsReconciliation,
        /// <summary>`export_type` with a value of "authorisation_requests"</summary>
        [EnumMember(Value = "authorisation_requests")]
        AuthorisationRequests,
        /// <summary>`export_type` with a value of "customer_bank_accounts"</summary>
        [EnumMember(Value = "customer_bank_accounts")]
        CustomerBankAccounts,
        /// <summary>`export_type` with a value of "users"</summary>
        [EnumMember(Value = "users")]
        Users,
        /// <summary>`export_type` with a value of "organisation_authorisations"</summary>
        [EnumMember(Value = "organisation_authorisations")]
        OrganisationAuthorisations,
        /// <summary>`export_type` with a value of "gc_invalid_authorisation_requests"</summary>
        [EnumMember(Value = "gc_invalid_authorisation_requests")]
        GcInvalidAuthorisationRequests,
        /// <summary>`export_type` with a value of "partner_fees"</summary>
        [EnumMember(Value = "partner_fees")]
        PartnerFees,
        /// <summary>`export_type` with a value of "payments_import_template"</summary>
        [EnumMember(Value = "payments_import_template")]
        PaymentsImportTemplate,
        /// <summary>`export_type` with a value of "payment_account_statement"</summary>
        [EnumMember(Value = "payment_account_statement")]
        PaymentAccountStatement,
    }

}
