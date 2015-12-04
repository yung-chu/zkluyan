(function ($) {
    function RoadshowShowOrder() {
        this.container = $('#show-order');

        var AllRoadshows = {
            init: function () {
                var $rsc = $('#all-roadshows');
                $rsc.find("li.list-group-item").each(function () {
                    var _this = $(this);
                    _this.on('click', function (e) {
                        if (e.target.tagName === 'INPUT') {
                            return;
                        }
                        var desc = _this.next();
                        var isHidden = desc.hasClass('hidden');
                        desc.toggleClass('hidden', !isHidden);
                    });
                });
                $rsc.find("input[type='checkbox']").each(function () {
                    $(this).on('click', function () {
                        var isChecked = $(this).attr('checked') === 'checked';
                        var isAdd = !isChecked;

                        if (isAdd && Top8Roadshows.isFull()) {
                            $(this).attr('checked', false);
                            return;
                        }

                        $(this).attr('checked', !isChecked);

                        Top8Roadshows.addToList(isAdd, {
                            id: $(this).val(),
                            videoName: $(this).next().text()
                        });
                    });
                });
            },

            cancelCheckedStatus: function (id) {
                $('#all-roadshows').find("li input[value='" + id + "']").attr('checked', false);                
            }
        };

        var Top8Roadshows = {
            init: function () {
                $('#top8-shows').find("span[name='delete']").each(function () {
                    var _this = $(this);
                    _this.on('click', function () {
                        var item = _this.closest('li');
                        AllRoadshows.cancelCheckedStatus(item.attr('id'));
                        item.remove();
                    });
                });
            },

            isFull: function(){
                var _lis = $('#top8-shows').find('li');
                if (_lis.length >= 8) {
                    $.AlertMessage.showWarning("最多只能添加 8 条路演, 请先移除一些路演！");
                    return true;
                }

                return false;
            },

            addToList: function (isAdd, data) {
                $.AlertMessage.hide();

                if (isAdd) {
                    var temp = "<li id='" + data.id + "' class='list-group-item'>" +
                               "<span>" + data.videoName + "</span>" +
                               "<span name='delete' class='glyphicon glyphicon-remove zkly-right zkly-pointer'></span></li>";

                    $('#top8-shows').append(temp);

                    Top8Roadshows.init();
                } else {
                    var needToRemove = $('#top8-shows').find("li[id='" + data.id + "']");
                    if (needToRemove !== null) {
                        needToRemove.remove();
                    }
                }
            }
        };

        var Saver = {
            init: function () {
                $('#save-shows').on('click', function () {
                    var ids = [];

                    $('#top8-shows').find('li').each(function () {
                        ids.push({ RoadshowId: $(this).attr('id') });
                    });

                    var datas = $.toJSON(ids);

                    $.ajaxWraper.post('SaveShows', datas, function (result) {
                        if (result) {
                            $.AlertMessage.showSuccess("数据保存成功！");
                        } else {
                            $.AlertMessage.showWarning("数据保存失败！");
                        }
                    });
                });
            }
        };

        return {
            init: function () {
                AllRoadshows.init();
                Top8Roadshows.init();
                Saver.init();
            }
        }
    }

    new RoadshowShowOrder().init();
}(jQuery));
