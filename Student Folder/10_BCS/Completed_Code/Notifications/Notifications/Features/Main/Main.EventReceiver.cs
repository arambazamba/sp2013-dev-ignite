using System;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Data;
using System.Data.SqlClient;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData;
using Microsoft.SharePoint.BusinessData.Runtime;
using Microsoft.SharePoint.BusinessData.SharedService;
using Microsoft.SharePoint.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.Administration;
using Microsoft.BusinessData;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.BusinessData.Runtime;
using Microsoft.BusinessData.MetadataModel.Collections;

namespace Notifications.Features.Main
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7b0aa520-80b1-45b8-aca5-5eb53b0d7fa0")]
    public class MainEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb site = properties.Feature.Parent as SPWeb;

            using (SPSite siteCollection = new SPSite(site.Site.ID))
            {
                //Get the ECT
                BdcService service = SPFarm.Local.Services.GetValue<BdcService>();
                SPServiceContextScope scope = new SPServiceContextScope(SPServiceContext.GetContext(siteCollection));
                DatabaseBackedMetadataCatalog catalog = service.GetDatabaseBackedMetadataCatalog(SPServiceContext.GetContext(siteCollection));
                IEntity ect = catalog.GetEntity("http://contoso.com/minicrm", "Customer");
                ILobSystemInstance lobi = ect.GetLobSystem().GetLobSystemInstances()["MiniCRM"];

                //This will call the subscribe method and tell the system what endpoint to call
                IMethodInstance mi = ect.GetMethodInstance("Subscribe", MethodInstanceType.EventSubscriber);
                IParameterCollection parameters = mi.GetMethod().GetParameters();
                string endpointUrl = site.Url + "/_vti_bin/Notifications/NotificationEndpoint.svc";
                object[] arguments = new object[parameters.Count];
                arguments[0] = endpointUrl;
                arguments[1] = EntityEventType.ItemAdded;
                ect.Execute(mi, lobi, ref arguments);

                //This saves the subscription ID
                PropertyInfo[] props = arguments[2].GetType().GetProperties();
                PropertyInfo prop = props[0];
                SqlDataReader reader = (SqlDataReader)(prop.GetValue(arguments[2], null));

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SPList notificationList = site.Lists["Notifications"];
                        SPListItem notificationItem = notificationList.Items.Add();
                        notificationItem["Title"] = "SubscriptionId";
                        string body = reader.GetInt32(0).ToString();
                        notificationItem["Body"] = "[" + body + "]";
                        notificationItem.SystemUpdate();
                    }
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb site = properties.Feature.Parent as SPWeb;
            SPQuery query = new SPQuery();
            query.ViewFields = "<FieldRef Name='Title'/><FieldRef Name='Body'/>";
            query.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='Text'>SubscriptionId</Value></Eq></Where>";
            SPList notificationList = site.Lists["Notifications"];
            SPListItemCollection items = notificationList.GetItems(query);
            XElement body = XElement.Parse(items[0]["Body"].ToString());
            items[0].Delete();

            int subscriptionId = 0;
            if (int.TryParse(body.Value.Substring(1, body.Value.Length - 2), out subscriptionId))
            {
                using (SPSite siteCollection = new SPSite(site.Site.ID))
                {

                    //Get the ECT
                    BdcService service = SPFarm.Local.Services.GetValue<BdcService>();
                    SPServiceContextScope scope = new SPServiceContextScope(SPServiceContext.GetContext(siteCollection));
                    DatabaseBackedMetadataCatalog catalog = service.GetDatabaseBackedMetadataCatalog(SPServiceContext.GetContext(siteCollection));
                    IEntity ect = catalog.GetEntity("http://contoso.com/minicrm", "Customer");
                    ILobSystemInstance lobi = ect.GetLobSystem().GetLobSystemInstances()["MiniCRM"];

                    //This will call the unsubscribe method
                    IMethodInstance mi = ect.GetMethodInstance("Unsubscribe", MethodInstanceType.EventUnsubscriber);
                    IParameterCollection parameters = mi.GetMethod().GetParameters();
                    object[] arguments = new object[parameters.Count];
                    arguments[0] = subscriptionId;
                    ect.Execute(mi, lobi, ref arguments);
                }
            }

        }

    }
}
