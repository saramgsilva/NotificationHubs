using System.Collections.Generic;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the DeviceInfo.
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// Gets or sets the registration identifier.
        /// </summary>
        /// <value>The registration identifier.</value>
        public string RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the handle.
        /// </summary>
        /// <value>The handle.</value>
        public string Handle { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public IEnumerable<string> Tags { get; set; }
    }
}