using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        /// Encrypted customer bank account details, containing:
        /// `iban`, `account_holder_name`, `swift_bank_code`,
        /// `swift_branch_code`, `swift_account_number`
        /// </summary>
        [JsonProperty("encrypted_customer_bank_details")]
        public string EncryptedCustomerBankDetails { get; set; }

        /// <summary>
        /// Random AES-256 key used to encrypt bank account details, itself
        /// encrypted with your public key.
        /// </summary>
        [JsonProperty("encrypted_decryption_key")]
        public string EncryptedDecryptionKey { get; set; }

        /// <summary>
        /// Resources linked to this TransferredMandate.
        /// </summary>
        [JsonProperty("links")]
        public TransferredMandateLinks Links { get; set; }

        /// <summary>
        /// The ID of an RSA-2048 public key, from your JWKS, used to encrypt
        /// the AES key.
        /// </summary>
        [JsonProperty("public_key_id")]
        public string PublicKeyId { get; set; }
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
