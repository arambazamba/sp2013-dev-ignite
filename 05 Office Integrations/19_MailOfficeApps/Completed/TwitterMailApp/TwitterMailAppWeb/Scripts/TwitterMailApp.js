// This function is run when the app is ready to start interacting with the host application
// It ensures the DOM is ready before updating the span elements with values from the current message
Office.initialize = function () {
    $(document).ready(function () {
        init(Office.context.mailbox.item.getRegExMatches().ScreenName);
    });
};

function init(screenNames) {
    for (var i = 0; i < screenNames.length; i++) {
        getTwitterData(screenNames[i]);
    }
}

function getTwitterData(screenName) {
    $.ajax({
        url: "https://api.twitter.com/1/statuses/user_timeline.json",
        type: "GET",
        dataType: "jsonp",
        data: {
            screen_name: screenName,
            include_rts: true,
            count: 5,
            include_entities: true
        },
        success: onDataReturned,
        error: onError
    });
}

function onDataReturned(data) {
    var ret = [];

    ret.push("<h1>Tweets for " + data[0].user.screen_name + "</h1>");
    ret.push("<table><th>Text</th><th>Time</th><tr>");
    for (var i = 0; i < data.length; i++) {
        ret.push("<tr>");
        ret.push("<td>" + data[i].text + "</td>");
        ret.push("<td>" + data[i].created_at + "</td>");
        ret.push("</tr>");
    }
    ret.push("</table>");


    $('#message').html(ret.join(''));

}
function onError(data, something, another) {
    alert(data);
}
