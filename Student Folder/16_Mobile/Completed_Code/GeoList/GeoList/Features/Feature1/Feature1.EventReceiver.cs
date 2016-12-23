using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace GeoList.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("86ec6c9c-2cb3-40f4-8d04-c4f30294dba4")]
    public class Feature1EventReceiver : SPFeatureReceiver
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

        public override void FeatureDeactivating(
                             SPFeatureReceiverProperties properties)
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
