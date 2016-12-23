var context;
var web;
var user;

// This code runs when the DOM is ready and creates a context object which is needed to use the SharePoint object model
$(document).ready(function () {
    context = SP.ClientContext.get_current();
    web = context.get_web();

    getUserName();
});

// This function prepares, loads, and then executes a SharePoint query to get the current users information
function getUserName() {
    user = web.get_currentUser();
    context.load(user);
    context.executeQueryAsync(onGetUserNameSuccess, onGetUserNameFail);
}

// This function is executed if the above OM call is successful
// It replaces the contents of the 'helloString' element with the user name
function onGetUserNameSuccess() {
    $('#message').text('Hello ' + user.get_title());
}

// This function is executed if the above call fails
function onGetUserNameFail(sender, args) {
    alert('Failed to get user name. Error:' + args.get_message());
}

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
