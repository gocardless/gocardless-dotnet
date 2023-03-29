using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a billing request flow resource.
    ///
    /// Billing Request Flows can be created to enable a payer to authorise a
    /// payment created for a scheme with strong payer
    /// authorisation (such as open banking single payments).
    /// </summary>
    public class BillingRequestFlow
    {
        /// <summary>
        /// URL for a GC-controlled flow which will allow the payer to fulfil
        /// the billing request
        /// </summary>
        [JsonProperty("authorisation_url")]
        public string AuthorisationUrl { get; set; }

        /// <summary>
        /// (Experimental feature) Fulfil the Billing Request on completion of
        /// the flow (true by default). Disabling the auto_fulfil is not allowed
        /// currently.
        /// </summary>
        [JsonProperty("auto_fulfil")]
        public bool? AutoFulfil { get; set; }

        /// <summary>
        /// Timestamp when the flow was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// URL that the payer can be taken to if there isn't a way to progress
        /// ahead in flow.
        /// </summary>
        [JsonProperty("exit_uri")]
        public string ExitUri { get; set; }

        /// <summary>
        /// Timestamp when the flow will expire. Each flow currently lasts for 7
        /// days.
        /// </summary>
        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BRF".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Sets the default language of the Billing Request Flow and the
        /// customer. [ISO
        /// 639-1](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes) code.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestFlow.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestFlowLinks Links { get; set; }

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
        /// Customer information used to prefill the payment page so your
        /// customer doesn't have to re-type details you already hold about
        /// them. It will be stored unvalidated and the customer will be able to
        /// review and amend it before completing the form.
        /// </summary>
        [JsonProperty("prefilled_customer")]
        public BillingRequestFlowPrefilledCustomer PrefilledCustomer { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after completing the request
        /// flow.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// Session token populated when responding to the initialise action
        /// </summary>
        [JsonProperty("session_token")]
        public string SessionToken { get; set; }

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
        /// the payer to the given redirect_uri. This functionality is
        /// applicable only for Android users as automatic redirection is not
        /// possible in such cases.
        /// </summary>
        [JsonProperty("show_success_redirect_button")]
        public bool? ShowSuccessRedirectButton { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this BillingRequestFlow
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
    /// Represents a billing request flow prefilled bank account resource.
    ///
    /// Bank account information used to prefill the payment page so your
    /// customer doesn't have to re-type details you already hold about them. It
    /// will be stored unvalidated and the customer will be able to review and
    /// amend it before completing the form.
    /// </summary>
    public class BillingRequestFlowPrefilledBankAccount
    {
        /// <summary>
        /// Bank account type for USD-denominated bank accounts. Must not be
        /// provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public BillingRequestFlowPrefilledBankAccountAccountType? AccountType { get; set; }
    }
    
    /// <summary>
    /// Bank account type for USD-denominated bank accounts. Must not be provided for bank accounts
    /// in other currencies. See [local details](#local-bank-details-united-states) for more
    /// information.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BillingRequestFlowPrefilledBankAccountAccountType {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`account_type` with a value of "savings"</summary>
        [EnumMember(Value = "savings")]
        Savings,
        /// <summary>`account_type` with a value of "checking"</summary>
        [EnumMember(Value = "checking")]
        Checking,
    }

    /// <summary>
    /// Represents a billing request flow prefilled customer resource.
    ///
    /// Customer information used to prefill the payment page so your customer
    /// doesn't have to re-type details you already hold about them. It will be
    /// stored unvalidated and the customer will be able to review and amend it
    /// before completing the form.
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
        /// For Danish customers only. The civic/company number (CPR or CVR) of
        /// the customer.
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
        /// For Swedish customers only. The civic/company number (personnummer,
        /// samordningsnummer, or organisationsnummer) of the customer.
        /// </summary>
        [JsonProperty("swedish_identity_number")]
        public string SwedishIdentityNumber { get; set; }
    }
    
}
