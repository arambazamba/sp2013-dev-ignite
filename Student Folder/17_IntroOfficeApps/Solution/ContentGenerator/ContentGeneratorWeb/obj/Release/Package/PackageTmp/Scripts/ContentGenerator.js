﻿Office.initialize = function (reason) {

  $(function () {
    $("#cmdGetContent").click(cmdGetContent);
    $("#cmdInsertContent").click(cmdInsertContent);
  });

}

function cmdGetContent() {
  //$("#display").text("Hello world of Apps for Office");
  $("#display").text(getQuote());
}

function cmdInsertContent() {
  var currentQuote = $("#display").text();
  Office.context.document.setSelectedDataAsync(currentQuote, {}, function () { });
}

function getQuote() {

  var quotes = [
      "I would rather have an Agave bottle in front of me than a frontal lobotomy.",
      "Better to remain silent and be thought a fool than to speak and erase all doubt.",
      "A two-year-old is kind of like having a blender, but you don't have a top for it.",
      "Behind every great man is a woman rolling her eyes.",
      "Between two evils, I always pick the one I never tried before.",
      "I always wanted to be somebody, but now I realize I should have been more specific.",
      "A pessimist sees the difficulty in every opportunity; an optimist sees the opportunity in every difficulty.",
      "In theory there is no difference between theory and practice. In practice there is.",
      "In Hollywood a marriage is a success if it outlasts milk.",
      "Money won't buy happiness, but it will pay the salaries of a large research staff to study the problem.",
      "Do not go where the path may lead, go instead where there is no path and leave a trail.",
      "It is a common experience that a problem difficult at night is resolved in the morning after the committee of sleep has worked on it.",
      "Never tell people how to do things. Tell them what to do and they will surprise you with their ingenuity."
  ];

  var index = Math.floor(Math.random() * quotes.length);
  return quotes[index];
}

