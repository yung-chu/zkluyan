//导航按钮
(function ($) {

    $('#invest-nav-tabs>ul>li').click(function () {
        var url = $(this).data('url');
        if (url !== undefined && !$(this).hasClass('disabled') && !$(this).hasClass('on')) {
            window.location.href = url;
        }
    });

})(jQuery);
