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

var _surlWeb = "";

$(document).ready(function()
{
    _surlWeb = _spPageContextInfo.webServerRelativeUrl;

    if (_surlWeb.length > 0 && _surlWeb.substring(
    _surlWeb.length - 1, _surlWeb.length ) != "/") {
        _surlWeb += "/"; }

    loadMessages();
});

function loadMessages()
{
    $.ajax({
        url: _surlWeb +
 "_api/lists/getbytitle('Quick Status')/items?$select=ID,Title,Author/Title&$expand=Author&$orderby=ID desc",
    headers: { 
        "accept": "application/json;odata=verbose",
        "X-RequestDigest": $("#__REQUESTDIGEST").val()
    },
    success: postMessageListRetrieve,
    error: oops
      

});
}

function postMessageListRetrieve(data)
{
    var items = [];

    items.push("<table class='messages'>");

    $.each(data.d.results, function(key, val) 
    {
        items.push('<tr id="' + val.ID + '"><td><div>' +
          val.Title + '</div><div class="subtle">' +
          val.Author.Title + '</div></td></tr>');
    });

    items.push("</table>");

    $('#messages').html(items.join(''));
}

function oops(data) {
    alert(data.responseText);
}

function addMessage()
{
    $.ajax({
        url: _surlWeb + "_api/lists/getbytitle('Quick Status')/items",
    type: "POST",
    data:  JSON.stringify(
            {  '__metadata': {
                'type': 'SP.Data.Quick_x0020_StatusListItem'}, 
                'Title': $('#messageInput').val() 
            }),
    headers: {
        "Content-Type" : "application/json;odata=verbose",
        "accept": "application/json;odata=verbose",
        "X-RequestDigest": $("#__REQUESTDIGEST").val()
    },
    success: loadMessages,
    error: oops
});
}
