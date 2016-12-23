<%@ Page language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0,Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0,Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0,Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Client Web Part</title>
    <WebPartPages:AllowFraming ID="AllowFraming" runat="server" />
    <SharePoint:ScriptLink ID="ScriptLink1" name="sp.js"
     runat="server" OnDemand="true" LoadAfterUI="true"
     Localizable="false" />
    <script type="text/javascript" src="../Scripts/App.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="appPartDiv"></div>
    <input type="button" value="Push Me!" onclick="helloAppPart();" />
    </div>
    </form>
</body>
</html>
