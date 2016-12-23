using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;

namespace AnnouncementAppWeb
{
    public class AnnouncementsReceiver : IRemoteEventService
    {
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
            {
                if (clientContext != null)
                {
                    switch (properties.EventType)
                    {
                        case SPRemoteEventType.ItemAdding:
                            result.ChangedItemProperties.Add("Body", properties.ItemEventProperties.AfterProperties["Body"] += "\n ** For internal use only ** \n");
                            clientContext.Load(clientContext.Web);
                            clientContext.ExecuteQuery();
                            break;

                        case SPRemoteEventType.ItemDeleting:
                            result.ErrorMessage = "Items cannot be deleted from this list";
                            result.Status = SPRemoteEventServiceStatus.CancelWithError;
                            break;
                    }
                }
            }

            return result;
        }

        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            if (properties.EventType == SPRemoteEventType.ItemAdded)
            {
                using (Microsoft.SharePoint.Client.ClientContext ctx =
                      new Microsoft.SharePoint.Client.ClientContext(
                      properties.ItemEventProperties.WebUrl))
                {
                    Microsoft.SharePoint.Client.List list =
                    ctx.Web.Lists.GetByTitle(
                    properties.ItemEventProperties.ListTitle);

                    ctx.Load(list);
                    Microsoft.SharePoint.Client.ListItem item =
                    list.GetItemById(
                    properties.ItemEventProperties.ListItemId);

                    ctx.Load(item);
                    ctx.ExecuteQuery();
                    item["Body"] +=
                       "\n Announcement Tracking ID: " +
                       Guid.NewGuid().ToString() + " \n";

                    item.Update();
                    ctx.ExecuteQuery();
                }
            }
        }
    }
}