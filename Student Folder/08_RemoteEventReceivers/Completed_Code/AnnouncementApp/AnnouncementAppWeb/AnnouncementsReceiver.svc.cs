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

        
    }
}