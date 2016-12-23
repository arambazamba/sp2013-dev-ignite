

$(document).ready(function () {

    //Namespace
    window.AppLevelECT = window.AppLevelECT || {};

    //Constructor
    AppLevelECT.Grid = function (hostElement, surlWeb) {
        this.hostElement = hostElement;
        if (surlWeb.length > 0 &&
        surlWeb.substring(surlWeb.length - 1, surlWeb.length) != "/")
            surlWeb += "/";
        this.surlWeb = surlWeb;
    }

    //Prototype
    AppLevelECT.Grid.prototype = {

        init: function () {

            $.ajax({
                url: this.surlWeb +
          "_api/lists/getbytitle('Alphabetical_list_of_products')/items?" +
                "$select=BdcIdentity,ProductID,ProductName",
                headers: {
                    "accept": "application/json;odata=verbose",
                    "X-RequestDigest": $("#__REQUESTDIGEST").val()
                },
                success: this.showItems
            });
        },
        error: function (data) {
            alert(data);
            $("#displayDiv").html(data);
        },
        showItems: function (data) {
            var items = [];

            items.push("<table>");
            items.push("<tr><td>Product ID</td>" +
                       "<td>Product Name</td></tr>");

            $.each(data.d.results, function (key, val) {
                items.push('<tr id="' + val.BdcIdentity + '"><td>' +
                    val.ProductID + '</td><td>' +
                    val.ProductName + '</td></tr>');
            });

            items.push("</table>");

            $("#displayDiv").html(items.join(''));
        }

    }

    ExecuteOrDelayUntilScriptLoaded(getProducts, "sp.js");
});


function getProducts() {
    var grid = new AppLevelECT.Grid($("#displayDiv"),
                  _spPageContextInfo.webServerRelativeUrl);
    grid.init();
}
