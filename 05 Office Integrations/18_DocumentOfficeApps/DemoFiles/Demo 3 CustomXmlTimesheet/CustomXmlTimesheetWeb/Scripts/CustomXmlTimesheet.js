var OfficeDoc;
var xmlTimeSummary
var xmlActivityNode;

Office.initialize = function (reason) {
    // initialize doc variable
    OfficeDoc = Office.context.document;

    // add document ready handler
    $(document).ready(function () {
        $("#month").change(onMonthChanged);
        postMessage("Timesheet app initialized");
    });
}



function onMonthChanged() {
    OfficeDoc.customXmlParts.getByNamespaceAsync("TimeSummary", onGotXml);
}
function onGotXml(asyncResult) {

    if (asyncResult.status == Office.AsyncResultStatus.Failed) {
        postMessage("ERROR: " + asyncResult.error.message);
        return;
    }

    xmlTimeSummary = asyncResult.value[0];
    xmlTimeSummary.getNodesAsync("/ns0:TimeSummary[1]/ns0:Activity[1]", onGotNode);

}

function onGotNode(asyncResult) {

    if (asyncResult.status == Office.AsyncResultStatus.Failed) {
        postMessage("ERROR: " + asyncResult.error.message);
        return;
    }


    xmlActivityNode = asyncResult.value[0];
    var timesheetName = "Timesheet" + $("#month option:selected").text() + ".xml";

    $.ajax({
        url: timesheetName,
        success: onXmlRetrieved,
        cache: false
    });

    postMessage("Retrieving activities from " + timesheetName);
}



function onXmlRetrieved(data) {


    var timeActivityNode = data.documentElement.firstElementChild;
    
    var xml = "<Activity xmlns='TimeSummary'>";
    
    while (timeActivityNode) {
        xml += getTimeActivityNodeOuterXml(timeActivityNode);
        timeActivityNode = timeActivityNode.nextElementSibling
    }
    xml += "</Activity>";
    

    xmlActivityNode.setXmlAsync(xml, onXmlWrite);
}

function getTimeActivityNodeOuterXml(timeActivityNode) {
    var dateNode = timeActivityNode.firstElementChild;
    var professionalNode = dateNode.nextElementSibling;
    var elapsedTimeNode = professionalNode.nextElementSibling;
    var chargeNode = elapsedTimeNode.nextElementSibling;

    var xml = "<TimeActivity>";
    xml += "<Date>" + dateNode.textContent + "</Date>";
    xml += "<Professional>" + professionalNode.textContent + "</Professional>";
    xml += "<ElapsedTime>" + elapsedTimeNode.textContent + "</ElapsedTime>";
    xml += "<Charge>" + chargeNode.textContent + "</Charge>";
    xml += "</TimeActivity>";

    return xml;
}
function onXmlWrite(asyncResult) {

    if (asyncResult.status == Office.AsyncResultStatus.Failed) {
        postMessage("ERROR: " + asyncResult.error.message);
        return;
    }

}

function postMessage(message) {
    $("#display").prepend($("<p>").text(message));
}