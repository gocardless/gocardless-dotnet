using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

namespace GoCardless.Resources
{

    /// <summary>
    /// Represents a transferred mandate resource.
    ///
    /// Mandates that have been transferred using Current Account Switch Service
    /// </summary>
    public class TransferredMandate
    {
        /// <summary>
        /// Encrypted bank account details
        /// </summary>
        [JsonProperty("encrypted_data")]
        public string EncryptedData { get; set; }

        /// <summary>
        /// Encrypted AES key
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Public key id used to encrypt AES key
        /// </summary>
        [JsonProperty("kid")]
        public string Kid { get; set; }

        /// <summary>
        /// Resources linked to this TransferredMandate.
        /// </summary>
        [JsonProperty("links")]
        public TransferredMandateLinks Links { get; set; }
    }
    
    /// <summary>
    /// Resources linked to this TransferredMandate
    /// </summary>
    public class TransferredMandateLinks
    {
        /// <summary>
        /// The ID of the updated
        /// [customer_bank_account](#core-endpoints-customer-bank-accounts)
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// The ID of the transferred mandate
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }
    }
    
}
