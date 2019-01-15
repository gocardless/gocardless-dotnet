

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Service class for working with customer notification resources.
    ///
    /// Customer Notifications represent the notification which is due to be
    /// sent to a customer
    /// after an event has happened. The event, the resource and the customer to
    /// be notified
    /// are all identified in the `links` property.
    ///
    /// Note that these are ephemeral records - once the notification has been
    /// actioned in some
    /// way, it is no longer visible using this API.
    ///
    /// <p class="restricted-notice"><strong>Restricted</strong>: This API is
    /// currently
    /// only available for approved integrators - please <a
    /// href="mailto:help@gocardless.com">get
    /// in touch</a> if you would like to use this API.</p>
    ///
    /// </summary>

    public class CustomerNotificationService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public CustomerNotificationService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// "Handling" a notification means that you have sent the notification
        /// yourself (and
        /// don't want GoCardless to send it).
        /// If the notification has already been actioned, or the deadline to
        /// notify has passed,
        /// this endpoint will return an `already_actioned` error and you should
        /// not take
        /// further action.
        ///
        /// </summary>
        /// <param name="identity">The id of the notification.</param>
        /// <param name="request">An optional `CustomerNotificationHandleRequest` representing the body for this handle request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single customer notification resource</returns>
        public Task<CustomerNotificationResponse> HandleAsync(string identity, CustomerNotificationHandleRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerNotificationHandleRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CustomerNotificationResponse>(HttpMethod.Post, "/customer_notifications/:identity/actions/handle", urlParams, request, null, "data", customiseRequestMessage);
        }
    }


    /// <summary>
    /// "Handling" a notification means that you have sent the notification
    /// yourself (and
    /// don't want GoCardless to send it).
    /// If the notification has already been actioned, or the deadline to notify
    /// has passed,
    /// this endpoint will return an `already_actioned` error and you should not
    /// take
    /// further action.
    ///
    /// </summary>
    public class CustomerNotificationHandleRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single customer notification.
    /// </summary>
    public class CustomerNotificationResponse : ApiResponse
    {
        /// <summary>
        /// The customer notification from the response.
        /// </summary>
        [JsonProperty("customer_notifications")]
        public CustomerNotification CustomerNotification { get; private set; }
    }
}
