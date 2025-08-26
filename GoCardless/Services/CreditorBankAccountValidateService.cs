

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
    /// Service class for working with creditor bank account validate resources.
    ///
    /// Creditor Bank Accounts hold the bank details of a
    /// [creditor](#core-endpoints-creditors). These are the bank accounts which
    /// your [payouts](#core-endpoints-payouts) will be sent to.
    /// 
    /// When all locale details and Iban are supplied validates creditor bank
    /// details without creating a creditor bank account and also provdes bank
    /// details such as name and icon url. When partial details are are provided
    /// the endpoint will only provide bank details such as name and icon url
    /// but will not be able to determine if the provided details are valid.
    /// 
    /// <p class="restricted-notice"><strong>Restricted</strong>: This API is
    /// not available for partner integrations.</p>
    /// </summary>

    public class CreditorBankAccountValidateService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public CreditorBankAccountValidateService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Validate bank details without creating a creditor bank account
        /// </summary>
        /// <param name="request">An optional `CreditorBankAccountValidateValidateRequest` representing the body for this validate request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single creditor bank account validate resource</returns>
        public Task<CreditorBankAccountValidateResponse> ValidateAsync(CreditorBankAccountValidateValidateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorBankAccountValidateValidateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<CreditorBankAccountValidateResponse>("POST", "/creditor_bank_accounts/validate", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Validate bank details without creating a creditor bank account
    /// </summary>
    public class CreditorBankAccountValidateValidateRequest
    {

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide
        /// [local details](#appendix-local-bank-details). IBANs are not
        /// accepted for Swedish bank accounts denominated in SEK - you must
        /// supply [local details](#local-bank-details-sweden).
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("local_details")]
        public CreditorBankAccountValidateLocalDetails LocalDetails { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public class CreditorBankAccountValidateLocalDetails
        {
                
                /// <summary>
                            /// Bank account number - see [local
            /// details](#appendix-local-bank-details) for more information.
            /// Alternatively you can provide an `iban`.
                /// </summary>
                [JsonProperty("bank_number")]
                public string BankNumber { get; set; }
                
                /// <summary>
                            /// Branch code - see [local details](#appendix-local-bank-details)
            /// for more information. Alternatively you can provide an `iban`.
                /// </summary>
                [JsonProperty("sort_code")]
                public string SortCode { get; set; }
        }
    }

    /// <summary>
    /// An API response for a request returning a single creditor bank account validate.
    /// </summary>
    public class CreditorBankAccountValidateResponse : ApiResponse
    {
        /// <summary>
        /// The creditor bank account validate from the response.
        /// </summary>
        [JsonProperty("creditor_bank_account_validates")]
        public CreditorBankAccountValidate CreditorBankAccountValidate { get; private set; }
    }
}
