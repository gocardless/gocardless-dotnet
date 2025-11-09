using System;

namespace GoCardless.Exceptions
{
    /// <summary>
    ///Thrown when a webhook body's signature header does not match the computed
    ///HMAC of the body.
    /// </summary>
    public class InvalidSignatureException : Exception { }
}
