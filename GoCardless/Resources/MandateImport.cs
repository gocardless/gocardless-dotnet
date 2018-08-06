using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
    ///   1. [Create a mandate
    /// import](#mandate-imports-create-a-new-mandate-import)
    ///   2. [Add entries](#mandate-import-entries-add-a-mandate-import-entry)
    /// to the import
    ///   3. [Submit](#mandate-imports-submit-a-mandate-import) the import
    ///   4. Wait until a member of the GoCardless team approves the import, at
    /// which point the mandates will be created
    ///   5. [Link up the
    /// mandates](#mandate-import-entries-list-all-mandate-import-entries) in
    /// your system
    /// 
    /// When you add entries to your mandate import, they are not turned into
    /// actual mandates
    /// until the mandate import is submitted by you via the API, and then
    /// processed by a member
    /// of the GoCardless team. When that happens, a mandate will be created for
    /// each entry in the import.
    /// 
    /// We will issue a `mandate_created` webhook for each entry, which will be
    /// the same as the webhooks
    /// triggered when [ creating a mandate ](#mandates-create-a-mandate) using
    /// the mandates API. Once these
    /// webhooks start arriving, any reconciliation can now be accomplished by
    /// [checking the current status](#mandate-imports-get-a-mandate-import) of
    /// the mandate import and
    /// [linking up the mandates to your
    /// system](#mandate-import-entries-list-all-mandate-import-entries).
    /// 
    /// <p class="notice">Note that all Mandate Imports have an upper limit of
    /// 30,000 entries, so
    /// we recommend you split your import into several smaller imports if
    /// you're planning to
    /// exceed this threshold.</p>
    /// 
    /// <p class="restricted-notice"><strong>Restricted</strong>: This API is
    /// currently
    /// only available for approved integrators - please <a
    /// href="mailto:help@gocardless.com">get
    /// in touch</a> if you would like to use this API.</p>
    /// </summary>
    public class MandateImport
    {
        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "IM".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The scheme of the mandates to be imported.<br>All mandates in a
        /// single mandate
        /// import must be for the same scheme.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// The status of the mandate import.
        /// <ul>
        /// <li>New mandate imports report the `created` status.</li>
        /// <li>When the integrator has finished adding mandates and
        /// <a href="#mandate-imports-submit-a-mandate-import">submitted</a> the
        /// import, the status will report as `submitted`.</li>
        /// <li>If the integrator decided to
        /// <a href="#mandate-imports-cancel-a-mandate-import">cancel</a> the
        /// mandate import,
        /// the status will report `cancelled`.</li>
        /// <li>Once a mandate import has been approved by a GoCardless team
        /// member, the status will
        /// initially report as `processing` (whilst the mandates are being
        /// imported).</li>
        /// <li>When the mandates have all been imported successfully, the
        /// status will report as `processed`.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public MandateImportStatus? Status { get; set; }
    }
    
    /// <summary>
    /// The status of the mandate import.
    /// <ul>
    /// <li>New mandate imports report the `created` status.</li>
    /// <li>When the integrator has finished adding mandates and
    /// <a href="#mandate-imports-submit-a-mandate-import">submitted</a> the
    /// import, the status will report as `submitted`.</li>
    /// <li>If the integrator decided to
    /// <a href="#mandate-imports-cancel-a-mandate-import">cancel</a> the mandate import,
    /// the status will report `cancelled`.</li>
    /// <li>Once a mandate import has been approved by a GoCardless team member, the status will
    /// initially report as `processing` (whilst the mandates are being imported).</li>
    /// <li>When the mandates have all been imported successfully, the status will report as
    /// `processed`.</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MandateImportStatus {

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
