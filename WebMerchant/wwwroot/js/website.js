﻿
function CaptionHistory(obj) {

    var url = $(obj).data('href');    
    $('#iframe-caption-history').attr('src', url);
}

function ContentHistory(obj) {
    var url = $(obj).data('href');
    $('#iframe-content-history').attr('src', url);
}

function CaptionHistoryReview(obj) {

    var url = $(obj).data('href');
    $('#iframe-caption-history-review').attr('src', url);
}

function ContentHistoryReview(obj) {
    var url = $(obj).data('href');
    $('#iframe-content-history-review').attr('src', url);
}