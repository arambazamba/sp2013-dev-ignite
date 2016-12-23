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

    Membership = {
        element: '',
        url: '',

        init: function (element) {
            Membership.element = element;
            Membership.url = _spPageContextInfo.webAbsoluteUrl + "/_api/site/rootweb/lists/getByTitle('User%20Information%20List')/items?$select=Title,Name";
        },

        load: function () {
            $.ajax(
                    {
                        url: Membership.url,
                        method: "GET",
                        headers: {
                            "Accept": "application/json;odata=verbose",
                        },
                        success: Membership.onSuccess,
                        error: Membership.onError
                    }
                );
        },

        onSuccess: function (data) {
            var results = data.d.results;
            var html = "<table>";

            for (var i = 0; i < results.length; i++) {
                html += "<tr><td>";
                html += results[i].Title;
                html += "</td><td>"
                html += results[i].Name;
                html += "</td><tr>";
            }

            html += "</table>";
            Membership.element.html(html);
        },

        onError: function (err) {
            alert(JSON.stringify(err));
        }
    },

    Membership.init($('#peopleDiv'));
    Membership.load();

});
