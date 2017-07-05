using System;

namespace GoCardless
{
    /// <summary>
    ///Base class for client library exceptions.
    /// </summary>
    public abstract class GoCardlessException : Exception
    {
        protected GoCardlessException(String message) : base(message)
        {
        }

    }
}