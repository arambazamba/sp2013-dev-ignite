using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;

namespace CustomListSolution.Features.MainSite
{
    [Guid("a70a8032-b64e-4416-852e-c790fb37427b")]
    public class MainSiteEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(
                        SPFeatureReceiverProperties properties)
        {
            SPSite siteCollection = (SPSite)properties.Feature.Parent;
            if (siteCollection != null)
            {
                SPWeb site = siteCollection.RootWeb;
                // create dropdown menu for custom site pages
                SPNavigationNodeCollection topNav =
                             site.Navigation.TopNavigationBar;
                topNav.AddAsLast(
                  new SPNavigationNode("Page 1", "ContosoSitePages/Page1.aspx"));
                topNav.AddAsLast(
                  new SPNavigationNode("Page 2", "ContosoSitePages/Page2.aspx"));
                topNav.AddAsLast(
                  new SPNavigationNode("Page 3", "ContosoSitePages/Page3.aspx"));
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite siteCollection = (SPSite)properties.Feature.Parent;
            SPWeb site = siteCollection.RootWeb;

            try
            {
                // delete folder of site pages provisioned during activation
                SPFolder sitePagesFolder = site.GetFolder("ContosoSitePages");
                sitePagesFolder.Delete();
            }
            catch { }

            SPNavigationNodeCollection topNav =
                                      site.Navigation.TopNavigationBar;
            for (int i = topNav.Count - 1; i >= 0; i--)
            {
                if (topNav[i].Url.Contains("ContosoSitePages"))
                {
                    // delete node
                    topNav[i].Delete();
                }
            }

        }


    }
}
