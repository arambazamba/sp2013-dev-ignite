using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace PersistingValues.Features.PersistingValues
{

    [Guid("a673e09e-384f-4899-b805-fc8b7f9936f0")]
    public class PersistingValuesEventReceiver : SPFeatureReceiver
    {

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;
            string listName = "MyCustomList";
            try
            {
                SPList list = web.Lists[listName];
            }
            catch (Exception)
            {
                Guid id = web.Lists.Add(listName, "A custom list", SPListTemplateType.GenericList);
                SPList list = web.Lists[id];
                if (list!=null)
                {
                    list.OnQuickLaunch = true;
                }
            }
        }

    }
}
