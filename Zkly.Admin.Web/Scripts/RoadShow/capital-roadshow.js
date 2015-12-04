(function ($) {

    var CapitalRoadshowManager = function () {
        function CapitalRecord() {
            this.init = function () {
                var self = this;

                $('#coverFile').on('change', function () {
                    $(".thumbnail").css("display", "none");
                    $("[name=i]").css("display", "block");
                });

                $('#capitals').find("a[name='recording']").each(function () {
                    $(this).on('click', function () {
                        var $recordingPanel = $(this).closest('tr').next();
                        var hidden = $recordingPanel.hasClass('hidden');
                        
                        if (hidden) {
                            self.loadRecPart($recordingPanel);
                            $recordingPanel.removeClass('hidden');
                        } else {
                            $recordingPanel.addClass('hidden');
                        }
                    });
                });
            },

            this.loadRecPart = function ($recordingPanel) {
                var self = this;
                $.ajax({
                    url: $recordingPanel.data('url'),
                    type: "GET",
                    contentType: "application/json",
                    success: function (html) {                        
                        $recordingPanel.find('div').html('').html(html);
                        self.bindGenerateEvent($recordingPanel);
                    }
                });
            },

            this.bindGenerateEvent = function ($recordingPanel) {
                $recordingPanel.find('a.gen').on('click', function () {
                    var _this = $(this);
                    _this.next().text('');
                    var subject = _this.prev().val();
                    if (subject === '') {
                        _this.next().text("录播主题不能为空");
                        return;
                    }

                    var select = false;
                    var datas = {};
                    $recordingPanel.find('tr.data').each(function () {
                        var ischecked = $(this).find("input[type='checkbox']").is(":checked");

                        if (ischecked) {
                            var vals = $(this).find('td:eq(4)').text().split(',');
                            datas[$(this).find('td:eq(1)').text()] = vals;
                            select = true;
                        }
                    });

                    if (!select) {
                        _this.next().text("至少要选择一个录播片段");
                        return;
                    }

                    var data = { subject: subject, rec_list: datas };
                    $.ajax({
                        url: _this.data('url'),
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json",
                        data: $.toJSON(data),
                        success: function (result) {
                            if (!result) {
                                _this.next().text("数据保存失败");
                            } else {
                                _this.next().text("录播已生成，请重新刷新");
                            }                            
                        }
                    });
                });
            }
        };

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
                new CapitalRecord().init();
            }
        }
    }

    new CapitalRoadshowManager().init();
}(jQuery));


function changeActivityCover(ischange) {
    if (ischange == 1) {
        document.getElementById("IsChange").value = "1";
        document.getElementById("activityCover").style.display = "none";
        document.getElementById("activityUpload").style.display = "block";
    }
    else {
        document.getElementById("IsChange").value = "0";
        document.getElementById("activityCover").style.display = "block";
        document.getElementById("activityUpload").style.display = "none";
    }
}