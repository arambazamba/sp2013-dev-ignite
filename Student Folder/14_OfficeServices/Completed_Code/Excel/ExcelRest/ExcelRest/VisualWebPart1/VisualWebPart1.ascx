<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualWebPart1.ascx.cs" Inherits="ExcelRest.VisualWebPart1.VisualWebPart1" %>
<script type="text/javascript" language="javascript"
 src="http://ajax.microsoft.com/ajax/jquery/jquery-1.7.1.min.js">
</script>

<script type="text/javascript">
    $(document).ready(function () {
        var e = ExecuteOrDelayUntilScriptLoaded(showSheet, "sp.js");
    });

    function showSheet() {

        Results = {
            element: '',
            url: '',

            init: function (element) {
                Results.element = element;
                Results.url = _spPageContextInfo.webAbsoluteUrl +
"/_vti_bin/ExcelRest.aspx/Company%20Documents/MiniCRM.xlsx/OData/Table1";
            },

            load: function () {
                $.ajax(
                    {
                        url: Results.url,
                        method: "GET",
                        headers: {
                            "accept": "application/json;odata=verbose",
                        },
                        success: Results.onSuccess,
                        error: Results.onError
                    }
                );
            },

            onSuccess: function (data) {
                var results = data.d.results;
                var html = "<table>";

                for (var i = 0; i < results.length; i++) {
                    html += "<tr><td>";
                    html += results[i].ns1FullName;
                    html += "</td><td>"
                    html += results[i].ns1Email;
                    html += "</td><tr>";
                }

                html += "</table>";
                Results.element.html(html);
            },

            onError: function (err) {
                alert(JSON.stringify(err));
            }
        }

        Results.init($('#resultsDiv'));
        Results.load();

    }


</script>

<div id="resultsDiv">Loading...</div>
