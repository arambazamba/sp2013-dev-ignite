using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;

namespace LifecycleAppWeb
{
    public class AppEventReceiver : IRemoteEventService
    {
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
SPRemoteEventResult result = new SPRemoteEventResult();
            
using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, false))
{
    if (clientContext != null)
    {
        if (properties.EventType == SPRemoteEventType.AppUninstalling)
        {
            List announcementsList = clientContext.Web.Lists.GetByTitle("Announcements");

            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem newItem = announcementsList.AddItem(itemCreateInfo);
            newItem["Title"] = "The app is being uninstalled!";
            newItem["Body"] = "The app is being uninstalled at " + System.DateTime.Now.ToLongTimeString();
            newItem.Update();

            clientContext.ExecuteQuery();
            clientContext.Load(clientContext.Web);
            clientContext.ExecuteQuery();
        }
    }
}

return result;
        }

        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            // This method is not used by app events
        }
    }
}
