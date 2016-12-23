
function hello() {
    $get("displayDiv").innerHTML = "<p>Hello, Apps!</p>";
}

function helloAppPart() {
    var message = getQueryStringValue("Message");
    document.getElementById("appPartDiv").innerHTML = "<p>" + message + "</p>";
}
function getQueryStringValue(paramName) {
    var params = document.URLUnencoded.split("?")[1].split("&");
    var strParams = "";
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramName)
            return decodeURIComponent(singleParam[1]);
    }
}

