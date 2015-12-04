(function ($) {

    $("#sms-register, #sms-balance, #sms-report, #sms-update-password").click(function () {
        $.get($(this).attr("href"), function (html) {
            $("#sms-content").html(html).show();

            $("#sms-register-btn").click(function () {
                var form = $($("#sms-content form")[0]),
                    url = form.attr("action"),
                    data = form.serialize();
                $.post(url, data, function (html2) {
                    $("#sms-content").html(html2).show();
                });
                return false;
            });
        });
        return false;
    });

})(jQuery);