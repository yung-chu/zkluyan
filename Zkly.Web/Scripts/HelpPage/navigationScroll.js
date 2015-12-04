(function($) {
    
    $("ul.expmenu li").click(function () {
        var arrow = $(this).find("span.arrow");

        if (arrow.hasClass("up")) {
            arrow.removeClass("up");
            arrow.addClass("down");
        } else if (arrow.hasClass("down")) {
            arrow.removeClass("down");
            arrow.addClass("up");
        }
        $(this).parent().find("ul.menu").slideToggle();
    });

}(jQuery))