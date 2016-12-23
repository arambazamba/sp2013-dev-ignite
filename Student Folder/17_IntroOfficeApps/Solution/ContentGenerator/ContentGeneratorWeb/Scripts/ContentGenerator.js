Office.initialize = function (reason) {
    $(function () {
        $("#cmdGetContent").click(cmdGetContent);
        $("#cmdInsertContent").click(cmdInsertContent);
    });

}
function cmdGetContent() {
    // display quote inside Office App
    $("#display").text(getQuote());

}

function cmdInsertContent() {
    var currentQuote = $("#display").text();
    Office.context.document.setSelectedDataAsync(currentQuote, {}, function () { });
}

function getQuote() {

    var quotes = [
        "I’d rather have an bottle in front of me than a frontal lobotomy.",
        "Behind every great man is a woman rolling her eyes.",
        "Between two evils, I always pick the one I never tried before."
    ];

    var index = Math.floor(Math.random() * quotes.length);
    return quotes[index];
}
