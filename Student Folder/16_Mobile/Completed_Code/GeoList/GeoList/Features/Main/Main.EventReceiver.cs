using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace GeoList.Features.Main
{

    [Guid("d699a0ad-6f9b-4954-9da5-e11b9352155b")]
    public class MainEventReceiver : SPFeatureReceiver
    {

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb site = properties.Feature.Parent as SPWeb;
            SPList list = site.Lists.TryGetList("Service Calls");
            if (list != null)
            {
                list.Fields.AddFieldAsXml(
                    "<Field Type='Geolocation' DisplayName='Location'/>", 
                    true, 
                    SPAddFieldOptions.Default);
                list.Update();
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb site = properties.Feature.Parent as SPWeb;
            SPList list = site.Lists.TryGetList("Service Calls");
            if (list != null)
            {
                list.Delete();
            }
        }
    }
}
