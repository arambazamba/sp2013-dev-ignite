using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace ApplicationPagePattern.Layouts.ApplicationPagePattern
{
    public partial class Demo : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ascx = Page.LoadControl("~/_ControlTemplates/15/ApplicationPagePattern/DemoPageContent.ascx");
            pContent.Controls.Add(ascx);
        }
    }
}
