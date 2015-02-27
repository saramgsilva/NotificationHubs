using System;
using System.Collections.Generic;
using System.Data.Entity;
using NotificationHubsSample.AMS.DataObjects;
using NotificationHubsSample.AMS.Models;

namespace NotificationHubsSample.AMS
{
    /// <summary>
    /// Define the MobileServiceInitializer.
    /// I do not recommend to use this in production.
    /// </summary>
    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(MobileServiceContext context)
        {
            var cars = new List<Car>
            {
                new Car { Id = Guid.NewGuid().ToString(), Name = "BMW Z1", IsElectric = false },
                new Car { Id = Guid.NewGuid().ToString(), Name = "BMW XPTO", IsElectric = true },
            };

            foreach (var car in cars)
            {
                context.Set<Car>().Add(car);
            }

            base.Seed(context);
        }
    }
}