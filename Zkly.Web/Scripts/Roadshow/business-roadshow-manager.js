(function ($) {
    function BusinessRoadshowManager() {
        var Attender = {
            init: function () {
                $('#attend').on('click', function () {
                    var roadshowId = $(this).data('roadshow-id');
                    
                    var d = { Id: roadshowId };

                    $.post('/Roadshow/Attend', d, function (data) {
                        if (data === 0) {
                            $.AlertMessage.showInfo("参与成功，已经发送邮件给中科金集");
                        } else if (data === 1) {
                            $.AlertMessage.showInfo("您不是投资人，请先注册为投资人 <a href='/Account/Register'>注册</a>");
                        } else {
                            $.AlertMessage.showWarning("参与失败");
                        }
                    }, 'json');
                });
            }
        };

        return {
            init: function () {
                Attender.init();
            }
        }
    }

    new BusinessRoadshowManager().init();
}(jQuery));