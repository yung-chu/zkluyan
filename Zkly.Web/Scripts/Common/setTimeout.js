(function ($) {

    delayURL($("#url").val());

}(jQuery));


function delayURL(url) {
    var delay = document.getElementById("time").innerHTML;
    if (delay > 0) {
        delay--;
        document.getElementById("time").innerHTML = delay;
    } else {
        window.top.location.href = url;
    }
    setTimeout("delayURL('" + url + "')", 1000);
}