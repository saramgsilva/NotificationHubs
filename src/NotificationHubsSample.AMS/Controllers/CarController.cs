using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using NotificationHubsSample.AMS.DataObjects;
using NotificationHubsSample.AMS.Models;

namespace NotificationHubsSample.AMS.Controllers
{
    /// <summary>
    /// Define the CarController.
    /// </summary>
    public class CarController : TableController<Car>
    {
        /// <summary>
        /// Initializes the <see cref="T:System.Web.Http.ApiController" /> instance with the specified controllerContext.
        /// </summary>
        /// <param name="controllerContext">The <see cref="T:System.Web.Http.Controllers.HttpControllerContext" /> object that is used for the initialization.</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Car>(context, Request, Services);
        }

        /// <summary>
        /// Gets all cars.
        /// GET tables/Car
        /// </summary>
        /// <returns>IQueryable&lt;Car&gt;.</returns>
        public IQueryable<Car> GetAllCars()
        {
            return Query();
        }

        /// <summary>
        /// Gets the car.
        /// GET tables/Car/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SingleResult&lt;Car&gt;.</returns>
        public SingleResult<Car> GetCar(string id)
        {
            return Lookup(id);
        }
        
        /// <summary>
        /// Patches the car.
        /// PATCH tables/Car/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patch">The patch.</param>
        /// <returns>Task&lt;Car&gt;.</returns>
        public Task<Car> PatchCar(string id, Delta<Car> patch)
        {
            return UpdateAsync(id, patch);
        }

        /// <summary>
        /// Posts the car.
        /// POST tables/Car.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        public async Task<IHttpActionResult> PostCar(Car item)
        {
            var current = await InsertAsync(item);
        
            // send a test notification
            var message = string.Format("Was inserted the car {0}.", item.Name);
            await SendNotificationAsync(message);
        
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// Sends the notification asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        private async Task SendNotificationAsync(string message)
        {
            try
            {
                await Services.Push.SendAsync(PushHelper.GetWindowsPushMessageForToastText01(message));
                await Services.Push.SendAsync(PushHelper.GetGooglePushMessage(message));
                await Services.Push.SendAsync(PushHelper.GetMPNSMessage(message));
                // await Services.Push.SendAsync(PushHelper.GetApplePushMessage(message));

                const string otherMessage = "A second tag was added in AMS.";

                await Services.Push.SendAsync(PushHelper.GetWindowsPushMessageForToastText01(otherMessage), NotificationHandler.SomeTag);
                await Services.Push.SendAsync(PushHelper.GetGooglePushMessage(otherMessage), NotificationHandler.SomeTag);
                await Services.Push.SendAsync(PushHelper.GetMPNSMessage(otherMessage), NotificationHandler.SomeTag);
                // await Services.Push.SendAsync(PushHelper.GetApplePushMessage(otherMessage), NotificationHandler.SomeTag);
            }
            catch (Exception exception)
            {
                Services.Log.Error(exception);
            }
        }

        /// <summary>
        /// Deletes the car.
        /// DELETE tables/Car/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public Task DeleteCar(string id)
        {
            return DeleteAsync(id);
        }
    }
}