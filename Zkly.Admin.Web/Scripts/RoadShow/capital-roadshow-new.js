(function ($) {

    var CapitalRoadshowManager = function () {
        function CapitalRecord() {

            this.init = function () {
                var self = this;
                $('#recording').on('click', function () {

                    var $recordingPanel = $('#recordpanel');
                    var hidden = $recordingPanel.hasClass('hidden');

                    if (hidden) {
                        self.loadRecPart($recordingPanel);
                        $recordingPanel.removeClass('hidden');
                    } else {
                        $recordingPanel.addClass('hidden');
                    }
                });
            },

            this.loadRecPart = function ($recordingPanel) {
                var self = this;
                $.ajax({
                    url: $recordingPanel.data('url'),
                    type: "GET",
                    contentType: "application/json",
                    success: function (html) {
                        $recordingPanel.html(html);
                        self.bindGenerateEvent($recordingPanel);
                    }
                });
            },

            this.getSessionIds = function (id, data, orgdata) {
                $.each(orgdata, function (i, item) {
                    if (item.session_id == id) {
                        var list = [];
                        $.each(item.list, function (j, listitem) {
                            list.push(listitem.id);
                        });
                        data[id] = list.join(',');
                        return;
                    }
                });
            },

            this.bindGenerateEvent = function ($recordingPanel) {
                var self = this;
                $recordingPanel.find('a.gen').on('click', function () {
                    var _this = $(this);
                    _this.next().text('');
                    var subject = _this.prev().val();
                    if (subject === '') {
                        _this.next().text("录播主题不能为空");
                        return;
                    }

                    var orgdata = eval($('#OriginData').val());
                    var select = false;
                    var datas = {};
                    $recordingPanel.find('tr.data').each(function () {
                        var ischecked = $(this).find("input[type='checkbox']").is(":checked");

                        if (ischecked) {
                            //var vals = $(this).find('td:eq(4)').text().split(',');
                            //datas[$(this).find('td:eq(1)').text()] = vals;
                            self.getSessionIds($(this).find('td:eq(1)').text(), datas, orgdata);
                            select = true;
                        }
                    });

                    if (!select) {
                        _this.next().text("至少要选择一个录播片段");
                        return;
                    }

                    var data = { subject: subject, reclist: datas };
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

$(document).ready(function() {
    $("#coverFile").fileinput({
        'allowedFileExtensions': ['jpg', 'png', 'gif']
    });
});


function changeActivityCover(ischange) {
    if (ischange === 1) {
        $("#IsChange").val("1");
        $("#activityCover").attr("class", "col-xs-10 hidden");
        $("#activityUpload").attr("class", "col-xs-10 show");
    }
    else {
        $("#IsChange").val("0");
        $("#activityCover").attr("class", "col-xs-10 show");
        $("#activityUpload").attr("class", "col-xs-10 hidden");
    }
}