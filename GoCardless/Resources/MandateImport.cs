using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a mandate import resource.
    ///
    /// Mandate Imports allow you to migrate existing mandates from other
    /// providers into the
    /// GoCardless platform.
    ///
    /// The process is as follows:
    ///
    /// <ol>
    /// <li><a
    /// href="https://developer.gocardless.com/api-reference/#mandate-imports-create-a-new-mandate-import">Create
    /// a mandate import</a></li>
    /// <li><a
    /// href="https://developer.gocardless.com/api-reference/#mandate-import-entries-add-a-mandate-import-entry">Add
    /// entries</a> to the import</li>
    /// <li><a
    /// href="https://developer.gocardless.com/api-reference/#mandate-imports-submit-a-mandate-import">Submit</a>
    /// the import</li>
    /// <li>Wait until a member of the GoCardless team approves the import, at
    /// which point the mandates will be created</li>
    /// <li><a
    /// href="https://developer.gocardless.com/api-reference/#mandate-import-entries-list-all-mandate-import-entries">Link
    /// up the mandates</a> in your system</li>
    /// </ol>
    /// When you add entries to your mandate import, they are not turned into
    /// actual mandates
    /// until the mandate import is submitted by you via the API, and then
    /// processed by a member
    /// of the GoCardless team. When that happens, a mandate will be created for
    /// each entry in the import.
    ///
    /// We will issue a <c>mandate_created</c> webhook for each entry, which
    /// will be the same as the webhooks
    /// triggered when <a
    /// href="https://developer.gocardless.com/api-reference/#mandates-create-a-mandate">
    /// creating a mandate </a> using the mandates API. Once these
    /// webhooks start arriving, any reconciliation can now be accomplished by
    /// <a
    /// href="https://developer.gocardless.com/api-reference/#mandate-imports-get-a-mandate-import">checking
    /// the current status</a> of the mandate import and
    /// <a
    /// href="https://developer.gocardless.com/api-reference/#mandate-import-entries-list-all-mandate-import-entries">linking
    /// up the mandates to your system</a>.
    ///
    /// <p class="notice">Note that all Mandate Imports have an upper limit of
    /// 30,000 entries, so we recommend you split your import into several
    /// smaller imports if you're planning to exceed this threshold.</p>
    /// <p class="restricted-notice">Restricted: This API is currently only
    /// available for approved integrators - please <a
    /// href="mailto:help@gocardless.com">get in touch</a> if you would like to
    /// use this API.</p>
    /// </summary>
    public class MandateImport
    {
        /// <summary>
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when this resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "IM".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this MandateImport.
        /// </summary>
        [JsonProperty("links")]
        public MandateImportLinks Links { get; set; }

        /// <summary>
        /// The scheme of the mandates to be imported.<br></br>All mandates in a
        /// single mandate
        /// import must be for the same scheme.
        /// </summary>
        [JsonProperty("scheme")]
        public MandateImportScheme? Scheme { get; set; }

        /// <summary>
        /// The status of the mandate import.
        ///
        /// <ul>
        /// <li><c>created</c>: A new mandate import.</li>
        /// <li><c>submitted</c>: After the integrator has finished adding
        /// mandates and <a
        /// href="https://developer.gocardless.com/api-reference/#mandate-imports-submit-a-mandate-import">submitted</a>
        /// the import.</li>
        /// <li><c>cancelled</c>: If the integrator <a
        /// href="https://developer.gocardless.com/api-reference/#mandate-imports-cancel-a-mandate-import">cancelled</a>
        /// the mandate import.</li>
        /// <li><c>processing</c>: Once a mandate import has been approved by a
        /// GoCardless team member it will be in this state while mandates are
        /// imported.</li>
        /// <li><c>processed</c>: When all mandates have been imported
        /// successfully.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public MandateImportStatus? Status { get; set; }
    }

    /// <summary>
    /// Represents a mandate import link resource.
    ///
    /// Related resources
    /// </summary>
    public class MandateImportLinks
    {
        /// <summary>
        /// ID of the associated creditor.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }
    }

    /// <summary>
    /// The scheme of the mandates to be imported.<br></br>All mandates in a single mandate
    /// import must be for the same scheme.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum MandateImportScheme
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`scheme` with a value of "ach"</summary>
        [EnumMember(Value = "ach")]
        Ach,

        /// <summary>`scheme` with a value of "autogiro"</summary>
        [EnumMember(Value = "autogiro")]
        Autogiro,

        /// <summary>`scheme` with a value of "bacs"</summary>
        [EnumMember(Value = "bacs")]
        Bacs,

        /// <summary>`scheme` with a value of "becs"</summary>
        [EnumMember(Value = "becs")]
        Becs,

        /// <summary>`scheme` with a value of "becs_nz"</summary>
        [EnumMember(Value = "becs_nz")]
        BecsNz,

        /// <summary>`scheme` with a value of "betalingsservice"</summary>
        [EnumMember(Value = "betalingsservice")]
        Betalingsservice,

        /// <summary>`scheme` with a value of "faster_payments"</summary>
        [EnumMember(Value = "faster_payments")]
        FasterPayments,

        /// <summary>`scheme` with a value of "pad"</summary>
        [EnumMember(Value = "pad")]
        Pad,

        /// <summary>`scheme` with a value of "pay_to"</summary>
        [EnumMember(Value = "pay_to")]
        PayTo,

        /// <summary>`scheme` with a value of "sepa_core"</summary>
        [EnumMember(Value = "sepa_core")]
        SepaCore,
    }

    /// <summary>
    /// The status of the mandate import.
    ///
    /// <ul>
    /// <li><c>created</c>: A new mandate import.</li>
    /// <li><c>submitted</c>: After the integrator has finished adding mandates and <a
    /// href="https://developer.gocardless.com/api-reference/#mandate-imports-submit-a-mandate-import">submitted</a>
    /// the import.</li>
    /// <li><c>cancelled</c>: If the integrator <a
    /// href="https://developer.gocardless.com/api-reference/#mandate-imports-cancel-a-mandate-import">cancelled</a>
    /// the mandate import.</li>
    /// <li><c>processing</c>: Once a mandate import has been approved by a GoCardless team member
    /// it will be in this state while mandates are imported.</li>
    /// <li><c>processed</c>: When all mandates have been imported successfully.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum MandateImportStatus
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`status` with a value of "created"</summary>
        [EnumMember(Value = "created")]
        Created,

        /// <summary>`status` with a value of "submitted"</summary>
        [EnumMember(Value = "submitted")]
        Submitted,

        /// <summary>`status` with a value of "cancelled"</summary>
        [EnumMember(Value = "cancelled")]
        Cancelled,

        /// <summary>`status` with a value of "processing"</summary>
        [EnumMember(Value = "processing")]
        Processing,

        /// <summary>`status` with a value of "processed"</summary>
        [EnumMember(Value = "processed")]
        Processed,
    }
}
