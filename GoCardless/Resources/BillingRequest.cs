using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a billing request resource.
    ///
    /// Billing Requests
    /// </summary>
    public class BillingRequest
    {
        /// <summary>
        /// List of actions that can be performed before this billing request
        /// can be fulfilled.
        /// </summary>
        [JsonProperty("actions")]
        public List<BillingRequestAction> Actions { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BRQ".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequest.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestLinks Links { get; set; }

        /// <summary>
        /// Request for a mandate
        /// </summary>
        [JsonProperty("mandate_request")]
        public BillingRequestMandateRequest MandateRequest { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Request for a one-off strongly authorised payment
        /// </summary>
        [JsonProperty("payment_request")]
        public BillingRequestPaymentRequest PaymentRequest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("resources")]
        public BillingRequestResources Resources { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: the billing_request is pending and can be used</li>
        /// <li>`ready_to_fulfil`: the billing_request is ready to fulfil</li>
        /// <li>`fulfilled`: the billing_request has been fulfilled and a
        /// payment created</li>
        /// <li>`cancelled`: the billing_request has been cancelled and cannot
        /// be used</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
    
    /// <summary>
    /// List of actions that can be performed before this billing request can be
    /// fulfilled.
    /// </summary>
    public class BillingRequestAction
    {
        /// <summary>
        /// Which other action types this action can complete.
        /// </summary>
        [JsonProperty("completes_actions")]
        public List<string> CompletesActions { get; set; }

        /// <summary>
        /// Informs you whether the action is required to fulfil the billing
        /// request or not.
        /// </summary>
        [JsonProperty("required")]
        public bool? Required { get; set; }

        /// <summary>
        /// Requires completing these actions before this action can be
        /// completed.
        /// </summary>
        [JsonProperty("requires_actions")]
        public List<string> RequiresActions { get; set; }

        /// <summary>
        /// Status of the action
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Unique identifier for the action.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// Status of the action
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BillingRequestActionStatus {

        /// <summary>`status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,
        /// <summary>`status` with a value of "completed"</summary>
        [EnumMember(Value = "completed")]
        Completed,
    }

    /// <summary>
    /// Unique identifier for the action.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BillingRequestActionType {

        /// <summary>`type` with a value of "choose_currency"</summary>
        [EnumMember(Value = "choose_currency")]
        ChooseCurrency,
        /// <summary>`type` with a value of "collect_customer_details"</summary>
        [EnumMember(Value = "collect_customer_details")]
        CollectCustomerDetails,
        /// <summary>`type` with a value of "collect_bank_account"</summary>
        [EnumMember(Value = "collect_bank_account")]
        CollectBankAccount,
        /// <summary>`type` with a value of "bank_authorisation"</summary>
        [EnumMember(Value = "bank_authorisation")]
        BankAuthorisation,
    }

    /// <summary>
    /// Resources linked to this BillingRequest
    /// </summary>
    public class BillingRequestLinks
    {
        /// <summary>
        /// (Optional) ID of the [bank
        /// authorisation](#billing-requests-bank-authorisations) that was used
        /// to verify this request.
        /// </summary>
        [JsonProperty("bank_authorisation")]
        public string BankAuthorisation { get; set; }

        /// <summary>
        /// ID of the [customer](#core-endpoints-customers) that will be used
        /// for this request
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// (Optional) ID of the
        /// [customer_bank_account](#core-endpoints-customer-bank-accounts) that
        /// will be used for this request
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// ID of the customer billing detail that will be used for this request
        /// </summary>
        [JsonProperty("customer_billing_detail")]
        public string CustomerBillingDetail { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request mandate request resource.
    ///
    /// Request for a mandate
    /// </summary>
    public class BillingRequestMandateRequest
    {
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently only "GBP" is supported as we only have one
        /// scheme that is per_payment_authorised.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestMandateRequest.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestMandateRequestLinks Links { get; set; }

        /// <summary>
        /// A Direct Debit scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "pad" and "sepa_core" are supported.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this BillingRequestMandateRequest
    /// </summary>
    public class BillingRequestMandateRequestLinks
    {
        /// <summary>
        /// (Optional) ID of the [mandate](#core-endpoints-mandates) that was
        /// created from this mandate request. this mandate request.
        /// 
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request payment request resource.
    ///
    /// Request for a one-off strongly authorised payment
    /// </summary>
    public class BillingRequestPaymentRequest
    {
        /// <summary>
        /// Amount in minor unit (e.g. pence in GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently only "GBP" is supported as we only have one
        /// scheme that is per_payment_authorised.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// A human-readable description of the payment. This will be displayed
        /// to the payer when authorising the billing request.
        /// 
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestPaymentRequest.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestPaymentRequestLinks Links { get; set; }

        /// <summary>
        /// A Direct Debit scheme. Currently "ach", "autogiro", "bacs", "becs",
        /// "becs_nz", "betalingsservice", "pad" and "sepa_core" are supported.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this BillingRequestPaymentRequest
    /// </summary>
    public class BillingRequestPaymentRequestLinks
    {
        /// <summary>
        /// (Optional) ID of the [payment](#core-endpoints-payments) that was
        /// created from this payment request.
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }
    }
    
    public class BillingRequestResources
    {
        /// <summary>
        /// Embedded customer
        /// </summary>
        [JsonProperty("customer")]
        public BillingRequestResourcesCustomer Customer { get; set; }

        /// <summary>
        /// Embedded customer bank account, only if a bank account is linked
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public BillingRequestResourcesCustomerBankAccount CustomerBankAccount { get; set; }

        /// <summary>
        /// Embedded customer billing detail
        /// </summary>
        [JsonProperty("customer_billing_detail")]
        public BillingRequestResourcesCustomerBillingDetail CustomerBillingDetail { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request resources customer resource.
    ///
    /// Embedded customer
    /// </summary>
    public class BillingRequestResourcesCustomer
    {
        /// <summary>
        /// Customer's company name. Required unless a `given_name` and
        /// `family_name` are provided. For Canadian customers, the use of a
        /// `company_name` value will mean that any mandate created from this
        /// customer will be considered to be a "Business PAD" (otherwise, any
        /// mandate will be considered to be a "Personal PAD").
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Customer's email address. Required in most cases, as this allows
        /// GoCardless to send notifications to this customer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Customer's surname. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Customer's first name. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// [ISO 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
        /// code. Used as the language for notification emails sent by
        /// GoCardless if your organisation does not send its own (see
        /// [compliance requirements](#appendix-compliance-requirements)).
        /// Currently only "en", "fr", "de", "pt", "es", "it", "nl", "da", "nb",
        /// "sl", "sv" are supported. If this is not provided, the language will
        /// be chosen based on the `country_code` (if supplied) or default to
        /// "en".
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// [ITU E.123](https://en.wikipedia.org/wiki/E.123) formatted phone
        /// number, including country code.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request resources customer bank account resource.
    ///
    /// Embedded customer bank account, only if a bank account is linked
    /// </summary>
    public class BillingRequestResourcesCustomerBankAccount
    {
        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this is
        /// the same as the name stored with the linked
        /// [creditor](#core-endpoints-creditors). This field will be
        /// transliterated, upcased and truncated to 18 characters. This field
        /// is required unless the request includes a [customer bank account
        /// token](#javascript-flow-customer-bank-account-tokens).
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// The last few digits of the account number. Currently 4 digits for
        /// NZD bank accounts and 2 digits for other currencies.
        /// </summary>
        [JsonProperty("account_number_ending")]
        public string AccountNumberEnding { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public BillingRequestResourcesCustomerBankAccountAccountType? AccountType { get; set; }

        /// <summary>
        /// Name of bank, taken from the bank details.
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements).
        /// Defaults to the country code of the `iban` if supplied, otherwise is
        /// required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Boolean value showing whether the bank account is enabled or
        /// disabled.
        /// </summary>
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BA".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Resources linked to this BillingRequestResourcesCustomerBankAccount.
        /// </summary>
        [JsonProperty("links")]
        public BillingRequestResourcesCustomerBankAccountLinks Links { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }
    
    /// <summary>
    /// Bank account type. Required for USD-denominated bank accounts. Must not be provided for bank
    /// accounts in other currencies. See [local details](#local-bank-details-united-states) for
    /// more information.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BillingRequestResourcesCustomerBankAccountAccountType {

        /// <summary>`account_type` with a value of "savings"</summary>
        [EnumMember(Value = "savings")]
        Savings,
        /// <summary>`account_type` with a value of "checking"</summary>
        [EnumMember(Value = "checking")]
        Checking,
    }

    /// <summary>
    /// Resources linked to this BillingRequestResourcesCustomerBankAccount
    /// </summary>
    public class BillingRequestResourcesCustomerBankAccountLinks
    {
        /// <summary>
        /// ID of the [customer](#core-endpoints-customers) that owns this bank
        /// account.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }
    }
    
    /// <summary>
    /// Represents a billing request resources customer billing detail resource.
    ///
    /// Embedded customer billing detail
    /// </summary>
    public class BillingRequestResourcesCustomerBillingDetail
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
        /// [ISO 3166-1 alpha-2
        /// code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when this
        /// resource was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// For Danish customers only. The civic/company number (CPR or CVR) of
        /// the customer. Must be supplied if the customer's bank account is
        /// denominated in Danish krone (DKK).
        /// </summary>
        [JsonProperty("danish_identity_number")]
        public string DanishIdentityNumber { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The customer's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The customer's address region, county or department. For US
        /// customers a 2 letter
        /// [ISO3166-2:US](https://en.wikipedia.org/wiki/ISO_3166-2:US) state
        /// code is required (e.g. `CA` for California).
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// The schemes associated with this customer billing detail
        /// </summary>
        [JsonProperty("schemes")]
        public List<string> Schemes { get; set; }

        /// <summary>
        /// For Swedish customers only. The civic/company number (personnummer,
        /// samordningsnummer, or organisationsnummer) of the customer. Must be
        /// supplied if the customer's bank account is denominated in Swedish
        /// krona (SEK). This field cannot be changed once it has been set.
        /// </summary>
        [JsonProperty("swedish_identity_number")]
        public string SwedishIdentityNumber { get; set; }
    }
    
    /// <summary>
    /// One of:
    /// <ul>
    /// <li>`pending`: the billing_request is pending and can be used</li>
    /// <li>`ready_to_fulfil`: the billing_request is ready to fulfil</li>
    /// <li>`fulfilled`: the billing_request has been fulfilled and a payment created</li>
    /// <li>`cancelled`: the billing_request has been cancelled and cannot be used</li>
    /// </ul>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BillingRequestStatus {

        /// <summary>`status` with a value of "pending"</summary>
        [EnumMember(Value = "pending")]
        Pending,
        /// <summary>`status` with a value of "ready_to_fulfil"</summary>
        [EnumMember(Value = "ready_to_fulfil")]
        ReadyToFulfil,
        /// <summary>`status` with a value of "fulfilled"</summary>
        [EnumMember(Value = "fulfilled")]
        Fulfilled,
        /// <summary>`status` with a value of "cancelled"</summary>
        [EnumMember(Value = "cancelled")]
        Cancelled,
    }

}
