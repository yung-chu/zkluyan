(function ($) {
    var CapitalRoadshowManager = function () {
        function CapitalShow() {
            this.init = function () {
                $('#like-this-capital').on('click', function () {
                    var self = $(this);
                    var data = {
                        Id: self.data('id')
                    };

                    $.ajaxWraper.post('/Roadshow/LikeThisCapitalRoadshow', $.toJSON(data), function (result) {
                        if (result) {
                            $.AlertMessage.showInfo("感谢您关注此公司，邮件已发送至中科金集");
                            self.addClass('disabled');
                        } else {
                            $.AlertMessage.showWarning("关注失败");
                        }
                    });                    
                });
            }
        };

        return {
            init: function () {
                new CapitalShow().init();
            }
        }
    }

    new CapitalRoadshowManager().init();
}(jQuery));