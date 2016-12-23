using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;

namespace ProviderHostedAppWeb.Pages
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
Uri hostWeb = new Uri(Request.QueryString["SPHostUrl"]);

using (var clientContext = TokenHelper.GetS2SClientContextWithWindowsIdentity(hostWeb, Request.LogonUserIdentity))
{
    var web = clientContext.Web;
    clientContext.Load(web.Lists,
                        lists => lists.Include(
                            list => list.Title,
                            list => list.DefaultViewUrl
                        )
                        );
    clientContext.ExecuteQuery();
    AppWebLists.DataSource = web.Lists;
    AppWebLists.DataBind();

}
        }
    }
}