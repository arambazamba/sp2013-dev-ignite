Office.initialize = function (reason) {

    $(function () {
        $("#cmdUpdateApp").click(onUpdateApp);
        $("#cmdUpdateDoc").click(onUpdateDoc);
    });

}
function onUpdateApp() {
    $("#display").text("Hello to the app!");
}


function onUpdateDoc() {
    var doc = Office.context.document;
    doc.setSelectedDataAsync("Hello to the doc!", function () { });
}

