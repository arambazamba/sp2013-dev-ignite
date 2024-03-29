﻿var videos;
var selectedVideo = -1;

Office.initialize = function (reason) {
    $(document).ready(function () {
        init(Office.context.mailbox.item.getRegExMatches().VideoURL);
    });
}

function parseDate(dateString) {
    var year = parseInt(dateString.substring(0, 4));
    var month = parseInt(dateString.substring(5, 7));
    var day = parseInt(dateString.substring(8, 10));

    var result = new Date();
    result.setUTCFullYear(year);
    result.setUTCMonth(month - 1);
    result.setUTCDate(day);

    return result;
}

function getVideoIndex(videoId) {
    for (i = 0; i < videos.length; i++) {
        if (videos[i].Id == videoId) {
            return i;
        }
    }

    return null;
}

function videoDetailsLoaded(videoFeed) {
    var videoIndex = getVideoIndex(videoFeed.entry.id.$t.substring(42));

    if (videoFeed.entry.media$group.media$thumbnail.length > 0) {
        videos[videoIndex].ThumbnailURL = videoFeed.entry.media$group.media$thumbnail[0].url.replace("http:", "https:");

        document.getElementById(videos[videoIndex].Id).src = videos[videoIndex].ThumbnailURL;
    }


    videos[videoIndex].Title = videoFeed.entry.title.$t;
    videos[videoIndex].PublishedDate = parseDate(videoFeed.entry.published.$t);
    videos[videoIndex].Description = videoFeed.entry.media$group.media$description.$t;
    videos[videoIndex].ViewCount = parseInt(videoFeed.entry.yt$statistics.viewCount);
    
    
    
    if (videoIndex == selectedVideo) {
        refreshVideoDetails(selectedVideo);
    }
}

function loadVideoDetails(videoIndex) {
    var script = document.createElement("script");
    script.setAttribute("src", "https://gdata.youtube.com/feeds/api/videos/" + videos[videoIndex].Id + "?alt=json-in-script&callback=videoDetailsLoaded");
    document.getElementsByTagName('head')[0].appendChild(script);
}

function refreshVideoDetails(videoIndex) {
    var html = "";

    if (videos[videoIndex].Title != undefined) {
        html += "<p class='videoTitle'>" + videos[videoIndex].Title + "</p>";
    }

    if (videos[videoIndex].Description != undefined) {
        html += "<p class='multiLineVideoDetails'>" + videos[videoIndex].Description + "</p>";
    }

    if (videos[videoIndex].PublishedDate != undefined) {
        html += "<p class='singleLineVideoDetails' style='margin-top: 8px;'>" + videos[videoIndex].PublishedDate + "</p>";
    }

    if (videos[videoIndex].ViewCount != undefined) {
        html += "<p class='singleLineVideoDetails'>" + videos[videoIndex].ViewCount + "</p>";
    }

    document.getElementById("details").innerHTML = html;
}

function selectVideo(videoIndex) {
    selectedVideo = videoIndex;

    for (i = 0; i < videos.length; i++) {
        document.getElementById(videos[i].Id + "frame").style.background = i == videoIndex ? "Black" : "White";
    }

    // document.getElementById("embeddedVideo").innerHTML = "<iframe width='354' height='200' frameborder='0' src='https://www.youtube.com/embed/" + videos[videoIndex].Id + "?autohide=1&showinfo=0'/>";
    document.getElementById("embeddedVideo").innerHTML = "<iframe width='354' height='200' frameborder='0' src='https://www.youtube.com/embed/" + videos[videoIndex].Id + "?html5=True'/>";

    refreshVideoDetails(videoIndex);
}

function init(videoURLs) {
    var html = "";
    videos = new Array();

    for (i = 0; i < Math.min(videoURLs.length, 5) ; i++) {
        var questionMarkPosition = videoURLs[i].indexOf("?v=");
        var videoId;

        if (questionMarkPosition >= 0) {
            videoId = videoURLs[i].substr(questionMarkPosition + 3, 11);
        }
        else {
            videoId = videoURLs[i].substr(16, 11);
        }

        videos[i] = { "Id": videoId };

        html += "<div class='thumbnailFrame' id='" + videos[i].Id + "frame'><img class='thumbnail' id='" + videos[i].Id + "' width='32' height='32' onclick='selectVideo(" + i + ");'/></div>";

        loadVideoDetails(i);
    }

    document.getElementById("thumbnails").innerHTML = html;

    if (videos.length == 1) {
        document.getElementById("thumbnails").style.display = "none";
    }

    selectVideo(0);
}

