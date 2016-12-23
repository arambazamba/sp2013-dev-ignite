using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace PushNotificationsList.Features.Main
{

    [Guid("fa87d224-9e34-4fa9-94a6-749625eb01cd")]
    public class MainEventReceiver : SPFeatureReceiver
    {
        internal const string PushNotificationFeatureId = "41e1d4bf-b1a2-47f7-ab80-d5d6cbba3092";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            base.FeatureActivated(properties);
            SPWeb spWeb = (SPWeb)properties.Feature.Parent;

            ListCreator listCreator = new ListCreator();
            listCreator.CreateJobsList(spWeb);
            listCreator.CreateNotificationResultsList(spWeb);

            // Then activate the Push Notification Feature on the server.
            // The Push Notification Feature is not activated by default in a SharePoint Server installation.
            spWeb.Features.Add(new Guid(PushNotificationFeatureId), false);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            base.FeatureDeactivating(properties);
            SPWeb spWeb = (SPWeb)properties.Feature.Parent;

            // Deactivate the Push Notification Feature on the server
            // when the PushNotificationsList Feature is deactivated.
            spWeb.Features.Remove(new Guid(PushNotificationFeatureId), false);
        }

    }
}
