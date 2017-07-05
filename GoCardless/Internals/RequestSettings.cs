using System;
using System.Net.Http;

namespace GoCardless.Internals
{
    public class RequestSettings
    {
        /// <summary>
        /// Use this to customise the HttpRequestMessage before it is sent to GoCardless
        /// </summary>
        public Action<HttpRequestMessage> CustomiseRequestMessage { get; set; }

        /// <summary>
        /// How many retries to attempt when the request to GoCardless has timed out. If not set,
        /// the value from GoCardlessClient.DefaultNumberOfRetriesOnTimeout will be used, which is set to
        /// 3 but can be changed
        /// </summary>
        public ushort? NumberOfRetriesOnTimeout { get; set; }

        /// <summary>
        /// How long to wait between retries when the request to GoCardless has timed out. If not set,
        /// the value from GoCardlessClient.DefaultWaitBetweenRetries will be used, which is set to
        /// 0.5 seconds but can be changed
        /// </summary>
        public TimeSpan? WaitBetweenRetries { get; set; }
    }
}
