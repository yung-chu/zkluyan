(function($) {
    function BusinessRoadshowManager() {
        var PriorityManager = {
            save: function (id, priority, callback) {
                $.ajax({
                    url: '/roadshow/SavePriority',
                    type: "POST",
                    dataType: "json",
                    data: 'id=' + id + '&priority=' + priority,
                    success: callback
                });
            }
        };

        this.init = function () {
            var $table = $("table[name='biz-roadshow']");
            $table.find("a[name='modify']").each(function () {
                var _this = $(this);
                _this.on('click', function () {
                    _this.next().removeClass('hidden');
                    _this.addClass('hidden');
                    var priority = _this.parent().prev();
                    var input = $("<input type='text' class='form-control' />");
                    priority.html(input.val(priority.text()));
                });
            });

            $table.find("a[name='save']").each(function () {
                var _this = $(this);
                _this.on('click', function () {
                    _this.prev().removeClass('hidden');
                    _this.addClass('hidden');
                    var priority = _this.parent().prev()
                    var input = priority.find('input.form-control');
                    var val = input.val();
                    
                    PriorityManager.save(_this.data('id'), val, function (result) {
                        if (result) {
                            priority.html('').text(val);
                        }
                    });
                });
            });
        }
    };

    new BusinessRoadshowManager().init();
}(jQuery));