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
    /// Service class for working with bank authorisation resources.
    ///
    /// Bank Authorisations can be used to authorise Billing Requests.
    /// Authorisations
    /// are created against a specific bank, usually the bank that provides the
    /// payer's
    /// account.
    ///
    /// Creation of Bank Authorisations is only permitted from GoCardless hosted
    /// UIs
    /// (see Billing Request Flows) to ensure we meet regulatory requirements
    /// for
    /// checkout flows.
    /// </summary>
    public class BankAuthorisationService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public BankAuthorisationService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Create a Bank Authorisation.
        /// </summary>
        /// <param name="request">An optional `BankAuthorisationCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank authorisation resource</returns>
        public Task<BankAuthorisationResponse> CreateAsync(
            BankAuthorisationCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BankAuthorisationCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BankAuthorisationResponse>(
                "POST",
                "/bank_authorisations",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "bank_authorisations",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a single bank authorisation.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "BAU".
        /// <param name="request">An optional `BankAuthorisationGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank authorisation resource</returns>
        public Task<BankAuthorisationResponse> GetAsync(
            string identity,
            BankAuthorisationGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BankAuthorisationGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BankAuthorisationResponse>(
                "GET",
                "/bank_authorisations/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Create a Bank Authorisation.
    /// </summary>
    public class BankAuthorisationCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public BankAuthorisationLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a BankAuthorisation.
        /// </summary>
        public class BankAuthorisationLinks
        {
            /// <summary>
            /// ID of the <a
            /// href="https://developer.gocardless.com/api-reference/#billing-requests-billing-requests">billing
            /// request</a> against which this authorisation was created.
            /// </summary>
            [JsonProperty("billing_request")]
            public string BillingRequest { get; set; }
        }

        /// <summary>
        /// URL that the payer can be redirected to after authorising the
        /// payment.
        ///
        /// On completion of bank authorisation, the query parameter of either
        /// <c>outcome=success</c> or <c>outcome=failure</c> will be
        /// appended to the <c>redirect_uri</c> to indicate the result of the
        /// bank authorisation. If the bank authorisation is
        /// expired, the query parameter <c>outcome=timeout</c> will be appended
        /// to the <c>redirect_uri</c>, in which case you should
        /// prompt the user to try the bank authorisation step again.
        ///
        /// Please note: bank authorisations can still fail despite an
        /// <c>outcome=success</c> on the <c>redirect_uri</c>. It is therefore
        /// recommended to wait for the relevant bank authorisation event, such
        /// as <a
        /// href="https://developer.gocardless.com/api-reference/#billing-request-bankauthorisationauthorised"><c>BANK_AUTHORISATION_AUTHORISED</c></a>,
        /// <a
        /// href="https://developer.gocardless.com/api-reference/#billing-request-bankauthorisationdenied"><c>BANK_AUTHORISATION_DENIED</c></a>,
        /// or <a
        /// href="https://developer.gocardless.com/api-reference/#billing-request-bankauthorisationfailed"><c>BANK_AUTHORISATION_FAILED</c></a>
        /// in order to show the correct outcome to the user.
        ///
        /// The BillingRequestFlow ID will also be appended to the
        /// <c>redirect_uri</c> as query parameter <c>id=BRF123</c>.
        ///
        /// Defaults to
        /// <c>https://pay.gocardless.com/billing/static/thankyou</c>.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// Get a single bank authorisation.
    /// </summary>
    public class BankAuthorisationGetRequest { }

    /// <summary>
    /// An API response for a request returning a single bank authorisation.
    /// </summary>
    public class BankAuthorisationResponse : ApiResponse
    {
        /// <summary>
        /// The bank authorisation from the response.
        /// </summary>
        [JsonProperty("bank_authorisations")]
        public BankAuthorisation BankAuthorisation { get; private set; }
    }
}
