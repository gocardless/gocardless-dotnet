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
        /// Resources linked to this BillingRequestFlow.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestFlowLinks Links { get; set; }

        /// <summary>
        /// If true, the payer will not be able to change their bank account
        /// within the flow. If the bank_account details are collected as part
        /// of bank_authorisation then GC will set this value to true mid flow
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
        /// bank_authorisation then GC will set this value to true mid flow
        /// </summary>
        [JsonProperty("lock_customer_details")]
        public bool? LockCustomerDetails { get; set; }

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
    
}
