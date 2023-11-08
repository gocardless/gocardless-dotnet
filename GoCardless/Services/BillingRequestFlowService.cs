

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
    /// Service class for working with billing request flow resources.
    ///
    /// Billing Request Flows can be created to enable a payer to authorise a
    /// payment created for a scheme with strong payer
    /// authorisation (such as open banking single payments).
    /// </summary>

    public class BillingRequestFlowService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BillingRequestFlowService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new billing request flow.
        /// </summary>
        /// <param name="request">An optional `BillingRequestFlowCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request flow resource</returns>
        public Task<BillingRequestFlowResponse> CreateAsync(BillingRequestFlowCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestFlowCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BillingRequestFlowResponse>("POST", "/billing_request_flows", urlParams, request, null, "billing_request_flows", customiseRequestMessage);
        }

        /// <summary>
        /// Returns the flow having generated a fresh session token which can be
        /// used to power
        /// integrations that manipulate the flow.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BRF".</param> 
        /// <param name="request">An optional `BillingRequestFlowInitialiseRequest` representing the body for this initialise request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single billing request flow resource</returns>
        public Task<BillingRequestFlowResponse> InitialiseAsync(string identity, BillingRequestFlowInitialiseRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BillingRequestFlowInitialiseRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BillingRequestFlowResponse>("POST", "/billing_request_flows/:identity/actions/initialise", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new billing request flow.
    /// </summary>
    public class BillingRequestFlowCreateRequest
    {

        /// <summary>
        /// (Experimental feature) Fulfil the Billing Request on completion of
        /// the flow (true by default). Disabling the auto_fulfil is not allowed
        /// currently.
        /// </summary>
        [JsonProperty("auto_fulfil")]
        public bool? AutoFulfil { get; set; }

        /// <summary>
        /// Identifies whether a Billing Request belongs to a specific customer
        /// </summary>
        [JsonProperty("customer_details_captured")]
        public bool? CustomerDetailsCaptured { get; set; }

        /// <summary>
        /// URL that the payer can be taken to if there isn't a way to progress
        /// ahead in flow.
        /// </summary>
        [JsonProperty("exit_uri")]
        public string ExitUri { get; set; }

        /// <summary>
        /// Sets the default language of the Billing Request Flow and the
        /// customer. [ISO
        /// 639-1](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes) code.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestFlowLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a BillingRequestFlow.
        /// </summary>
        public class BillingRequestFlowLinks
        {
                
                /// <summary>
                            /// ID of the [billing request](#billing-requests-billing-requests)
            /// against which this flow was created.
                /// </summary>
                [JsonProperty("billing_request")]
                public string BillingRequest { get; set; }
        }

        /// <summary>
        /// If true, the payer will not be able to change their bank account
        /// within the flow. If the bank_account details are collected as part
        /// of bank_authorisation then GC will set this value to true mid flow.
        /// 
        /// You can only lock bank account if these have already been completed
        /// as a part of the billing request.
        /// 
        /// </summary>
        [JsonProperty("lock_bank_account")]
        public bool? LockBankAccount { get; set; }

        /// <summary>
        /// If true, the payer will not be able to change their currency/scheme
        /// manually within the flow. Note that this only applies to the mandate
        /// only flows - currency/scheme can never be changed when there is a
        /// specified subscription or payment.
        /// </summary>
        [JsonProperty("lock_currency")]
        public bool? LockCurrency { get; set; }

        /// <summary>
        /// If true, the payer will not be able to edit their customer details
        /// within the flow. If the customer details are collected as part of
        /// bank_authorisation then GC will set this value to true mid flow.
        /// 
        /// You can only lock customer details if these have already been
        /// completed as a part of the billing request.
        /// 
        /// </summary>
        [JsonProperty("lock_customer_details")]
        public bool? LockCustomerDetails { get; set; }

        /// <summary>
        /// Bank account information used to prefill the payment page so your
        /// customer doesn't have to re-type details you already hold about
        /// them. It will be stored unvalidated and the customer will be able to
        /// review and amend it before completing the form.
        /// </summary>
        [JsonProperty("prefilled_bank_account")]
        public BillingRequestFlowPrefilledBankAccount PrefilledBankAccount { get; set; }
        /// <summary>
        /// Bank account information used to prefill the payment page so your
        /// customer doesn't have to re-type details you already hold about
        /// them. It will be stored unvalidated and the customer will be able to
        /// review and amend it before completing the form.
        /// </summary>
        public class BillingRequestFlowPrefilledBankAccount
        {
                
                /// <summary>
                            /// Bank account type for USD-denominated bank accounts. Must not be
            /// provided for bank accounts in other currencies. See [local
            /// details](#local-bank-details-united-states) for more
            /// information.
                /// </summary>
                [JsonProperty("account_type")]
                public BillingRequestFlowAccountType? AccountType { get; set; }
        /// <summary>
        /// Bank account type for USD-denominated bank accounts. Must not be
        /// provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BillingRequestFlowAccountType
        {
    
            /// <summary>`account_type` with a value of "savings"</summary>
            [EnumMember(Value = "savings")]
            Savings,
            /// <summary>`account_type` with a value of "checking"</summary>
            [EnumMember(Value = "checking")]
            Checking,
        }
        }

        /// <summary>
        /// Customer information used to prefill the payment page so your
        /// customer doesn't have to re-type details you already hold about
        /// them. It will be stored unvalidated and the customer will be able to
        /// review and amend it before completing the form.
        /// </summary>
        [JsonProperty("prefilled_customer")]
        public BillingRequestFlowPrefilledCustomer PrefilledCustomer { get; set; }
        /// <summary>
        /// Customer information used to prefill the payment page so your
        /// customer doesn't have to re-type details you already hold about
        /// them. It will be stored unvalidated and the customer will be able to
        /// review and amend it before completing the form.
        /// </summary>
        public class BillingRequestFlowPrefilledCustomer
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
                            /// Customer's company name. Company name should only be provided if
            /// `given_name` and `family_name` are null.
                /// </summary>
                [JsonProperty("company_name")]
                public string CompanyName { get; set; }
                
                /// <summary>
                            /// [ISO 3166-1 alpha-2
            /// code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
                /// </summary>
                [JsonProperty("country_code")]
                public string CountryCode { get; set; }
                
                /// <summary>
                            /// For Danish customers only. The civic/company number (CPR or CVR)
            /// of the customer.
                /// </summary>
                [JsonProperty("danish_identity_number")]
                public string DanishIdentityNumber { get; set; }
                
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
        /// URL that the payer can be redirected to after completing the request
        /// flow.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// If true, the payer will be able to see redirect action buttons on
        /// Thank You page. These action buttons will provide a way to connect
        /// back to the billing request flow app if opened within a mobile app.
        /// For successful flow, the button will take the payer back the billing
        /// request flow where they will see the success screen. For failure,
        /// button will take the payer to url being provided against exit_uri
        /// field.
        /// </summary>
        [JsonProperty("show_redirect_buttons")]
        public bool? ShowRedirectButtons { get; set; }

        /// <summary>
        /// If true, the payer will be able to see a redirect action button on
        /// the Success page. This action button will provide a way to redirect
        /// the payer to the given redirect_uri. This functionality is helpful
        /// when merchants do not want payers to be automatically redirected or
        /// on Android devices, where automatic redirections are not possible.
        /// </summary>
        [JsonProperty("show_success_redirect_button")]
        public bool? ShowSuccessRedirectButton { get; set; }
    }

        
    /// <summary>
    /// Returns the flow having generated a fresh session token which can be
    /// used to power
    /// integrations that manipulate the flow.
    /// </summary>
    public class BillingRequestFlowInitialiseRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single billing request flow.
    /// </summary>
    public class BillingRequestFlowResponse : ApiResponse
    {
        /// <summary>
        /// The billing request flow from the response.
        /// </summary>
        [JsonProperty("billing_request_flows")]
        public BillingRequestFlow BillingRequestFlow { get; private set; }
    }
}
