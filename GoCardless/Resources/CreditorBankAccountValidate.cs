using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a creditor bank account validate resource.
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
    public class CreditorBankAccountValidate
    {
        /// <summary>
        /// Name of bank, taken from the bank details.
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// URL of the bank's icon.
        /// </summary>
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        /// <summary>
        /// The reason why the bank details are invalid, if applicable.
        /// </summary>
        [JsonProperty("invalid_reasons")]
        public List<CreditorBankAccountValidateInvalidReason> InvalidReasons { get; set; }

        /// <summary>
        /// Whether the bank account details are valid.
        /// </summary>
        [JsonProperty("is_valid")]
        public bool? IsValid { get; set; }
    }
    
    /// <summary>
    /// The reason why the bank details are invalid, if applicable.
    /// </summary>
    public class CreditorBankAccountValidateInvalidReason
    {
        /// <summary>
        /// The name of the field with the error
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// The error message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
    
}
