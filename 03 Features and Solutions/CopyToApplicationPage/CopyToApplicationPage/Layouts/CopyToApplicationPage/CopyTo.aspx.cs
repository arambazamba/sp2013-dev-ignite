using System;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CopyToApplicationPage.Layouts.CopyToApplicationPage
{
    public partial class CopyTo : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void PopulateTree(object sender, EventArgs e)
        {
            tvSite.Nodes.Clear();
            var col = new SPSite(txtSiteCol.Text);

            var rootNode = new TreeNode(col.RootWeb.Title,
                                             new TreeNodeContext(txtSiteCol.Text, col.RootWeb.ID).ToString());
            AddVisibleLists(rootNode, col.RootWeb);
            AddChildWebs(rootNode, col.RootWeb);
            tvSite.Nodes.Add(rootNode);
        }

        protected void AddChildWebs(TreeNode pn, SPWeb web)
        {
            foreach (SPWeb chw in web.GetSubwebsForCurrentUser())
            {
                var chn = new TreeNode(chw.Title, new TreeNodeContext(txtSiteCol.Text, chw.ID).ToString());
                AddChildWebs(chn, chw);
                AddVisibleLists(chn, chw);
                pn.ChildNodes.Add(chn);
            }
        }

        protected void AddVisibleLists(TreeNode pn, SPWeb web)
        {
            foreach (SPList list in web.GetListsOfType(SPBaseType.DocumentLibrary))
            {
                if (list.OnQuickLaunch)
                {
                    var chn = new TreeNode(list.Title,
                                                new TreeNodeContext(txtSiteCol.Text, web.ID, list.ID).ToString());
                    pn.ChildNodes.Add(chn);
                }
            }
        }

        protected void NodeSelected(object sender, EventArgs e)
        {
            var tv = (TreeView)sender;
            var ctx = new TreeNodeContext(tv.SelectedValue);

            var col = new SPSite(ctx.SiteCollection);
            SPWeb sw = col.AllWebs[ctx.WebID];

            if (ctx.IsList)
            {
                SPList sl = sw.Lists[ctx.ListID];
                lblList.Text = sl.Title;
            }
            else
            {
                lblList.Text = string.Empty;
            }
        }

        protected void CopyItem(object sender, EventArgs e)
        {
            int itemid = Convert.ToInt32(Request.QueryString["ItemId"]);
            var listid = new Guid(Request.QueryString["ListId"]);

            SPWeb sourceweb = SPContext.Current.Web;
            SPList sourcelist = sourceweb.Lists[listid];
            SPListItem sourceitem = sourcelist.GetItemById(itemid);

            lblListItem.Text = sourceitem.Name;

            var ctx = new TreeNodeContext(tvSite.SelectedValue);

            var destcol = new SPSite(ctx.SiteCollection);
            SPWeb destweb = destcol.AllWebs[ctx.WebID];
            if (ctx.IsList)
            {
                SPList destlist = destweb.Lists[ctx.ListID];

                SPFolder folder = destlist.RootFolder;

                try
                {
                    folder.Files.Add(sourceitem.Name, sourceitem.File.OpenBinary());
                }
                catch (Exception ex)
                {
                    lblStatus.Text = string.Format("Error uploading File {0}", sourceitem.Name);
                }

                lblStatus.Text = string.Format("{0} copied successfully to {1}", sourceitem.Name, destlist.Title);
            }
        }
    }
}
