using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a mandate import entry resource.
    ///
    /// Mandate Import Entries are added to a <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandate-imports">Mandate
    /// Import</a>.
    /// Each entry corresponds to one mandate to be imported into GoCardless.
    ///
    /// To import a mandate you will need:
    ///
    /// <ol>
    ///   <li>Identifying information about the customer (name/company and
    /// address)</li>
    ///   <li>Bank account details, consisting of an account holder name and
    /// either an IBAN or <a
    /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
    /// bank details</a></li>
    ///   <li>Amendment details (SEPA only)</li>
    /// </ol>
    /// We suggest you provide a <c>record_identifier</c> (which is unique
    /// within the context of a
    /// single mandate import) to help you to identify mandates that have been
    /// created once the
    /// import has been processed by GoCardless. You can
    /// <a
    /// href="https://developer.gocardless.com/api-reference/#mandate-import-entries-list-all-mandate-import-entries">list
    /// the mandate import entries</a>,
    /// match them up in your system using the <c>record_identifier</c>, and
    /// look at the <c>links</c>
    /// fields to find the mandate, customer and customer bank account that have
    /// been imported.
    ///
    /// <p class="restricted-notice">Restricted: This API is currently only
    /// available for approved integrators - please <a
    /// href="mailto:help@gocardless.com">get in touch</a> if you would like to
    /// use this API.</p>
    /// </summary>
    public class MandateImportEntry
    {
        /// <summary>
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when this resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Resources linked to this MandateImportEntry.
        /// </summary>
        [JsonProperty("links")]
        public MandateImportEntryLinks Links { get; set; }

        /// <summary>
        /// Per-resource processing errors
        /// </summary>
        [JsonProperty("processing_errors")]
        public IDictionary<string, string> ProcessingErrors { get; set; }

        /// <summary>
        /// A unique identifier for this entry, which you can use (once the
        /// import has been
        /// processed by GoCardless) to identify the records that have been
        /// created. Limited
        /// to 255 characters.
        /// </summary>
        [JsonProperty("record_identifier")]
        public string RecordIdentifier { get; set; }
    }

    /// <summary>
    /// Represents a mandate import entry link resource.
    ///
    /// Related resources
    /// </summary>
    public class MandateImportEntryLinks
    {
        /// <summary>
        /// The ID of the customer which was created when the mandate import was
        /// processed.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// The ID of the customer bank account which was created when the
        /// mandate import
        /// was processed.
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// The ID of the mandate which was created when the mandate import was
        /// processed.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// The ID of the mandate import. This is returned when you
        /// <a
        /// href="https://developer.gocardless.com/api-reference/#mandate-imports-create-a-new-mandate-import">create
        /// a Mandate Import</a>.
        /// </summary>
        [JsonProperty("mandate_import")]
        public string MandateImport { get; set; }
    }
}
