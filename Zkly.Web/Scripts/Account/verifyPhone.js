
(function($) {


    /*一分钟才能提交一次*/
    $('button[data-loading-text]').click(function() {

        $("#inputVerifyCode").show(500);

        var btn = $(this).button('loading');
        setTimeout(function() {
            btn.button('reset');
        }, 1000 * 60);


        //发送验证码
        $.ajax({
            type: "POST",
            url: "/Account/SendPhoneConfirmationToken",
            data: { userId: $("#userId").val() },
            success: function(data) {

                if (!data.result) {
                    $.AlertMessage.showWarning(data.message);
                }
            },
            error: function() {

                $.AlertMessage.showInfo(data.message);
            }

        });


    });


    $("#btnSubmit").click(function() {

        if ($("#code").val() !== "") {
            $.ajax({
                url: "/Account/VerifyPhoneNumber",
                type: "POST",
                data: { userId:encodeURI($("#userId").val()), code: $("#code").val() },
                success: function(data) {

                    if (data.result) {

                        location.href = "/Account/RegisterSuccess?userId=" + $("#userId").val();

                    } else {
                        $.AlertMessage.showWarning("验证出错,请1分钟后重新验证");
                    }
                },
                error: function() {

                    $.AlertMessage.showInfo("验证出错,请与管理员取得联系");
                }

            });
        }


    });


}(jQuery));

