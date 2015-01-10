using System;

namespace NotificationHubsSample.WinUsingAMS.Model
{
    /// <summary>
    /// Define the Car.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is electric.
        /// </summary>
        /// <value><c>true</c> if this instance is electric; otherwise, <c>false</c>.</value>
        public bool IsElectric { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Car"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>The updated at.</value>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public byte[] Version { get; set; }
    }
}