
$(onPageLoad);

function onPageLoad() {
    $("#cmdGetSiteInfo").click(onGetSiteInfo);
}

function onGetSiteInfo() {

    // create URL to make REST call into host web
    var requestUri = _spPageContextInfo.siteAbsoluteUrl +
                    "/_api/web?$select=Title, Url";
    
    // execute AJAX request 
    $.ajax({
        type: "GET",
        url: requestUri,
        contentType: "application/json",
        headers: { Accept: "application/json;odata=verbose" },
        success: callbackGetSiteInfo,
        error: onError
    });


}

// deal with success when making call
function callbackGetSiteInfo(data) {
    $("#results").text("The host site title is " + data.d.Title);
}

// deal with success when making call
function onError() {
    $("#results").text("Error making web service call");
}