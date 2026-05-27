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
    /// Service class for working with mandate import resources.
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
    /// 30,000 entries, so we recommend you split your import into several
    /// smaller imports if you're planning to exceed this threshold.</p>
    ///
    /// <p class="restricted-notice"><strong>Restricted</strong>: This API is
    /// currently only available for approved integrators - please <a
    /// href="mailto:help@gocardless.com">get in touch</a> if you would like to
    /// use this API.</p>
    /// </summary>
    public class MandateImportService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public MandateImportService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Mandate imports are first created, before mandates are added
        /// one-at-a-time, so
        /// this endpoint merely signals the start of the import process. Once
        /// you've finished
        /// adding entries to an import, you should
        /// [submit](#mandate-imports-submit-a-mandate-import) it.
        /// </summary>
        /// <param name="request">An optional `MandateImportCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single mandate import resource</returns>
        public Task<MandateImportResponse> CreateAsync(
            MandateImportCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<MandateImportResponse>(
                "POST",
                "/mandate_imports",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "mandate_imports",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a single mandate import.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "IM".</param>
        /// <param name="request">An optional `MandateImportGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single mandate import resource</returns>
        public Task<MandateImportResponse> GetAsync(
            string identity,
            MandateImportGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<MandateImportResponse>(
                "GET",
                "/mandate_imports/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Submits the mandate import, which allows it to be processed by a
        /// member of the
        /// GoCardless team. Once the import has been submitted, it can no
        /// longer have entries
        /// added to it.
        ///
        /// In our sandbox environment, to aid development, we automatically
        /// process mandate
        /// imports approximately 10 seconds after they are submitted. This will
        /// allow you to
        /// test both the "submitted" response and wait for the webhook to
        /// confirm the
        /// processing has begun.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "IM".</param>
        /// <param name="request">An optional `MandateImportSubmitRequest` representing the body for this submit request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single mandate import resource</returns>
        public Task<MandateImportResponse> SubmitAsync(
            string identity,
            MandateImportSubmitRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportSubmitRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<MandateImportResponse>(
                "POST",
                "/mandate_imports/:identity/actions/submit",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Cancels the mandate import, which aborts the import process and
        /// stops the mandates
        /// being set up in GoCardless. Once the import has been cancelled, it
        /// can no longer have
        /// entries added to it. Mandate imports which have already been
        /// submitted or processed
        /// cannot be cancelled.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "IM".</param>
        /// <param name="request">An optional `MandateImportCancelRequest` representing the body for this cancel request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single mandate import resource</returns>
        public Task<MandateImportResponse> CancelAsync(
            string identity,
            MandateImportCancelRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new MandateImportCancelRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<MandateImportResponse>(
                "POST",
                "/mandate_imports/:identity/actions/cancel",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Mandate imports are first created, before mandates are added
    /// one-at-a-time, so
    /// this endpoint merely signals the start of the import process. Once
    /// you've finished
    /// adding entries to an import, you should
    /// [submit](#mandate-imports-submit-a-mandate-import) it.
    /// </summary>
    public class MandateImportCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public MandateImportLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a MandateImport.
        /// </summary>
        public class MandateImportLinks
        {
            /// <summary>
            /// ID of the associated creditor. Only required if your account
            /// manages multiple creditors.
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }
        }

        /// <summary>
        /// A bank payment scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "faster_payments", "pad", "pay_to"
        /// and "sepa_core" are supported.
        /// </summary>
        [JsonProperty("scheme")]
        public MandateImportScheme? Scheme { get; set; }

        /// <summary>
        /// A bank payment scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "faster_payments", "pad", "pay_to"
        /// and "sepa_core" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MandateImportScheme
        {
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
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// Returns a single mandate import.
    /// </summary>
    public class MandateImportGetRequest { }

    /// <summary>
    /// Submits the mandate import, which allows it to be processed by a member
    /// of the
    /// GoCardless team. Once the import has been submitted, it can no longer
    /// have entries
    /// added to it.
    ///
    /// In our sandbox environment, to aid development, we automatically process
    /// mandate
    /// imports approximately 10 seconds after they are submitted. This will
    /// allow you to
    /// test both the "submitted" response and wait for the webhook to confirm
    /// the
    /// processing has begun.
    /// </summary>
    public class MandateImportSubmitRequest { }

    /// <summary>
    /// Cancels the mandate import, which aborts the import process and stops
    /// the mandates
    /// being set up in GoCardless. Once the import has been cancelled, it can
    /// no longer have
    /// entries added to it. Mandate imports which have already been submitted
    /// or processed
    /// cannot be cancelled.
    /// </summary>
    public class MandateImportCancelRequest { }

    /// <summary>
    /// An API response for a request returning a single mandate import.
    /// </summary>
    public class MandateImportResponse : ApiResponse
    {
        /// <summary>
        /// The mandate import from the response.
        /// </summary>
        [JsonProperty("mandate_imports")]
        public MandateImport MandateImport { get; private set; }
    }
}
