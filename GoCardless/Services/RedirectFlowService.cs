

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
    /// Service class for working with redirect flow resources.
    ///
    /// Redirect flows enable you to use GoCardless' [hosted payment
    /// pages](https://pay-sandbox.gocardless.com/AL000000AKFPFF) to set up
    /// mandates with your customers. These pages are fully compliant and have
    /// been translated into Dutch, French, German, Italian, Portuguese, Spanish
    /// and Swedish.
    /// 
    /// The overall flow is:
    /// 
    /// 1.
    /// You [create](#redirect-flows-create-a-redirect-flow) a redirect flow for
    /// your customer, and redirect them to the returned redirect url, e.g.
    /// `https://pay.gocardless.com/flow/RE123`.
    /// 
    /// 2. Your
    /// customer supplies their name, email, address, and bank account details,
    /// and submits the form. This securely stores their details, and redirects
    /// them back to your `success_redirect_url` with `redirect_flow_id=RE123`
    /// in the querystring.
    /// 
    /// 3. You
    /// [complete](#redirect-flows-complete-a-redirect-flow) the redirect flow,
    /// which creates a [customer](#core-endpoints-customers), [customer bank
    /// account](#core-endpoints-customer-bank-accounts), and
    /// [mandate](#core-endpoints-mandates), and returns the ID of the mandate.
    /// You may wish to create a [subscription](#core-endpoints-subscriptions)
    /// or [payment](#core-endpoints-payments) at this point.
    /// 
    ///
    /// Once you have [completed](#redirect-flows-complete-a-redirect-flow) the
    /// redirect flow via the API, you should display a confirmation page to
    /// your customer, confirming that their Direct Debit has been set up. You
    /// can build your own page, or redirect to the one we provide in the
    /// `confirmation_url` attribute of the redirect flow.
    /// 
    ///
    /// Redirect flows expire 30 minutes after they are first created. You
    /// cannot complete an expired redirect flow.
    /// </summary>

    public class RedirectFlowService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public RedirectFlowService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a redirect flow object which can then be used to redirect
        /// your customer to the GoCardless hosted payment pages.
        /// </summary>
        /// <returns>A single redirect flow resource</returns>
        public Task<RedirectFlowResponse> CreateAsync(RedirectFlowCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RedirectFlowCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<RedirectFlowResponse>("POST", "/redirect_flows", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "redirect_flows", customiseRequestMessage);
        }

        /// <summary>
        /// Returns all details about a single redirect flow
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "RE".</param>
        /// <returns>A single redirect flow resource</returns>
        public Task<RedirectFlowResponse> GetAsync(string identity, RedirectFlowGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RedirectFlowGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<RedirectFlowResponse>("GET", "/redirect_flows/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// This creates a [customer](#core-endpoints-customers), [customer bank
        /// account](#core-endpoints-customer-bank-accounts), and
        /// [mandate](#core-endpoints-mandates) using the details supplied by
        /// your customer and returns the ID of the created mandate.
        ///
        /// 
        /// This will return a `redirect_flow_incomplete` error if
        /// your customer has not yet been redirected back to your site, and a
        /// `redirect_flow_already_completed` error if your integration has
        /// already completed this flow. It will return a `bad_request` error if
        /// the `session_token` differs to the one supplied when the redirect
        /// flow was created.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "RE".</param>
        /// <returns>A single redirect flow resource</returns>
        public Task<RedirectFlowResponse> CompleteAsync(string identity, RedirectFlowCompleteRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new RedirectFlowCompleteRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<RedirectFlowResponse>("POST", "/redirect_flows/:identity/actions/complete", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    public class RedirectFlowCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// A description of the item the customer is paying for. This will be
        /// shown on the hosted payment pages.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("links")]
        public RedirectFlowLinks Links { get; set; }
        public class RedirectFlowLinks
        {

            /// <summary>
            /// The [creditor](#core-endpoints-creditors) for whom the mandate
            /// will be created. The `name` of the creditor will be displayed on
            /// the payment page. Required if your account manages multiple
            /// creditors.
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }
        }

        /// <summary>
        /// Information used to prefill the payment page so your customer
        /// doesn't have to re-type details you already hold about them. It will
        /// be stored unvalidated and the customer will be able to review and
        /// amend it before completing the form.
        /// </summary>
        [JsonProperty("prefilled_customer")]
        public RedirectFlowPrefilledCustomer PrefilledCustomer { get; set; }
        public class RedirectFlowPrefilledCustomer
        {

            /// <summary>
            /// The first line of the customer's address.
            /// </summary>
            [JsonProperty("address_line1")]
            public string AddressLine1 { get; set; }

            /// <summary>
            /// The second line of the customer's address.
            /// </summary>
            [JsonProperty("address_line2")]
            public string AddressLine2 { get; set; }

            /// <summary>
            /// The third line of the customer's address.
            /// </summary>
            [JsonProperty("address_line3")]
            public string AddressLine3 { get; set; }

            /// <summary>
            /// The city of the customer's address.
            /// </summary>
            [JsonProperty("city")]
            public string City { get; set; }

            /// <summary>
            /// Customer's company name.
            /// </summary>
            [JsonProperty("company_name")]
            public string CompanyName { get; set; }

            /// <summary>
            /// [ISO
            /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
            /// alpha-2 code.
            /// </summary>
            [JsonProperty("country_code")]
            public string CountryCode { get; set; }

            /// <summary>
            /// Customer's email address.
            /// </summary>
            [JsonProperty("email")]
            public string Email { get; set; }

            /// <summary>
            /// Customer's surname.
            /// </summary>
            [JsonProperty("family_name")]
            public string FamilyName { get; set; }

            /// <summary>
            /// Customer's first name.
            /// </summary>
            [JsonProperty("given_name")]
            public string GivenName { get; set; }

            /// <summary>
            /// [ISO
            /// 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
            /// code.
            /// </summary>
            [JsonProperty("language")]
            public string Language { get; set; }

            /// <summary>
            /// The customer's postal code.
            /// </summary>
            [JsonProperty("postal_code")]
            public string PostalCode { get; set; }

            /// <summary>
            /// The customer's address region, county or department.
            /// </summary>
            [JsonProperty("region")]
            public string Region { get; set; }

            /// <summary>
            /// For Swedish customers only. The civic/company number
            /// (personnummer, samordningsnummer, or organisationsnummer) of the
            /// customer.
            /// </summary>
            [JsonProperty("swedish_identity_number")]
            public string SwedishIdentityNumber { get; set; }
        }

        /// <summary>
        /// The Direct Debit scheme of the mandate. If specified, the payment
        /// pages will only allow the set-up of a mandate for the specified
        /// scheme. It is recommended that you leave this blank so the most
        /// appropriate scheme is picked based on the customer's bank account.
        /// </summary>
        [JsonProperty("scheme")]
        public RedirectFlowScheme? Scheme { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum RedirectFlowScheme
        {
            /// <summary>
            /// The Direct Debit scheme of the mandate. If specified, the
            /// payment pages will only allow the set-up of a mandate for the
            /// specified scheme. It is recommended that you leave this blank so
            /// the most appropriate scheme is picked based on the customer's
            /// bank account.
            /// </summary>
    
            [EnumMember(Value = "autogiro")]
            Autogiro,
            [EnumMember(Value = "bacs")]
            Bacs,
            [EnumMember(Value = "sepa_core")]
            SepaCore,
        }

        /// <summary>
        /// The customer's session ID must be provided when the redirect flow is
        /// set up and again when it is completed. This allows integrators to
        /// ensure that the user who was originally sent to the GoCardless
        /// payment pages is the one who has completed them.
        /// </summary>
        [JsonProperty("session_token")]
        public string SessionToken { get; set; }

        /// <summary>
        /// The URL to redirect to upon successful mandate setup. You must use a
        /// URL beginning `https` in the live environment.
        /// </summary>
        [JsonProperty("success_redirect_url")]
        public string SuccessRedirectUrl { get; set; }

        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    public class RedirectFlowGetRequest
    {
    }

        
    public class RedirectFlowCompleteRequest
    {

        /// <summary>
        /// The customer's session ID must be provided when the redirect flow is
        /// set up and again when it is completed. This allows integrators to
        /// ensure that the user who was originally sent to the GoCardless
        /// payment pages is the one who has completed them.
        /// </summary>
        [JsonProperty("session_token")]
        public string SessionToken { get; set; }
    }

    public class RedirectFlowResponse : ApiResponse
    {
        [JsonProperty("redirect_flows")]
        public RedirectFlow RedirectFlow { get; private set; }
    }
}
