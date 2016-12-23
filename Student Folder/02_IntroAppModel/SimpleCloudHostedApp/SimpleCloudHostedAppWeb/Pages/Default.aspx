<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SimpleCloudHostedAppWeb.Pages.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Simple SharePoint-Hosted app</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var hostWebUrl;
        var hostLayoutsUrl;

        // Load the SharePoint resources.
        $(document).ready(function () {

            // Get the URI decoded app web URL.
            hostWebUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
            hostLayoutsUrl = hostWebUrl + "/_layouts/15/";



            // Load the js file and continue to the success handler.
            $.getScript(hostLayoutsUrl + "SP.UI.Controls.js", renderChromeControl)
        });

        // Function to prepare the options and render the control.
        function renderChromeControl() {
            var options = {
                "appTitle": "Simple Cloud Hosted App"
            };

            // activate the chrome control
            var nav = new SP.UI.Controls.Navigation("SharePointChromeControl", options);
            nav.setVisible(true);
        }

        // Function to retrieve a query string value.
        function getQueryStringParameter(paramToRetrieve) {
            var params = document.URL.split("?")[1].split("&");
            var strParams = "";
            for (var i = 0; i < params.length; i = i + 1) {
                var singleParam = params[i].split("=");
                if (singleParam[0] == paramToRetrieve)
                    return singleParam[1];
            }
        }
    </script>
</head>
<body>
    <div id="SharePointChromeControl"></div>
    <form id="form1" runat="server">
    <div style="margin: 20px;">
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in ornare diam. Vivamus sollicitudin convallis mollis. Nulla massa ante, mattis non placerat tincidunt, gravida vehicula lectus. Aliquam bibendum dolor sed felis rhoncus eu tincidunt libero sollicitudin. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nam dui erat, gravida eu fringilla id, faucibus vitae ipsum. Nullam eget elit quam. Vestibulum tristique nibh vel metus lobortis id posuere diam aliquam.</p>

        <div class="contentRow">
            <h2 class="ms-accentText">Take Notice Of...</h2>
            <ul>
                <li>Branding from hosting AppWeb bleeds through to app</li>
                <li>Use IE Dev Toolbar to look at CSS coming through</li>
                <li>App running in its own AppWeb (SPWeb)</li>
                <li>Link back to hosting SPWeb</li>
            </ul>
        </div>

        <div class="contentRow">
            <h2 class="ms-accentText">Lists in the App's Host Web:</h2>
            <div class="ms-textSmall">Retrieved via server-side .NET CSOM.</div>
            <asp:DataList ID="AppWebLists" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CellPadding="5">
                <ItemTemplate>
                    &raquo; <asp:HyperLink runat="server" NavigateUrl='<%# Eval("DefaultViewUrl") %>' Text='<%# Eval("Title") %>' /></asp:HyperLink>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
    </form>
</body>
</html>
