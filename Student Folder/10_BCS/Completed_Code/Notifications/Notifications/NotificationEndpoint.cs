using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.SharePoint.Client.Services;
using Microsoft.SharePoint;

namespace Notifications
{
    [ServiceContract]
    public interface INotificationEndpoint
    {
        [OperationContract]
        void Notify(string EventType, string Details);
    }

    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class NotificationEndpoint : INotificationEndpoint
    {
        public void Notify(string EventType, string Details)
        {
            SPList notificationList = SPContext.Current.Web.Lists["Notifications"];
            SPListItem notificationItem = notificationList.Items.Add();
            notificationItem["Title"] = EventType;
            notificationItem["Body"] = Details;
            notificationItem.Update();
        }
    }
}
